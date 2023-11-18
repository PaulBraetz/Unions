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

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<DownCastFunctionModel> context) =>
            context.Source.SetDownCastFunction(context.Model);
    private static DownCastFunctionModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = new StringBuilder("public TSuperset DownCast<TSuperset>()")
            .AppendLine(" where TSuperset : global::RhoMicro.Unions.Abstractions.IUnion<TSuperset,")
            .AppendAggregateJoin(
                ",",
                attributes,
                (b, a) => b.AppendSymbol(a.RepresentableTypeSymbol))
            .Append('>');

#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append("=> TSuperset.Create<").AppendSymbol(attributes[0].RepresentableTypeSymbol)
                .Append(">(").Append(attributes[0].GetInstanceVariableExpression(target)).Append(')');
        } else
        {
            _ = attributes.Aggregate(
                sourceTextBuilder.Append(" => __tag switch{"),
                (b, a) => b.Append(a.TagValueExpression)
                    .Append(" => ")
                    .Append("TSuperset.Create<").AppendSymbol(a.RepresentableTypeSymbol)
                    .Append(">(").Append(a.GetInstanceVariableExpression(target)).Append(')')
                    .AppendLine(","))
                .Append("_ => ")
                .Append(ConstantSources.InvalidTagStateThrow)
                .Append("}");
        }
#pragma warning restore IDE0045 // Convert to conditional expression

        var sourceText = sourceTextBuilder.Append(';').ToString();
        var result = new DownCastFunctionModel(sourceText);

        return result;
    }
}
