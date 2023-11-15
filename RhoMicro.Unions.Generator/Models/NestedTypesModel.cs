﻿namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct NestedTypesModel
{
    public readonly String SourceText;
    private NestedTypesModel(String sourceText) => SourceText = sourceText;
    public static void Integrate(ModelIntegrationContext<NestedTypesModel> context) =>
        context.Source.SetNestedTypes(context.Model);

    public static NestedTypesModel Create(ModelFactoryInvocationContext context)
    {
        var sourceTextBuilder = new StringBuilder();

        var attributes = context.Parameters.Attributes;

        if(attributes.AllUnionTypeAttributes.Count > 1)
        {
            _ = sourceTextBuilder.Append("private enum Tag : Byte {")
                .Append(String.Join(",", attributes.AllUnionTypeAttributes
                    .OrderBy(a => a.RepresentableTypeSymbol.IsReferenceType)
                    .Select(a => a.SafeAlias)))
                .Append('}');
        }

        if(attributes.ValueTypeAttributes.Count > 0)
        {
            _ = sourceTextBuilder.AppendLine("[StructLayout(LayoutKind.Explicit)]")
                .AppendLine("private struct ValueTypeContainer")
                .AppendLine("{");

            _ = attributes.ValueTypeAttributes
                .Aggregate(
                    sourceTextBuilder,
                    (b, a) => b.AppendLine("[FieldOffset(0)]")
                        .Append("public ")
                        .Append(a.RepresentableTypeSymbol.ToFullString())
                        .Append(' ')
                        .Append(a.SafeAlias)
                        .AppendLine(";"))
                .AppendLine("}");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new NestedTypesModel(sourceText);

        return result;
    }
}