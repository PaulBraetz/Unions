#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using RhoMicro.Unions;

using System;

abstract partial class StorageStrategy
{
    sealed class FieldContainerStrategy : StorageStrategy
    {
        public FieldContainerStrategy(
        String safeAlias,
        String fullTypeName,
        StorageOption selectedOption,
        RepresentableTypeNature typeNature,
        StorageSelectionViolation violation)
        : base(safeAlias, fullTypeName, selectedOption, typeNature, violation) =>
            _fieldName = SafeAlias.ToGeneratedCamelCase();

        private readonly String _fieldName;

        public override String GetConvertedInstanceVariableExpression(String targetType, String instance = "this") =>
            $"(Util.UnsafeConvert<{FullTypeName}, {targetType}>({instance}.{_fieldName}))";
        public override String GetInstanceVariableExpression(String instance = "this") =>
           $"({instance}.{_fieldName})";
        public override String GetInstanceVariableAssignmentExpression(String valueExpression, String instance = "this") =>
            $"{instance}.{_fieldName} = {valueExpression}";

        public override void Visit(StrategySourceHost host) => host.AddDedicatedField(this);
    }
}
