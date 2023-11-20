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

    internal partial class Program
    {
        private static void Main(String[] _)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            unsafe
            {
                foreach(var t in new[]
                {
                    (((Result<Int32>.MyUnionType)0).ToString(), sizeof(Result<Int32>.MyUnionType)),
                    (((OneOf<Int32, String, IConvertible, List<String>, List<Int32>>)0).ToString(),
                    sizeof(OneOf<Int32, String, IConvertible, List<String>, List<Int32>>))
                })
                {
                    Console.WriteLine($"sizeof({t.Item1}): {t.Item2}");
                }
            }

            Result<Int32>.MyUnionType u = 32;
            Console.WriteLine(u);
            Console.WriteLine(u.GetRepresentedType());
            u = "Hello, World!";
            Console.WriteLine(u);
            Console.WriteLine(u.GetRepresentedType());
            IConvertible newHello = "Hello, New World!";
            u = Result<Int32>.MyUnionType.Create(newHello);
            Console.WriteLine(u);
            Console.WriteLine(u.GetRepresentedType());

            var result = GetString();
            result.Switch(
                onString: static s => Console.WriteLine(s),
                onBoolean: static b => Console.WriteLine($"Failed {b}"));
        }

        [UnionType(typeof(String))]
        [UnionType(typeof(Double))]
        [UnionType(typeof(Int32))]
        readonly partial struct Stringifyable { }

        static String StringifyString(String s) => Stringify(s);
        static String Stringify(Stringifyable stringifyable)
        {
            return stringifyable.Match(
                s => s,
                d => d.ToString("0.00"),
                i => i.ToString());
        }

        static ResultUnion GetString() => "Hello, World!";

        [UnionType(typeof(String))]
        [UnionType(typeof(Boolean))]
        readonly partial struct ResultUnion { }

        [UnionType(nameof(T))] //setting: box, value, auto
        /*
                | box |value| auto | field
         struct | rc! | vc  | vc   | cc
         class  | rc  | rc! | rc   | cc
         none   | rc! | vc! | rc!  | cc
        */
        sealed partial class Result<T>
            where T : struct
        {
            T _fieldForT;

            Object _container;

            [StructLayout(LayoutKind.Explicit)]
            readonly struct Container
            {
                [FieldOffset(0)]
                public readonly T T;
            }

            static Result<String> Create() => new Result<string>();
        }
    }
}