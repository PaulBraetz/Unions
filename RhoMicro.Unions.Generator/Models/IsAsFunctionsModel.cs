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
    public static void Integrate(ModelIntegrationContext<IsAsFunctionsModel> context) =>
        context.Source.SetIsAsFunctions(context.Model);

    public static IsAsFunctionsModel Create(ModelFactoryInvocationContext context)
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
                    (b, a) => b.Append(a.TagValueExpression).Append(" => typeof(").Append(a.RepresentableTypeSymbol.ToFullString()).AppendLine("),"))
                .AppendLine("_ => ").Append(ConstantSources.InvalidTagStateThrow)
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(T) == typeof(")
                .Append(attributes[0].RepresentableTypeSymbol.ToFullString())
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
                    .Append(a.RepresentableTypeSymbol.ToFullString()).Append(")?").Append(a.GetConvertedInstanceVariableExpression(target, "T"))
                    .Append(':').Append(ConstantSources.InvalidConversionThrow("typeof(T).Name")).AppendLine(","))
                .AppendLine("_ => ").Append(ConstantSources.InvalidConversionThrow("typeof(T).Name"))
                .AppendLine("};");
        } else
        {
            _ = sourceTextBuilder.Append("typeof(T) == typeof(")
                .Append(attributes[0].RepresentableTypeSymbol.ToFullString())
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
                 .Append(a.RepresentableTypeSymbol.ToFullString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(a.SafeAlias)
                 .Append(" => __tag == ").Append(a.TagValueExpression).AppendLine(";")

                 .AppendLine("/// <summary>")
                 .Append("/// Attempts to retrieve the value represented by this instance as a <c>")
                 .Append(a.RepresentableTypeSymbol.ToFullString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("/// <exception cref=\"global::System.InvalidOperationException\">Thrown if the instance is not representing a value of type <c>")
                 .Append(a.RepresentableTypeSymbol.ToFullString()).AppendLine("</c>.</exception>")
                 .Append("public ").Append(a.RepresentableTypeSymbol.ToFullString())
                 .Append(" As").Append(a.SafeAlias)
                 .Append(" => __tag == ").Append(a.TagValueExpression).Append('?')
                 .Append(a.GetInstanceVariableExpression(target)).Append(':')
                 .Append(ConstantSources.InvalidConversionThrow($"typeof({a.RepresentableTypeSymbol.ToFullString()}).Name")).AppendLine(";"));
        } else
        {
            var attribute = attributes[0];

            _ = sourceTextBuilder.AppendLine("/// <summary>")
                 .Append("/// Gets a value indicating whether this instance is representing a value of type <c>")
                 .Append(attribute.RepresentableTypeSymbol.ToFullString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public global::System.Boolean Is").Append(attribute.SafeAlias)
                 .AppendLine(" => true;")

                 .AppendLine("/// <summary>")
                 .Append("/// Retrieves the value represented by this instance as a <c>")
                 .Append(attribute.RepresentableTypeSymbol.ToFullString()).AppendLine("</c>.")
                 .AppendLine("/// </summary>")
                 .Append("public ").Append(attribute.RepresentableTypeSymbol.ToFullString())
                 .Append(" As").Append(attribute.SafeAlias)
                 .Append(" => ").Append(attribute.GetInstanceVariableExpression(target)).AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new IsAsFunctionsModel(sourceText);

        return result;
    }
}
