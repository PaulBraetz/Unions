namespace RhoMicro.Unions;
using RhoMicro.AttributeFactoryGenerator;

using System;
using System.Collections.Generic;

[GenerateFactory]
partial class UnionTypeSettingsAttribute : IEquatable<UnionTypeSettingsAttribute?>
{
    public override Boolean Equals(Object? obj) => Equals(obj as UnionTypeSettingsAttribute);
    public Boolean Equals(UnionTypeSettingsAttribute? other) => other is not null && base.Equals(other) && ToStringSetting == other.ToStringSetting && Layout == other.Layout && DiagnosticsLevel == other.DiagnosticsLevel && ConstructorAccessibility == other.ConstructorAccessibility;

    public Boolean IsReservedGenericTypeName(String name) => _reservedGenericTypeNames.Contains(name);

    public override Int32 GetHashCode()
    {
        var hashCode = 304521224;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + ToStringSetting.GetHashCode();
        hashCode = hashCode * -1521134295 + Layout.GetHashCode();
        hashCode = hashCode * -1521134295 + DiagnosticsLevel.GetHashCode();
        hashCode = hashCode * -1521134295 + ConstructorAccessibility.GetHashCode();
        return hashCode;
    }
}