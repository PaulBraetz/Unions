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

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
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

    public static ConversionOperatorModel Create(UnionTypeAttribute attribute, ModelFactoryParameters parameters)
    {
        var target = parameters.TargetSymbol;
        var allAttributes = parameters.Attributes.AllUnionTypeAttributes;

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <summary>")
            .Append("/// Converts an instance of <see cref=\"").AppendSymbol(attribute.RepresentableTypeSymbol).Append("\"/> to the union type <see cref=\"").AppendSymbol(target).AppendLine("\"/>.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"value\">The value to convert.</param>")
            .AppendLine("/// <returns>The converted value.</returns>")
            .Append("public static implicit operator ")
            .AppendSymbol(target)
            .Append('(')
            .AppendSymbol(attribute.RepresentableTypeSymbol)
            .AppendLine(" value) => new(value);");

        var generateSolitaryExplicit = allAttributes.Count > 1 || !attribute.Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary);
        if(generateSolitaryExplicit)
        {
            _ = sourceTextBuilder
                .Append("public static explicit operator ")
                .AppendSymbol(attribute.RepresentableTypeSymbol)
                .Append('(')
                .AppendSymbol(target)
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
                .AppendSymbol(attribute.RepresentableTypeSymbol)
                .Append('(')
                .Append(target.Name)
                .Append(" union) => ")
                .Append(attribute.GetInstanceVariableExpression(target, "union"))
                .AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }
}