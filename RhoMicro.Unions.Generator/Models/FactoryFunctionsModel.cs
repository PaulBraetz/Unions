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
        var attributes = context.Parameters.Attributes.AllUnionTypeAttributes;
        var target = context.Parameters.TargetSymbol;

        var sourceText = new StringBuilder()
            .AppendAggregate(
                attributes,
                (b, a) => b.AppendLine(
                    $$"""
                    /// </inheritdoc>
                    public static {{target.ToOpenString()}} Create({{a.RepresentableTypeSymbol.ToFullString()}} value) => new(value);
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
                attributes,
                (b, a) => b.Append("case Type ").Append(a.SafeAlias).Append("Type when ")
                .Append(a.SafeAlias).Append("Type == typeof(").AppendFull(a).Append("):")
                .AppendLine("instance = new(Util.UnsafeConvert<")
                .Append(ConstantSources.GenericFactoryIsAsType)
                .Append(',').AppendFull(a)
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
                attributes,
                (b, a) => b.Append("case Type ").Append(a.SafeAlias).Append("Type when ")
                .Append(a.SafeAlias).Append("Type == typeof(").AppendFull(a).Append("):")
                .AppendLine("return new(Util.UnsafeConvert<")
            .Append(ConstantSources.GenericFactoryIsAsType)
            .Append(",").AppendFull(a).Append(">(value));"))
            .Append("default: ").Append(ConstantSources.InvalidCreationThrow($"\"{target.ToOpenString()}\"", "value"))
            .AppendLine(";}}")
            .ToString();

        var result = new FactoryFunctionsModel(sourceText);

        return result;
    }
}
