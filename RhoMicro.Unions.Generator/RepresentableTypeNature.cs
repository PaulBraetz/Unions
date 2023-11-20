#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;

using System;
using System.Linq;

// ATTENTION: order influences type order => changes are breaking
enum RepresentableTypeNature
{
    ReferenceType,
    ValueType,
    UnknownType
}
static class RepresentableTypeNatureFactory
{
    public static RepresentableTypeNature Create(UnionTypeAttribute attribute, INamedTypeSymbol target)
    {
        var isValueType = (attribute.RepresentableTypeIsGenericParameter ?
            target.TypeParameters.SingleOrDefault(p => p.Name == attribute.GenericRepresentableTypeName)?.IsValueType :
            attribute.RepresentableTypeSymbol?.IsValueType) ??
            false;
        if(isValueType)
            return RepresentableTypeNature.ValueType;

        var isReferenceType = (attribute.RepresentableTypeIsGenericParameter ?
            target.TypeParameters.SingleOrDefault(p => p.Name == attribute.GenericRepresentableTypeName)?.IsReferenceType :
            attribute.RepresentableTypeSymbol?.IsReferenceType) ??
            false;
        if(isReferenceType)
            return RepresentableTypeNature.ReferenceType;

        return RepresentableTypeNature.UnknownType;
    }
}
