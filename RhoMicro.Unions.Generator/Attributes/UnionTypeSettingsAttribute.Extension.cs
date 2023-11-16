namespace RhoMicro.Unions;
using RhoMicro.AttributeFactoryGenerator;

using System;

[GenerateFactory]
partial class UnionTypeSettingsAttribute : IEquatable<UnionTypeSettingsAttribute?>
{
    public override Boolean Equals(Object? obj) => Equals(obj as UnionTypeSettingsAttribute);
    public Boolean Equals(UnionTypeSettingsAttribute? other)
    {
        var result = other is not null &&
            ToStringSetting == other.ToStringSetting &&
            Layout == other.Layout;

        return result;
    }

    public override Int32 GetHashCode()
    {
        var hashCode = 436255176;
        hashCode = hashCode * -1521134295 + ToStringSetting.GetHashCode();
        hashCode = hashCode * -1521134295 + Layout.GetHashCode();
        return hashCode;
    }
}
