namespace RhoMicro.Unions;

using Microsoft.CodeAnalysis;

using RhoMicro.AttributeFactoryGenerator;

using System;

[GenerateFactory]
public partial class SubsetOfAttribute
{
    private SubsetOfAttribute(Object supersetUnionTypeSymbolContainer) =>
        _supersetUnionTypeSymbolContainer = supersetUnionTypeSymbolContainer;
}
