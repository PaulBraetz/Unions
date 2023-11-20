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
using Microsoft.CodeAnalysis.Diagnostics;

sealed class DiagnosticsModelBuilder
{
    public sealed class Model
    {
        public Model(IEnumerable<Diagnostic> diagnostics)
        {
            _diagnostics = diagnostics;
            IsError = diagnostics.Any(Extensions.IsError);
        }
        private Model(Boolean isError)
        {
            _diagnostics = Array.Empty<Diagnostic>();
            IsError = isError;
        }

        public readonly static Model Error = new(isError: true);
        public readonly static Model NoError = new(isError: false);

        public readonly Boolean IsError;
        private readonly IEnumerable<Diagnostic> _diagnostics;

        public void AddToContext(SyntaxNodeAnalysisContext context)
        {
            foreach(var diagnostic in _diagnostics)
            {
                context.ReportDiagnostic(diagnostic);
            }
        }
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

    private Boolean _reportDiagnostics;

    private DiagnosticsModelBuilder(ICollection<Diagnostic> diagnostics)
    {
        _diagnostics = diagnostics;
        IsError = diagnostics.Any(d => d.IsError());
    }

    public DiagnosticsModelBuilder() : this(new List<Diagnostic>())
    {

    }

    public DiagnosticsModelBuilder ReportDiagnostics()
    {
        _reportDiagnostics = true;
        return this;
    }

    #region Auto Diagnosers
    private void DiagnoseReservedGenericParameterName(TargetDataModel parameters)
    {
        var collisions = parameters.TargetSymbol.TypeParameters
            .Select(p => p.Name)
            .Where(ConstantSources.ReservedGenericTypeNames.Contains)
            .ToArray();

        if(collisions.Length == 0)
        {
            return;
        }

        var location = parameters.TargetDeclaration.GetLocation();

        foreach(var collision in collisions)
        {
            var diagnostic = Diagnostics.ReservedGenericParameterName(location, collision);
            _ = Add(diagnostic);
        }
    }
    private void DiagnoseOperatorOmissions(TargetDataModel parameters)
    {
        var omissions = parameters.OperatorOmissions;
        var location = parameters.TargetDeclaration.GetLocation();

        foreach(var interfaceOmission in omissions.Interfaces)
        {
            var diagnostic = Diagnostics.RepresentableTypeIsInterface(location, interfaceOmission.RepresentableTypeSymbol!.Name);
            _ = Add(diagnostic);
        }

        foreach(var supertypes in omissions.Supertypes)
        {
            var diagnostic = Diagnostics.RepresentableTypeIsSupertype(location, supertypes.RepresentableTypeSymbol!.Name);
            _ = Add(diagnostic);
        }
    }
    private void DiagnoseUnionTypeSettingsOnNonUnionType(TargetDataModel parameters)
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
    private void DiagnoseImplicitConversionIfSolitary(TargetDataModel parameters)
    {
        var attributes = parameters.Attributes.AllUnionTypeAttributes;
        var location = parameters.TargetDeclaration.GetLocation();

        if(attributes.Count == 1 &&
           attributes[0].Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary))
        {
            var diagnostic = Diagnostics.ImplicitConversionOptionOnSolitary(
                parameters.TargetSymbol.Name,
                attributes[0].TypeName,
                location);
            _ = Add(diagnostic);
        } else if(attributes.Count > 1 &&
           attributes.Any(a => a.Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary)))
        {
            var diagnostic = Diagnostics.ImplicitConversionOptionOnNonSolitary(location);
            _ = Add(diagnostic);
        }
    }
    private void DiagnoseUnionTypeCount(TargetDataModel parameters)
    {
        var count = parameters.Attributes.AllUnionTypeAttributes.Count;
        if(count <= Byte.MaxValue)
            return;

        var location = parameters.TargetDeclaration.GetLocation();
        var diagnostics = Diagnostics.TooManyTypes(location);
        _ = Add(diagnostics);
    }
    private void DiagnosePartiality(TargetDataModel parameters)
    {
        if(parameters.TargetDeclaration.IsPartial())
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.NonPartialDeclaration(location);
        _ = Add(diagnostics);
    }
    private void DiagnoseNonStatic(TargetDataModel parameters)
    {
        if(!parameters.TargetDeclaration.Modifiers.Any(SyntaxKind.StaticKeyword))
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.StaticTarget(location);
        _ = Add(diagnostics);
    }
    private void DiagnoseNonRecord(TargetDataModel parameters)
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
    private void DiagnoseUnionTypeAttribute(TargetDataModel parameters)
    {
        if(parameters.Attributes.AllUnionTypeAttributes.Count > 0)
            return;

        var location = parameters.TargetDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.MissingUnionTypeAttribute(location);
        _ = Add(diagnostics);
    }
    //TODO: support duplicate representable types
    //-> no factories, impl/expl, ISupertype interface declarations
    private void DiagnoseUniqueUnionTypeAttributes(TargetDataModel parameters)
    {
        _ = parameters.Attributes.AllUnionTypeAttributes
            .GroupBy(t => t.FullTypeName)
            .Select(g => (Name: g.Key, Locations: g.Select(t => parameters.TargetDeclaration.GetLocation()).ToArray()))
            .Where(t => t.Locations.Length > 1)
            .SelectMany(t => t.Locations.Skip(1).Select(l => Diagnostics.DuplicateUnionTypeAttributes(t.Name, l)))
            .Aggregate(this, (b, d) => b.Add(d));
    }
    private void DiagnoseAliasCollisions(TargetDataModel parameters)
    {
        var duplicates = parameters.Attributes.AllUnionTypeAttributes
            .GroupBy(a => a.SafeAlias)
            .Where(g => g.Skip(1).Any())
            .Select(g => g.First().TypeName);
        if(!duplicates.Any())
        {
            return;
        }

        var location = parameters.TargetDeclaration.GetLocation();
        _ = duplicates.Select(d => Diagnostics.AliasCollision(location, d))
            .Aggregate(this, (b, d) => b.Add(d));
    }
    #endregion

    private static readonly IEnumerable<Action<DiagnosticsModelBuilder, TargetDataModel, CancellationToken>> _diagnosers = GetDiagnosers();
    private static List<Action<DiagnosticsModelBuilder, TargetDataModel, CancellationToken>> GetDiagnosers()
    {
        var result = typeof(DiagnosticsModelBuilder)
                .GetMethods(
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly |
                    BindingFlags.NonPublic)
                .Where(m => m.ReturnType == typeof(void))
                .Where(m =>
                {
                    var parameters = m.GetParameters();
                    var result = parameters.Length == 1 &&
                        parameters[0].ParameterType == typeof(TargetDataModel);

                    return result;
                })
                .Select(diagnoseMethod =>
                {
                    var instanceParam = Expression.Parameter(typeof(DiagnosticsModelBuilder));
                    var parametersParam = Expression.Parameter(typeof(TargetDataModel));
                    var tokenParam = Expression.Parameter(typeof(CancellationToken));

                    var throwMethod = typeof(CancellationToken).GetMethod(nameof(CancellationToken.ThrowIfCancellationRequested));
                    var throwExpr = Expression.Call(tokenParam, throwMethod);

                    var callExpr = Expression.Call(instanceParam, diagnoseMethod, parametersParam);

                    var body = Expression.Block(throwExpr, callExpr);

                    var lambda = Expression.Lambda(body, instanceParam, parametersParam, tokenParam);
                    var result = lambda.Compile();

                    return result;
                })
                .Cast<Action<DiagnosticsModelBuilder, TargetDataModel, CancellationToken>>()
                .ToList();

        return result;
    }
    internal DiagnosticsModelBuilder Diagnose(TargetDataModel parameters, CancellationToken token)
    {
        foreach(var d in _diagnosers)
            d.Invoke(this, parameters, token);

        return this;
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

        var result = new DiagnosticsModelBuilder(diagnostics);
        if(_reportDiagnostics)
        {
            _ = result.ReportDiagnostics();
        }

        return result;
    }

    public Model Build(UnionTypeSettingsAttribute settings)
    {
        if(!_reportDiagnostics)
        {
            return IsError ?
                Model.Error :
                Model.NoError;
        }

        IEnumerable<Diagnostic> reportableDiagnostics;
        lock(_syncRoot)
        {
            reportableDiagnostics = _diagnostics
                .Where(d => d.ShouldReport(settings.DiagnosticsLevel))
                .ToList();
        }

        return new Model(reportableDiagnostics);
    }
}
