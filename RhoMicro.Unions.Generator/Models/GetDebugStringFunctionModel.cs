namespace RhoMicro.Unions.Generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct GetDebugStringFunctionModel
{
    public readonly String SourceText;
    private GetDebugStringFunctionModel(String sourceText) => SourceText = sourceText;

    public static void Integrate(ModelIntegrationContext<GetDebugStringFunctionModel> context) =>
        context.Source.SetGetDebugStringFunction(context.Model);
    public static GetDebugStringFunctionModel Create(ModelFactoryInvocationContext context)
    {
        var target = context.Parameters.TargetSymbol;
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;

        var sourceTextBuilder = new StringBuilder("private String GetDebugString() => $\"")
            .Append(target.Name)
            .Append('(');

        _ = attributes.Count == 1 ?
            sourceTextBuilder.Append('<').Append(attributes[0].RepresentableTypeSymbol.Name).Append('>') :
            sourceTextBuilder.AppendAggregateJoin(" | ", attributes, (b, a) =>
                    b.Append("{(").Append("__tag == ").Append(a.TagValueExpression).Append('?')
                    .Append("\"<").Append(a.RepresentableTypeSymbol.Name).Append(">\"").Append(':')
                    .Append('\"').Append(a.RepresentableTypeSymbol.Name).Append("\")}"));

        var sourceText = sourceTextBuilder
            .Append("){{{this}}}\";")
            .ToString();
        var result = new GetDebugStringFunctionModel(sourceText);

        //$"Union(Int32 | String | <Byte> | String){this}"

        return result;
    }
}
