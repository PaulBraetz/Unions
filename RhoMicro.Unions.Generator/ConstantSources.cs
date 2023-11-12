namespace RhoMicro.Unions.Generator;

using System;

internal static class ConstantSources
{
    public const String InvalidTagStateThrow = "throw new global::System.InvalidOperationException(\"The union type was not initialized correctly.\")";
    public const String InvalidExplicitCastThrow = "throw new global::System.InvalidOperationException(\"The union type instance cannot be converted to an instance of {0}.\")";
}
