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
using System.Linq.Expressions;

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

    #region Auto Diagnosers
    public void DiagnoseOperatorOmissions(ModelFactoryParameters parameters)
    {
        var omissions = parameters.OperatorOmissions;
        var location = parameters.TargetDeclaration.GetLocation();

        foreach(var interfaceOmission in omissions.Interfaces)
        {
            var diagnostic = Diagnostics.RepresentableTypeIsInterface(location, interfaceOmission.RepresentableTypeSymbol.Name);
            _ = Add(diagnostic);
        }

        foreach(var supertypes in omissions.Supertypes)
        {
            var diagnostic = Diagnostics.RepresentableTypeIsSupertype(location, supertypes.RepresentableTypeSymbol.Name);
            _ = Add(diagnostic);
        }
    }
    public void DiagnoseUnionTypeSettingsOnNonUnionType(ModelFactoryParameters parameters)
    {
        var unionTypeAttributes = parameters.Attributes.AllUnionTypeAttributes;
        var relationAttributes = parameters.Attributes.AllRelationAttributes;

        if(unionTypeAttributes.Count > 0 ||
           relationAttributes.Count > 0 ||
           !parameters.TargetSymbol
            .GetAttributes()
            .OfUnionTypeSettingsAttribute()
            .Any())
        {
            return;
        }

        var location = parameters.TargetDeclaration.GetLocation();
        var diagnostics = Diagnostics.UnionTypeSettingsOnNonUnionType(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseImplicitConversionIfSolitary(ModelFactoryParameters parameters)
    {
        var attributes = parameters.Attributes.AllUnionTypeAttributes;
        var location = parameters.TargetDeclaration.GetLocation();

        if(attributes.Count == 1 &&
           attributes[0].Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary))
        {
            var diagnostic = Diagnostics.ImplicitConversionOptionOnSolitary(
                parameters.TargetSymbol.Name,
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
    public void DiagnoseUnionTypeCount(ModelFactoryParameters parameters)
    {
        var count = parameters.Attributes.AllUnionTypeAttributes.Count;
        if(count <= Byte.MaxValue)
            return;

        var location = parameters.TargetDeclaration.GetLocation();
        var diagnostics = Diagnostics.TooManyTypes(location);
        _ = Add(diagnostics);
    }
    public void DiagnosePartiality(ModelFactoryParameters parameters)
    {
        if(parameters.TargetDeclaration.IsPartial())
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.NonPartialDeclaration(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseNonStatic(ModelFactoryParameters parameters)
    {
        if(!parameters.TargetDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.StaticTarget(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseNonRecord(ModelFactoryParameters parameters)
    {
        if(!parameters.TargetDeclaration.IsKind(SyntaxKind.RecordDeclaration) &&
            !parameters.TargetDeclaration.IsKind(SyntaxKind.RecordStructDeclaration))
        {
            return;
        }

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.RecordTarget(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseUnionTypeAttribute(ModelFactoryParameters parameters)
    {
        if(parameters.Attributes.AllUnionTypeAttributes.Count > 0)
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.MissingUnionTypeAttribute(location);
        _ = Add(diagnostics);
    }
    public void DiagnoseUniqueUnionTypeAttributes(ModelFactoryParameters parameters)
    {
        _ = parameters.Attributes.AllUnionTypeAttributes
            .GroupBy(t => t.RepresentableTypeSymbol.ToFullString())
            .Select(g => (Name: g.Key, Locations: g.Select(t => parameters.TargetDeclaration.GetLocation()).ToArray()))
            .Where(t => t.Locations.Length > 1)
            .SelectMany(t => t.Locations.Skip(1).Select(l => Diagnostics.DuplicateUnionTypeAttributes(t.Name, l)))
            .Aggregate(this, (b, d) => b.Add(d));
    }
    public void DiagnoseAliasCollisions(ModelFactoryParameters parameters)
    {
        var duplicates = parameters.Attributes.AllUnionTypeAttributes
            .GroupBy(a => a.SafeAlias)
            .Where(g => g.Skip(1).Any())
            .Select(g => g.First().RepresentableTypeSymbol.Name);
        if(!duplicates.Any())
        {
            return;
        }

        var location = parameters.TargetDeclaration.GetLocation();
        _ = duplicates.Select(d => Diagnostics.AliasCollision(location, d))
            .Aggregate(this, (b, d) => b.Add(d));
    }
    #endregion

    private static readonly IEnumerable<Action<DiagnosticsModelBuilder, ModelFactoryParameters, CancellationToken>> _diagnosers = GetDiagnosers();
    private static List<Action<DiagnosticsModelBuilder, ModelFactoryParameters, CancellationToken>> GetDiagnosers()
    {
        var result = typeof(DiagnosticsModelBuilder)
                .GetMethods()
                .Where(m => m.ReturnType == typeof(void))
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    var result = parameters.Length == 1 &&
                        parameters[0].ParameterType == typeof(ModelFactoryParameters);

                    return result;
                })
                .Select(m =>
                {
                    var instanceParam = Expression.Parameter(typeof(DiagnosticsModelBuilder));
                    var parametersParam = Expression.Parameter(typeof(ModelFactoryParameters));
                    var tokenParam = Expression.Parameter(typeof(CancellationToken));

                    var throwMethod = typeof(CancellationToken).GetMethod(nameof(CancellationToken.ThrowIfCancellationRequested));
                    var throwExpr = Expression.Call(tokenParam, throwMethod);

                    var callExpr = Expression.Call(instanceParam, m, parametersParam);

                    var body = Expression.Block(throwExpr, callExpr);

                    var lambda = Expression.Lambda(body, instanceParam, parametersParam, tokenParam);
                    var result = lambda.Compile();

                    return result;
                })
                .Cast<Action<DiagnosticsModelBuilder, ModelFactoryParameters, CancellationToken>>()
                .ToList();

        return result;
    }
    internal void DiagnoseAll(ModelFactoryParameters parameters, CancellationToken token)
    {
        foreach(var d in _diagnosers)
            d.Invoke(this, parameters, token);
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
