﻿namespace RhoMicro.Unions.Generator.Models;

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
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

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
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(").AppendFull(a).AppendLine("),"))
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
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(").AppendFull(a).AppendLine("),"))
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
                    (b, a) => b.Append(a.TagValueExpression).AppendLine(" => typeof(")
                    .Append(ConstantSources.GenericFactoryIsAsType)
                    .Append(") == typeof(")
                    .AppendFull(a).Append(")?").Append(a.GetConvertedInstanceVariableExpression(target, ConstantSources.GenericFactoryIsAsType))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})")).AppendLine(","))
                .AppendLine("_ => ").Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})"))
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(") == typeof(")
                .AppendFull(attributes[0])
                .AppendLine(")?").Append(attributes[0].GetConvertedInstanceVariableExpression(target, ConstantSources.GenericFactoryIsAsType))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow($"nameof({ConstantSources.GenericFactoryIsAsType})")).AppendLine(";");
        }
#pragma warning restore IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendAggregate(
                 attributes,
                 (b, a) => b.AppendLine("/// <summary>")
                 .Append("/// Gets a value indicating whether this instance is representing a value of type <c>")
                 .Append(a.RepresentableTypeSymbol.ToDocCompatString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(a.SafeAlias)
                 .Append(" => __tag == ").Append(a.TagValueExpression).AppendLine(";")

                 .AppendLine("/// <summary>")
                 .Append("/// Attempts to retrieve the value represented by this instance as a <c>")
                 .Append(a.RepresentableTypeSymbol.ToDocCompatString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("/// <exception cref=\"global::System.InvalidOperationException\">Thrown if the instance is not representing a value of type <c>")
                 .Append(a.RepresentableTypeSymbol.ToDocCompatString()).AppendLine("</c>.</exception>")
                 .Append("public ").AppendFull(a)
                 .Append(" As").Append(a.SafeAlias)
                 .Append(" => __tag == ").Append(a.TagValueExpression).Append('?')
                 .Append(a.GetInstanceVariableExpression(target)).Append(':')
                 .Append(ConstantSources.InvalidConversionThrow($"typeof({a.RepresentableTypeSymbol.ToFullString()}).Name")).AppendLine(";"));
        } else
        {
            var attribute = attributes[0];

            _ = sourceTextBuilder.AppendLine("/// <summary>")
                 .Append("/// Gets a value indicating whether this instance is representing a value of type <c>")
                 .Append(attribute.RepresentableTypeSymbol.ToDocCompatString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(attribute.SafeAlias)
                 .AppendLine(" => true;")

                 .AppendLine("/// <summary>")
                 .Append("/// Retrieves the value represented by this instance as a <c>")
                 .Append(attribute.RepresentableTypeSymbol.ToDocCompatString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public ").AppendFull(attribute)
                 .Append(" As").Append(attribute.SafeAlias)
                 .Append(" => ").Append(attribute.GetInstanceVariableExpression(target)).AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new IsAsFunctionsModel(sourceText);

        return result;
    }
}
