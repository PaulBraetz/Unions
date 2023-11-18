namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using System;
using System.Linq;
using System.Text;

readonly struct MatchFunctionModel
{
    public readonly String SourceText;
    private MatchFunctionModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<MatchFunctionModel> context) =>
        context.Source.SetMatchFunction(context.Model);
    static MatchFunctionModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = attributes
            .Select((a, i) => (Attribute: a, Index: i))
            .Aggregate(
                new StringBuilder("public TResult Match<TResult>("),
                (b, t) => b.Append("global::System.Func<")
                    .AppendSymbol(t.Attribute.RepresentableTypeSymbol)
                    .Append(", TResult> on")
                    .Append(t.Attribute.SafeAlias)
                    .AppendLine(t.Index == attributes.Count - 1 ? String.Empty : ","))
            .AppendLine(") =>");

        if(attributes.Count == 1)
        {
            _ = sourceTextBuilder.Append("on")
                .Append(attributes[0].SafeAlias)
                .Append(".Invoke(")
                .Append(attributes[0].GetInstanceVariableExpression(target))
                .AppendLine(");");
        } else
        {
            _ = sourceTextBuilder.AppendLine("__tag switch{");
            _ = attributes.Aggregate(
                sourceTextBuilder,
                (b, a) => b.Append(a.TagValueExpression)
                    .AppendLine(" => ")
                    .Append("on").Append(a.SafeAlias).Append(".Invoke(")
                    .Append(a.GetInstanceVariableExpression(target)).AppendLine("),"))
                .AppendLine("_ =>")
                .AppendLine(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new MatchFunctionModel(sourceText);

        return result;
    }
}