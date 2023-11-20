namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Formatting;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct IsAsFunctionsModel
{
    public readonly String SourceText;
    private IsAsFunctionsModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<IsAsFunctionsModel> context) =>
        context.Source.SetIsAsFunctions(context.Model);

    static IsAsFunctionsModel Create(ModelCreationContext context)
    {
        var attributes = context.TargetData.Annotations.AllRepresentableTypes;
        var target = context.TargetData.TargetSymbol;

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <inheritdoc/>")
            .Append("public global::System.Boolean Is<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(">() => ");
#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("typeof(")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(") == __tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.CorrespondingTag).Append(" => typeof(").AppendFull(a).AppendLine("),"))
                .AppendLine("_ => ").Append(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(") == typeof(")
                .AppendFull(attributes[0])
                .AppendLine(");");
        }

        _ = sourceTextBuilder
            .AppendLine("/// <inheritdoc/>")
            .Append("public global::System.Boolean Is(Type type) => ");
#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("type == __tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.CorrespondingTag).Append(" => typeof(").AppendFull(a).AppendLine("),"))
                .AppendLine("_ => ").Append(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("type == typeof(")
                .AppendFull(attributes[0])
                .AppendLine(");");
        }

        _ = sourceTextBuilder.AppendLine("/// <inheritdoc/>")
            .Append("public ")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(" As<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(">() => ");

        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("__tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.CorrespondingTag).AppendLine(" => typeof(")
                    .Append(ConstantSources.GenericFactoryIsAsType)
                    .Append(") == typeof(")
                    .AppendFull(a).Append(")?").Append(a.Storage.GetConvertedInstanceVariableExpression(ConstantSources.GenericFactoryIsAsType))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})")).AppendLine(","))
                .AppendLine("_ => ").Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})"))
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(") == typeof(")
                .AppendFull(attributes[0])
                .AppendLine(")?").Append(attributes[0].Storage.GetConvertedInstanceVariableExpression(ConstantSources.GenericFactoryIsAsType))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})")).AppendLine(";");
        }
#pragma warning restore IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendAggregate(
                 attributes,
                 (b, a) => b.AppendLine("/// <summary>")
                 .Append("/// Gets a value indicating whether this instance is representing a value of type ")
                 .Append(a.DocCommentRef).AppendLine(".")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(a.Names.SafeAlias)
                 .Append(" => __tag == ").Append(a.CorrespondingTag).AppendLine(";")

                 .AppendLine("/// <summary>")
                 .Append("/// Attempts to retrieve the value represented by this instance as a ")
                 .Append(a.DocCommentRef).AppendLine(".")
                 .AppendLine("/// </summary>")
                 .Append("/// <exception cref=\"global::System.InvalidOperationException\">Thrown if the instance is not representing a value of type ")
                 .Append(a.DocCommentRef).AppendLine(".</exception>")
                 .Append("public ").AppendFull(a)
                 .Append(" As").Append(a.Names.SafeAlias)
                 .Append(" => __tag == ").Append(a.CorrespondingTag).Append('?')
                 .Append(a.Storage.GetInstanceVariableExpression()).Append(':')
                 .Append(ConstantSources.InvalidConversionThrow($"typeof({a.Names.FullTypeName}).Name")).AppendLine(";"));
        } else
        {
            var attribute = attributes[0];

            _ = sourceTextBuilder.AppendLine("/// <summary>")
                 .Append("/// Gets a value indicating whether this instance is representing a value of type <c>")
                 .Append(attribute.DocCommentRef).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(attribute.Names.SafeAlias)
                 .AppendLine(" => true;")

                 .AppendLine("/// <summary>")
                 .Append("/// Retrieves the value represented by this instance as a <c>")
                 .Append(attribute.DocCommentRef).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public ").AppendFull(attribute)
                 .Append(" As").Append(attribute.Names.SafeAlias)
                 .Append(" => ").Append(attribute.Storage.GetInstanceVariableExpression()).AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new IsAsFunctionsModel(sourceText);

        return result;
    }
}
