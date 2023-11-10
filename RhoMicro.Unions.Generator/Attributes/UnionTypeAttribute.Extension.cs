namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.AttributeFactoryGenerator;
using RhoMicro.Unions.Generator;

using System;

[GenerateFactory]
public partial class UnionTypeAttribute
{
    public UnionTypeAttribute(Object representableTypeSymbolContainer) =>
        _representableTypeSymbolContainer = representableTypeSymbolContainer;

    private String _parameterName;
    private String _safeAlias;

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
}
