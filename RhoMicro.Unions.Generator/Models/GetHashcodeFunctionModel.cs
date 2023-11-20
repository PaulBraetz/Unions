namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Linq;
using System.Text;
readonly struct GetHashcodeFunctionModel
{
    private GetHashcodeFunctionModel(String sourceText) => SourceText = sourceText;
    public readonly String SourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<GetHashcodeFunctionModel> context) =>
        context.Source.SetGethashcodeFunction(context.Model);

    private static GetHashcodeFunctionModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = new StringBuilder("public override Int32 GetHashCode() =>");

        if(attributes.ReferenceTypeAttributes.Count > 0)
        {
            if(attributes.ValueTypeAttributes.Count > 0)
            {
                var inflectionValue = TagInflectionValueModel.Create(attributes).SourceText;
                _ = sourceTextBuilder.Append("__tag < ").Append(inflectionValue).Append('?');
            }

            _ = sourceTextBuilder.Append("__referenceTypeContainer?.GetHashCode() ?? 0");

            if(attributes.ValueTypeAttributes.Count > 0)
                _ = sourceTextBuilder.AppendLine(":");
        }

        if(attributes.ValueTypeAttributes.Count > 1)
        {
            _ = sourceTextBuilder.Append("__tag switch{");

            _ = attributes.ValueTypeAttributes.Aggregate(
                sourceTextBuilder,
                (b, a) => b.Append(a.TagValueExpression)
                    .Append(" => ")
                    .Append(a.GetInstanceVariableExpression(target))
                    .AppendLine(".GetHashCode(),"))
                .AppendLine("_ => throw new InvalidOperationException()}");
        } else if(attributes.ValueTypeAttributes.Count == 1)
        {
            _ = sourceTextBuilder.Append(attributes.ValueTypeAttributes[0].GetInstanceVariableExpression(target)).Append(".GetHashCode()");
        }

        var sourceText = sourceTextBuilder.Append(';').ToString();
        var result = new GetHashcodeFunctionModel(sourceText);

        return result;
    }
}