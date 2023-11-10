//TODO: continue here

namespace RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct ConversionOperatorModel : IEquatable<ConversionOperatorModel>
{
    public readonly String SourceText;

    private ConversionOperatorModel(String sourceText) => SourceText = sourceText;

    public static ConversionOperatorModel Create(
        TargetModel target,
        UnionTypeAttribute attribute)
    {
        var sourceTextBuilder = new StringBuilder("public static implicit operator ")
            .AppendSymbol(target.Symbol)
            .Append('(')
            .AppendSymbol(attribute.RepresentableTypeSymbol)
            .AppendLine(" value) => new(value);")
            .Append("public static explicit operator ")
            .AppendSymbol(attribute.RepresentableTypeSymbol)
            .Append('(')
            .AppendSymbol(target.Symbol)
            .Append(" union) => union._tag == Tag.")
            .Append(attribute.SafeAlias)
            .Append('?');

        _ = attribute.RepresentableTypeSymbol.IsReferenceType
            ? sourceTextBuilder.Append('(')
                .AppendSymbol(attribute.RepresentableTypeSymbol)
                .Append(")union._referenceTypeContainer")
            : sourceTextBuilder.Append("union._valueTypeContainer.")
                .Append(attribute.SafeAlias);

        _ = sourceTextBuilder
            .Append(':')
            .Append(ConstantSources.InvalidTagStateThrow)
            .Append(';');

        var sourceText = sourceTextBuilder.ToString();
        var result = new ConversionOperatorModel(sourceText);

        return result;
    }

    public static ConversionOperatorModel Create(
        TypeDeclarationAttributeAggregate aggregate,
        SupersetOfAttribute attribute)
    {
        var sourceTextBuilder = new StringBuilder("public static implicit operator ")
            .AppendSymbol(aggregate.Target.Symbol)
            .Append('(')
            .AppendSymbol(attribute.SubsetUnionTypeSymbol)
            .Append(" subsetUnion) => subsetUnion.DownCast<")
            .AppendSymbol(aggregate.Target.Symbol)
            .AppendLine(">();")
            .Append("public static explicit operator ")
            .AppendSymbol(attribute.SubsetUnionTypeSymbol)
            .Append('(')
            .AppendSymbol(aggregate.Target.Symbol)
            .AppendLine(" union) => union._tag switch")
            .AppendLine("{");

        foreach(var unionTypeAttribute in aggregate.UnionTypeAttributes)
        {
            _ = sourceTextBuilder.Append("Tag.")
                .Append(unionTypeAttribute.SafeAlias)
                .Append(" => ")
                .AppendSymbol(attribute.SubsetUnionTypeSymbol)
                .Append("union._");

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
        TypeDeclarationAttributeAggregate aggregate,
        SubsetOfAttribute attribute)
    {
        //public static implicit operator Union(SubsetUnion subset) => subset.Match(static v => v);
        //target is subset union here!
        var sourceTextBuilder = new StringBuilder("public static implicit operator ")
            .AppendSymbol(attribute.SupersetUnionTypeSymbol)
            .Append('(')
            .AppendSymbol(aggregate.Target.Symbol)
            .Append(" subsetUnion) => subsetUnion.Match(")
            .Append(String.Join(",", Enumerable.Repeat("static v => v", aggregate.UnionTypeAttributes.Count)))
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
            .AppendSymbol(aggregate.Target.Symbol)
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