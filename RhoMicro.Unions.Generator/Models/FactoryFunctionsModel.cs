namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using System;
using System.Linq;
using System.Text;

readonly struct FactoryFunctionsModel
{
    public readonly String SourceText;
    private FactoryFunctionsModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>>
        Project(IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider)
        => provider.SelectCarry(Create, Integrate);

    private static void Integrate(ModelIntegrationContext<FactoryFunctionsModel> context) =>
        context.Source.SetFactoryFunctions(context.Model);

    private static FactoryFunctionsModel Create(ModelCreationContext context)
    {
        var representableTypes = context.TargetData.Annotations.AllRepresentableTypes;
        var target = context.TargetData.Symbol;
        var settings = context.TargetData.Annotations.Settings;

        var sourceText = new StringBuilder()
            .AppendAggregate(
                representableTypes,
                (b, a) => b.AppendLine(
                    $$"""
                    /// </inheritdoc>
                    public static {{target.ToOpenString()}} Create({{a.Names.FullTypeName}} value) => new(value);
                    /// <summary>
                    /// Creates a new instance of <see cref="{{target.ToDocCompatString()}}"/>.
                    /// </summary>
                    public static {{target.ToOpenString()}} {{a.Names.CreateFromFunctionName}}({{a.Names.FullTypeName}} value) => new(value);
                    """))
            .AppendLine("/// </inheritdoc>")
            .Append("public static Boolean TryCreate<")
            .Append(settings.GenericTValueName)
            .Append(">(")
            .Append(settings.GenericTValueName)
            .Append(" value, out ").AppendOpen(target).AppendLine(" instance){switch(typeof(")
            .Append(settings.GenericTValueName)
            .Append(")){")
            .AppendAggregate(
                representableTypes,
                (b, t) => b.Append("case Type ").Append(t.Names.SafeAlias).Append("Type when ")
                .Append(t.Names.SafeAlias).Append("Type == typeof(").AppendFull(t).Append("):")
                .Append("instance = new(")
                .Append(ConstantSources.UnsafeConvert(settings.GenericTValueName, t.Names.FullTypeName, "value"))
                .Append(')')
                .Append(";return true;"))
            .AppendLine("default: instance = default; return false;}}")
            .AppendLine("/// </inheritdoc>")
            .Append("public static ").AppendOpen(target).AppendLine(" Create<")
            .Append(settings.GenericTValueName)
            .Append(">(")
            .Append(settings.GenericTValueName)
            .Append(" value){switch(typeof(")
            .Append(settings.GenericTValueName)
            .Append(")){")
            .AppendAggregate(
                representableTypes,
                (b, t) => b.Append("case Type ").Append(t.Names.SafeAlias).Append("Type when ")
                .Append(t.Names.SafeAlias).Append("Type == typeof(").AppendFull(t).Append("):")
                .Append("return new(")
                .Append(ConstantSources.UnsafeConvert(settings.GenericTValueName, t.Names.FullTypeName, "value"))
                .Append(");"))
            .Append("default: ").Append(ConstantSources.InvalidCreationThrow($"\"{target.ToOpenString()}\"", "value"))
            .AppendLine(";}}")
            .ToString();

        var result = new FactoryFunctionsModel(sourceText);

        return result;
    }
}
