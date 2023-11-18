namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

sealed class AttributesModel : IEquatable<AttributesModel>
{
    public readonly IReadOnlyList<UnionTypeAttribute> ReferenceTypeAttributes;
    public readonly IReadOnlyList<UnionTypeAttribute> ValueTypeAttributes;
    public readonly IReadOnlyList<UnionTypeAttribute> AllUnionTypeAttributes;

    public readonly IReadOnlyList<RelationAttribute> AllRelationAttributes;

    public readonly UnionTypeSettingsAttribute Settings;

    private AttributesModel(
        IReadOnlyList<UnionTypeAttribute> referenceTypeAttributes,
        IReadOnlyList<UnionTypeAttribute> valueTypeAttributes,
        IReadOnlyList<UnionTypeAttribute> allUnionTypeAttributes,
        IReadOnlyList<RelationAttribute> allRelationAttributes,
        UnionTypeSettingsAttribute settings)
    {
        ReferenceTypeAttributes = referenceTypeAttributes;
        ValueTypeAttributes = valueTypeAttributes;
        AllUnionTypeAttributes = allUnionTypeAttributes;
        AllRelationAttributes = allRelationAttributes;
        Settings = settings;
    }

    public static AttributesModel Create(ITypeSymbol target)
    {
        var attributes = target.GetAttributes();

        var allRelationAttributes = attributes.OfRelationAttribute().ToList();

        //DO NOT CHANGE THIS ALGO, compatibility depends on deterministic order of types
        var orderedUnionTypeAttributes = attributes
            .OfUnionTypeAttribute()
            .OrderBy(a => a.RepresentableTypeSymbol.IsValueType)
            .ThenBy(a => a.RepresentableTypeSymbol.ToFullString());

        var allUnionTypeAttributes = new List<UnionTypeAttribute>();
        var referenceTypeAttributes = new List<UnionTypeAttribute>();
        var valueTypeAttributes = new List<UnionTypeAttribute>();

        foreach(var attribute in orderedUnionTypeAttributes)
        {
            if(attribute.RepresentableTypeSymbol.IsValueType)
            {
                valueTypeAttributes.Add(attribute);
            } else if(attribute.RepresentableTypeSymbol.IsReferenceType)
            {
                referenceTypeAttributes.Add(attribute);
            }

            allUnionTypeAttributes.Add(attribute);
        }

        var settings = attributes.OfUnionTypeSettingsAttribute().SingleOrDefault() ??
            target.ContainingAssembly.GetAttributes().OfUnionTypeSettingsAttribute().SingleOrDefault() ??
            new UnionTypeSettingsAttribute();

        var result = new AttributesModel(
            referenceTypeAttributes,
            valueTypeAttributes,
            allUnionTypeAttributes,
            allRelationAttributes,
            settings);

        return result;
    }

    public override Boolean Equals(Object? obj) =>
        obj is AttributesModel other && Equals(other);
    public Boolean Equals(AttributesModel other) =>
        other is not null &&
        other.AllUnionTypeAttributes.SequenceEqual(AllUnionTypeAttributes) &&
        other.AllRelationAttributes.SequenceEqual(AllRelationAttributes);

    public override Int32 GetHashCode()
    {
        var hashCode = AllUnionTypeAttributes.Aggregate(-40951477, (h, a) => h * -1521134295 + a.GetHashCode());
        hashCode = AllRelationAttributes.Aggregate(hashCode, (h, a) => h * -1521134295 + a.GetHashCode());

        return hashCode;
    }

    public static Boolean operator ==(AttributesModel left, AttributesModel right) => EqualityComparer<AttributesModel>.Default.Equals(left, right);
    public static Boolean operator !=(AttributesModel left, AttributesModel right) => !(left == right);
}
