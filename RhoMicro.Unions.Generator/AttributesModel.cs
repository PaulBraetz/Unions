namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

sealed class AttributesModel : IEquatable<AttributesModel>
{
    public readonly IReadOnlyList<UnionTypeAttribute> ReferenceTypeAttributes;
    public readonly IReadOnlyList<UnionTypeAttribute> ValueTypeAttributes;
    public readonly IReadOnlyList<UnionTypeAttribute> AllUnionTypeAttributes;

    public readonly Boolean HasSingleAttribute;
    public readonly Boolean HasValueTypeAttributes;
    public readonly Boolean HasReferenceTypeAttributes;

    private AttributesModel(
        IReadOnlyList<UnionTypeAttribute> referenceTypeAttributes,
        IReadOnlyList<UnionTypeAttribute> valueTypeAttributes,
        IReadOnlyList<UnionTypeAttribute> allAttributes)
    {
        ReferenceTypeAttributes = referenceTypeAttributes;
        ValueTypeAttributes = valueTypeAttributes;
        AllUnionTypeAttributes = allAttributes;

        HasSingleAttribute = AllUnionTypeAttributes.Count == 1;
        HasValueTypeAttributes = ValueTypeAttributes.Count > 0;
        HasReferenceTypeAttributes = ReferenceTypeAttributes.Count > 0;
    }

    public static AttributesModel Create(IEnumerable<UnionTypeAttribute> attributes)
    {
        var orderedAttributes = attributes
            .OrderBy(a => a.RepresentableTypeSymbol.IsValueType)
            .ThenBy(a => a.RepresentableTypeSymbol.ToFullString());

        var allAttributes = new List<UnionTypeAttribute>();
        var referenceTypeAttributes = new List<UnionTypeAttribute>();
        var valueTypeAttributes = new List<UnionTypeAttribute>();

        foreach(var attribute in orderedAttributes)
        {
            if(attribute.RepresentableTypeSymbol.IsValueType)
            {
                valueTypeAttributes.Add(attribute);
            } else if(attribute.RepresentableTypeSymbol.IsReferenceType)
            {
                referenceTypeAttributes.Add(attribute);
            }

            allAttributes.Add(attribute);
        }

        var result = new AttributesModel(referenceTypeAttributes, valueTypeAttributes, allAttributes);

        return result;
    }

    public override Boolean Equals(Object? obj) =>
        obj is AttributesModel other && Equals(other);
    public Boolean Equals(AttributesModel other) =>
        other is not null &&
        other.AllUnionTypeAttributes.SequenceEqual(AllUnionTypeAttributes);

    public override Int32 GetHashCode()
    {
        var hashCode = AllUnionTypeAttributes.Aggregate(-40951477, (h, a) => h * -1521134295 + a.GetHashCode());

        return hashCode;
    }

    public static Boolean operator ==(AttributesModel left, AttributesModel right) => EqualityComparer<AttributesModel>.Default.Equals(left, right);
    public static Boolean operator !=(AttributesModel left, AttributesModel right) => !(left == right);
}
