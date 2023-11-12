namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Threading;
using RhoMicro.Unions;

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
            foreach (var diagnostic in _diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
    }

    private readonly ICollection<Diagnostic> _diagnostics = new List<Diagnostic>();
    private readonly Object _syncRoot = new();

    public readonly Boolean IsError;

    private DiagnosticsModelBuilder(ICollection<Diagnostic> diagnostics)
    {
        _diagnostics = diagnostics;
        IsError = diagnostics.Any(d => d.IsError());
    }

    public DiagnosticsModelBuilder() : this(new List<Diagnostic>()) { }

    public void ValidatePartiality(TypeDeclarationSyntax typeDeclaration)
    {
        if (typeDeclaration.IsPartial())
            return;

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.NonPartialDeclaration.Create(location);
        _ = Add(diagnostics);
    }
    public void ValidateNonStatic(TypeDeclarationSyntax typeDeclaration)
    {
        if (!typeDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            return;

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.StaticTarget.Create(location);
        _ = Add(diagnostics);
    }
    public void ValidateNonRecord(TypeDeclarationSyntax typeDeclaration)
    {
        if (!typeDeclaration.IsKind(SyntaxKind.RecordDeclaration) &&
            !typeDeclaration.IsKind(SyntaxKind.RecordStructDeclaration))
        {
            return;
        }

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.RecordTarget.Create(location);
        _ = Add(diagnostics);
    }
    public void ValidateUnionTypeAttribute(TypeDeclarationSyntax typeDeclaration, SemanticModel semanticModel)
    {
        var symbol = semanticModel.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;
        if (symbol.HasUnionTypeAttribute())
            return;

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.MissingUnionTypeAttribute.Create(location);
        _ = Add(diagnostics);
    }
    public void ValidateUniqueUnionTypeAttributes(
        TypeDeclarationSyntax typeDeclaration,
        SemanticModel semanticModel)
    {
        var symbol = semanticModel.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;
        _ = symbol.GetAttributes()
            .OfUnionTypeAttribute()
            .GroupBy(t => t.RepresentableTypeSymbol.ToDisplayString())
            .Select(g => (Name: g.Key, Locations: g.Select(t => typeDeclaration.GetLocation()).ToArray()))
            .Where(t => t.Locations.Length > 1)
            .SelectMany(t => t.Locations.Select(l => Diagnostics.DuplicateUnionTypeAttributes.Create(t.Name, l)))
            .Aggregate(this, (b, d) => b.Add(d));
    }

    //TODO:
    //detect redundant subset/superset (research IsAssignableFrom equivalent in roslyn)
    //diagnose faulty subset/superset declarations
    //  -> error if superset of disjunct union
    //  -> error if subset of disjunct union

    internal void DiagnoseTarget(
        TypeDeclarationSyntax typeDeclarationSyntax,
        SemanticModel semanticModel,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        ValidatePartiality(typeDeclarationSyntax);

        token.ThrowIfCancellationRequested();
        ValidateNonRecord(typeDeclarationSyntax);

        token.ThrowIfCancellationRequested();
        ValidateNonStatic(typeDeclarationSyntax);

        token.ThrowIfCancellationRequested();
        ValidateUnionTypeAttribute(typeDeclarationSyntax, semanticModel);

        token.ThrowIfCancellationRequested();
        ValidateUniqueUnionTypeAttributes(typeDeclarationSyntax, semanticModel);
    }

    public DiagnosticsModelBuilder Add(Diagnostic diagnostic)
    {
        lock (_syncRoot)
        {
            _diagnostics.Add(diagnostic);
        }

        return this;
    }
    public DiagnosticsModelBuilder Clone()
    {
        ICollection<Diagnostic> diagnostics;
        lock (_syncRoot)
        {
            diagnostics = _diagnostics.ToList();
        }

        return new(diagnostics);
    }

    public Model Build()
    {
        lock (_syncRoot)
        {
            return new Model(_diagnostics.ToList());
        }
    }
}
