namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

sealed class AnnotationDataModel : IEquatable<AnnotationDataModel>
{
    public readonly IReadOnlyList<RepresentableTypeData> AllRepresentableTypes;
    public readonly IReadOnlyList<RepresentableTypeData> RepresentableReferenceTypes;
    public readonly IReadOnlyList<RepresentableTypeData> RepresentableValueTypes;
    public readonly IReadOnlyList<RepresentableTypeData> RepresentableUnknownTypes;
    public readonly UnionTypeSettingsAttribute Settings;

    public Boolean HasReferenceTypes => RepresentableReferenceTypes.Count > 0;
    public Boolean HasValueTypes => RepresentableValueTypes.Count > 0;
    public Boolean HasUnknownTypes => RepresentableUnknownTypes.Count > 0;
    public Boolean HasTypes => AllRepresentableTypes.Count > 0;

    private AnnotationDataModel(
        UnionTypeSettingsAttribute settings,
        IReadOnlyList<RepresentableTypeData> allRepresentableTypes,
        IReadOnlyList<RepresentableTypeData> representableReferenceTypes,
        IReadOnlyList<RepresentableTypeData> representableValueTypes,
        IReadOnlyList<RepresentableTypeData> representableUnknownTypes)
    {
        Settings = settings;
        AllRepresentableTypes = allRepresentableTypes;
        RepresentableReferenceTypes = representableReferenceTypes;
        RepresentableValueTypes = representableValueTypes;
        RepresentableUnknownTypes = representableUnknownTypes;
    }

    public static AnnotationDataModel Create(INamedTypeSymbol target)
    {
        var attributes = target.GetAttributes();

        //DO NOT CHANGE THIS ALGO, compatibility depends on deterministic order of types
        var orderedRepresentableTypes = attributes
            .OfUnionTypeAttribute()
            .Select(a => a.ExtractData(target))
            .GroupBy(a => a.Nature)
            .OrderBy(g => g.Key) //generic params come last
            .Select(g =>
                g.Key == RepresentableTypeNature.UnknownType ?
                g.OrderBy(a => a.Names.SimpleTypeName) :
                g.OrderBy(a => a.Nature == RepresentableTypeNature.ValueType) //reference types come first, bang here bc !RepresentableTypeIsGenericParameter
                 .ThenBy(a => a.Names.FullTypeName))
            .SelectMany(g => g);

        // reference types
        // value types
        // generic params

        List<RepresentableTypeData> allRepresentableTypes = [];
        List<RepresentableTypeData> representableReferenceTypes = [];
        List<RepresentableTypeData> representableValueTypes = [];
        List<RepresentableTypeData> representableUnknownTypes = [];

        foreach(var representableType in orderedRepresentableTypes)
        {
            (representableType.Nature switch
            {
                RepresentableTypeNature.ReferenceType => representableReferenceTypes,
                RepresentableTypeNature.ValueType => representableValueTypes,
                _ => representableUnknownTypes
            }).Add(representableType);
            allRepresentableTypes.Add(representableType);
        }

        var settings = attributes.OfUnionTypeSettingsAttribute().SingleOrDefault() ??
            target.ContainingAssembly.GetAttributes().OfUnionTypeSettingsAttribute().SingleOrDefault() ??
            new UnionTypeSettingsAttribute();

        var result = new AnnotationDataModel(
            settings,
            allRepresentableTypes,
            representableReferenceTypes,
            representableValueTypes,
            representableUnknownTypes);

        return result;
    }

    public override Boolean Equals(Object? obj) =>
        obj is AnnotationDataModel other && Equals(other);
    public Boolean Equals(AnnotationDataModel other) =>
        other is not null &&
        other.AllRepresentableTypes.SequenceEqual(AllRepresentableTypes) &&
        other.Settings.Equals(Settings);

    public override Int32 GetHashCode()
    {
        var hashCode = AllRepresentableTypes.Aggregate(-40951477, (h, a) => h * -1521134295 + a.GetHashCode());
        hashCode = hashCode * -1521134295 + Settings.GetHashCode();

        return hashCode;
    }

    public static Boolean operator ==(AnnotationDataModel left, AnnotationDataModel right) => EqualityComparer<AnnotationDataModel>.Default.Equals(left, right);
    public static Boolean operator !=(AnnotationDataModel left, AnnotationDataModel right) => !(left == right);
}
