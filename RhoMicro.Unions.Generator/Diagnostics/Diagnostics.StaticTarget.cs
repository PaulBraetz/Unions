namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    readonly partial struct Id
    {
        public static readonly Id StaticTarget = new(5);
    }
    public static class StaticTarget
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
        static readonly Id _id = Id.StaticTarget;
        static readonly LocalizableString _title = GetLocalized("StaticTarget_Title");
        static readonly LocalizableString _message = GetLocalized("StaticTarget_Message");
    }
}
