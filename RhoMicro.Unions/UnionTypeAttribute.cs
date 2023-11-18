#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;
using System.Collections.Generic;

/// <summary>
/// Defines options for generating union types.
/// </summary>
[Flags]
public enum UnionTypeOptions
{
    /// <summary>
    /// </summary>
    None = 0b0,
    /// <summary>
    /// Instructs the generator to emit an implicit conversion to the representable type if it is the only one.
    /// In effect, this option will enable the union type to act as an alias wrapper for the representable type.
    /// </summary>
    ImplicitConversionIfSolitary = 0b1
}

/// <summary>
/// Marks the target type as a union type being able to represent the type passed to the constructor.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed partial class UnionTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="representableType">
    /// The type representable by the target union type.
    /// </param>
    public UnionTypeAttribute(Type representableType) =>
        RepresentableType = representableType;

    /// <summary>
    /// Gets or sets the alias to use for members representing the type represented by the union.
    /// For example, the represented type <see cref="List{T}"/> would be represented using names like
    /// <c>list</c>. Setting this property to <c>yourAlias</c> will instruct the generator to use
    /// member names like <c>yourAlias</c> instead of <c>list</c>. Use this property to avoid
    /// name collisions in generated code. Since the alias will be used for member names, it will
    /// only be taken into account if it is a valid identifier name.
    /// </summary>
    public String? Alias { get; set; }

    /// <summary>
    /// Gets or sets the generator options to use.
    /// </summary>
    public UnionTypeOptions Options { get; set; }

    /// <summary>
    /// Gets the type representable by the target union type.
    /// </summary>
    public Type RepresentableType { get; }
}