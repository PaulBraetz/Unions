﻿#pragma warning disable CS8618

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
        RepresentableTypeNature typeNature,
        StorageSelectionViolation violation)
    {
        SafeAlias = safeAlias;
        FullTypeName = fullTypeName;
        SelectedOption = selectedOption;
        TypeNature = typeNature;
        Violation = violation;
    }
    #endregion
    #region Fields
    public readonly String SafeAlias;
    public readonly String FullTypeName;
    public readonly StorageOption SelectedOption;
    public readonly RepresentableTypeNature TypeNature;
    public readonly StorageSelectionViolation Violation;
    #endregion
    #region Factory
    public static StorageStrategy Create(
        String safeAlias,
        String fullTypeName,
        StorageOption selectedOption,
        RepresentableTypeNature typeNature,
        Boolean targetIsGeneric)
    {

        var result = typeNature switch
        {
            RepresentableTypeNature.PureValueType => createForPureValueType(),
            RepresentableTypeNature.ImpureValueType => createForImpureValueType(),
            RepresentableTypeNature.ReferenceType => createForReferenceType(),
            _ => createForUnknownType(),
        };

        return result;

        StorageStrategy createReference(StorageSelectionViolation violation = StorageSelectionViolation.None) =>
            new ReferenceContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature, violation);
        StorageStrategy createValue(StorageSelectionViolation violation = StorageSelectionViolation.None) =>
            new ValueContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature, violation);
        StorageStrategy createField(StorageSelectionViolation violation = StorageSelectionViolation.None) =>
            new FieldContainerStrategy(safeAlias, fullTypeName, selectedOption, typeNature, violation);

        /*
        read tables like so:
    type nature | selected strategy | generic strat(diag) : nongeneric strat(diag)
        */

        /*
    PureValue   Reference   => reference(box)
                Value       => field(generic) : value
                Field       => field        
                Auto        => field : value
        */
        StorageStrategy createForPureValueType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(StorageSelectionViolation.PureValueReferenceSelection),
                StorageOption.Value => targetIsGeneric ? createField(StorageSelectionViolation.PureValueValueSelectionGeneric) : createValue(),
                StorageOption.Field => createField(),
                _ => targetIsGeneric ? createField() : createValue()
            };
        /*
    ImpureValue Reference   => reference(box)
                Value       => field(tle)            
                Field       => field
                Auto        => field
        */
        StorageStrategy createForImpureValueType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(StorageSelectionViolation.ImpureValueReference),
                StorageOption.Value => createField(StorageSelectionViolation.ImpureValueValue),
                StorageOption.Field => createField(),
                _ => createField()
            };
        /*
    Reference   Reference   => reference
                Value       => reference(tle)        
                Field       => field        
                Auto        => reference
        */
        StorageStrategy createForReferenceType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(),
                StorageOption.Value => createReference(StorageSelectionViolation.ReferenceValue),
                StorageOption.Field => createField(),
                _ => createReference()
            };
        /*
    Unknown     Reference   => reference(pbox)
                Value       => field(ptle)            
                Field       => field
                Auto        => field
        */
        StorageStrategy createForUnknownType() =>
            selectedOption switch
            {
                StorageOption.Reference => createReference(StorageSelectionViolation.UnknownReference),
                StorageOption.Value => createField(StorageSelectionViolation.UnknownValue),
                StorageOption.Field => createField(),
                _ => createField()
            };
    }
    #endregion
    #region Template Methods
    public abstract String GetConvertedInstanceVariableExpression(String targetType, String instance = "this");
    public abstract String GetInstanceVariableExpression(String instance = "this");
    public abstract String GetInstanceVariableAssignmentExpression(String valueExpression, String instance = "this");
    #endregion
    public String GetToStringInvocation(String instance = "this") =>
           $"({GetInstanceVariableExpression(instance)}{(TypeNature == RepresentableTypeNature.ReferenceType ? "?" : String.Empty)}.ToString())";
    public String GetGetHashCodeInvocation(String instance = "this") =>
        $"(global::System.Collections.Generic.EqualityComparer<{FullTypeName}>.Default.GetHashCode({GetInstanceVariableExpression(instance)}))";
    public String GetEqualsInvocation(String instance = "this", String parameter = "obj") =>
        $"(global::System.Collections.Generic.EqualityComparer<{FullTypeName}>.Default.Equals({GetInstanceVariableExpression(instance)}, {GetInstanceVariableExpression(parameter)}))";

    public abstract void Visit(StrategySourceHost host);
}
