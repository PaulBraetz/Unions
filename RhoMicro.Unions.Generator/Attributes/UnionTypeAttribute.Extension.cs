#pragma warning disable CS8618

namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.AttributeFactoryGenerator;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;

[GenerateFactory]
public partial class UnionTypeAttribute : IEquatable<UnionTypeAttribute?>
{
    [ExcludeFromFactory]
    public UnionTypeAttribute(Object representableTypeSymbolContainer) =>
        _representableTypeSymbolContainer = representableTypeSymbolContainer;

    private String _safeAlias;

    private String? _commentRef;
    public String CommentRef => _commentRef ??=
        (RepresentableTypeIsGenericParameter ?
        $"<typeparamref name=\"{GenericRepresentableTypeName}\"/>" :
        $"<see cref=\"{RepresentableTypeSymbol?.ToOpenString().Replace('<', '{').Replace('>', '}')}\"/>");

    private String? _openTypeName;
    public String OpenTypeName => _openTypeName ??=
        (RepresentableTypeIsGenericParameter ?
        GenericRepresentableTypeName :
        RepresentableTypeSymbol?.ToOpenString()) ??
        throw new InvalidOperationException("Unable to determine representable type name; neither a generic nor a concrete name could be determined.");

    private String? _closedTypeName;
    public String FullTypeName => _closedTypeName ??=
        (RepresentableTypeIsGenericParameter ?
        GenericRepresentableTypeName :
        RepresentableTypeSymbol?.ToOpenString()) ??
        throw new InvalidOperationException("Unable to determine representable type name; neither a generic nor a concrete name could be determined.");

    private String? _typeName;
    public String TypeName => _typeName ??=
        (RepresentableTypeIsGenericParameter ?
        GenericRepresentableTypeName :
        RepresentableTypeSymbol?.Name) ??
        throw new InvalidOperationException("Unable to determine representable type name; neither a generic nor a concrete name could be determined.");

    public Boolean IsValueType(INamedTypeSymbol target)
    {
        if(!RepresentableTypeIsGenericParameter)
        {
            return RepresentableTypeSymbol!.IsValueType;
        }


        //TODO: refactor attribute into factory for representablyTypeData model
        // factory creates target type => essentially decorator with all the util
    }

    public String GetConvertedInstanceVariableExpression(ITypeSymbol target, String targetType, String instance = "this") =>
        RepresentableTypeSymbol.IsValueType ?
        $"Util.UnsafeConvert<{SafeAlias}, {targetType}>({instance}.__valueTypeContainer.{SafeAlias})" :
        $"(({targetType}){instance}.__referenceTypeContainer)";
    public String GetInstanceVariableExpression(ITypeSymbol target, String instance = "this") =>
        RepresentableTypeSymbol.IsValueType ?
        $"({instance}.__valueTypeContainer.{SafeAlias})" :
        $"(({RepresentableTypeSymbol.ToFullString()}){instance}.__referenceTypeContainer)";
    public String TagValueExpression => $"Tag.{SafeAlias}";

    public String SafeAlias => _safeAlias ??=
        Alias != null && SyntaxFacts.IsValidIdentifier(Alias) ?
        Alias :
        RepresentableTypeIsGenericParameter && GenericRepresentableTypeName != null ?
        GenericRepresentableTypeName :
        RepresentableTypeSymbol.Name;

    public override Boolean Equals(Object? obj) => Equals(obj as UnionTypeAttribute);
    public Boolean Equals(UnionTypeAttribute? other)
    {
        var result = other is not null &&
            Alias == other.Alias &&
            Options == other.Options &&
            (RepresentableTypeIsGenericParameter ?
            GenericRepresentableTypeName == other.GenericRepresentableTypeName :
            SymbolEqualityComparer.Default.Equals(RepresentableTypeSymbol, other.RepresentableTypeSymbol));

        return result;
    }

    public override Int32 GetHashCode()
    {
        var hashCode = 1581354465;
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(Alias);
        hashCode = hashCode * -1521134295 + Options.GetHashCode();
        hashCode = RepresentableTypeIsGenericParameter ?
            hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(GenericRepresentableTypeName) :
            hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(RepresentableTypeSymbol);

        return hashCode;
    }
}
