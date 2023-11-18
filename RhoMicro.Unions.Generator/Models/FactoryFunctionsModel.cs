namespace RhoMicro.Unions.Generator.Models;
using Microsoft.CodeAnalysis;

using System;
using System.Text;

readonly struct FactoryFunctionsModel
{
    public readonly String SourceText;
    private FactoryFunctionsModel(String sourceText) => SourceText = sourceText;

    public static IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>>
        Project(IncrementalValuesProvider<SourceCarry<ModelFactoryParameters>> provider)
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
                    public static {{target.ToFullString()}} Create({{a.RepresentableTypeSymbol.ToFullString()}} value) => new(value);
                    """))
            .AppendLine("/// </inheritdoc>")
            .Append("public static Boolean TryCreate<T>(T value, out ").Append(target.Name).AppendLine(" instance){switch(typeof(T)){")
            .AppendAggregate(
                attributes,
                (b, a) => b.Append("case Type ").Append(a.SafeAlias).Append("Type when ")
                .Append(a.SafeAlias).Append("Type == typeof(").AppendSymbol(a.RepresentableTypeSymbol).Append("):")
                .AppendLine("instance = new(Util.UnsafeConvert<T,").AppendSymbol(a.RepresentableTypeSymbol).Append(">(value));return true;"))
            .AppendLine("default: instance = default; return false;}}")
            .AppendLine("/// </inheritdoc>")
            .Append("public static ").Append(target.Name).AppendLine(" Create<T>(T value){switch(typeof(T)){")
            .AppendAggregate(
                attributes,
                (b, a) => b.Append("case Type ").Append(a.SafeAlias).Append("Type when ")
                .Append(a.SafeAlias).Append("Type == typeof(").AppendSymbol(a.RepresentableTypeSymbol).Append("):")
                .AppendLine("return new(Util.UnsafeConvert<T,").AppendSymbol(a.RepresentableTypeSymbol).Append(">(value));"))
            .Append("default: ").Append(ConstantSources.InvalidCreationThrow($"nameof({target.Name})", "value"))
            .AppendLine(";}}")
            .ToString();

        var result = new FactoryFunctionsModel(sourceText);

        return result;
    }
}
