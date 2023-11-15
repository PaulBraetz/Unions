using RhoMicro.Unions;

Union union = 400;
Console.WriteLine(union);
union = "huh";

var eqUnion = union.DownCast<EquivalentUnion>();
Console.WriteLine(eqUnion);

[UnionType(typeof(String))]
[UnionType(typeof(Int32))]
readonly partial struct Union
{

}

[UnionType(typeof(Int32))]
[UnionType(typeof(String))]
readonly partial struct EquivalentUnion { }

[UnionType(typeof(String))]
readonly partial struct StringAlias
{

}

[UnionType(typeof(Int32))]
readonly partial struct Int32Alias
{

}