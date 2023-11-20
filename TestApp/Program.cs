#pragma warning disable IDE0210 // Convert to top-level statements
#pragma warning disable IDE0161 // Convert to file-scoped namespace

namespace TestApp
{
    using RhoMicro.Unions;
    using OneOf;
    using System.Globalization;
    using System.Numerics;
    using System.Collections;
    using RhoMicro.Unions.Abstractions;
    using System.Runtime.InteropServices;
    using System.ComponentModel;

    internal partial class Program
    {
        private static void Main(String[] _0)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            unsafe
            {
                foreach(var t in new[]
                {
                    (((Union<Stream>)0).ToString(), sizeof(Union<Stream>)),
                    (new OneOf<Stream>(0).ToString(), sizeof(OneOf<Stream>))
                })
                {
                    Console.WriteLine($"sizeof({t.Item1}): {t.Item2}");
                }
            }
        }

        [UnionType(typeof(IEnumerable<List<String>>))]
        [UnionType(typeof(Int128))]
        [UnionType(typeof(Int32))]
        readonly partial struct MyUnion_1 { }

        [UnionType(typeof(Int32))]
        [UnionType(typeof(String))]
        [UnionType(typeof(List<Int32>))]
        [UnionType(typeof(List<DateTime>), Alias = "DateList")]
        [UnionType(nameof(T))]
        readonly partial struct Union<T>
            where T : class
        {

        }

        sealed class OneOf<T> : OneOfBase<Int32, String, List<Int32>, List<DateTime>, T>
            where T : class
        {
            public OneOf(OneOf<Int32, String, List<Int32>, List<DateTime>, T> input) : base(input)
            {
            }
        }
    }
}