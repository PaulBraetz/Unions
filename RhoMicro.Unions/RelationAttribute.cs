#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;

/// <summary>
/// Marks the target type to be related to another union type.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed partial class RelationAttribute : Attribute
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
}
