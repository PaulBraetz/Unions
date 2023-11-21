#pragma warning disable IDE0210 // Convert to top-level statements
#pragma warning disable IDE0161 // Convert to file-scoped namespace

namespace TestApp.MoreNames.Models
{
    using RhoMicro.Unions;

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
            for(var i = 0; i < 100; i++)
            {
                Foo().Switch(
                e => Console.WriteLine(e),
                r => Console.WriteLine(r),
                u => Console.WriteLine(u));
            }
        }

        static ApiResult Foo()
        {
            var value = (Int32)(Random.Shared.NextDouble() * 3);
            var result = value switch
            {
                0 => (ApiResult)new Result("Yay, Success!"),
                1 => (ApiResult)new Error(value, "Nay, Error!"),
                _ => (ApiResult)new Unknown(),
            };
            return result;
        }

        [UnionType(typeof(Result))]
        [UnionType(typeof(Error))]
        [UnionType(typeof(Unknown))]
        readonly partial struct ApiResult;

        readonly record struct Unknown;
        readonly record struct Result(String Value);
        readonly record struct Error(Int32 Code, String Message);
    }

    [UnionType(typeof(Int32))]
    [UnionType(typeof(String))]
    [UnionType(typeof(FooMix))]
    partial class TestUnion;

    record struct Foo(TimeSpan Bar);
    record struct FooMix(DateTime Bar, Foo Foo, String Int);
}