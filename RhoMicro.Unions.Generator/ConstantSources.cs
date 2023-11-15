namespace RhoMicro.Unions.Generator;

using System;

internal static class ConstantSources
{
    public const String UnionInterfaceName = "IUnion";
    public const String InvalidTagStateThrow = "throw new global::System.InvalidOperationException(\"Unable to determine the represented value. The union type was likely not initialized correctly.\")";
    public const String InvalidExplicitCastThrow = "throw new global::System.InvalidOperationException(\"The union type instance cannot be converted to an instance of {0}.\")";
}
