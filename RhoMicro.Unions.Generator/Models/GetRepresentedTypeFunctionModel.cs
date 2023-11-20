namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Text;

readonly struct GetRepresentedTypeFunctionModel
{
    public readonly String SourceText;
    private GetRepresentedTypeFunctionModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<GetRepresentedTypeFunctionModel> context) =>
        context.Source.SetGetRepresentedTypeFunction(context.Model);

    static GetRepresentedTypeFunctionModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;

        var sourceTextBuilder = new StringBuilder("public Type GetRepresentedType() => ");

#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append("typeof(")
                .AppendFull(attributes[0])
                .AppendLine(");");
        } else
        {
            _ = sourceTextBuilder.AppendLine("__tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(")
                        .AppendFull(a).AppendLine("),"))
                .Append("_ => ").AppendLine(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        }
#pragma warning restore IDE0045 // Convert to conditional expression

        var sourceText = sourceTextBuilder.ToString();
        var result = new GetRepresentedTypeFunctionModel(sourceText);

        return result;
    }
}
