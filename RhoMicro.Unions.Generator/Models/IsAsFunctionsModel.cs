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

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<IsAsFunctionsModel> context) =>
        context.Source.SetIsAsFunctions(context.Model);

    static IsAsFunctionsModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <inheritdoc/>")
            .Append("public global::System.Boolean Is<T>() => ");
#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("typeof(T) == __tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(").AppendSymbol(a.RepresentableTypeSymbol).AppendLine("),"))
                .AppendLine("_ => ").Append(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(T) == typeof(")
                .AppendSymbol(attributes[0].RepresentableTypeSymbol)
                .AppendLine(");");
        }
       
        _=sourceTextBuilder
            .AppendLine("/// <inheritdoc/>")
            .Append("public global::System.Boolean Is(Type type) => ");
#pragma warning disable IDE0045 // Convert to conditional expression
        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("type == __tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(").AppendSymbol(a.RepresentableTypeSymbol).AppendLine("),"))
                .AppendLine("_ => ").Append(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("type == typeof(")
                .AppendSymbol(attributes[0].RepresentableTypeSymbol)
                .AppendLine(");");
        }

        _ = sourceTextBuilder.AppendLine("/// <inheritdoc/>")
            .Append("public T As<T>() => ");

        if(attributes.Count > 1)
        {
            _ = sourceTextBuilder.AppendLine("__tag switch {")
                .AppendAggregate(
                    attributes,
                    (b, a) => b.Append(a.TagValueExpression).AppendLine(" => typeof(T) == typeof(")
                    .AppendSymbol(a.RepresentableTypeSymbol).Append(")?").Append(a.GetConvertedInstanceVariableExpression(target, "T"))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow("typeof(T).Name")).AppendLine(","))
                .AppendLine("_ => ").Append(ConstantSources.InvalidConversionThrow("typeof(T).Name"))
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(T) == typeof(")
                .AppendSymbol(attributes[0].RepresentableTypeSymbol)
                .AppendLine(")?").Append(attributes[0].GetConvertedInstanceVariableExpression(target, "T"))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow("typeof(T).Name")).AppendLine(";");
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
                 .Append("public ").AppendSymbol(a.RepresentableTypeSymbol)
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
                 .Append("public ").AppendSymbol(attribute.RepresentableTypeSymbol)
                 .Append(" As").Append(attribute.SafeAlias)
                 .Append(" => ").Append(attribute.GetInstanceVariableExpression(target)).AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new IsAsFunctionsModel(sourceText);

        return result;
    }
}
