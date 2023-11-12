namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

internal static class Extensions
{
    public static Boolean IsError(this Diagnostic diagnostic) =>
        diagnostic.IsWarningAsError || diagnostic.Severity == DiagnosticSeverity.Error;
    public static Boolean HasUnionTypeAttribute(this ITypeSymbol symbol) =>
        symbol.GetAttributes().Any();
    public static String ToFullString(this ITypeSymbol symbol) =>
        symbol.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    public static StringBuilder AppendSymbol(this StringBuilder builder, ITypeSymbol symbol) =>
        builder.Append(symbol.ToFullString());
    public static IncrementalValuesProvider<SourceCarry<TResult>> SelectCarry<TSource, TResult>(
        this IncrementalValuesProvider<SourceCarry<TSource>> provider,
        Func<TSource, DiagnosticsModelBuilder, SourceModelBuilder, CancellationToken, TResult> project) =>
        provider.Select((c, t) => c.Project((s, d, b) => project.Invoke(s, d, b, t)));
}
