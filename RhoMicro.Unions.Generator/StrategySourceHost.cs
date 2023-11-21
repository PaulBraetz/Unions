#pragma warning disable CS8618

namespace RhoMicro.Unions.Generator;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;

sealed class StrategySourceHost
{
    public StrategySourceHost(TargetDataModel target) => _target = target;

    private readonly TargetDataModel _target;

    private readonly List<Action<StringBuilder>> _dedicatedFieldAdditions = [];
    public void AddDedicatedField(StorageStrategy strategy) =>
        _dedicatedFieldAdditions.Add((s) =>
            s.Append("private readonly ")
                .Append(strategy.FullTypeName).Append(' ')
                .Append(strategy.SafeAlias.ToGeneratedCamelCase())
                .AppendLine(";"));
    public void AppendDedicatedFields(StringBuilder sourceTextBuilder) =>
        _dedicatedFieldAdditions.ForEach(a => a.Invoke(sourceTextBuilder));

    private Boolean _referenceFieldRequired;
    public void AddReferenceTypeContainerField() => _referenceFieldRequired = true;
    public void AppendReferenceTypeContainerField(StringBuilder sourceTextBuilder)
    {
        if(!_referenceFieldRequired)
            return;
        _ = sourceTextBuilder.AppendLine("private readonly global::System.Object __referenceTypeContainer;");
    }

    private Boolean _valueTypeContainerTypeRequired;
    public void AddValueTypeContainerType() => _valueTypeContainerTypeRequired = true;
    public void AddValueTypeContainerField() => _valueTypeContainerTypeRequired = true;
    private readonly List<Action<StringBuilder>> _valueTypeFieldAdditions = [];
    public void AddValueTypeVontainerInstanceFieldAndCtor(StorageStrategy strategy) =>
        _valueTypeFieldAdditions.Add((s) =>
        {
            if(!_target.TargetSymbol.IsGenericType)
            {
                _ = s.AppendLine("[global::System.Runtime.InteropServices.FieldOffset(0)]");
            }

            _ = s.Append("public readonly ").Append(strategy.FullTypeName).Append(' ')
                .Append(strategy.SafeAlias).AppendLine(";")
                .Append("public ")
                .Append(_target.ValueTypeContainerName)
                .Append('(').Append(strategy.FullTypeName)
                .AppendLine(" value) => ")
                .Append(strategy.SafeAlias).AppendLine(" = value;");
        });

    public void AppendValueTypeContainerField(StringBuilder sourceTextBuilder)
    {
        if(!_valueTypeContainerTypeRequired)
            return;
        _ = sourceTextBuilder.AppendLine("private readonly ")
            .Append(_target.ValueTypeContainerName)
            .Append(" __valueTypeContainer;");
    }
    public void AppendValueTypeContainerType(StringBuilder sourceTextBuilder)
    {
        if(!_valueTypeContainerTypeRequired)
            return;

        if(!_target.TargetSymbol.IsGenericType)
        {
            _ = sourceTextBuilder.AppendLine(
                "[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit)]");
        }

        _ = sourceTextBuilder
                .AppendLine("internal readonly struct ")
                .Append(_target.ValueTypeContainerName)
                .AppendLine("{")
                .AppendAggregate(
                    _valueTypeFieldAdditions,
                    (b, a) =>
                    {
                        a.Invoke(b);
                        return b;
                    })
                .AppendLine("}");
    }
}
