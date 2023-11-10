namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Immutable;
using System.Globalization;
using System.Threading;

#pragma warning disable RS1009 // Only internal implementations of this interface are allowed
sealed class FullNameTypeSymbolDecorator : ITypeSymbol
#pragma warning restore RS1009 // Only internal implementations of this interface are allowed
{
    public FullNameTypeSymbolDecorator(ITypeSymbol decorated) => _decorated = decorated;
    private readonly ITypeSymbol _decorated;

    public override String ToString() => _decorated.ToFullString();

    public ISymbol FindImplementationForInterfaceMember(ISymbol interfaceMember) => _decorated.FindImplementationForInterfaceMember(interfaceMember);
    public String ToDisplayString(NullableFlowState topLevelNullability, SymbolDisplayFormat format = null) => _decorated.ToDisplayString(topLevelNullability, format);
    public ImmutableArray<SymbolDisplayPart> ToDisplayParts(NullableFlowState topLevelNullability, SymbolDisplayFormat format = null) => _decorated.ToDisplayParts(topLevelNullability, format);
    public String ToMinimalDisplayString(SemanticModel semanticModel, NullableFlowState topLevelNullability, Int32 position, SymbolDisplayFormat format = null) => _decorated.ToMinimalDisplayString(semanticModel, topLevelNullability, position, format);
    public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, NullableFlowState topLevelNullability, Int32 position, SymbolDisplayFormat format = null) => _decorated.ToMinimalDisplayParts(semanticModel, topLevelNullability, position, format);
    public ITypeSymbol WithNullableAnnotation(NullableAnnotation nullableAnnotation) => _decorated.WithNullableAnnotation(nullableAnnotation);

    public TypeKind TypeKind => _decorated.TypeKind;

    public INamedTypeSymbol BaseType => _decorated.BaseType;

    public ImmutableArray<INamedTypeSymbol> Interfaces => _decorated.Interfaces;

    public ImmutableArray<INamedTypeSymbol> AllInterfaces => _decorated.AllInterfaces;

    public Boolean IsReferenceType => _decorated.IsReferenceType;

    public Boolean IsValueType => _decorated.IsValueType;

    public Boolean IsAnonymousType => _decorated.IsAnonymousType;

    public Boolean IsTupleType => _decorated.IsTupleType;

    public Boolean IsNativeIntegerType => _decorated.IsNativeIntegerType;

    public ITypeSymbol OriginalDefinition => _decorated.OriginalDefinition;

    public SpecialType SpecialType => _decorated.SpecialType;

    public Boolean IsRefLikeType => _decorated.IsRefLikeType;

    public Boolean IsUnmanagedType => _decorated.IsUnmanagedType;

    public Boolean IsReadOnly => _decorated.IsReadOnly;

    public Boolean IsRecord => _decorated.IsRecord;

    public NullableAnnotation NullableAnnotation => _decorated.NullableAnnotation;

    public ImmutableArray<ISymbol> GetMembers() => _decorated.GetMembers();
    public ImmutableArray<ISymbol> GetMembers(String name) => _decorated.GetMembers(name);
    public ImmutableArray<INamedTypeSymbol> GetTypeMembers() => _decorated.GetTypeMembers();
    public ImmutableArray<INamedTypeSymbol> GetTypeMembers(String name) => _decorated.GetTypeMembers(name);
    public ImmutableArray<INamedTypeSymbol> GetTypeMembers(String name, Int32 arity) => _decorated.GetTypeMembers(name, arity);

    public Boolean IsNamespace => _decorated.IsNamespace;

    public Boolean IsType => _decorated.IsType;

    public ImmutableArray<AttributeData> GetAttributes() => _decorated.GetAttributes();
    public void Accept(SymbolVisitor visitor) => _decorated.Accept(visitor);
    public TResult Accept<TResult>(SymbolVisitor<TResult> visitor) => _decorated.Accept(visitor);
    public String GetDocumentationCommentId() => _decorated.GetDocumentationCommentId();
    public String GetDocumentationCommentXml(CultureInfo preferredCulture = null, Boolean expandIncludes = false, CancellationToken cancellationToken = default) => _decorated.GetDocumentationCommentXml(preferredCulture, expandIncludes, cancellationToken);
    public String ToDisplayString(SymbolDisplayFormat format = null) => _decorated.ToDisplayString(format);
    public ImmutableArray<SymbolDisplayPart> ToDisplayParts(SymbolDisplayFormat format = null) => _decorated.ToDisplayParts(format);
    public String ToMinimalDisplayString(SemanticModel semanticModel, Int32 position, SymbolDisplayFormat format = null) => _decorated.ToMinimalDisplayString(semanticModel, position, format);
    public ImmutableArray<SymbolDisplayPart> ToMinimalDisplayParts(SemanticModel semanticModel, Int32 position, SymbolDisplayFormat format = null) => _decorated.ToMinimalDisplayParts(semanticModel, position, format);
    public Boolean Equals(ISymbol other, SymbolEqualityComparer equalityComparer) => _decorated.Equals(other, equalityComparer);

    public SymbolKind Kind => _decorated.Kind;

    public String Language => _decorated.Language;

    public String Name => _decorated.Name;

    public String MetadataName => _decorated.MetadataName;

    public Int32 MetadataToken => _decorated.MetadataToken;

    public ISymbol ContainingSymbol => _decorated.ContainingSymbol;

    public IAssemblySymbol ContainingAssembly => _decorated.ContainingAssembly;

    public IModuleSymbol ContainingModule => _decorated.ContainingModule;

    public INamedTypeSymbol ContainingType => _decorated.ContainingType;

    public INamespaceSymbol ContainingNamespace => _decorated.ContainingNamespace;

    public Boolean IsDefinition => _decorated.IsDefinition;

    public Boolean IsStatic => _decorated.IsStatic;

    public Boolean IsVirtual => _decorated.IsVirtual;

    public Boolean IsOverride => _decorated.IsOverride;

    public Boolean IsAbstract => _decorated.IsAbstract;

    public Boolean IsSealed => _decorated.IsSealed;

    public Boolean IsExtern => _decorated.IsExtern;

    public Boolean IsImplicitlyDeclared => _decorated.IsImplicitlyDeclared;

    public Boolean CanBeReferencedByName => _decorated.CanBeReferencedByName;

    public ImmutableArray<Location> Locations => _decorated.Locations;

    public ImmutableArray<SyntaxReference> DeclaringSyntaxReferences => _decorated.DeclaringSyntaxReferences;

    public Accessibility DeclaredAccessibility => _decorated.DeclaredAccessibility;

    ISymbol ISymbol.OriginalDefinition => ((ISymbol)_decorated).OriginalDefinition;

    public Boolean HasUnsupportedMetadata => _decorated.HasUnsupportedMetadata;

    public Boolean Equals(ISymbol other) => _decorated.Equals(other);
}
