#pragma warning disable
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Explicit, Pack = 1)]
internal partial struct Union_3 : global::RhoMicro.Unions.Abstractions.IUnion<global::System.Int16, global::System.Int32, global::System.Int64>, global::System.IEquatable<Union_3>, global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int16, Union_3>, global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int32, Union_3>, global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int64, Union_3>
{
    #region Nested Types
    private enum Tag : Byte
    {
        Int16,
        Int32,
        Int64
    }

    [global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit)]
    private struct ValueTypeContainer
    {
        [global::System.Runtime.InteropServices.FieldOffset(0)]
        public readonly global::System.Int16 Int16;
        public ValueTypeContainer(global::System.Int16 value) => Int16 = value;
        [global::System.Runtime.InteropServices.FieldOffset(0)]
        public readonly global::System.Int32 Int32;
        public ValueTypeContainer(global::System.Int32 value) => Int32 = value;
        [global::System.Runtime.InteropServices.FieldOffset(0)]
        public readonly global::System.Int64 Int64;
        public ValueTypeContainer(global::System.Int64 value) => Int64 = value;
    }

    #endregion
    #region Constructors
    private Union_3(global::System.Int16 value)
    {
        __tag = Tag.Int16;
        __valueTypeContainer = new(value);
    }

    private Union_3(global::System.Int32 value)
    {
        __tag = Tag.Int32;
        __valueTypeContainer = new(value);
    }

    private Union_3(global::System.Int64 value)
    {
        __tag = Tag.Int64;
        __valueTypeContainer = new(value);
    }

    #endregion
    #region Fields
    /*
    0 [reference container]
    ...
    8 [tag]
    9 [value container]
    ..
    18 ~~~~ <- size is not 8n
    .. buffer for size to become 8n
    24 -----
    */
    [FieldOffset(0)]
    private readonly Object _referenceTypeContainer;
    [FieldOffset(8)]
    private readonly Tag __tag;
    [FieldOffset(9)]
    private readonly ValueTypeContainer __valueTypeContainer;
    #endregion
    #region Methods
    public TSuperset DownCast<TSuperset>()
        where TSuperset : global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int16, TSuperset>, global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int32, TSuperset>, global::RhoMicro.Unions.Abstractions.ISuperset<global::System.Int64, TSuperset> => __tag switch
        {
            Tag.Int16 => (this.__valueTypeContainer.Int16),
            Tag.Int32 => (this.__valueTypeContainer.Int32),
            Tag.Int64 => (this.__valueTypeContainer.Int64),
            _ => throw new global::System.InvalidOperationException("Unable to determine the represented value. The union type was likely not initialized correctly.")
        };
    public void Switch(global::System.Action<global::System.Int16> onInt16, global::System.Action<global::System.Int32> onInt32, global::System.Action<global::System.Int64> onInt64)
    {
        switch(__tag)
        {
            case Tag.Int16:
                onInt16.Invoke((this.__valueTypeContainer.Int16));
                return;
            case Tag.Int32:
                onInt32.Invoke((this.__valueTypeContainer.Int32));
                return;
            case Tag.Int64:
                onInt64.Invoke((this.__valueTypeContainer.Int64));
                return;
            default:
                throw new global::System.InvalidOperationException("Unable to determine the represented value. The union type was likely not initialized correctly.");
        }
    }

    public TResult Match<TResult>(global::System.Func<global::System.Int16, TResult> onInt16, global::System.Func<global::System.Int32, TResult> onInt32, global::System.Func<global::System.Int64, TResult> onInt64) => __tag switch
    {
        Tag.Int16 => onInt16.Invoke((this.__valueTypeContainer.Int16)),
        Tag.Int32 => onInt32.Invoke((this.__valueTypeContainer.Int32)),
        Tag.Int64 => onInt64.Invoke((this.__valueTypeContainer.Int64)),
        _ => throw new global::System.InvalidOperationException("Unable to determine the represented value. The union type was likely not initialized correctly.")
    };
    /// <inheritdoc/>
    public global::System.Boolean Is<T>() => typeof(T) == __tag switch
    {
        Tag.Int16 => typeof(global::System.Int16),
        Tag.Int32 => typeof(global::System.Int32),
        Tag.Int64 => typeof(global::System.Int64),
        _ => throw new global::System.InvalidOperationException("Unable to determine the represented value. The union type was likely not initialized correctly.")
    };
    /// <inheritdoc/>
    public T As<T>() => __tag switch
    {
        Tag.Int16 => typeof(T) == typeof(global::System.Int16) ? Util.UnsafeConvert<Int16, T>(this.__valueTypeContainer.Int16) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(T).Name}."),
        Tag.Int32 => typeof(T) == typeof(global::System.Int32) ? Util.UnsafeConvert<Int32, T>(this.__valueTypeContainer.Int32) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(T).Name}."),
        Tag.Int64 => typeof(T) == typeof(global::System.Int64) ? Util.UnsafeConvert<Int64, T>(this.__valueTypeContainer.Int64) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(T).Name}."),
        _ => throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(T).Name}.")
    };
    /// <summary>
    /// Gets a value indicating whether this instance is representing a value of type <c>global::System.Int16</c>.
    /// </summary>
    public global::System.Boolean IsInt16 => __tag == Tag.Int16;
    /// <summary>
    /// Attempts to retrieve the value represented by this instance as a <c>global::System.Int16</c>.
    /// </summary>
    /// <exception cref = "global::System.InvalidOperationException">Thrown if the instance is not representing a value of type <c>global::System.Int16</c>.</exception>
    public global::System.Int16 AsInt16 => __tag == Tag.Int16 ? (this.__valueTypeContainer.Int16) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int16).Name}.");
    /// <summary>
    /// Gets a value indicating whether this instance is representing a value of type <c>global::System.Int32</c>.
    /// </summary>
    public global::System.Boolean IsInt32 => __tag == Tag.Int32;
    /// <summary>
    /// Attempts to retrieve the value represented by this instance as a <c>global::System.Int32</c>.
    /// </summary>
    /// <exception cref = "global::System.InvalidOperationException">Thrown if the instance is not representing a value of type <c>global::System.Int32</c>.</exception>
    public global::System.Int32 AsInt32 => __tag == Tag.Int32 ? (this.__valueTypeContainer.Int32) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int32).Name}.");
    /// <summary>
    /// Gets a value indicating whether this instance is representing a value of type <c>global::System.Int64</c>.
    /// </summary>
    public global::System.Boolean IsInt64 => __tag == Tag.Int64;
    /// <summary>
    /// Attempts to retrieve the value represented by this instance as a <c>global::System.Int64</c>.
    /// </summary>
    /// <exception cref = "global::System.InvalidOperationException">Thrown if the instance is not representing a value of type <c>global::System.Int64</c>.</exception>
    public global::System.Int64 AsInt64 => __tag == Tag.Int64 ? (this.__valueTypeContainer.Int64) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int64).Name}.");

    #endregion
    #region Overrides & Equality
    public override String ToString()
    {
        var stringRepresentation = __tag switch
        {
            Tag.Int16 => __valueTypeContainer.Int16.ToString(),
            Tag.Int32 => __valueTypeContainer.Int32.ToString(),
            Tag.Int64 => __valueTypeContainer.Int64.ToString(),
            _ => throw new global::System.InvalidOperationException("Unable to determine the represented value. The union type was likely not initialized correctly.")
        };
        var result = $"Union_3({(__tag == Tag.Int16 ? "<Int16>" : "Int16")} | {(__tag == Tag.Int32 ? "<Int32>" : "Int32")} | {(__tag == Tag.Int64 ? "<Int64>" : "Int64")}){{{stringRepresentation}}}";
        return result;
    }

    public override Int32 GetHashCode() => __tag switch
    {
        Tag.Int16 => (this.__valueTypeContainer.Int16).GetHashCode(),
        Tag.Int32 => (this.__valueTypeContainer.Int32).GetHashCode(),
        Tag.Int64 => (this.__valueTypeContainer.Int64).GetHashCode(),
        _ => throw new InvalidOperationException()
    };
    public override Boolean Equals(Object obj) => obj is Union_3 union && Equals(union);
    public Boolean Equals(Union_3 obj) => __tag switch
    {
        Tag.Int16 => EqualityComparer<global::System.Int16>.Default.Equals((this.__valueTypeContainer.Int16), (obj.__valueTypeContainer.Int16)),
        Tag.Int32 => EqualityComparer<global::System.Int32>.Default.Equals((this.__valueTypeContainer.Int32), (obj.__valueTypeContainer.Int32)),
        Tag.Int64 => EqualityComparer<global::System.Int64>.Default.Equals((this.__valueTypeContainer.Int64), (obj.__valueTypeContainer.Int64)),
    };
    #endregion
    #region Conversion Operators
    /// <summary>
    /// Converts an instance of <see cref = "global::System.Int16"/> to the union type <see cref = "global::Union_3"/>.
    /// </summary>
    /// <param name = "value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator global::Union_3(global::System.Int16 value) => new(value);
    public static explicit operator global::System.Int16(global::Union_3 union) => union.__tag == Tag.Int16 ? (union.__valueTypeContainer.Int16) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int16).Name}.");
    /// <summary>
    /// Converts an instance of <see cref = "global::System.Int32"/> to the union type <see cref = "global::Union_3"/>.
    /// </summary>
    /// <param name = "value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator global::Union_3(global::System.Int32 value) => new(value);
    public static explicit operator global::System.Int32(global::Union_3 union) => union.__tag == Tag.Int32 ? (union.__valueTypeContainer.Int32) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int32).Name}.");
    /// <summary>
    /// Converts an instance of <see cref = "global::System.Int64"/> to the union type <see cref = "global::Union_3"/>.
    /// </summary>
    /// <param name = "value">The value to convert.</param>
    /// <returns>The converted value.</returns>
    public static implicit operator global::Union_3(global::System.Int64 value) => new(value);
    public static explicit operator global::System.Int64(global::Union_3 union) => union.__tag == Tag.Int64 ? (union.__valueTypeContainer.Int64) : throw new global::System.InvalidOperationException($"The union type instance cannot be converted to an instance of {typeof(global::System.Int64).Name}.");
    #endregion
}

file static class Util
{
    public static TTo UnsafeConvert<TFrom, TTo>(in TFrom from)
    {
        var copy = from;
        return global::System.Runtime.CompilerServices.Unsafe.As<TFrom, TTo>(ref copy);
    }
}