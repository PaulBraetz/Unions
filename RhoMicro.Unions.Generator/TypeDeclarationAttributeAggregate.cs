namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

sealed class TypeDeclarationAttributeAggregate : IEquatable<TypeDeclarationAttributeAggregate>
{
    private TypeDeclarationAttributeAggregate(
        IReadOnlyList<UnionTypeAttribute> unionTypeAttributes,
        IReadOnlyList<SubsetOfAttribute> subsetOfAttributes,
        IReadOnlyList<SupersetOfAttribute> supersetOfAttributes,
        TargetModel target)
    {
        UnionTypeAttributes = unionTypeAttributes;
        SubsetOfAttributes = subsetOfAttributes;
        SupersetOfAttributes = supersetOfAttributes;
        Target = target;
    }

    public readonly TargetModel Target;
    public readonly IReadOnlyList<UnionTypeAttribute> UnionTypeAttributes;
    public readonly IReadOnlyList<SubsetOfAttribute> SubsetOfAttributes;
    public readonly IReadOnlyList<SupersetOfAttribute> SupersetOfAttributes;

    public static SourceDiagnosticsCarry<TypeDeclarationAttributeAggregate> Create(
        SourceDiagnosticsCarry<TargetModel> context,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        var result = context.Project(static (target, d) =>
        {
            var attributeData = target.Symbol.GetAttributes();

            var result = new TypeDeclarationAttributeAggregate(
                attributeData.OfUnionTypeAttribute()
                    .OrderBy(a => a.RepresentableTypeSymbol.ToFullString())
                    .ToArray(),
                attributeData.OfSubsetOfAttribute()
                    .OrderBy(a => a.SupersetUnionTypeSymbol.ToFullString())
                    .ToArray(),
                attributeData.OfSupersetOfAttribute()
                    .OrderBy(a => a.SubsetUnionTypeSymbol.ToFullString())
                    .ToArray(),
                target);

            return result;
        });

        return result;
    }

    public override Boolean Equals(Object obj) => Equals(obj as TypeDeclarationAttributeAggregate);
    public Boolean Equals(TypeDeclarationAttributeAggregate other) =>
        other is not null &&
        Target == other.Target &&
        UnionTypeAttributes.SequenceEqual(other.UnionTypeAttributes) &&
        SubsetOfAttributes.SequenceEqual(other.SubsetOfAttributes) &&
        SupersetOfAttributes.SequenceEqual(other.SupersetOfAttributes);

    public override Int32 GetHashCode()
    {
        var hashCode = -1771718068;
        hashCode = hashCode * -1521134295 + EqualityComparer<TargetModel>.Default.GetHashCode(Target);
        hashCode = UnionTypeAttributes.Aggregate(hashCode, (h, a) => hashCode * -1521134295 + a.GetHashCode());
        hashCode = SubsetOfAttributes.Aggregate(hashCode, (h, a) => hashCode * -1521134295 + a.GetHashCode());
        hashCode = SupersetOfAttributes.Aggregate(hashCode, (h, a) => hashCode * -1521134295 + a.GetHashCode());

        return hashCode;
    }

    public static Boolean operator ==(TypeDeclarationAttributeAggregate left, TypeDeclarationAttributeAggregate right) => EqualityComparer<TypeDeclarationAttributeAggregate>.Default.Equals(left, right);
    public static Boolean operator !=(TypeDeclarationAttributeAggregate left, TypeDeclarationAttributeAggregate right) => !(left == right);
}
