#pragma warning disable CS8618

namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.AttributeFactoryGenerator;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;

[GenerateFactory]
public partial class UnionTypeAttribute : IEquatable<UnionTypeAttribute?>
{
    [ExcludeFromFactory]
    public UnionTypeAttribute(Object representableTypeSymbolContainer) =>
        _representableTypeSymbolContainer = representableTypeSymbolContainer;

    internal RepresentableTypeData ExtractData(INamedTypeSymbol target) =>
        RepresentableTypeData.Create(this, target);

    public override Boolean Equals(Object? obj) => Equals(obj as UnionTypeAttribute);
    public Boolean Equals(UnionTypeAttribute? other)
    {
        var result = other is not null &&
            Alias == other.Alias &&
            Options == other.Options &&
            (RepresentableTypeIsGenericParameter ?
            GenericRepresentableTypeName == other.GenericRepresentableTypeName :
            SymbolEqualityComparer.Default.Equals(RepresentableTypeSymbol, other.RepresentableTypeSymbol));

        return result;
    }

    public override Int32 GetHashCode()
    {
        var hashCode = 1581354465;
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(Alias);
        hashCode = hashCode * -1521134295 + Options.GetHashCode();
        hashCode = RepresentableTypeIsGenericParameter ?
            hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(GenericRepresentableTypeName) :
            hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(RepresentableTypeSymbol);

        return hashCode;
    }
}
