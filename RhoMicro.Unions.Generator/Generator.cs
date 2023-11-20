namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    private static readonly Func<IncrementalValuesProvider<SourceCarry<TargetDataModel>>, IncrementalValuesProvider<SourceCarry<TargetDataModel>>> _projections =
        GetProjections();

    private static Func<IncrementalValuesProvider<SourceCarry<TargetDataModel>>, IncrementalValuesProvider<SourceCarry<TargetDataModel>>> GetProjections()
    {
        var parameter = Expression.Parameter(typeof(IncrementalValuesProvider<SourceCarry<TargetDataModel>>), "provider");

        var selectCarryMethod = typeof(Extensions).GetMethods()
            .Single(m =>
                m.Name == nameof(Extensions.SelectCarry) &&
                m.ReturnType == typeof(IncrementalValuesProvider<SourceCarry<TargetDataModel>>) &&
                m.GetParameters().Length == 3);

        var body = typeof(Generator).Assembly
            .GetTypes()
            .Where(t => t.Namespace == typeof(ConstructorsModel).Namespace)
            .Select<Type, (Boolean Success, MethodInfo? Project)>(t =>
            {
                var methods = t.GetMethods();

                var project = methods.Where(m =>
                {
                    if(m.Name != nameof(ConversionOperatorsModel.Project) ||
                       m.ReturnType != typeof(IncrementalValuesProvider<SourceCarry<TargetDataModel>>))
                    {
                        return false;
                    }

                    var parameters = m.GetParameters();

                    if(parameters.Length != 1 || parameters[0].ParameterType != typeof(IncrementalValuesProvider<SourceCarry<TargetDataModel>>))
                    {
                        return false;
                    }

                    return true;
                }).SingleOrDefault();

                if(project == null)
                {
                    return (Success: false, Project: null);
                }

                return (Success: true, Project: project);
            })
            .Where(t => t.Success)
            .Select(t => t.Project)
            .Aggregate(
                (Expression)parameter,
                (p, m) => Expression.Call(m, p));

        var result = Expression.Lambda<
            Func<IncrementalValuesProvider<SourceCarry<TargetDataModel>>,
            IncrementalValuesProvider<SourceCarry<TargetDataModel>>>>(body, parameter).Compile();

        return result;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var handledTargets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var models = context.SyntaxProvider.CreateSyntaxProvider(
            Extensions.IsUnionDeclaration,
            SourceCarry<TargetDataModel>.Create)
            .SelectCarry(
                c => c.TargetData,
                c => c.Diagnostics.Diagnose(c.Model, c.CancellationToken))
            .SelectCarry((c, d, s, t) =>(Context: c, IsFirst: handledTargets.Add(c.TargetSymbol)))
            .Where(c => !c.HasContext || c.Context.IsFirst)
            .SelectCarry((c, d, s, t) => c.Context)
            .SelectCarry(
                c => c.TargetData.TargetSymbol,
                c => c.Source.SetTarget(c.Model));

        models = _projections.Invoke(models);

        context.RegisterSourceOutput(models, (c, sc) => sc.AddToContext(c));
    }
}
