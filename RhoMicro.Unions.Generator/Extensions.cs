namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

internal static class Extensions
{
    public static String GetSpecificAccessibility(this ModelFactoryParameters parameters, UnionTypeAttribute attribute)
    {
        var accessibility = parameters.Attributes.Settings.ConstructorAccessibility;

        if(accessibility == ConstructorAccessibilitySetting.PublicIfInconvertible &&
           parameters.OperatorOmissions.AllOmissions.Contains(attribute))
        {
            accessibility = ConstructorAccessibilitySetting.Public;
        }

        var result = accessibility == ConstructorAccessibilitySetting.Public ?
            "public" :
            "private";

        return result;
    }
    public static Boolean IsError(this Diagnostic diagnostic) =>
        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
    public static Boolean HasUnionTypeAttribute(this ITypeSymbol symbol) =>
        symbol.GetAttributes().OfUnionTypeAttribute().Any();
    public static String ToDocCompatString(this ITypeSymbol symbol) =>
        symbol.ToFullString().Replace("<", "&lt;").Replace(">", "&gt;");
    public static String ToFullString(this ISymbol symbol) =>
        symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat
    .WithMiscellaneousOptions(
    //    /*
    //         get rid of special types
    //
    //         10110
    //    NAND 00100
    //      => 10010

    //         10110
    //      &! 00100
    //      => 10010

    //         00100
    //       ^ 11111
    //      => 11011

    //         10110
    //       & 11011
    //      => 10010
    //*/
    SymbolDisplayFormat.FullyQualifiedFormat.MiscellaneousOptions &
    (SymbolDisplayMiscellaneousOptions.UseSpecialTypes ^ (SymbolDisplayMiscellaneousOptions)Int32.MaxValue)));
    public static StringBuilder AppendSymbol(this StringBuilder builder, ITypeSymbol symbol) =>
        builder.Append(symbol.ToFullString());

    public static IncrementalValuesProvider<SourceCarry<TResult>> SelectCarry<TSource, TResult>(
        this IncrementalValuesProvider<SourceCarry<TSource>> provider,
        Func<TSource, DiagnosticsModelBuilder, SourceModelBuilder, CancellationToken, TResult> project) =>
        provider.Select((c, t) => c.Project((s, d, b) => project.Invoke(s, d, b, t)));
    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> SelectCarry<TModel>(
        this IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider,
        Func<ModelCreationContext, TModel> modelFactory,
        Action<ModelIntegrationContext<TModel>> modelIntegration) =>
        provider.SelectCarry((c, d, s, t) =>
        {
            t.ThrowIfCancellationRequested();

            var invocationContext = new ModelCreationContext(c, t);
            var model = modelFactory.Invoke(invocationContext);

            var integrationContext = new ModelIntegrationContext<TModel>(d, s, t, model);
            modelIntegration.Invoke(integrationContext);

            return c;
        });

    public static String GetContainingClassHead(this ITypeSymbol nestedType)
    {
        var resultBuilder = new StringBuilder();
        var headers = getContainingTypes(nestedType)
            .Select(s => s.TypeKind switch
            {
                TypeKind.Class => "class ",
                TypeKind.Struct => "struct ",
                TypeKind.Interface => "interface ",
                _ => null
            } + s.Name)
            .Where(k => k != null)
            .Aggregate(
                resultBuilder,
                (b, s) => b.Append("partial ").Append(s).AppendLine("{"));

        var result = resultBuilder.ToString();

        return result;

        static IEnumerable<ITypeSymbol> getContainingTypes(ITypeSymbol symbol)
        {
            if(symbol.ContainingType != null)
            {
                return getContainingTypes(symbol.ContainingType).Append(symbol.ContainingType);
            }

            return Array.Empty<ITypeSymbol>();
        }
    }
    public static String GetContainingClassTail(this ITypeSymbol nestedType)
    {
        var resultBuilder = new StringBuilder();
        var containingType = nestedType.ContainingType;
        while(containingType != null)
        {
            _ = resultBuilder.AppendLine("}");

            containingType = containingType.ContainingType;
        }

        var result = resultBuilder.ToString();

        return result;
    }

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
    public static StringBuilder AppendAggregate<T>(
        this StringBuilder builder,
        IEnumerable<T> values,
        Func<StringBuilder, T, StringBuilder> aggregation) => values.Aggregate(builder, aggregation);
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

    public static Boolean InheritsFrom(this ITypeSymbol subtype, ITypeSymbol supertype)
    {
        var baseTypes = getBaseTypes(subtype);
        if(baseTypes.Contains(supertype, SymbolEqualityComparer.Default))
        {
            return true;
        }

        var interfaces = subtype.AllInterfaces;
        if(interfaces.Contains(supertype, SymbolEqualityComparer.Default))
        {
            return true;
        }

        return false;

        static IEnumerable<INamedTypeSymbol> getBaseTypes(ITypeSymbol symbol)
        {
            var baseType = symbol.BaseType;
            while(baseType != null)
            {
                yield return baseType;

                baseType = baseType.BaseType;
            }
        }
    }
}
