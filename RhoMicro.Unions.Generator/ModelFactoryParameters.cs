namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Net.Http;

sealed class ModelFactoryParameters : IEquatable<ModelFactoryParameters>
{
    private ModelFactoryParameters(
        ITypeSymbol targetSymbol,
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

    public readonly ITypeSymbol TargetSymbol;
    public readonly TypeDeclarationSyntax TargetDeclaration;
    public readonly SemanticModel SemanticModel;
    public readonly AttributesModel Attributes;
    public readonly OperatorOmissionModel OperatorOmissions;

    public void Deconstruct(
        out ITypeSymbol targetSymbol,
        out TypeDeclarationSyntax targetDeclaration,
        out SemanticModel semanticModel,
        out AttributesModel attributes) =>
        (targetSymbol, targetDeclaration, semanticModel, attributes) =
        (TargetSymbol, TargetDeclaration, SemanticModel, Attributes);

    public static ModelFactoryParameters Create(TypeDeclarationSyntax targetDeclaration, SemanticModel semanticModel)
    {
        var targetSymbol = (semanticModel.GetDeclaredSymbol(targetDeclaration) as ITypeSymbol) ??
            throw new ArgumentException(
                $"targetDeclaration {targetDeclaration.Identifier.Text} could not be retrieved as an instance of ITypeSymbol from the semantic model provided.",
                nameof(targetDeclaration));

        var attributes = AttributesModel.Create(targetSymbol);
        var omissions = OperatorOmissionModel.Create(targetSymbol, attributes);
        var result = new ModelFactoryParameters(
            targetSymbol,
            targetDeclaration,
            semanticModel,
            attributes,
            omissions);

        return result;
    }

    public override Boolean Equals(Object? obj) =>
        obj is ModelFactoryParameters other && Equals(other);
    public Boolean Equals(ModelFactoryParameters? other) =>
        other is not null && Attributes.Equals(other.Attributes);
    public override Int32 GetHashCode() => Attributes.GetHashCode();

    public static Boolean operator ==(ModelFactoryParameters left, ModelFactoryParameters right) => EqualityComparer<ModelFactoryParameters>.Default.Equals(left, right);
    public static Boolean operator !=(ModelFactoryParameters left, ModelFactoryParameters right) => !(left == right);
}
