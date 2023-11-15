#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;

/// <summary>
/// Marks the target type as a subset union type of the type passed to the constructor.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed partial class SubsetOfAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="supersetUnionType">
    /// The type to register as a superset of the target union type.
    /// </param>
    public SubsetOfAttribute(Type supersetUnionType) =>
        SupersetUnionType = supersetUnionType;

    /// <summary>
    /// Gets the type to register as a superset of the target union type.
    /// </summary>
    public Type SupersetUnionType { get; }
}