namespace RhoMicro.Unions.Generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

readonly struct DownCastFunctionModel
{
    public readonly String SourceText;
    private DownCastFunctionModel(String sourceText) => SourceText = sourceText;

    public static void Integrate(ModelIntegrationContext<DownCastFunctionModel> context) =>
        context.Source.SetDownCastFunction(context.Model);
    public static DownCastFunctionModel Create(ModelFactoryInvocationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = new StringBuilder("public TSuperset DownCast<TSuperset>()")
            .AppendLine(" where TSuperset :");

        _ = attributes
            .Select((a, i) => (Attribute: a, Suffix: i == attributes.Count - 1 ? attributes.Count > 1 ? " => __tag switch {" : " => " : ","))
            .Aggregate(
            sourceTextBuilder,
            static (b, t) => b.Append("global::RhoMicro.Unions.Abstractions.ISuperset<")
                .Append(t.Attribute.RepresentableTypeSymbol.ToFullString())
                .Append(", TSuperset>")
                .AppendLine(t.Suffix));

#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append(attributes[0].GetInstanceVariableExpression(target));
        } else
        {
            _ = attributes.Aggregate(
                sourceTextBuilder,
                (b, a) => b.Append(a.TagValueExpression)
                    .Append(" => ")
                    .Append(a.GetInstanceVariableExpression(target))
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
