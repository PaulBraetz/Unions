using OneOf;

using RhoMicro.Unions;
using RhoMicro.Unions.Abstractions;

using System.Runtime.InteropServices;

unsafe
{
    Console.WriteLine($"sizeof tag: {sizeof(Union.Tag)}");
    Console.WriteLine($"sizeof reference type container: {sizeof(Object)}");
    Console.WriteLine($"sizeof value type container: {sizeof(Union.ValueTypeContainer)}");
    Console.WriteLine($"sizeof union: {sizeof(Union)}");
    Console.WriteLine($"sizeof reference OneOf: {sizeof(OneOf<Int32, String, Byte, List<Double>>)}");
}

Union unionInstance = "Hello, World!";
Console.WriteLine(unionInstance);
unionInstance = 11;
Console.WriteLine(unionInstance);
unionInstance = (Byte)255;
Console.WriteLine(unionInstance);
unionInstance = new List<Double>() { };
Console.WriteLine(unionInstance);

//SubsetUnion s = 17;
//unionInstance = s;
Console.WriteLine(unionInstance);
var converted = unionInstance.DownCast<Union>();
Console.WriteLine(converted);

[UnionType(typeof(String))]
class NonPartialUnion { }

[SubsetOf(typeof(String))]
partial class MissingAttributeUnion { }

[UnionType(typeof(String))]
[UnionType(typeof(String))]
partial class DuplicateUnion { }

[UnionType(typeof(String))]
record RecordUnion { }

[UnionType(typeof(Int32))]
[UnionType(typeof(String))]
[UnionType(typeof(Byte))]
[UnionType(typeof(List<Double>))]
[SupersetOf(typeof(SubsetUnion))]
readonly partial struct Union
{

}

[UnionType(typeof(Int32))]
[SubsetOf(typeof(Union))]
readonly partial struct SubsetUnion
{

}

partial struct SubsetUnion
    : IUnion<Int32>,
    ISuperset<Int32, SubsetUnion>
{
    private readonly Int32 _value;

    private SubsetUnion(Int32 value) => _value = value;

    public TResult Match<TResult>(Func<Int32, TResult> projection1) =>
        projection1.Invoke(_value);
    public TSupersetUnion DownCast<TSupersetUnion>()
        where TSupersetUnion : ISuperset<Int32, TSupersetUnion> =>
        _value;

    public static implicit operator SubsetUnion(Int32 value) => new(value);
    public static explicit operator Int32(SubsetUnion union) => union._value;

    public static implicit operator Union(SubsetUnion subset) => subset._value;
    public static explicit operator SubsetUnion(Union union) =>
        union.Match(
            static v => v,
            static v => throw new InvalidOperationException(),
            static v => throw new InvalidOperationException(),
            static v => throw new InvalidOperationException());
}

#nullable disable
partial struct Union :
    IEquatable<Union>,
    IUnion<Int32, String, Byte, List<Double>>,
    ISuperset<Int32, Union>,
    ISuperset<String, Union>,
    ISuperset<Byte, Union>,
    ISuperset<List<Double>, Union>,
    ISuperset<SubsetUnion, Union>
{
    #region Nested Types
    //sort these reference type first
    //make private when generating
    public enum Tag : Byte
    {
        String,
        List_Double,
        Int32,
        Byte
    }
    //make this private when generating
    [StructLayout(LayoutKind.Explicit)]
    public struct ValueTypeContainer
    {
        [FieldOffset(0)]
        public Int32 Int32Value;
        [FieldOffset(0)]
        public Byte ByteValue;
    }
    #endregion
    #region Constructors
    private Union(String stringValue)
    {
        _tag = Tag.String;
        _referenceTypeContainer = stringValue;
    }
    private Union(List<Double> listValue)
    {
        _tag = Tag.List_Double;
        _referenceTypeContainer = listValue;
    }
    private Union(Int32 int32Value)
    {
        _tag = Tag.Int32;
        _valueTypeContainer.Int32Value = int32Value;
    }
    private Union(Byte byteValue)
    {
        _tag = Tag.Byte;
        _valueTypeContainer.ByteValue = byteValue;
    }
    #endregion
    #region Fields
    private readonly Tag _tag;
    //generate this on condition of possible reference type value
    private readonly Object _referenceTypeContainer;
    //generate this on condition of possible value type value
    private readonly ValueTypeContainer _valueTypeContainer;
    #endregion
    #region Switch & Match
    public readonly void Switch(
        Action<Int32> onInt32,
        Action<String> onString,
        Action<Byte> onByte,
        Action<List<Double>> onList)
    {
        switch(_tag)
        {
            case Tag.String:
                onString.Invoke((String)_referenceTypeContainer);
                return;
            case Tag.List_Double:
                onList.Invoke((List<Double>)_referenceTypeContainer);
                return;
            case Tag.Int32:
                onInt32.Invoke(_valueTypeContainer.Int32Value);
                return;
            case Tag.Byte:
                onByte.Invoke(_valueTypeContainer.ByteValue);
                return;
            default:
                throw new InvalidOperationException();
        }
    }
    public readonly TResult Match<TResult>(
        Func<Int32, TResult> onInt32,
        Func<String, TResult> onString,
        Func<Byte, TResult> onByte,
        Func<List<Double>, TResult> onList) =>
        _tag switch
        {
            Tag.String => onString.Invoke((String)_referenceTypeContainer),
            Tag.List_Double => onList.Invoke((List<Double>)_referenceTypeContainer),
            Tag.Int32 => onInt32.Invoke(_valueTypeContainer.Int32Value),
            Tag.Byte => onByte.Invoke(_valueTypeContainer.ByteValue),
            _ => throw new InvalidOperationException()
        };
    #endregion
    #region ToString
#nullable restore
    public override readonly String? ToString() =>
        _tag < Tag.Int32 ?
            _referenceTypeContainer?.ToString() :
            _tag switch
            {
                Tag.Int32 => _valueTypeContainer.Int32Value.ToString(),
                Tag.Byte => _valueTypeContainer.ByteValue.ToString(),
                _ => throw new InvalidOperationException()
            };
#nullable disable
    #endregion
    #region Operators
    //generated due to union type attr
    public static implicit operator Union(String value) => new(value);
    public static explicit operator String(Union union) =>
        union._tag == Tag.String ?
            (String)union._referenceTypeContainer :
            throw new InvalidOperationException();

    public static implicit operator Union(List<Double> value) => new(value);
    public static explicit operator List<Double>(Union union) =>
        union._tag == Tag.List_Double ?
            (List<Double>)union._referenceTypeContainer :
            throw new InvalidOperationException();

    public static implicit operator Union(Int32 value) => new(value);
    public static explicit operator Int32(Union union) =>
        union._tag == Tag.Int32 ?
            union._valueTypeContainer.Int32Value :
            throw new InvalidOperationException();

    public static implicit operator Union(Byte value) => new(value);
    public static explicit operator Byte(Union union) =>
        union._tag == Tag.Byte ?
            union._valueTypeContainer.ByteValue :
            throw new InvalidOperationException();

    //generated due to superset attr
    public static implicit operator Union(SubsetUnion subsetUnion) => subsetUnion.DownCast<Union>();
    public static explicit operator SubsetUnion(Union union) =>
        union._tag switch { Tag.Int32 => (SubsetUnion)union._valueTypeContainer.Int32Value, _ => throw new InvalidOperationException() };
    #endregion
    #region Equality & Hashing
    public override readonly Int32 GetHashCode() =>
        _tag < Tag.Int32 ?
            _referenceTypeContainer?.GetHashCode() ?? 0 :
            _tag switch
            {
                Tag.Int32 => _valueTypeContainer.Int32Value.GetHashCode(),
                Tag.Byte => _valueTypeContainer.ByteValue.GetHashCode(),
                _ => throw new InvalidOperationException()
            };
    public override readonly Boolean Equals(Object obj) =>
        obj is Union union && Equals(union);
    public readonly Boolean Equals(Union obj) =>
        _tag switch
        {
            Tag.String => EqualityComparer<String>.Default.Equals((String)_referenceTypeContainer, (String)obj._referenceTypeContainer),
            Tag.List_Double => EqualityComparer<List<Double>>.Default.Equals((List<Double>)_referenceTypeContainer, (List<Double>)obj._referenceTypeContainer),
            Tag.Int32 => EqualityComparer<Int32>.Default.Equals(_valueTypeContainer.Int32Value, obj._valueTypeContainer.Int32Value),
            Tag.Byte => EqualityComparer<Byte>.Default.Equals(_valueTypeContainer.ByteValue, obj._valueTypeContainer.ByteValue),
            _ => throw new InvalidOperationException()
        };
    #endregion
    #region Conversion & Creation
    public readonly TSupersetUnion DownCast<TSupersetUnion>()
        where TSupersetUnion :
            ISuperset<Int32, TSupersetUnion>,
            ISuperset<String, TSupersetUnion>,
            ISuperset<Byte, TSupersetUnion>,
            ISuperset<List<Double>, TSupersetUnion> =>
        _tag switch
        {
            Tag.String => (String)_referenceTypeContainer,
            Tag.List_Double => (List<Double>)_referenceTypeContainer,
            Tag.Int32 => _valueTypeContainer.Int32Value,
            Tag.Byte => _valueTypeContainer.ByteValue,
            _ => throw new InvalidOperationException()
        };
    #endregion
}
#nullable restore