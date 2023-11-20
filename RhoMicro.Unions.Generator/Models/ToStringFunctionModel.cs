namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct ToStringFunctionModel : IEquatable<ToStringFunctionModel>
{
    private ToStringFunctionModel(String sourceText) => SourceText = sourceText;

    public readonly String SourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<ToStringFunctionModel> context) =>
        context.Source.SetToStringFunction(context.Model);

    private static ToStringFunctionModel CreateDetailed(ModelCreationContext context)
    {
        var target = context.Parameters.TargetSymbol;
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;

        var sourceTextBuilder = new StringBuilder("public override String ToString(){var stringRepresentation = ");

        AppendSimpleToStringExpression(context, sourceTextBuilder);

        _ = sourceTextBuilder
            .Append("; var result = $\"")
            .Append(target.Name)
            .Append('(');

        _ = attributes.Count == 1 ?
            sourceTextBuilder.Append('<').Append(attributes[0].RepresentableTypeSymbol.Name).Append('>') :
            sourceTextBuilder.AppendAggregateJoin(" | ", attributes, (b, a) =>
                    b.Append("{(").Append("__tag == ").Append(a.TagValueExpression).Append('?')
                    .Append("\"<").Append(a.SafeAlias).Append(">\"").Append(':')
                    .Append('\"').Append(a.SafeAlias).Append("\")}"));

        var sourceText = sourceTextBuilder
            .Append("){{{stringRepresentation}}}\"; return result;}")
            .ToString();
        var result = new ToStringFunctionModel(sourceText);

        return result;
    }
    private static ToStringFunctionModel CreateSimple(ModelCreationContext context)
    {
        var sourceTextBuilder = new StringBuilder()
            .AppendLine("#nullable enable")
            .Append("public override String? ToString() => ");

        AppendSimpleToStringExpression(context, sourceTextBuilder);

        var sourceText = sourceTextBuilder.AppendLine(";")
            .AppendLine("#nullable restore")
            .ToString();

        var result = new ToStringFunctionModel(sourceText);

        return result;
    }

    private static void AppendSimpleToStringExpression(ModelCreationContext context, StringBuilder sourceTextBuilder)
    {
        var attributes = context.Parameters.Attributes;

        if(attributes.AllUnionTypeAttributes.Count == 1)
        {
            _ = attributes.AllUnionTypeAttributes[0].RepresentableTypeSymbol.IsValueType
                ? sourceTextBuilder.Append("__valueTypeContainer.").Append(attributes.AllUnionTypeAttributes[0].SafeAlias).AppendLine(".ToString()")
                : sourceTextBuilder.Append("__referenceTypeContainer?.ToString()");
        } else
        {
            if(context.Parameters.Attributes.ReferenceTypeAttributes.Count > 0)
            {
                var inflectionModel = TagInflectionValueModel.Create(attributes);

                _ = sourceTextBuilder.Append("__tag < ")
                .Append(inflectionModel.SourceText)
                .Append(" ? __referenceTypeContainer?.ToString() : ");
            }

            _ = sourceTextBuilder.Append("__tag switch{");

            _ = attributes.ReferenceTypeAttributes
                .Select(a => a.SafeAlias)
                .Aggregate(sourceTextBuilder, (b, a) => b.Append("Tag.").Append(a).Append(" => __referenceTypeContainer?.ToString(),"));

            _ = attributes.ValueTypeAttributes
                .Select(a => a.SafeAlias)
                .Aggregate(sourceTextBuilder, (b, a) => b.Append("Tag.").Append(a).Append(" => __valueTypeContainer.").Append(a).AppendLine(".ToString(),"))
                .Append("_ => ").AppendLine(ConstantSources.InvalidTagStateThrow)
                .AppendLine("}");
        }
    }

    private static ToStringFunctionModel CreateOmitted(ModelCreationContext _)
    {
        var sourceText = String.Empty;
        var result = new ToStringFunctionModel(sourceText);

        return result;
    }

    public static ToStringFunctionModel Create(ModelCreationContext context)
    {
        var result = context.Parameters.Attributes.Settings.ToStringSetting switch
        {
            ToStringSetting.Simple => CreateSimple(context),
            ToStringSetting.Detailed => CreateDetailed(context),
            _ => CreateOmitted(context)
        };

        return result;
    }

    public override Boolean Equals(Object obj) => obj is ToStringFunctionModel model && Equals(model);
    public Boolean Equals(ToStringFunctionModel other) => SourceText == other.SourceText;
    public override Int32 GetHashCode() => -1893336041 + EqualityComparer<String>.Default.GetHashCode(SourceText);

    public static Boolean operator ==(ToStringFunctionModel left, ToStringFunctionModel right) => left.Equals(right);
    public static Boolean operator !=(ToStringFunctionModel left, ToStringFunctionModel right) => !(left == right);
}
