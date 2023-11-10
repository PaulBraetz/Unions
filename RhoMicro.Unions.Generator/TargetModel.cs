namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Collections.Generic;
using System.Threading;

sealed class TargetModel : IEquatable<TargetModel>
{
    public readonly SemanticModel SemanticModel;
    public readonly TypeDeclarationSyntax Declaration;
    public readonly ITypeSymbol Symbol;

    private TargetModel(SemanticModel semanticModel, TypeDeclarationSyntax declaration, ITypeSymbol symbol)
    {
        SemanticModel = semanticModel;
        Declaration = declaration;
        Symbol = symbol;
    }

    public override Boolean Equals(Object obj) => Equals(obj as TargetModel);
    public Boolean Equals(TargetModel other) => other is not null && EqualityComparer<SemanticModel>.Default.Equals(SemanticModel, other.SemanticModel) && EqualityComparer<TypeDeclarationSyntax>.Default.Equals(Declaration, other.Declaration);

    public static SourceDiagnosticsCarry<TargetModel> Create(
        GeneratorSyntaxContext context,
        CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        if(context.Node is not TypeDeclarationSyntax target)
        {
            return new SourceDiagnosticsCarry<TargetModel>();
        }

        var builder = new DiagnosticsModelBuilder();
        var semanticModel = context.SemanticModel;
        var targetSymbol = semanticModel.GetDeclaredSymbol(target, token) as ITypeSymbol ??
            throw new InvalidOperationException($"Unable to locate declared symbol for {target.Identifier.Text}.");
        var decoratedTargetSymbol = new FullNameTypeSymbolDecorator(targetSymbol);
        var initialContext = new TargetModel(semanticModel, target, decoratedTargetSymbol);

        DiagnosticsModel.DiagnoseTarget(target, semanticModel, builder, token);

        return new SourceDiagnosticsCarry<TargetModel>(initialContext, builder);
    }

    public override Int32 GetHashCode()
    {
        var hashCode = -2013048082;
        hashCode = hashCode * -1521134295 + EqualityComparer<SemanticModel>.Default.GetHashCode(SemanticModel);
        hashCode = hashCode * -1521134295 + EqualityComparer<TypeDeclarationSyntax>.Default.GetHashCode(Declaration);
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.IncludeNullability.GetHashCode(Symbol);
        return hashCode;
    }

    public static Boolean operator ==(TargetModel left, TargetModel right) => EqualityComparer<TargetModel>.Default.Equals(left, right);
    public static Boolean operator !=(TargetModel left, TargetModel right) => !(left == right);
}