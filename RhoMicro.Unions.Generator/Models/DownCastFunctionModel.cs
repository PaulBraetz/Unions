namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

readonly struct DownCastFunctionModel
{
    public readonly String SourceText;
    private DownCastFunctionModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<DownCastFunctionModel> context) =>
            context.Source.SetDownCastFunction(context.Model);
    private static DownCastFunctionModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = new StringBuilder("public")
            .Append(' ')
            .Append(ConstantSources.GenericTResultType)
            .Append(" DownCast<")
            .Append(ConstantSources.GenericTResultType)
            .Append(">()")
            .AppendLine(" where ")
            .Append(ConstantSources.GenericTResultType)
            .Append(" : global::RhoMicro.Unions.Abstractions.IUnion<")
            .Append(ConstantSources.GenericTResultType)
            .Append(',')
            .AppendAggregateJoin(
                ",",
                attributes,
                (b, a) => b.AppendFull(a))
            .Append('>');

#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append(" => ")
                .Append(ConstantSources.GenericTResultType)
                .Append(".Create<").AppendFull(attributes[0])
                .Append(">(").Append(attributes[0].GetInstanceVariableExpression(target)).Append(')');
        } else
        {
            _ = attributes.Aggregate(
                sourceTextBuilder.Append(" => __tag switch{"),
                (b, a) => b.Append(a.TagValueExpression)
                    .Append(" => ")
                    .Append(ConstantSources.GenericTResultType)
                    .Append(".Create<").AppendFull(a)
                    .Append(">(").Append(a.GetInstanceVariableExpression(target)).Append(')')
                    .AppendLine(","))
                .Append("_ => ")
                .Append(ConstantSources.InvalidTagStateThrow)
                .Append('}');
        }
#pragma warning restore IDE0045 // Convert to conditional expression

        var sourceText = sourceTextBuilder.Append(';').ToString();
        var result = new DownCastFunctionModel(sourceText);

        return result;
    }
}
