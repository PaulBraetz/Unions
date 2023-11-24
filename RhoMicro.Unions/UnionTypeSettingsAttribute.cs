﻿#if GENERATOR
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
    /// The generator will emit an implementation that returns detailed information, including:
    /// <list type="bullet">
    /// <item><description>the name of the union type</description></item>
    /// <item><description>a list of types representable by the union type</description></item>
    /// <item><description>an indication of which type is being represented by the instance</description></item>
    /// <item><description>the value currently being represented by the instance</description></item>
    /// </list>
    /// </summary>
    Detailed,
    /// <summary>
    /// The generator will not generate an implementation of <see cref="Object.ToString"/>.
    /// </summary>
    None,
    /// <summary>
    /// The generator will generate an implementation that returns the result of calling <see cref="Object.ToString"/> on the currently represented value.
    /// </summary>
    Simple
}

/// <summary>
/// Defines settings for annotating the target with an instance of <see cref="System.Runtime.InteropServices.StructLayoutAttribute"/>.
/// </summary>
public enum LayoutSetting
{    /// <summary>
     /// Generate an annotation optimized for size.
     /// </summary>
    Small,
    /// <summary>
    /// Do not generate any annotations.
    /// </summary>
    Auto
}

/// <summary>
/// Defines settings for controlling the accessibility of generated constructors.
/// </summary>
public enum ConstructorAccessibilitySetting
{
    /// <summary>
    /// Generated constructors should always be private, unless
    /// no conversion operators are generated for the type they
    /// accept. This would be the case for interface types or
    /// supertypes of the target union.
    /// </summary>
    PublicIfInconvertible,
    /// <summary>
    /// Generated constructors should always be private.
    /// </summary>
    Private,
    /// <summary>
    /// Generated constructors should always be public
    /// </summary>
    Public
}
/// <summary>
/// Defines settings for the kind of diagnostics to report.
/// </summary>
[Flags]
public enum DiagnosticsLevelSettings
{
    /// <summary>
    /// Instructs the analyzer to report info diagnostics.
    /// </summary>
    Info = 0x01,
    /// <summary>
    /// Instructs the analyzer to report warning diagnostics.
    /// </summary>
    Warning = 0x02,
    /// <summary>
    /// Instructs the analyzer to report error diagnostics.
    /// </summary>
    Error = 0x04,
    /// <summary>
    /// Instructs the analyzer to report all diagnostics.
    /// </summary>
    All = Info + Warning + Error
}

/// <summary>
/// Supplies the generator with additional settings on how to generate a targeted union type.
/// If the target member is an assembly, the attribute supplies default values for any union type not defining its own settings.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
public sealed partial class UnionTypeSettingsAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public UnionTypeSettingsAttribute() => _reservedGenericTypeNames =
        [_genericTValueName, _downcastTypeName, _matchTypeName];

    private String _genericTValueName = "TValue";
    private String _downcastTypeName = "TSuperset";
    private String _matchTypeName = "TResult";

    /// <summary>
    /// Gets or sets a setting defining how to generate an implementation <see cref="Object.ToString"/>.
    /// </summary>
    public ToStringSetting ToStringSetting { get; set; }
    /// <summary>
    /// Gets or sets a setting defining whether to generate a size optimizing annotation.
    /// </summary>
    public LayoutSetting Layout { get; set; }
    /// <summary>
    /// Gets or sets the level of diagnostics to be reported by the analyzer.
    /// </summary>
    public DiagnosticsLevelSettings DiagnosticsLevel { get; set; } = DiagnosticsLevelSettings.All;
    /// <summary>
    /// Gets or sets a value indicating the desired accessibility of generated constructors.
    /// </summary>
    public ConstructorAccessibilitySetting ConstructorAccessibility { get; set; }
    /// <summary>
    /// Gets or sets the name of the generic parameter for generic <c>Is</c>, <c>As</c> and factory methods. 
    /// Set this property in order to avoid name collisions with generic union type parameters
    /// </summary>
    public String GenericTValueName
    {
        get => _genericTValueName;
        set => SetReservedName(ref _genericTValueName, value);
    }
    /// <summary>
    /// Gets or sets the name of the generic parameter for the <c>DownCast</c> method. 
    /// Set this property in order to avoid name collisions with generic union type parameters
    /// </summary>
    public String DowncastTypeName
    {
        get => _downcastTypeName;
        set => SetReservedName(ref _downcastTypeName, value);
    }
    /// <summary>
    /// Gets or sets the name of the generic parameter for the <c>Match</c> method. 
    /// Set this property in order to avoid name collisions with generic union type parameters
    /// </summary>
    public String MatchTypeName
    {
        get => _matchTypeName;
        set => SetReservedName(ref _matchTypeName, value);
    }
    private void SetReservedName(ref String field, String newValue)
    {
        if(field != null)
        {
            _ = _reservedGenericTypeNames.Remove(field);
        }

        _ = _reservedGenericTypeNames.Add(newValue);
        field = newValue;
    }
    private readonly HashSet<String> _reservedGenericTypeNames;
}
