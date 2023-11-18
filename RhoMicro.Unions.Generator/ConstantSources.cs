namespace RhoMicro.Unions.Generator;

using System;

internal static class ConstantSources
{
    public const String Util =
    """
    file static class Util
    {
        public static TTo UnsafeConvert<TFrom, TTo>(in TFrom from)
        {
            var copy = from;

            return global::System.Runtime.CompilerServices.Unsafe.As<TFrom, TTo>(ref copy);
        }
    }
    """;
    public const String UnionInterfaceName = "IUnion";
    public const String InvalidTagStateThrow = "throw new global::System.InvalidOperationException(\"Unable to determine the represented type or value. The union type was likely not initialized correctly.\")";
    private const String _invalidExplicitCastThrow = "throw new global::System.InvalidOperationException($\"The union type instance cannot be converted to an instance of {0}{1}{2}.\")";
    private const String _invalidCreation = "throw new global::System.ArgumentException($\"The value provided for \\\"{3}\\\" cannot be converted to an instance of {0}{1}{2}.\", \"{3}\")";
    public static String InvalidConversionThrow(String typeName) =>
        String.Format(_invalidExplicitCastThrow, "{", typeName, "}");
    public static String InvalidCreationThrow(String unionTypeName, String valueName) =>
        String.Format(_invalidCreation, "{", unionTypeName, "}", valueName);
}
