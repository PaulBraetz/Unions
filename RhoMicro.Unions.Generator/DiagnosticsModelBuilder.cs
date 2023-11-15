namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using RhoMicro.Unions;
using System.Reflection;

sealed class DiagnosticsModelBuilder
{
    public sealed class Model
    {
        public Model(IEnumerable<Diagnostic> diagnostics)
        {
            _diagnostics = diagnostics;
            IsError = diagnostics.Any(Extensions.IsError);
        }
        public Model(Diagnostic diagnostic) : this(new[] { diagnostic }) { }
        private Model() : this(Array.Empty<Diagnostic>()) { }

        public readonly static Model Empty = new();
        public readonly Boolean IsError;
        private readonly IEnumerable<Diagnostic> _diagnostics;

        public void AddToContext(SourceProductionContext context)
        {
            foreach(var diagnostic in _diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private readonly ICollection<Diagnostic> _diagnostics = new List<Diagnostic>();
    private readonly Object _syncRoot = new();

    public Boolean IsError { get; private set; }

    private DiagnosticsModelBuilder(ICollection<Diagnostic> diagnostics)
    {
        _diagnostics = diagnostics;
        IsError = diagnostics.Any(d => d.IsError());
    }

    public DiagnosticsModelBuilder() : this(new List<Diagnostic>()) { }

    public void DiagnoseInvalidTargetNode(SyntaxNode node, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if(node is TypeDeclarationSyntax)
        {
            return;
        }

        var location = node.GetLocation();
        var diagnostic = Diagnostics.InvalidAttributeTarget(location);
        _ = Add(diagnostic);
    }
    public void DiagnoseImplicitConversionIfSolitary(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var attributes = context.Attributes.AllUnionTypeAttributes;
        var location = context.TargetDeclaration.GetLocation();

        if(attributes.Count == 1 &&
           attributes[0].Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary))
        {
            var diagnostic = Diagnostics.ImplicitConversionOptionOnSolitary(
                context.TargetSymbol.Name,
                attributes[0].RepresentableTypeSymbol.Name,
                location);
            _ = Add(diagnostic);
        } else if(attributes.Count > 1 &&
           attributes.Any(a => a.Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary)))
        {
            var diagnostic = Diagnostics.ImplicitConversionOptionOnNonSolitary(location);
            _ = Add(diagnostic);
        }
    }
    public void DiagnoseUnionTypeCount(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var count = context.Attributes.AllUnionTypeAttributes.Count;
        if(count <= Byte.MaxValue)
            return;

        var location = context.TargetDeclaration.GetLocation();
        var diagnostics = Diagnostics.TooManyTypes(location);
        _ = Add(diagnostics);
    }
    public void DiagnosePartiality(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if(context.TargetDeclaration.IsPartial())
            return;

        var location = context.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.NonPartialDeclaration(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseNonStatic(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if(!context.TargetDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            return;

        var location = context.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.StaticTarget(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseNonRecord(ModelFactoryParameters context, CancellationToken token)
    {
        if(!context.TargetDeclaration.IsKind(SyntaxKind.RecordDeclaration) &&
            !context.TargetDeclaration.IsKind(SyntaxKind.RecordStructDeclaration))
        {
            return;
        }

        var location = context.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.RecordTarget(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseUnionTypeAttribute(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if(context.Attributes.AllUnionTypeAttributes.Count > 0)
            return;

        var location = context.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.MissingUnionTypeAttribute(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseUniqueUnionTypeAttributes(ModelFactoryParameters context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        _ = context.Attributes.AllUnionTypeAttributes
            .GroupBy(t => t.RepresentableTypeSymbol.ToFullString())
            .Select(g => (Name: g.Key, Locations: g.Select(t => context.TargetDeclaration.GetLocation()).ToArray()))
            .Where(t => t.Locations.Length > 1)
            .SelectMany(t => t.Locations.Skip(1).Select(l => Diagnostics.DuplicateUnionTypeAttributes(t.Name, l)))
            .Aggregate(this, (b, d) => b.Add(d));
    }

    //TODO:
    //detect redundant subset/superset (research IsAssignableFrom equivalent in roslyn)
    //diagnose faulty subset/superset declarations
    //  -> error if superset of disjunct union
    //  -> error if subset of disjunct union

    internal void DiagnoseAll(ModelFactoryParameters context, CancellationToken token)
    {
        var diagnosers = new[]
        {
            DiagnoseImplicitConversionIfSolitary,
            DiagnoseUnionTypeCount,
            DiagnosePartiality,
            DiagnosePartiality,
            DiagnoseNonStatic,
            DiagnoseNonRecord,
            DiagnoseUnionTypeAttribute,
            DiagnoseUniqueUnionTypeAttributes
        };

        foreach(var d in diagnosers)
            d.Invoke(context, token);
    }

    public DiagnosticsModelBuilder Add(Diagnostic diagnostic)
    {
        lock(_syncRoot)
        {
            if(diagnostic.IsError())
            {
                IsError = true;
            }

            _diagnostics.Add(diagnostic);
        }

        return this;
    }
    public DiagnosticsModelBuilder Clone()
    {
        ICollection<Diagnostic> diagnostics;
        lock(_syncRoot)
        {
            diagnostics = _diagnostics.ToList();
        }

        return new(diagnostics);
    }

    public Model Build()
    {
        lock(_syncRoot)
        {
            return new Model(_diagnostics.ToList());
        }
    }
}
