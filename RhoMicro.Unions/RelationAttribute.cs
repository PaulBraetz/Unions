#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;

/// <summary>
/// Defines the type of relations that can be defined between two union types.
/// </summary>
internal enum RelationType
{
    /// <summary>
    /// The relation should be automatically determined.
    /// <para>
    /// This option is not available if the provided type has already defined a relation to the target type.
    /// </para>
    /// </summary>
    Auto = 0b1,
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
    Superset = 0b01,
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
    Subset = 0b001,
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
    Intersection = 0b0001,
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
    Congruent = 0b0_0001
}

/// <summary>
/// Marks the target type to be related to another union type.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
internal  sealed partial class RelationAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="relatedType">
    /// The type to register as related to the target union type.
    /// </param>
    public RelationAttribute(Type relatedType) =>
        RelatedType = relatedType;

    /// <summary>
    /// Gets the type to register as related to the target union type.
    /// </summary>
    public Type RelatedType { get; }
    /// <summary>
    /// Gets or sets the relation type between the target type and the related type.
    /// </summary>
    public RelationType RelationType { get; set; }
}