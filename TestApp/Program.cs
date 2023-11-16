#pragma warning disable IDE0210 // Convert to top-level statements
#pragma warning disable IDE0161 // Convert to file-scoped namespace

namespace TestApp
{
    using RhoMicro.Unions;
    using OneOf;
    using System.Globalization;
    using System.Numerics;
    using System.Collections;

    internal partial class Program
    {
        private static void Main(String[] _)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            unsafe
            {
                foreach(var t in new[]
                {
                    (((Union1)0).ToString(), sizeof(Union1)),
                    (((OneOf<Int16, Int32, Object, IEnumerable>)0).ToString(), sizeof(OneOf<Int16, Int32, Object, IEnumerable>))
                })
                {
                    Console.WriteLine($"sizeof({t.Item1}): {t.Item2}");
                }
            }
        }

        [UnionType(typeof(Int16))]
        [UnionType(typeof(Int32))]
        [UnionType(typeof(Object))]
        [UnionType(typeof(IEnumerable))]
        [UnionTypeSettings(Layout = LayoutSetting.Small)]
        readonly partial struct Union1
        {

        }
    }
}