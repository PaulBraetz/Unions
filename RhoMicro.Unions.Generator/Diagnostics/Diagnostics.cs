namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Threading;

internal static partial class Diagnostics
{
    readonly partial struct Id : IEquatable<Id>
    {
        public Id(Int32 value)
        {
            lock(_reservedValues)
            {
                if(!_reservedValues.Add(value))
                {
                    throw new ArgumentOutOfRangeException(nameof(value), "The value provided has already been reserved.");
                }
            }

            _value = value;
            _string = $"RUG{_value:0000}";
        }

        private readonly static HashSet<Int32> _reservedValues = new();

        private readonly Int32 _value;
        private readonly String _string;

        public static implicit operator String(Id id) => id._string;

        public static Boolean operator ==(Id left, Id right) => left.Equals(right);
        public static Boolean operator !=(Id left, Id right) => !(left == right);

        public override String ToString() => _string;

        public override Boolean Equals(Object obj) => obj is Id id && Equals(id);
        public Boolean Equals(Id other) => _value == other._value;
        public override Int32 GetHashCode() => -1937169414 + _value.GetHashCode();
    }

    static LocalizableString GetLocalized(String name) =>
            new LocalizableResourceString(
                name,
                Resources.Diagnostics.ResourceManager,
                typeof(Resources.Diagnostics));
}
