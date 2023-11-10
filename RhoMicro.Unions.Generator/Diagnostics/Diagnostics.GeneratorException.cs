namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    readonly partial struct Id
    {
        public static readonly Id GeneratorException = new(2);
    }
    public static class GeneratorException
    {
        public static Diagnostic Create(Exception exception)
        {
            var result = Diagnostic.Create(
                new DiagnosticDescriptor(
                    _id,
                    _title,
                    _message,
                    _category,
                    DiagnosticSeverity.Warning,
                    true),
                Location.None,
                exception.Message);

            return result;
        }

        const String _category = "RhoMicro.Unions.Generator";
        static readonly Id _id = Id.GeneratorException;
        static readonly LocalizableString _title = GetLocalized("GeneratorException_Title");
        static readonly LocalizableString _message = GetLocalized("GeneratorException_Message");
    }
}
