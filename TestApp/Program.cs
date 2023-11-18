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

    internal partial class Program
    {
        private static void Main(String[] _)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            unsafe
            {
                foreach(var t in new[]
                {
                    (((MyUnionType)0).ToString(), sizeof(MyUnionType)),
                    (((OneOf<Int32, String, IConvertible, List<String>, List<Int32>>)0).ToString(),
                    sizeof(OneOf<Int32, String, IConvertible, List<String>, List<Int32>>))
                })
                {
                    Console.WriteLine($"sizeof({t.Item1}): {t.Item2}");
                }
            }

            MyUnionType u = 32;
            Console.WriteLine(u);
            u = "Hello, World!";
            Console.WriteLine(u);
        }

        [UnionType(typeof(Int32))]
        [UnionType(typeof(String))]
        [UnionType(typeof(IConvertible))]
        [UnionType(typeof(List<Int32>), Alias = "StringList")]
        [UnionType(typeof(List<Int32>), Alias = "IntList")]
        [UnionTypeSettings(EmitDiagnostics = false)]
        readonly partial struct MyUnionType { }
    }
}