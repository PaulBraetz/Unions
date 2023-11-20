#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using RhoMicro.Unions;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Security;

abstract partial class StorageStrategy
{
    #region Constructor
    private StorageStrategy(
        String safeAlias,
        String fullTypeName,
        StorageOption selectedOption,
        RepresentableTypeNature typeNature)
    {
        SafeAlias = safeAlias;
        FullTypeName = fullTypeName;
        SelectedOption = selectedOption;
        TypeNature = typeNature;
    }
    #endregion
    #region Fields
    public readonly String SafeAlias;
    public readonly String FullTypeName;
    public readonly StorageOption SelectedOption;
    public readonly RepresentableTypeNature TypeNature;
    #endregion
    #region Factory
    public static StorageStrategy Create(
        String safeAlias,
        String fullTypeName,
        StorageOption selectedOption,
        RepresentableTypeNature typeNature)
    {
        var result = typeNature switch
        {
            RepresentableTypeNature.ValueType => createForValueType(),
            RepresentableTypeNature.ReferenceType => createForReferenceType(),
            //RepresentableTypeNature.UnknownType is default
            _ => createForUnknownType(),
        };

        return result;

        StorageStrategy createReference() => new ReferenceContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature);
        StorageStrategy createValue() => new ValueContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature);
        StorageStrategy createField() => new FieldContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature);

        StorageStrategy createForValueType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(),
                StorageOption.Value => createValue(),
                StorageOption.Field => createField(),
                //StorageOption.Auto is default
                _ => createValue()
            };
        StorageStrategy createForReferenceType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(),
                StorageOption.Value => createReference(),
                StorageOption.Field => createField(),
                //StorageOption.Auto is default
                _ => createReference()
            };
        StorageStrategy createForUnknownType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(),
                StorageOption.Value => createReference(),
                StorageOption.Field => createField(),
                //StorageOption.Auto is default
                _ => createReference()
            };
    }
    #endregion
    public abstract String GetConvertedInstanceVariableExpression(String targetType, String instance = "this");
    public abstract String GetInstanceVariableExpression(String instance = "this");
    public abstract String GetInstanceVariableAssignmentExpression(String valueExpression, String instance = "this");

    public String GetToStringInvocation(String instance = "this") =>
           $"({GetInstanceVariableExpression(instance)}{(TypeNature == RepresentableTypeNature.ReferenceType ? "?" : String.Empty)}.ToString())";
    public String GetGetHashCodeInvocation(String instance = "this") =>
        $"(global::System.Collections.Generic.EqualityComparer<{FullTypeName}>.Default.GetHashCode({GetInstanceVariableExpression(instance)}))";
    public String GetEqualsInvocation(String instance = "this", String parameter = "obj") =>
        $"(global::System.Collections.Generic.EqualityComparer<{FullTypeName}>.Default.Equals({GetInstanceVariableExpression(instance)}, {GetInstanceVariableExpression(parameter)}))";

    public abstract void Visit(StrategySourceHost host);
    public abstract void Visit(DiagnosticsModelBuilder diagnostics, TargetDataModel target);
}
