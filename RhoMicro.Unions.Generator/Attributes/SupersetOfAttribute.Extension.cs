#pragma warning disable CS8618

namespace RhoMicro.Unions;

using RhoMicro.AttributeFactoryGenerator;

using System;

[GenerateFactory]
public partial class SupersetOfAttribute
{
    [ExcludeFromFactory]
    public SupersetOfAttribute(Object subsetUnionTypeSymbolContainer) =>
        _subsetUnionTypeSymbolContainer = subsetUnionTypeSymbolContainer;
}
