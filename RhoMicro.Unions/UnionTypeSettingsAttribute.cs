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
/// Defines settings for annotating the target with an instance of <see cref="System.Runtime.InteropServices.StructLayoutAttribute"/>.
/// </summary>
public enum LayoutSetting
{
    /// <summary>
    /// Do not generate any annotations.
    /// </summary>
    Auto,
    /// <summary>
    /// Generate an annotation optimized for size.
    /// </summary>
    Small
}

/// <summary>
/// Supplies the generator with additional settings on how to generate a targeted union type.
/// If the target member is an assembly, the attribute supplies default values for any union type not defining its own settings.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed partial class UnionTypeSettingsAttribute : Attribute
{
    /// <summary>
    /// Gets or sets a setting defining how to generate an implementation <see cref="Object.ToString"/>.
    /// </summary>
    public ToStringSetting ToStringSetting { get; set; } = ToStringSetting.Detailed;
    /// <summary>
    /// Gets or sets a setting defining whether to generate a size optimizing annotation.
    /// </summary>
    public LayoutSetting Layout { get; set; } = LayoutSetting.Small;
}
