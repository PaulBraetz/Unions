namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    readonly partial struct Id
    {
        public static readonly Id RecordTarget = new(6);
    }
    public static class RecordTarget
    {
        public static Diagnostic Create(Location location)
        {
            var result = Diagnostic.Create(
                new DiagnosticDescriptor(
                    _id,
                    _title,
                    _message,
                    _category,
                    DiagnosticSeverity.Error,
                    true),
                location);

            return result;
        }

        const String _category = "RhoMicro.Unions.Generator";
        static readonly Id _id = Id.RecordTarget;
        static readonly LocalizableString _title = GetLocalized("RecordTarget_Title");
        static readonly LocalizableString _message = GetLocalized("RecordTarget_Message");
    }
}
