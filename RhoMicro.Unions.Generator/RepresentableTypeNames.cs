#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.Unions;

using System;

readonly struct RepresentableTypeNames(
    String fullTypeName,
    String openTypeName,
    String simpleTypeName,
    String safeAlias)
{
    public readonly String FullTypeName = fullTypeName;
    public readonly String OpenTypeName = openTypeName;
    public readonly String SimpleTypeName = simpleTypeName;
    public readonly String SafeAlias = safeAlias;

    public static RepresentableTypeNames Create(UnionTypeAttribute attribute)
    {
        var openTypeName =
            (attribute.RepresentableTypeIsGenericParameter ?
            attribute.GenericRepresentableTypeName :
            attribute.RepresentableTypeSymbol?.ToOpenString()) ??
            String.Empty;
        var fullTypeName =
            (attribute.RepresentableTypeIsGenericParameter ?
            attribute.GenericRepresentableTypeName :
            attribute.RepresentableTypeSymbol?.ToFullString()) ??
            String.Empty;
        var simpleTypeName =
            (attribute.RepresentableTypeIsGenericParameter ?
            attribute.GenericRepresentableTypeName :
            attribute.RepresentableTypeSymbol?.Name) ??
            String.Empty;
        var safeAlias = attribute.Alias != null && SyntaxFacts.IsValidIdentifier(attribute.Alias) ?
                attribute.Alias :
                (attribute.RepresentableTypeIsGenericParameter ?
                attribute.GenericRepresentableTypeName :
                attribute.RepresentableTypeSymbol?.ToIdentifierCompatString()) ??
                simpleTypeName;

        var result = new RepresentableTypeNames(fullTypeName, openTypeName, simpleTypeName, safeAlias);

        return result;
    }
}