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
        RepresentableTypeSymbol.Name;

    public Boolean RepresentableTypeIsSupertypeOfTarget(
        ITypeSymbol target) =>
        target.InheritsFrom(RepresentableTypeSymbol);

    public Boolean IsConflictingDefinition(UnionTypeAttribute other) =>
        SymbolEqualityComparer.Default.Equals(RepresentableTypeSymbol, other.RepresentableTypeSymbol) &&
        (Alias != other.Alias || Options != other.Options);
    public override Boolean Equals(Object? obj) => Equals(obj as UnionTypeAttribute);
    public Boolean Equals(UnionTypeAttribute? other)
    {
        var result = other is not null &&
            Alias == other.Alias &&
            Options == other.Options &&
            SymbolEqualityComparer.Default.Equals(RepresentableTypeSymbol, other.RepresentableTypeSymbol);

        return result;
    }

    public override Int32 GetHashCode()
    {
        var hashCode = 1581354465;
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(Alias);
        hashCode = hashCode * -1521134295 + Options.GetHashCode();
        hashCode = hashCode * -1521134295 + SymbolEqualityComparer.Default.GetHashCode(RepresentableTypeSymbol);
        return hashCode;
    }
}
