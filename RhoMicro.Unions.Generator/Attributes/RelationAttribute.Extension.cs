#pragma warning disable CS8618

namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;

using RhoMicro.AttributeFactoryGenerator;

using System;
using System.Collections.Generic;

[GenerateFactory]
internal sealed partial class RelationAttribute : IEquatable<RelationAttribute?>
{
    [ExcludeFromFactory]
    private RelationAttribute(Object relatedTypeSymbolContainer) => _relatedTypeSymbolContainer = relatedTypeSymbolContainer;

    public override Boolean Equals(Object? obj) => Equals(obj as RelationAttribute);
    public Boolean Equals(RelationAttribute? other) => other is not null &&
        base.Equals(other) &&
        RelationType == other.RelationType &&
        SymbolEqualityComparer.Default.Equals(RelatedTypeSymbol, other.RelatedTypeSymbol);

    public override Int32 GetHashCode()
    {
        var hashCode = 2127939515;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + RelationType.GetHashCode();
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(RelatedTypeSymbol);
        return hashCode;
    }

    public static Boolean operator ==(RelationAttribute? left, RelationAttribute? right) =>
        left == null ?
        right == null :
        right == null ?
        left == null :
        left.Equals(right);
    public static Boolean operator !=(RelationAttribute? left, RelationAttribute? right) => !(left == right);
}
