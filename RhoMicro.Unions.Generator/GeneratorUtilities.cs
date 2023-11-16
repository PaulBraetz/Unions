namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
internal static class GeneratorUtilities
{
    private static readonly HashSet<String> _attributeNames = new()
    {
        nameof(UnionTypeAttribute),
        nameof(UnionTypeAttribute).Substring(0, nameof(UnionTypeAttribute).Length-nameof(Attribute).Length),
        nameof(UnionTypeSettingsAttribute),
        nameof(UnionTypeSettingsAttribute).Substring(0, nameof(UnionTypeSettingsAttribute).Length-nameof(Attribute).Length),
        nameof(RelationAttribute),
        nameof(RelationAttribute).Substring(0, nameof(RelationAttribute).Length-nameof(Attribute).Length)
    };

    public static Boolean IsUnionDeclaration(SyntaxNode node, CancellationToken _)
    {
        var result = node is TypeDeclarationSyntax typeDeclaration &&
            typeDeclaration.AttributeLists
            .SelectMany(a => a.Attributes)
            .Select(a => a.Name.ToString())
            .Any(_attributeNames.Contains);

        return result;
    }
    public static Boolean IsPartial(this TypeDeclarationSyntax declarationSyntax) =>
        declarationSyntax.Modifiers.Any(SyntaxKind.PartialKeyword);
}
