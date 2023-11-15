#pragma warning disable CS8618

namespace RhoMicro.Unions;

using RhoMicro.AttributeFactoryGenerator;

using System;

[GenerateFactory]
public partial class SubsetOfAttribute
{
    [ExcludeFromFactory]
    private SubsetOfAttribute(Object supersetUnionTypeSymbolContainer) =>
        _supersetUnionTypeSymbolContainer = supersetUnionTypeSymbolContainer;
}
