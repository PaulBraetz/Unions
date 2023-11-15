//TODO: continue here

namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;

using RhoMicro.Unions;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
readonly struct ConversionOperatorModel : IEquatable<ConversionOperatorModel>
{
    public readonly String SourceText;

    private ConversionOperatorModel(String sourceText) => SourceText = sourceText;

    public static ConversionOperatorModel Create(
        ITypeSymbol target,
        UnionTypeAttribute attribute,
        IReadOnlyCollection<UnionTypeAttribute> allAttributes)
    {
        var sourceTextBuilder = new StringBuilder()
            .AppendLine("/// <summary>")
            .Append("/// Converts an instance of <see cref=\"").AppendSymbol(attribute.RepresentableTypeSymbol).Append("\"/> to the union type <see cref=\"").AppendSymbol(target).AppendLine("\"/>.")
            .AppendLine("/// </summary>")
            .AppendLine("/// <param name=\"value\">The value to convert.</param>")
            .AppendLine("/// <returns>The converted value.</returns>")
            .Append("public static implicit operator ")
            .AppendSymbol(target)
            .Append('(')
            .AppendSymbol(attribute.RepresentableTypeSymbol)
            .AppendLine(" value) => new(value);");

        var generateSolitaryExplicit = allAttributes.Count > 1 || !attribute.Options.HasFlag(UnionTypeOptions.ImplicitConversionIfSolitary);
        if(generateSolitaryExplicit)
        {
            _ = sourceTextBuilder
                .Append("public static explicit operator ")
                .AppendSymbol(attribute.RepresentableTypeSymbol)
                .Append('(')
                .AppendSymbol(target)
                .Append(" union) => ");

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append("union.__tag == Tag.")
                    .Append(attribute.SafeAlias)
                    .Append('?');
            }

            _ = sourceTextBuilder.Append(attribute.GetInstanceVariableExpression(target, "union"));

            if(allAttributes.Count > 1)
            {
                _ = sourceTextBuilder
                    .Append(':')
                    .Append(String.Format(
                        ConstantSources.InvalidExplicitCastThrow,
                        attribute.RepresentableTypeSymbol.ToFullString()));
            }

            _ = sourceTextBuilder.Append(';');
        } else
        {
            _ = sourceTextBuilder.Append("public static implicit operator ")
                .Append(attribute.RepresentableTypeSymbol.ToFullString())
                .Append('(')
                .Append(target.Name)
                .Append(" union) => ")
                .Append(attribute.GetInstanceVariableExpression(target, "union"))
                .AppendLine(";");
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }

    public static ConversionOperatorModel Create(
        ITypeSymbol symbol,
        IEnumerable<UnionTypeAttribute> unionTypeAttributes,
        SupersetOfAttribute attribute)
    {
        var sourceTextBuilder = new StringBuilder("public static implicit operator ")
            .AppendSymbol(symbol)
            .Append('(')
            .AppendSymbol(attribute.SubsetUnionTypeSymbol)
            .Append(" subsetUnion) => subsetUnion.DownCast<")
            .AppendSymbol(symbol)
            .AppendLine(">();")
            .Append("public static explicit operator ")
            .AppendSymbol(attribute.SubsetUnionTypeSymbol)
            .Append('(')
            .AppendSymbol(symbol)
            .AppendLine(" union) => union.__tag switch")
            .AppendLine("{");

        foreach(var unionTypeAttribute in unionTypeAttributes)
        {
            _ = sourceTextBuilder.Append("Tag.")
                .Append(unionTypeAttribute.SafeAlias)
                .Append(" => ")
                .AppendSymbol(attribute.SubsetUnionTypeSymbol)
                .Append("union.__");

            _ = unionTypeAttribute.RepresentableTypeSymbol.IsValueType ?
                sourceTextBuilder.Append("valueTypeContainer.")
                    .Append(unionTypeAttribute.SafeAlias) :
                sourceTextBuilder.Append("referenceTypeContainer");

            _ = sourceTextBuilder.AppendLine(",");
        }

        _ = sourceTextBuilder.Append("_ => ")
            .Append(ConstantSources.InvalidTagStateThrow)
            .AppendLine("};");

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }

    public static ConversionOperatorModel Create(
        ITypeSymbol symbol,
        IReadOnlyCollection<UnionTypeAttribute> unionTypeAttributes,
        SubsetOfAttribute attribute)
    {
        //public static implicit operator Union(SubsetUnion subset) => subset.Match(static v => v);
        //target is subset union here!
        var sourceTextBuilder = new StringBuilder("public static implicit operator ")
            .AppendSymbol(attribute.SupersetUnionTypeSymbol)
            .Append('(')
            .AppendSymbol(symbol)
            .Append(" subsetUnion) => subsetUnion.Match(")
            .Append(String.Join(",", Enumerable.Repeat("static v => v", unionTypeAttributes.Count)))
            .AppendLine(");")
            /*
            public static explicit operator SubsetUnion(Union union) =>
               union.Match(
                   static v => v,
                   static v => throw new InvalidOperationException(),
                   static v => throw new InvalidOperationException(),
                   static v => throw new InvalidOperationException());
            */
            .Append("public static explicit operator ")
            .AppendSymbol(symbol)
            .Append('(')
            .AppendSymbol(attribute.SupersetUnionTypeSymbol)
            .AppendLine(" union) => union.Match(");

        foreach(var _ in attribute.SupersetUnionTypeSymbol.GetAttributes().OfUnionTypeAttribute())
        {
            //TODO
        }

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }

    public override Boolean Equals(Object obj) => obj is ConversionOperatorModel model && Equals(model);
    public Boolean Equals(ConversionOperatorModel other) => SourceText == other.SourceText;
    public override Int32 GetHashCode() => -1893336041 + EqualityComparer<String>.Default.GetHashCode(SourceText);

    public static Boolean operator ==(ConversionOperatorModel left, ConversionOperatorModel right) => left.Equals(right);
    public static Boolean operator !=(ConversionOperatorModel left, ConversionOperatorModel right) => !(left == right);
}