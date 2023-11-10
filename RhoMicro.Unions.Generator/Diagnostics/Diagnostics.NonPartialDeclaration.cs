namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    partial struct Id
    {
        public static readonly Id NonPartialDeclaration = new(4);
    }
    public static class NonPartialDeclaration
    {
        public static Diagnostic Create(Location location)
        {
            var result = Diagnostic.Create(
                new DiagnosticDescriptor(
                    _id.ToString(),
                    _title,
                    _message,
                    _category,
                    DiagnosticSeverity.Error,
                    true),
                location);

            return result;
        }

        const String _category = "RhoMicro.Unions.Generator";
        static readonly Id _id = Id.NonPartialDeclaration;
        static readonly LocalizableString _title = GetLocalized("NonPartialDeclaration_Title");
        static readonly LocalizableString _message = GetLocalized("NonPartialDeclaration_Message");
    }
}
