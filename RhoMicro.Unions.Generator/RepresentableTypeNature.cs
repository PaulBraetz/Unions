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
    ImpureValueType,
    PureValueType,
    UnknownType
}
static class RepresentableTypeNatureFactory
{
    public static RepresentableTypeNature Create(UnionTypeAttribute attribute, INamedTypeSymbol target)
    {
        var representedSymbol = attribute.RepresentableTypeIsGenericParameter ?
            target.TypeParameters.SingleOrDefault(p => p.Name == attribute.GenericRepresentableTypeName) :
            attribute.RepresentableTypeSymbol;

        if(representedSymbol == null)
            return RepresentableTypeNature.UnknownType;

        if(representedSymbol.IsPureValueType())
            return RepresentableTypeNature.PureValueType;

        if(representedSymbol.IsValueType)
            return RepresentableTypeNature.ImpureValueType;

        if(representedSymbol.IsReferenceType)
            return RepresentableTypeNature.ReferenceType;

        return RepresentableTypeNature.UnknownType;
    }
}
