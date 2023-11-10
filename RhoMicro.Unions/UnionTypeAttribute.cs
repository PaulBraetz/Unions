#nullable enable
namespace RhoMicro.Unions;

using System;
using System.Collections.Generic;

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
    /// name collisions in generated code.
    /// </summary>
    public String? Alias { get; set; }

    /// <summary>
    /// Gets the type representable by the target union type.
    /// </summary>
    public Type RepresentableType { get; }
}