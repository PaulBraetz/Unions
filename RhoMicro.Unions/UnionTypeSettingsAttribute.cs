#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;
using System.Collections.Generic;

/// <summary>
/// Defines settings for generating an implementation of <see cref="Object.ToString"/>.
/// </summary>
public enum ToStringSetting
{
    /// <summary>
    /// The generator will not generate an implementation of <see cref="Object.ToString"/>.
    /// </summary>
    None = 0,
    /// <summary>
    /// The generator will generate an implementation that returns the result of calling <see cref="Object.ToString"/> on the currently represented value.
    /// </summary>
    Simple,
    /// <summary>
    /// The generator will emit an implementation that returns detailed information, including:
    /// <list type="bullet">
    /// <item><description>the name of the union type</description></item>
    /// <item><description>a list of types representable by the union type</description></item>
    /// <item><description>an indication of which type is being represented by the instance</description></item>
    /// <item><description>the value currently being represented by the instance</description></item>
    /// </list>
    /// </summary>
    Detailed
}

/// <summary>
/// Supplies the generator with additional settings on how to generate a targeted union type.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed partial class UnionTypeSettingsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets settings defining how to generate an implementation <see cref="Object.ToString"/>.
    /// </summary>
    public ToStringSetting ToStringSetting { get; set; } = ToStringSetting.Detailed;
}
