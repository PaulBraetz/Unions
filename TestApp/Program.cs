using OneOf;

using RhoMicro.Unions;

internal class Program
{
    private static void Main(String[] _)
    {
        Union u = DateTime.Now;
        //Output: Union(<DateTime> | Double | String){23/11/2023 17:58:58}
        Console.WriteLine(u);
        var eu = u.DownCast<CongruentUnion>();
        //Output: EquivalentUnion(<DateTime> | Double | String){23/11/2023 17:58:58}
        Console.WriteLine(eu);

        var r = Result<String>.CreateFromResult("Hello, World!");
        var error = Result<String>.CreateFromErrorMessage("FAIL!");

        if(r.IsErrorMessage)
        {
            //handle error
        } else if(r.IsResult)
        {
            //handle result
        }

        ////alternatively:
        //r.Switch(
        //    onErrorMessage: m =>/*handle error*/,
        //    onResult: r =>/*handle result*/);

        Union union = "Hello, World!";
        Console.WriteLine(union);
        union = DateTime.Now;
        Console.WriteLine(union);
        union = 2.5;
        Console.WriteLine(union);

        CongruentUnion congruent = "Hello, World! (from Congruent)";
        Console.WriteLine(congruent);
        union = congruent;
        Console.WriteLine(union);
        congruent = union;
        Console.WriteLine(congruent);

        SupersetUnion superset = "Hello, World! (from Superset)";
        Console.WriteLine(superset);
        union = (Union)superset;
        Console.WriteLine(union);
        superset = union;
        Console.WriteLine(superset);

        SubsetUnion subset = "Hello, World! (from Subset)";
        Console.WriteLine(subset);
        union = subset;
        Console.WriteLine(union);
        subset = (SubsetUnion)union;
        Console.WriteLine(subset);

        IntersectionUnion intersecting = "Hello, World! (from Intersecting)";
        Console.WriteLine(intersecting);
        union = (Union)intersecting;
        Console.WriteLine(union);
        intersecting = (IntersectionUnion)union;
        Console.WriteLine(intersecting);
    }
}

[UnionType(typeof(Int32), Storage = StorageOption.Field)]
[UnionTypeSettings(DowncastTypeName = "", MatchTypeName = "", GenericTValueName = "")]
readonly partial struct WrapperUnion;

[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
[UnionType(typeof(Double))]
[Relation(typeof(CongruentUnion))]
[Relation(typeof(SubsetUnion))]
[Relation(typeof(SupersetUnion))]
[Relation(typeof(IntersectionUnion))]
readonly partial struct Union;

[UnionType(typeof(Double))]
[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
sealed partial class CongruentUnion;

[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
partial class SubsetUnion;

[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
[UnionType(typeof(Double))]
[UnionType(typeof(Int32))]
partial struct SupersetUnion;

[UnionType(typeof(Int16))]
[UnionType(typeof(String))]
[UnionType(typeof(Double))]
[UnionType(typeof(List<Byte>))]
partial class IntersectionUnion;
