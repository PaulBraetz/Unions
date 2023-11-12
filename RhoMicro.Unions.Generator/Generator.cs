namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System.Collections.Generic;
using System.Linq;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var handledTargets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var models = context.SyntaxProvider.CreateSyntaxProvider(
                GeneratorUtilities.IsUnionDeclaration,
                (c, t) =>
                {
                    t.ThrowIfCancellationRequested();

                    if (c.Node is not TypeDeclarationSyntax target)
                    {
                        return new SourceCarry<(SemanticModel SemanticModel, TypeDeclarationSyntax Target, ITypeSymbol Symbol)>();
                    }

                    var semanticModel = c.SemanticModel;
                    var symbol = semanticModel.GetDeclaredSymbol(target, t) as ITypeSymbol;

                    if (!handledTargets.Add(symbol))
                    {
                        return new SourceCarry<(SemanticModel SemanticModel, TypeDeclarationSyntax Target, ITypeSymbol Symbol)>();
                    }

                    var diagnostics = new DiagnosticsModelBuilder();
                    var source = new SourceModelBuilder();

                    source.SetTarget(symbol);

                    diagnostics.DiagnoseTarget(target, semanticModel, t);

                    return new SourceCarry<(SemanticModel SemanticModel, TypeDeclarationSyntax Target, ITypeSymbol Symbol)>(
                        (SemanticModel: semanticModel, Target: target, Symbol: symbol),
                        diagnostics,
                        source);
                })
            .SelectCarry((c, d, b, t) =>
            {
                var attributeData = c.Symbol.GetAttributes();

                var unionAttributes = attributeData.OfUnionTypeAttribute()
                        .OrderBy(a => a.RepresentableTypeSymbol.ToFullString())
                        .ToArray();
                var supersetUnionAttributes = attributeData.OfSubsetOfAttribute()
                        .OrderBy(a => a.SupersetUnionTypeSymbol.ToFullString())
                        .ToArray();
                var subsetUnionAttributes = attributeData.OfSubsetOfAttribute()
                        .OrderBy(a => a.SupersetUnionTypeSymbol.ToFullString())
                        .ToArray();

                return (
                    c.Symbol,
                    c.Target,
                    c.SemanticModel,
                    UnionAttributes: unionAttributes,
                    SupersetUnionAttributes: supersetUnionAttributes,
                    SubsetUnionAttributes: subsetUnionAttributes
                    );
            })
            .SelectCarry((c, d, b, t) =>
            {
                var models = c.UnionAttributes
                    .Select(a =>
                        ConversionOperatorModel.Create(c.Symbol, a))
                    //.Concat(
                    //    c.SupersetUnionAttributes.Select(a =>
                    //        ConversionOperatorModel.Create(c.Symbol, c.UnionAttributes, a)))
                    //.Concat(
                    //    c.SupersetUnionAttributes.Select(a =>
                    //        ConversionOperatorModel.Create(c.Symbol, c.UnionAttributes, a)))
                    ;

                b.SetOperators(models);

                return c;
            });

        context.RegisterSourceOutput(models, (c, sc) => sc.AddToContext(c));
    }
}
