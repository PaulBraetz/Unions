namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions;

using System.Collections.Generic;
using System.Linq;

sealed class OperatorOmissionModel
{
    public readonly HashSet<UnionTypeAttribute> Interfaces;
    public readonly HashSet<UnionTypeAttribute> Supertypes;

    public readonly HashSet<UnionTypeAttribute> AllOmissions;

    private OperatorOmissionModel(
        HashSet<UnionTypeAttribute> interfaces,
        HashSet<UnionTypeAttribute> supertypes,
        HashSet<UnionTypeAttribute> allOmissions)
    {
        Interfaces = interfaces;
        Supertypes = supertypes;
        AllOmissions = allOmissions;
    }

    public static OperatorOmissionModel Create(ITypeSymbol target, AttributesModel attributes)
    {
        var interfaces = new HashSet<UnionTypeAttribute>();
        var supertypes = new HashSet<UnionTypeAttribute>();
        var allOmissions = new HashSet<UnionTypeAttribute>();

        var concreteAttributes = attributes.AllUnionTypeAttributes.Where(a => !a.RepresentableTypeIsGenericParameter);

        foreach(var attribute in concreteAttributes)
        {
            if(target.InheritsFrom(attribute.RepresentableTypeSymbol!))
            {
                _ = allOmissions.Add(attribute);
                _ = supertypes.Add(attribute);
            }

            if(attribute.RepresentableTypeSymbol!.TypeKind == TypeKind.Interface)
            {
                _ = allOmissions.Add(attribute);
                _ = interfaces.Add(attribute);
            }
        }

        var result = new OperatorOmissionModel(interfaces, supertypes, allOmissions);

        return result;
    }
}
