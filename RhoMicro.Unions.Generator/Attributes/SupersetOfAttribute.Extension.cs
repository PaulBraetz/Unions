namespace RhoMicro.Unions;

using RhoMicro.AttributeFactoryGenerator;

using System;

[GenerateFactory]
public partial class SupersetOfAttribute
{
    [ExcludeConstructor]
    public SupersetOfAttribute(Object subsetUnionTypeSymbolContainer) =>
        _subsetUnionTypeSymbolContainer = subsetUnionTypeSymbolContainer;
}
