#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;

[DebuggerDisplay("{Names.SimpleTypeName}")]
sealed class RepresentableTypeData : IEquatable<RepresentableTypeData?>
{
    public RepresentableTypeData(
        UnionTypeAttribute attribute,
        INamedTypeSymbol target,
        String commentRef,
        RepresentableTypeNature nature,
        RepresentableTypeNames names,
        StorageStrategy storage)
    {
        Attribute = attribute;
        Target = target;
        DocCommentRef = commentRef;
        Nature = nature;
        Names = names;
        Storage = storage;

        CorrespondingTag = $"Tag.{names.SafeAlias}";
    }

    public readonly UnionTypeAttribute Attribute;
    public readonly INamedTypeSymbol Target;
    public readonly String DocCommentRef;
    public readonly RepresentableTypeNature Nature;
    public readonly RepresentableTypeNames Names;
    public readonly StorageStrategy Storage;
    public readonly String CorrespondingTag;

    public static RepresentableTypeData Create(
        UnionTypeAttribute attribute,
        INamedTypeSymbol target)
    {
        var commentRef =
            attribute.RepresentableTypeIsGenericParameter ?
            $"<typeparamref name=\"{attribute.GenericRepresentableTypeName}\"/>" :
            $"<see cref=\"{attribute.RepresentableTypeSymbol?.ToOpenString().Replace('<', '{').Replace('>', '}')}\"/>";
        var names = RepresentableTypeNames.Create(attribute);
        var nature = RepresentableTypeNatureFactory.Create(attribute, target);

        var storageStrategy = StorageStrategy.Create(
            names.SafeAlias,
            names.FullTypeName,
            attribute.Storage,
            nature,
            target.IsGenericType);

        var result = new RepresentableTypeData(
            attribute,
            target,
            commentRef,
            nature,
            names,
            storageStrategy);

        return result;
    }

    public override Boolean Equals(Object? obj) => Equals(obj as RepresentableTypeData);
    public Boolean Equals(RepresentableTypeData? other) =>
        other is not null &&
        Attribute.Equals(other.Attribute) &&
        SymbolEqualityComparer.Default.Equals(Target, other.Target);

    public override Int32 GetHashCode()
    {
        var hashCode = 1823267663;
        hashCode = hashCode * -1521134295 + Attribute.GetHashCode();
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(Target);
        return hashCode;
    }
}
