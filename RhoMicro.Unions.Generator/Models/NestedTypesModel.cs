namespace RhoMicro.Unions.Generator.Models;

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

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<NestedTypesModel> context) =>
            context.Source.SetNestedTypes(context.Model);

    static NestedTypesModel Create(ModelCreationContext context)
    {
        var sourceTextBuilder = new StringBuilder();

        var attributes = context.Parameters.Attributes;

        if(attributes.AllUnionTypeAttributes.Count > 1)
        {
            _ = sourceTextBuilder.Append("private enum Tag : Byte {")
                .AppendJoin(',', attributes.AllUnionTypeAttributes.Select(a => a.SafeAlias))
                .Append('}');
        }

        if(attributes.ValueTypeAttributes.Count > 0)
        {
            _ = sourceTextBuilder.AppendLine("[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit)]")
                .AppendLine("private struct ValueTypeContainer")
                .AppendLine("{");

            _ = attributes.ValueTypeAttributes
                .Aggregate(
                    sourceTextBuilder,
                    (b, a) => b.AppendLine("[global::System.Runtime.InteropServices.FieldOffset(0)]")
                        .Append("public readonly ")
                        .Append(a.RepresentableTypeSymbol.ToFullString())
                        .Append(' ')
                        .Append(a.SafeAlias)
                        .AppendLine(";")
                        .Append("public ValueTypeContainer(").Append(a.RepresentableTypeSymbol.ToFullString()).Append(" value) => ")
                        .Append(a.SafeAlias).AppendLine(" = value;"))
                .AppendLine("}");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new NestedTypesModel(sourceText);

        return result;
    }
}
