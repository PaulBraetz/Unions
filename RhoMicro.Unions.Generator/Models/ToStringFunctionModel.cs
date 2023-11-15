namespace RhoMicro.Unions.Generator.Models;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct ToStringFunctionModel : IEquatable<ToStringFunctionModel>
{
    private ToStringFunctionModel(String sourceText) => SourceText = sourceText;

    public readonly String SourceText;

    public static void Integrate(ModelIntegrationContext<ToStringFunctionModel> context) =>
        context.Source.SetToStringFunction(context.Model);

    public static ToStringFunctionModel Create(ModelFactoryInvocationContext context)
    {
        var attributes = context.Parameters.Attributes;

        var inflectionModel = TagInflectionValueModel.Create(attributes);

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("#nullable enable")
            .Append("public override String? ToString() => ");

        if(attributes.AllUnionTypeAttributes.Count == 1)
        {
            _ = attributes.AllUnionTypeAttributes[0].RepresentableTypeSymbol.IsValueType
                ? sourceTextBuilder.Append("__valueTypeContainer.").Append(attributes.AllUnionTypeAttributes[0].SafeAlias).AppendLine(".ToString();")
                : sourceTextBuilder.Append("__referenceTypeContainer?.ToString();");
        } else
        {
            _ = sourceTextBuilder.Append("__tag < ")
                .Append(inflectionModel.SourceText)
                .Append(" ? __referenceTypeContainer?.ToString() : __tag switch{");

            _ = attributes.ReferenceTypeAttributes
                .Select(a => a.SafeAlias)
                .Aggregate(sourceTextBuilder, (b, a) => b.Append("Tag.").Append(a).Append(" => __referenceTypeContainer?.ToString(),"));

            _ = attributes.ValueTypeAttributes
                .Select(a => a.SafeAlias)
                .Aggregate(sourceTextBuilder, (b, a) => b.Append("Tag.").Append(a).Append(" => __valueTypeContainer.").Append(a).AppendLine(".ToString(),"))
                .Append("_ => ").AppendLine(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};")
                .AppendLine("#nullable restore");
        }

        var sourceText = sourceTextBuilder.ToString();

        var result = new ToStringFunctionModel(sourceText);

        return result;
    }

    public override Boolean Equals(Object obj) => obj is ToStringFunctionModel model && Equals(model);
    public Boolean Equals(ToStringFunctionModel other) => SourceText == other.SourceText;
    public override Int32 GetHashCode() => -1893336041 + EqualityComparer<String>.Default.GetHashCode(SourceText);

    public static Boolean operator ==(ToStringFunctionModel left, ToStringFunctionModel right) => left.Equals(right);
    public static Boolean operator !=(ToStringFunctionModel left, ToStringFunctionModel right) => !(left == right);
}
