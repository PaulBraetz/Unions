namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;
using RhoMicro.Unions.Generator;

using System;
using System.Linq;
using System.Text;

readonly struct InterfaceImplementationModel
{
    private InterfaceImplementationModel(String sourceText) => SourceText = sourceText;
    public readonly String SourceText;

    public static void Integrate(ModelIntegrationContext<InterfaceImplementationModel> context) =>
        context.Source.SetInterfaceImplementation(context.Model);

    public static InterfaceImplementationModel Create(ModelFactoryInvocationContext context)
    {
        var attributes = context.Parameters.Attributes;
        var target = context.Parameters.TargetSymbol;

        var sourceTextBuilder = attributes.AllUnionTypeAttributes
            .Select((a, i) => (Name: a.RepresentableTypeSymbol.ToFullString(), Index: i))
            .Aggregate(
                new StringBuilder(": global::RhoMicro.Unions.Abstractions.IUnion<"),
                (b, n) => b.Append(n.Name).Append(n.Index != attributes.AllUnionTypeAttributes.Count - 1 ? "," : String.Empty))
            .AppendLine(">,")
            .Append("global::System.IEquatable<")
            .Append(target.Name)
            .Append('>');

        if(!(attributes.AllUnionTypeAttributes.Count == 1 && attributes.AllUnionTypeAttributes[0].Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary)))
        {
            _ = attributes.AllUnionTypeAttributes
                .Select(a => a.RepresentableTypeSymbol.ToFullString())
                .Aggregate(
                    sourceTextBuilder,
                    (b, n) => b.AppendLine(",")
                        .Append(" global::RhoMicro.Unions.Abstractions.ISuperset<")
                        .Append(n)
                        .Append(',')
                        .Append(target.Name)
                        .Append('>'));
        }

        var sourceText = sourceTextBuilder.ToString();

        var result = new InterfaceImplementationModel(sourceText);

        return result;
    }
}
