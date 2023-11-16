namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    private static readonly Func<IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>, IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>> _projections =
        GetProjections();

    private static Func<IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>, IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>> GetProjections()
    {
        var parameter = Expression.Parameter(typeof(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>), "provider");

        var selectCarryMethod = typeof(Extensions).GetMethods()
            .Single(m =>
                m.Name == nameof(Extensions.SelectCarry) &&
                m.ReturnType == typeof(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>) &&
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
                       m.ReturnType != typeof(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>))
                    {
                        return false;
                    }

                    var parameters = m.GetParameters();

                    if(parameters.Length != 1 || parameters[0].ParameterType != typeof(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>))
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
            Func<IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>,
            IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>>>(body, parameter).Compile();

        return result;
    }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var handledTargets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var models = context.SyntaxProvider.CreateSyntaxProvider(
            GeneratorUtilities.IsUnionDeclaration,
            SourceCarry<ModelFactoryParameters>.Create)
            .SelectCarry(
                c => c.Parameters,
                c => c.Diagnostics.DiagnoseAll(c.Model, c.CancellationToken))
            .SelectCarry((c, d, s, t) => (Context: c, IsFirst: handledTargets.Add(c.TargetSymbol)))
            .Where(c => !c.HasContext || c.Context.IsFirst)
            .SelectCarry((c, d, s, t) => c.Context)
            .SelectCarry(
                c => c.Parameters.TargetSymbol,
                c => c.Source.SetTarget(c.Model));

        models = _projections.Invoke(models);

        context.RegisterSourceOutput(models, (c, sc) => sc.AddToContext(c));
    }
}
