namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;

readonly struct LayoutModel
{
    public readonly String SourceText;
    private LayoutModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
        => provider.SelectCarry(Create, Integrate);

    static void Integrate(ModelIntegrationContext<LayoutModel> context) =>
        context.Source.SetLayout(context.Model);

    static LayoutModel Create(ModelCreationContext context)
    {
        var sourceText = context.Parameters.Attributes.Settings.Layout == LayoutSetting.Small ?
            "[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]" :
            String.Empty;
        var result = new LayoutModel(sourceText);

        return result;
    }
}
