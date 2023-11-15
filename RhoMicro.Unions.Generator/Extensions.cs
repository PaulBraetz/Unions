namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

internal static class Extensions
{
    public static Boolean IsError(this Diagnostic diagnostic) =>
        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
    public static Boolean HasUnionTypeAttribute(this ITypeSymbol symbol) =>
        symbol.GetAttributes().OfUnionTypeAttribute().Any();
    public static String ToFullString(this ISymbol symbol) =>
        symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    public static StringBuilder AppendSymbol(this StringBuilder builder, ITypeSymbol symbol) =>
        builder.Append(symbol.ToFullString());
    public static IncrementalValuesProvider<SourceCarry<TResult>> SelectCarry<TSource, TResult>(
        this IncrementalValuesProvider<SourceCarry<TSource>> provider,
        Func<TSource, DiagnosticsModelBuilder, SourceModelBuilder, CancellationToken, TResult> project) =>
        provider.Select((c, t) => c.Project((s, d, b) => project.Invoke(s, d, b, t)));
    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> SelectCarry<TModel>(
        this IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider,
        Func<ModelFactoryInvocationContext, TModel> modelFactory,
        Action<ModelIntegrationContext<TModel>> modelIntegration) =>
        provider.SelectCarry((c, d, s, t) =>
        {
            var invocationContext = new ModelFactoryInvocationContext(c, t);
            var model = modelFactory.Invoke(invocationContext);

            var integrationContext = new ModelIntegrationContext<TModel>(d, s, t, model);
            modelIntegration.Invoke(integrationContext);

            return c;
        });

    public static StringBuilder AppendJoin<T>(this StringBuilder builder, String separator, IEnumerable<T> values)
    {
        using var iterator = values.GetEnumerator();

        if(iterator == null || !iterator.MoveNext())
        {
            return builder;
        }

        _ = builder.Append(iterator.Current?.ToString() ?? String.Empty);

        while(iterator.MoveNext())
        {
            _ = builder.Append(separator).Append(iterator.Current?.ToString() ?? String.Empty);
        }

        return builder;
    }
    public static StringBuilder AppendAggregateJoin<T>(
        this StringBuilder builder,
        String separator,
        IEnumerable<T> values,
        Func<StringBuilder, T, StringBuilder> aggregation)
    {
        using var iterator = values.GetEnumerator();

        if(iterator == null || !iterator.MoveNext())
        {
            return builder;
        }

        builder = aggregation.Invoke(builder, iterator.Current);

        while(iterator.MoveNext())
        {
            builder = aggregation.Invoke(builder.Append(separator), iterator.Current);
        }

        return builder;
    }
    public static StringBuilder AppendJoin<T>(this StringBuilder builder, Char separator, IEnumerable<T> values)
    {
        using var iterator = values.GetEnumerator();

        if(iterator == null || !iterator.MoveNext())
        {
            return builder;
        }

        _ = builder.Append(iterator.Current?.ToString() ?? String.Empty);

        while(iterator.MoveNext())
        {
            _ = builder.Append(separator).Append(iterator.Current?.ToString() ?? String.Empty);
        }

        return builder;
    }
}
