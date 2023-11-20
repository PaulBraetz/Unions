namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

sealed class TargetDataModel : IEquatable<TargetDataModel>
{
    private TargetDataModel(
        INamedTypeSymbol targetSymbol,
        TypeDeclarationSyntax targetDeclaration,
        SemanticModel semanticModel,
        AnnotationDataModel annotations,
        OperatorOmissionModel operatorOmissions,
        String valueTypeContainerName,
        String valueTypeContainerNamespace)
    {
        TargetSymbol = targetSymbol;
        TargetDeclaration = targetDeclaration;
        SemanticModel = semanticModel;
        Annotations = annotations;
        OperatorOmissions = operatorOmissions;
        ValueTypeContainerName = valueTypeContainerName;
        ValueTypeContainerNamespace = valueTypeContainerNamespace;

        FullValueTypeContainerName = $"{ValueTypeContainerNamespace}.{ValueTypeContainerName}";
    }

    public readonly INamedTypeSymbol TargetSymbol;
    public readonly TypeDeclarationSyntax TargetDeclaration;
    public readonly SemanticModel SemanticModel;
    public readonly AnnotationDataModel Annotations;
    public readonly OperatorOmissionModel OperatorOmissions;
    public readonly String ValueTypeContainerName;
    public readonly String ValueTypeContainerNamespace;
    public readonly String FullValueTypeContainerName;

    public static TargetDataModel Create(TypeDeclarationSyntax targetDeclaration, SemanticModel semanticModel)
    {
        var targetSymbol = (semanticModel.GetDeclaredSymbol(targetDeclaration) as INamedTypeSymbol) ??
            throw new ArgumentException(
                $"targetDeclaration {targetDeclaration.Identifier.Text} could not be retrieved as an instance of ITypeSymbol from the semantic model provided.",
                nameof(targetDeclaration));

        var annotations = AnnotationDataModel.Create(targetSymbol);
        var omissions = OperatorOmissionModel.Create(targetSymbol, annotations);

        var namePrefix = targetSymbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat).Replace(", ", "_")
            .Replace('<', '_')
            .Replace('>', '_');
        var valueTypeContainerName = $"{namePrefix}_ValueTypeContainer";
        var valueTypeContainerNamespace = "RhoMicro.Unions.Generated.Containers";

        var result = new TargetDataModel(
            targetSymbol,
            targetDeclaration,
            semanticModel,
            annotations,
            omissions,
            valueTypeContainerName,
            valueTypeContainerNamespace);

        return result;
    }
    public String GetSpecificAccessibility(RepresentableTypeData representableType)
    {
        var accessibility = Annotations.Settings.ConstructorAccessibility;

        if(accessibility == ConstructorAccessibilitySetting.PublicIfInconvertible &&
           OperatorOmissions.AllOmissions.Contains(representableType))
        {
            accessibility = ConstructorAccessibilitySetting.Public;
        }

        var result = accessibility == ConstructorAccessibilitySetting.Public ?
            "public" :
            "private";

        return result;
    }
    public override Boolean Equals(Object? obj) =>
        obj is TargetDataModel other && Equals(other);
    public Boolean Equals(TargetDataModel? other) =>
        other is not null && Annotations.Equals(other.Annotations);
    public override Int32 GetHashCode() => Annotations.GetHashCode();

    public static Boolean operator ==(TargetDataModel left, TargetDataModel right) => EqualityComparer<TargetDataModel>.Default.Equals(left, right);
    public static Boolean operator !=(TargetDataModel left, TargetDataModel right) => !(left == right);
}
