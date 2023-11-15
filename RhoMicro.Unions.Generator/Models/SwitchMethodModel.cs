namespace RhoMicro.Unions.Generator.Models;
using RhoMicro.Unions.Generator;

using System;
using System.Linq;
using System.Text;

/*
*/

readonly struct SwitchMethodModel
{
    public readonly String SourceText;
    private SwitchMethodModel(String sourceText) => SourceText = sourceText;

    public static void Integrate(ModelIntegrationContext<SwitchMethodModel> context) =>
        context.Source.SetSwitchMethod(context.Model);
    public static SwitchMethodModel Create(ModelFactoryInvocationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = attributes
            .Select((a, i) => (Attribute: a, Index: i))
            .Aggregate(
                new StringBuilder("public void Switch("),
                (b, t) => b.Append("global::System.Action<")
                    .Append(t.Attribute.RepresentableTypeSymbol.ToFullString())
                    .Append("> on")
                    .Append(t.Attribute.SafeAlias)
                    .AppendLine(t.Index == attributes.Count - 1 ? String.Empty : ","))
            .Append("){");

        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append("on")
                .Append(attributes[0].SafeAlias)
                .Append(".Invoke(")
                .Append(attributes[0].GetInstanceVariableExpression(target))
                .AppendLine(");");
        } else
        {
            _ = sourceTextBuilder.AppendLine("switch(__tag){");
            _ = attributes.Aggregate(
                sourceTextBuilder,
                (b, a) => b.Append("case ")
                    .Append(a.TagValueExpression)
                    .AppendLine(":")
                    .Append("on").Append(a.SafeAlias).Append(".Invoke(")
                    .Append(a.GetInstanceVariableExpression(target)).AppendLine(");return;"))
                .AppendLine("default:")
                .Append(ConstantSources.InvalidTagStateThrow).AppendLine(";}");
        }

        var sourceText = sourceTextBuilder.Append('}').ToString();
        var result = new SwitchMethodModel(sourceText);

        return result;
    }
}
