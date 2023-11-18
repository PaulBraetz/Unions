//TODO: continue here

namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Linq;
using System.Text;

readonly struct ConstructorsModel
{
    public readonly String SourceText;
    private ConstructorsModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<ConstructorsModel> context) =>
        context.Source.SetConstructors(context.Model);

    private static ConstructorsModel Create(ModelCreationContext context)
    {
        var (symbol, _, _, attributes) = context.Parameters;

        var sourceText = attributes.AllUnionTypeAttributes
            .Aggregate(new StringBuilder(), (b, a) =>
            {
                var accessibility = context.Parameters.GetSpecificAccessibility(a);

                _ = b.Append(accessibility).Append(' ').Append(symbol.Name).Append('(').AppendSymbol(a.RepresentableTypeSymbol).AppendLine(" value){");

                if(attributes.AllUnionTypeAttributes.Count > 1)
                    _ = b.Append("__tag = Tag.").Append(a.SafeAlias).AppendLine(";");

                var result = (a.RepresentableTypeSymbol.IsValueType ?
                    b.AppendLine("__valueTypeContainer = new(value);") :
                    b.AppendLine("__referenceTypeContainer = value;"))
                    .AppendLine("}");

                return result;
            }).ToString();

        var result = new ConstructorsModel(sourceText);

        return result;
    }
}
