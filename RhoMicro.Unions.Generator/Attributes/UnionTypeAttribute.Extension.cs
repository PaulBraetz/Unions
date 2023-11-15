#pragma warning disable CS8618

namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;

using RhoMicro.AttributeFactoryGenerator;
using RhoMicro.Unions.Generator;

using System;

[GenerateFactory]
public partial class UnionTypeAttribute
{
    [ExcludeFromFactory]
    public UnionTypeAttribute(Object representableTypeSymbolContainer) =>
        _representableTypeSymbolContainer = representableTypeSymbolContainer;

    private String _parameterName;
    private String _safeAlias;

    public String GetInstanceVariableExpression(ITypeSymbol target, String instance = "this") =>
        RepresentableTypeSymbol.IsValueType ?
        $"({instance}.__valueTypeContainer.{SafeAlias})" :
        $"(({RepresentableTypeSymbol.ToFullString()}){instance}.__referenceTypeContainer)";
    public String TagValueExpression => $"Tag.{SafeAlias}";

    public String SafeAlias => _safeAlias ??= Alias ?? RepresentableTypeSymbol.Name;
    public String ParameterName
    {
        get
        {
            if(_parameterName != null)
            {
                return _parameterName;
            }

            var alias = SafeAlias;
            var parameterName = alias;
            if(Char.IsUpper(alias, 0))
            {
                parameterName = Char.ToLowerInvariant(alias[0]) + alias.Remove(0);
            }

            _parameterName = parameterName;

            return parameterName;
        }
    }

    public Boolean IsConflictingDefinition(UnionTypeAttribute other) =>
        SymbolEqualityComparer.Default.Equals(RepresentableTypeSymbol, other.RepresentableTypeSymbol) &&
        (Alias != other.Alias || Options != other.Options);
}
