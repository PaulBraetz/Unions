namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
internal sealed class DiagnosticsModel : ISourceProductionModel
{
    public DiagnosticsModel(IEnumerable<Diagnostic> diagnostics)
    {
        _diagnostics = diagnostics;
        //_isError = diagnostics.Any(Extensions.IsError); TODO
    }
    public DiagnosticsModel(Diagnostic diagnostic) : this(new[] { diagnostic }) { }
    private DiagnosticsModel() : this(Array.Empty<Diagnostic>()) { }

    public readonly static DiagnosticsModel Empty = new();

    private readonly IEnumerable<Diagnostic> _diagnostics;

    public void AddToContext(SourceProductionContext context)
    {
        foreach(var diagnostic in _diagnostics)
        {
            context.ReportDiagnostic(diagnostic);
        }
    }

    public static void ValidatePartiality(
        TypeDeclarationSyntax typeDeclaration,
        DiagnosticsModelBuilder builder)
    {
        if(typeDeclaration.IsPartial())
            return;

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.NonPartialDeclaration.Create(location);
        _ = builder.Add(diagnostics);
    }
    public static void ValidateUnionTypeAttribute(
        TypeDeclarationSyntax typeDeclaration,
        SemanticModel semanticModel,
        DiagnosticsModelBuilder builder)
    {
        var symbol = semanticModel.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;
        if(symbol.HasUnionTypeAttribute())
            return;

        var location = typeDeclaration.Identifier.GetLocation();
        var diagnostics = Diagnostics.MissingUnionTypeAttribute.Create(location);
        _ = builder.Add(diagnostics);
    }
    public static void ValidateUniqueUnionTypeAttributes(
        TypeDeclarationSyntax typeDeclaration,
        SemanticModel semanticModel,
        DiagnosticsModelBuilder builder)
    {
        var symbol = semanticModel.GetDeclaredSymbol(typeDeclaration) as ITypeSymbol;
        _ = symbol.GetAttributes()
            .OfUnionTypeAttribute()
            .GroupBy(t => t.RepresentableTypeSymbol.ToDisplayString())
            .Select(g => (Name: g.Key, Locations: g.Select(t => typeDeclaration.GetLocation()).ToArray()))
            .Where(t => t.Locations.Length > 1)
            .SelectMany(t => t.Locations.Select(l => Diagnostics.DuplicateUnionTypeAttributes.Create(t.Name, l)))
            .Aggregate(builder, (b, d) => b.Add(d));
    }

    //TODO:
    //detect redundant subset/superset (research IsAssignableFrom equivalent in roslyn)
    //diagnose faulty subset/superset declarations
    //  -> error if superset of disjunct union
    //  -> error if subset of disjunct union

    internal static void DiagnoseTarget(
        TypeDeclarationSyntax typeDeclarationSyntax,
        SemanticModel semanticModel,
        DiagnosticsModelBuilder builder,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        ValidatePartiality(typeDeclarationSyntax, builder);

        token.ThrowIfCancellationRequested();
        ValidateUnionTypeAttribute(typeDeclarationSyntax, semanticModel, builder);

        token.ThrowIfCancellationRequested();
        ValidateUniqueUnionTypeAttributes(typeDeclarationSyntax, semanticModel, builder);
    }
}
