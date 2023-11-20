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

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<EqualsFunctionsModel> context) =>
        context.Source.SetEqualsFunctions(context.Model);

    private static EqualsFunctionsModel Create(ModelCreationContext context)
    {
        var target = context.Parameters.TargetSymbol;
        var attributes = context.Parameters.Attributes;

        var sourceTextBuilder = new StringBuilder("public override Boolean Equals(Object obj) => obj is ")
            .AppendOpen(target).AppendLine(" union && Equals(union);")
            .Append("public Boolean Equals(")
            .AppendOpen(target).AppendLine(" obj) =>");

        if(target.IsReferenceType)
        {
            _ = sourceTextBuilder.Append(" obj != null && ");
        }

        if(attributes.AllUnionTypeAttributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine(" __tag == obj.__tag && __tag switch{");
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
                .AppendFull(attribute)
                .Append(">.Default.Equals(")
                .Append(attribute.GetInstanceVariableExpression(target))
                .Append(',')
                .Append(attribute.GetInstanceVariableExpression(target, "obj"))
                .Append(')');
        }
    }
}
