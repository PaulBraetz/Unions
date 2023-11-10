namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    partial struct Id
    {
        public static readonly Id MissingUnionTypeAttribute = new(3);
    }
    public static class MissingUnionTypeAttribute
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
        static readonly Id _id = Id.MissingUnionTypeAttribute;
        static readonly LocalizableString _title = GetLocalized("MissingUnionTypeAttribute_Title");
        static readonly LocalizableString _message = GetLocalized("MissingUnionTypeAttribute_Message");
    }
}
