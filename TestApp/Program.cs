using RhoMicro.Unions;
using OneOf;
using System.Globalization;

CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

unsafe
{
    foreach(var t in new[]
    {
        //(((Union)0).ToString(), sizeof(Union)),
        //(((Union_2)0).ToString(), sizeof(Union_2)),
        ("Tag", sizeof(Byte)),
        (nameof(Int16), sizeof(Int16)),
        (nameof(Int32), sizeof(Int32)),
        (nameof(Int64), sizeof(Int64)),
        (nameof(Object), sizeof(Object)),
        (((Union_3)0).ToString(), sizeof(Union_3))
    })
    {
        Console.WriteLine($"sizeof({t.Item1}): {t.Item2}");
    }
}

//[UnionType(typeof(Int16))]
//readonly partial struct Union
//{

//}

//[UnionType(typeof(Int32))]
//[UnionType(typeof(Int16))]
//readonly partial struct Union_2
//{

//}

[UnionType(typeof(Int64))]
[UnionType(typeof(Int32))]
[UnionType(typeof(Int16))]
readonly partial struct Union_3
{

}
