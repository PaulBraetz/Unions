﻿#pragma warning disable CS8618

namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;

using RhoMicro.AttributeFactoryGenerator;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Immutable;
using System.Linq;

using static RhoMicro.Unions.Generator.UnionDataModel;

/// <summary>
/// Defines the type of relations that can be defined between two union types.
/// </summary>
internal enum RelationType
{
    /// <summary>
    /// There is no relation between the provided type and target type.
    /// </summary>
    None,
    /// <summary>
    /// The target type is a superset of the provided type.
    /// This means that two conversion operations will be generated:
    /// <list type="bullet">
    /// <item><description>
    /// an <em>implicit</em> conversion operator from the provided type to the target type
    /// </description></item>
    /// <item><description>
    /// an <em>explicit</em> conversion operator from the target type to the provided type
    /// </description></item>
    /// </list>
    /// This option is not available if the provided type has already defined a relation to the target type.
    /// </summary>
    Superset,
    /// <summary>
    /// The target type is a subset of the provided type.
    /// This means that two conversion operations will be generated:
    /// <list type="bullet">
    /// <item><description>
    /// an <em>implicit</em> conversion operator from the target type to the provided type
    /// </description></item>
    /// <item><description>
    /// an <em>explicit</em> conversion operator from the provided type to the target type
    /// </description></item>
    /// </list>
    /// This option is not available if the provided type has already defined a relation to the target type.
    /// </summary>
    Subset,
    /// <summary>
    /// The target type intersects the provided type.
    /// This means that two conversion operations will be generated:
    /// <list type="bullet">
    /// <item><description>
    /// an <em>explicit</em> conversion operator from the target type to the provided type
    /// </description></item>
    /// <item><description>
    /// an <em>explicit</em> conversion operator from the provided type to the target type
    /// </description></item>
    /// </list>
    /// This option is not available if the provided type has already defined a relation to the target type.
    /// </summary>
    Intersection,
    /// <summary>
    /// The target type is congruent to the provided type.
    /// This means that two conversion operations will be generated:
    /// <list type="bullet">
    /// <item><description>
    /// an <em>implicit</em> conversion operator from the target type to the provided type
    /// </description></item>
    /// <item><description>
    /// an <em>implicit</em> conversion operator from the provided type to the target type
    /// </description></item>
    /// </list>
    /// This option is not available if the provided type has already defined a relation to the target type.
    /// </summary>
    Congruent
}

[GenerateFactory]
public sealed partial class RelationAttribute : IEquatable<RelationAttribute?>
{
    [ExcludeFromFactory]
    private RelationAttribute(Object relatedTypeSymbolContainer) => _relatedTypeSymbolContainer = relatedTypeSymbolContainer;

    internal RelationTypeData ExtractData(TargetDataModel target) =>
            RelationTypeData.Create(this, target);

    public override Boolean Equals(Object? obj) => Equals(obj as RelationAttribute);
    public Boolean Equals(RelationAttribute? other)
    {
        var result = other is not null &&
            SymbolEqualityComparer.Default.Equals(RelatedTypeSymbol, other.RelatedTypeSymbol);

        return result;
    }

    public override Int32 GetHashCode()
    {
        var hashCode = 2127939515;
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(RelatedTypeSymbol);
        return hashCode;
    }
}
