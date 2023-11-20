namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct ConversionOperatorsModel
{
    private readonly IEnumerable<ConversionOperatorModel> _models;

    private ConversionOperatorsModel(IEnumerable<ConversionOperatorModel> models) => _models = models;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<ConversionOperatorsModel> context) =>
        context.Source.SetOperators(context.Model._models);

    private static ConversionOperatorsModel Create(ModelCreationContext context)
    {
        var parameters = context.Parameters;
        var omissions = context.Parameters.OperatorOmissions.AllOmissions;
        var models = parameters.Attributes.AllUnionTypeAttributes
            .Where(a => !omissions.Contains(a))
            .Select(attribute => ConversionOperatorModel.Create(attribute, parameters));

        var result = new ConversionOperatorsModel(models);

        return result;
    }
}

readonly struct ConversionOperatorModel
{
    public readonly String SourceText;

    private ConversionOperatorModel(String sourceText) => SourceText = sourceText;

    public static ConversionOperatorModel Create(UnionTypeAttribute attribute, TargetDataModel data)
    {
        var target = data.TargetSymbol;
        var allAttributes = data.Attributes.AllUnionTypeAttributes;

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <summary>")
            .Append("/// Converts an instance of ").AppendCommentRef(attribute).Append(" to the union type ").AppendCommentRef(data).AppendLine("\"/>.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"value\">The value to convert.</param>")
            .AppendLine("/// <returns>The converted value.</returns>")
            .Append("public static implicit operator ")
            .AppendOpen(target)
            .Append('(')
            .AppendFull(attribute)
            .AppendLine(" value) => new(value);");

        var generateSolitaryExplicit = allAttributes.Count > 1 || !attribute.Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary);
        if(generateSolitaryExplicit)
        {
            _ = sourceTextBuilder
                .Append("public static explicit operator ")
                .AppendFull(attribute)
                .Append('(')
                .AppendOpen(target)
                .Append(" union) => ");

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append("union.__tag == Tag.")
                    .Append(attribute.SafeAlias)
                    .Append('?');
            }

            _ = sourceTextBuilder.Append(attribute.GetInstanceVariableExpression(target, "union"));

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append(':')
                    .Append(ConstantSources.InvalidConversionThrow($"typeof({attribute.RepresentableTypeSymbol.ToFullString()}).Name"));
            }

            _ = sourceTextBuilder.Append(';');
        } else
        {
            _ = sourceTextBuilder.Append("public static implicit operator ")
                .AppendFull(attribute)
                .Append('(')
                .AppendOpen(target)
                .Append(" union) => ")
                .Append(attribute.GetInstanceVariableExpression(target, "union"))
                .AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }
}