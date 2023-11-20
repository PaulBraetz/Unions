#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using RhoMicro.Unions;

using System;

abstract partial class StorageStrategy
{
    sealed class ReferenceContainerStrategy(
        String safeAlias,
        String fullTypeName,
        StorageOption selectedOption,
        RepresentableTypeNature typeNature) : StorageStrategy(safeAlias, fullTypeName, selectedOption, typeNature)
    {
        public override String GetConvertedInstanceVariableExpression(String targetType, String instance = "this") =>
            $"(({targetType}){instance}.__referenceTypeContainer)";
        public override String GetInstanceVariableExpression(String instance = "this") =>
            $"(({FullTypeName}){instance}.__referenceTypeContainer)";
        public override String GetInstanceVariableAssignmentExpression(String valueExpression, String instance = "this") =>
            $"{instance}.__referenceTypeContainer = {valueExpression}";
        public override void Visit(StrategySourceHost host) => host.AddReferenceTypeContainerField();
        public override void Visit(DiagnosticsModelBuilder diagnostics, TargetDataModel target) =>
            diagnostics.DiagnoseReferenceStorage(this, target);
    }
}
