namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;

internal static partial class Diagnostics
{
    partial struct Id
    {
        public static readonly Id DuplicateUnionTypeAttributes = new(1);
    }
    public static class DuplicateUnionTypeAttributes
    {
        public static Diagnostic Create(String unionTypeName, Location location)
        {
            var result = Diagnostic.Create(
                new DiagnosticDescriptor(
                    _id,
                    _title,
                    _message,
                    _category,
                    DiagnosticSeverity.Error,
                    true),
                location,
                unionTypeName);

            return result;
        }

        const String _category = "RhoMicro.Unions.Generator";
        static readonly Id _id = Id.DuplicateUnionTypeAttributes;
        static readonly LocalizableString _title = GetLocalized("DuplicateUnionTypeAttributes_Title");
        static readonly LocalizableString _message = GetLocalized("DuplicateUnionTypeAttributes_Message");
    }
}
