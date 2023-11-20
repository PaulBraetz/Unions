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
        var parameters = context.TargetData;
        var omissions = context.TargetData.OperatorOmissions.AllOmissions;
        var models = parameters.Annotations.AllRepresentableTypes
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

    public static ConversionOperatorModel Create(
        RepresentableTypeData representableType,
        TargetDataModel data)
    {
        var target = data.TargetSymbol;
        var allAttributes = data.Annotations.AllRepresentableTypes;

        if(representableType.Attribute.RepresentableTypeIsGenericParameter &&
           !representableType.Attribute.Options.HasFlag(UnionTypeOptions.SupersetOfParameter))
        {
            return new(String.Empty);
        }

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <summary>")
            .Append("/// Converts an instance of ").AppendCommentRef(representableType)
            .Append(" to the union type ").AppendCommentRef(data).AppendLine("\"/>.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"value\">The value to convert.</param>")
            .AppendLine("/// <returns>The converted value.</returns>")
            .Append("public static implicit operator ")
            .AppendOpen(target)
            .Append('(')
            .AppendFull(representableType)
            .AppendLine(" value) => new(value);");

        var generateSolitaryExplicit =
            allAttributes.Count > 1 ||
            !representableType.Attribute.Options.HasFlag(
                UnionTypeOptions.ImplicitConversionIfSolitary);
        if(generateSolitaryExplicit)
        {
            _ = sourceTextBuilder
                .Append("public static explicit operator ")
                .AppendFull(representableType)
                .Append('(')
                .AppendOpen(target)
                .Append(" union) => ");

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append("union.__tag == Tag.")
                    .Append(representableType.Names.SafeAlias)
                    .Append('?');
            }

            _ = sourceTextBuilder.Append(
                representableType.Storage.GetInstanceVariableExpression("union"));

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append(':')
                    .Append(ConstantSources.InvalidConversionThrow(
                        $"typeof({representableType.Names.FullTypeName}).Name"));
            }

            _ = sourceTextBuilder.Append(';');
        } else
        {
            _ = sourceTextBuilder.Append("public static implicit operator ")
                .AppendFull(representableType)
                .Append('(')
                .AppendOpen(target)
                .Append(" union) => ")
                .Append(representableType.Storage.GetInstanceVariableExpression("union"))
                .AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }
}