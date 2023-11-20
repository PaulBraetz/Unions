namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using System;
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
        var target = context.TargetData.TargetSymbol;

        var sourceText = new StringBuilder()
            .AppendAggregate(
                representableTypes,
                (b, a) => b.AppendLine(
                    $$"""
                    /// </inheritdoc>
                    public static {{target.ToOpenString()}} Create({{a.Names.FullTypeName}} value) => new(value);
                    """))
            .AppendLine("/// </inheritdoc>")
            .Append("public static Boolean TryCreate<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(">(")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(" value, out ").AppendOpen(target).AppendLine(" instance){switch(typeof(")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(")){")
            .AppendAggregate(
                representableTypes,
                (b, t) => b.Append("case Type ").Append(t.Names.SafeAlias).Append("Type when ")
                .Append(t.Names.SafeAlias).Append("Type == typeof(").AppendFull(t).Append("):")
                .AppendLine("instance = new(Util.UnsafeConvert<")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(',').AppendFull(t)
                .Append(">(value));return true;"))
            .AppendLine("default: instance = default; return false;}}")
            .AppendLine("/// </inheritdoc>")
            .Append("public static ").AppendOpen(target).AppendLine(" Create<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(">(")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(" value){switch(typeof(")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(")){")
            .AppendAggregate(
                representableTypes,
                (b, t) => b.Append("case Type ").Append(t.Names.SafeAlias).Append("Type when ")
                .Append(t.Names.SafeAlias).Append("Type == typeof(").AppendFull(t).Append("):")
                .AppendLine("return new(Util.UnsafeConvert<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(",").AppendFull(t).Append(">(value));"))
            .Append("default: ").Append(ConstantSources.InvalidCreationThrow($"\"{target.ToOpenString()}\"", "value"))
            .AppendLine(";}}")
            .ToString();

        var result = new FactoryFunctionsModel(sourceText);

        return result;
    }
}
