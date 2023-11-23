using RhoMicro.Unions;

using System.ComponentModel;

internal class Program
{
    private static void Main(String[] _)
    {
        Union u = DateTime.Now;
        //Output: Union(<DateTime> | Double | String){23/11/2023 17:58:58}
        Console.WriteLine(u);
        var eu = u.DownCast<EquivalentUnion>();
        //Output: EquivalentUnion(<DateTime> | Double | String){23/11/2023 17:58:58}
        Console.WriteLine(eu);

        var r = Result<String>.CreateFromResult("Hello, World!");
        if(r.IsErrorMessage)
        {
            //handle error
        } else if(r.IsResult)
        {
            //handle result
        }

        //alternatively:
        r.Switch(
            onErrorMessage: m =>/*handle error*/,
            onResult: r =>/*handle result*/);
    }
}

[UnionType(typeof(String), Alias = "ErrorMessage")]
[UnionType(nameof(T), Alias = "Result")]
readonly partial struct Result<T>;

[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
[UnionType(typeof(Double))]
readonly partial struct Union;

[UnionType(typeof(Double))]
[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
sealed partial class EquivalentUnion;