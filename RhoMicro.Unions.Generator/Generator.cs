namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var models = context.SyntaxProvider.CreateSyntaxProvider(
                GeneratorUtilities.IsUnionDeclaration,
                TargetModel.Create)
            .Select(TypeDeclarationAttributeAggregate.Create)
            .Select()
            .Select(DiagnosticsCarryContextTerminal.Create);

        context.RegisterSourceOutput(models, (c, m) => m.AddToContext(c));
    }
}
sealed class SourceBuilder
{
    private String _targetName;
    private String _targetNamespace;
    private String _targetAccessibility;
    private String _conversionOperators;
    private String _switchMethod;
    private String _matchFunction;
    private String _downCastFunction;
}