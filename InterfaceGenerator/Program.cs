using System.Text;

var result = new StringBuilder(
    """
    namespace RhoMicro.Unions.Abstractions;

    /// <summary>
    /// Represents a superset union type.
    /// </summary>
    /// <typeparam name="TSubset">The subset union type that this union type is a superset of.</typeparam>
    /// <typeparam name="TSuperset">This type (akin to <c>TSelf</c>).</typeparam>
    public interface ISuperset<TSubset, TSuperset>
        where TSuperset : ISuperset<TSubset, TSuperset>
    {
        /// <summary>
        /// Implicitly converts an instance of the subset union type to this superset union type.
        /// </summary>
        /// <param name="subset">The subset union type instance to convert.</param>
        static abstract implicit operator TSuperset(TSubset subset);
        /// <summary>
        /// Explicitly converts an instance of this superset union type to the subset union type.
        /// </summary>
        /// <param name="superset">The superset union type instance to convert.</param>
        static abstract explicit operator TSubset(TSuperset superset);
    }

    """
);
for(var i = 1; i < 33; i++)
{
    var typeParameters = String.Join(',', Enumerable.Range(1, i).Select(i => $"T{i}"));
    var constraints = String.Join(",\n", Enumerable.Range(1, i).Select(i => $"\t\tISuperset<T{i}, TSupersetUnion>"));
    var typeParamComments = String.Join("\n", Enumerable.Range(1, i).Select(i => $"/// <typeparam name=\"T{i}\">The {ordinalOf(i)} type unions of this type are able to represent.</typeparam>"));
    var definition =
        $"/// <summary>\n" +
        $"/// Represents a union type that is able to represent {i} type{(i > 1 ? "s" : String.Empty)}\n" +
        $"/// </summary>\n" +
        typeParamComments +
        $"\npublic interface IUnion<{typeParameters}>\n" +
        $"{{\n" +
        $"\t/// <summary>\n" +
        $"\t/// Safely converts this instance to a union type that is either a superset of or congruent to this one.\n" +
        $"\t/// </summary>\n" +
        $"\t/// <typeparam name=\"TSupersetUnion\">The type of union to convert this instance to.</typeparam>\n" +
        $"\tTSupersetUnion DownCast<TSupersetUnion>()\n" +
        $"\twhere TSupersetUnion : \n" +
        $"{constraints};\n" +
        $"}}";
    _ = result.Append(definition);
}

File.WriteAllText("../../../../RhoMicro.Unions/Abstractions/IUnion.cs", result.ToString());

static String ordinalOf(Int32 cardinal) => cardinal switch
{
    1 => "first",
    2 => "second",
    3 => "third",
    4 => "fourth",
    5 => "fifth",
    6 => "sixth",
    7 => "seventh",
    8 => "eigth",
    9 => "ninth",
    10 => "tenth",
    11 => "eleventh",
    12 => "twelfth",
    21 => "21st",
    22 => "22nd",
    31 => "31st",
    32 => "32nd",
    < 31 => $"{cardinal}th",
    _ => throw new ArgumentOutOfRangeException(nameof(cardinal), cardinal, $"{nameof(cardinal)} must be between 1 and 32 (inclusive).")
};
