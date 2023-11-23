﻿namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

readonly struct DownCastFunctionModel
{
    public readonly String SourceText;
    private DownCastFunctionModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<DownCastFunctionModel> context) =>
            context.Source.SetDownCastFunction(context.Model);
    private static DownCastFunctionModel Create(ModelCreationContext context)
    {
        var representableTypes = context.TargetData.Annotations.AllRepresentableTypes;
        var target = context.TargetData.TargetSymbol;
        var settings = context.TargetData.Annotations.Settings;

        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// </inheritdoc>")
            .Append("public ")
            .Append(settings.GenericTResultName)
            .Append(" DownCast<")
            .Append(settings.GenericTResultName)
            .Append(">()")
            .AppendLine(" where ")
            .Append(settings.GenericTResultName)
            .Append(" : global::RhoMicro.Unions.Abstractions.IUnion<")
            .Append(settings.GenericTResultName)
            .Append(',')
            .AppendAggregateJoin(
                ",",
                representableTypes,
                (b, a) => b.AppendFull(a))
            .Append('>');

#pragma warning disable IDE0045 // Convert to conditional expression
        if(representableTypes.Count == 1)
        {
            _ = sourceTextBuilder.Append(" => ")
                .Append(settings.GenericTResultName)
                .Append(".Create<").AppendFull(representableTypes[0])
                .Append(">(").Append(representableTypes[0].Storage.GetInstanceVariableExpression()).Append(')');
        } else
        {
            _ = representableTypes.Aggregate(
                sourceTextBuilder.Append(" => __tag switch{"),
                (b, a) => b.Append(a.CorrespondingTag)
                    .Append(" => ")
                    .Append(settings.GenericTResultName)
                    .Append(".Create<").AppendFull(a)
                    .Append(">(").Append(a.Storage.GetInstanceVariableExpression()).Append(')')
                    .AppendLine(","))
                .Append("_ => ")
                .Append(ConstantSources.InvalidTagStateThrow)
                .Append('}');
        }
#pragma warning restore IDE0045 // Convert to conditional expression

        var sourceText = sourceTextBuilder.Append(';').ToString();
        var result = new DownCastFunctionModel(sourceText);

        return result;
    }
}
