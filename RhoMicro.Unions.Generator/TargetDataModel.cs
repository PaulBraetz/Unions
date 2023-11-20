namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Net.Http;

sealed class TargetDataModel : IEquatable<TargetDataModel>
{
    private TargetDataModel(
        INamedTypeSymbol targetSymbol,
        TypeDeclarationSyntax targetDeclaration,
        SemanticModel semanticModel,
        AttributesModel attributes,
        OperatorOmissionModel operatorOmissions)
    {
        TargetSymbol = targetSymbol;
        TargetDeclaration = targetDeclaration;
        SemanticModel = semanticModel;
        Attributes = attributes;
        OperatorOmissions = operatorOmissions;
    }

    public readonly INamedTypeSymbol TargetSymbol;
    public readonly TypeDeclarationSyntax TargetDeclaration;
    public readonly SemanticModel SemanticModel;
    public readonly AttributesModel Attributes;
    public readonly OperatorOmissionModel OperatorOmissions;

    public void Deconstruct(
        out INamedTypeSymbol targetSymbol,
        out TypeDeclarationSyntax targetDeclaration,
        out SemanticModel semanticModel,
        out AttributesModel attributes) =>
        (targetSymbol, targetDeclaration, semanticModel, attributes) =
        (TargetSymbol, TargetDeclaration, SemanticModel, Attributes);

    public static TargetDataModel Create(TypeDeclarationSyntax targetDeclaration, SemanticModel semanticModel)
    {
        var targetSymbol = (semanticModel.GetDeclaredSymbol(targetDeclaration) as INamedTypeSymbol) ??
            throw new ArgumentException(
                $"targetDeclaration {targetDeclaration.Identifier.Text} could not be retrieved as an instance of ITypeSymbol from the semantic model provided.",
                nameof(targetDeclaration));

        var attributes = AttributesModel.Create(targetSymbol);
        var omissions = OperatorOmissionModel.Create(targetSymbol, attributes);
        var result = new TargetDataModel(
            targetSymbol,
            targetDeclaration,
            semanticModel,
            attributes,
            omissions);

        return result;
    }

    public override Boolean Equals(Object? obj) =>
        obj is TargetDataModel other && Equals(other);
    public Boolean Equals(TargetDataModel? other) =>
        other is not null && Attributes.Equals(other.Attributes);
    public override Int32 GetHashCode() => Attributes.GetHashCode();

    public static Boolean operator ==(TargetDataModel left, TargetDataModel right) => EqualityComparer<TargetDataModel>.Default.Equals(left, right);
    public static Boolean operator !=(TargetDataModel left, TargetDataModel right) => !(left == right);
}
