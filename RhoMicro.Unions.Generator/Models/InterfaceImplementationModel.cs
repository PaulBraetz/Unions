namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct InterfaceImplementationModel
{
    private InterfaceImplementationModel(String sourceText) => SourceText = sourceText;

    public readonly String SourceText;

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<InterfaceImplementationModel> context) => 
        context.Source.SetInterfaceImplementation(context.Model);

    static InterfaceImplementationModel Create(ModelCreationContext context)
    {
        var attributes = context.Parameters.Attributes;
        var target = context.Parameters.TargetSymbol;
        var targetDeclaration = context.Parameters.TargetDeclaration;

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
            var omissions = context.Parameters.OperatorOmissions.AllOmissions;
            _ = attributes.AllUnionTypeAttributes
                .Where(a => !omissions.Contains(a))
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
