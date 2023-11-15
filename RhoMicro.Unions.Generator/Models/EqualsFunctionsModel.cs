namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;
using RhoMicro.Unions.Generator;

using System;
using System.Linq;
using System.Text;

readonly struct EqualsFunctionsModel
{
    private EqualsFunctionsModel(String sourceText) => SourceText = sourceText;
    public readonly String SourceText;

    public static void Integrate(ModelIntegrationContext<EqualsFunctionsModel> context) =>
        context.Source.SetEqualsFunctions(context.Model);

    public static EqualsFunctionsModel Create(ModelFactoryInvocationContext context)
    {
        var target = context.Parameters.TargetSymbol;
        var attributes = context.Parameters.Attributes;

        var sourceTextBuilder = new StringBuilder("public override Boolean Equals(Object obj) => obj is ")
            .Append(target.Name).AppendLine(" union && Equals(union);")
            .Append("public Boolean Equals(")
            .Append(target.Name).AppendLine(" obj) =>");

        if(attributes.AllUnionTypeAttributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("__tag switch{");
            foreach(var attribute in attributes.AllUnionTypeAttributes)
            {
                _ = sourceTextBuilder.Append(attribute.TagValueExpression).Append(" => ");
                appendEqualityExpression(attribute);
                _ = sourceTextBuilder.AppendLine(",");
            }

            _ = sourceTextBuilder.Append('}');
        } else if(attributes.AllUnionTypeAttributes.Count == 1)
        {
            appendEqualityExpression(attributes.AllUnionTypeAttributes[0]);
        }

        var sourceText = sourceTextBuilder.AppendLine(";").ToString();
        var result = new EqualsFunctionsModel(sourceText);

        return result;

        void appendEqualityExpression(UnionTypeAttribute attribute)
        {
            _ = sourceTextBuilder.Append("EqualityComparer<")
                .Append(attribute.RepresentableTypeSymbol.ToFullString())
                .Append(">.Default.Equals(")
                .Append(attribute.GetInstanceVariableExpression(target))
                .Append(',')
                .Append(attribute.GetInstanceVariableExpression(target, "obj"))
                .Append(')');
        }
    }
}
