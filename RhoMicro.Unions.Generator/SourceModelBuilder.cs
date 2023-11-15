namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

sealed partial class SourceModelBuilder : IEquatable<SourceModelBuilder>
{
    private Boolean _isInitialized;

    private String? _targetName;
    private String? _targetStructOrClass;
    private String? _targetNamespace;
    private String? _targetAccessibility;
    public void SetTarget(ITypeSymbol symbol)
    {
        _isInitialized = true;

        _targetName = symbol.Name;
        _targetStructOrClass = symbol.IsValueType ?
            "struct" :
            "class";
        _targetNamespace = symbol.ContainingNamespace.IsGlobalNamespace ?
            String.Empty :
            $"namespace {symbol.ContainingNamespace.ToFullString()};";
        _targetAccessibility = SyntaxFacts.GetText(symbol.DeclaredAccessibility);
    }

    private String? _conversionOperators;
    public void SetOperators(IEnumerable<ConversionOperatorModel> models)
    {
        _isInitialized = true;

        _conversionOperators = String.Join("\n", models.Select(m => m.SourceText));
    }

    private String? _constructors;
    public void SetConstructors(ConstructorsModel model)
    {
        _isInitialized = true;

        _constructors = model.SourceText;
    }

    private String? _nestedTypes;
    internal void SetNestedTypes(NestedTypesModel model)
    {
        _isInitialized = true;

        _nestedTypes = model.SourceText;
    }

    private String? _fields;
    internal void SetFields(FieldsModel model)
    {
        _isInitialized = true;

        _fields = model.SourceText;
    }

    private String? _toStringFunction;
    public void SetToStringFunction(ToStringFunctionModel model)
    {
        _isInitialized = true;

        _toStringFunction = model.SourceText;
    }

    private String? _interfaceImplementation;
    public void SetInterfaceImplementation(InterfaceImplementationModel model)
    {
        _isInitialized = true;

        _interfaceImplementation = model.SourceText;
    }

    private String? _getHashcodeFunction;
    public void SetGethashcodeFunction(GetHashcodeFunctionModel model)
    {
        _isInitialized = true;

        _getHashcodeFunction = model.SourceText;
    }

    private String? _equalsFunctions;
    public void SetEqualsFunctions(EqualsFunctionsModel model)
    {
        _isInitialized = true;

        _equalsFunctions = model.SourceText;
    }

    private String? _downCastFunction;
    public void SetDownCastFunction(DownCastFunctionModel model)
    {
        _isInitialized = true;

        _downCastFunction = model.SourceText;
    }
#pragma warning disable IDE0044 // Add readonly modifier
    private String? _switchMethod;
    private String? _matchFunction;
#pragma warning restore IDE0044 // Add readonly modifier

    public Model Build()
    {
        if(!_isInitialized)
        {
            return Model.Instance;
        }

        var builder = new StringBuilder()
            .AppendLine("#pragma warning disable")
            .AppendLine("using System.Runtime.InteropServices;")
            .AppendLine("using System;")
            .Append(_targetNamespace)
            .Append(_targetAccessibility).Append(" partial ").Append(_targetStructOrClass).Append(' ').AppendLine(_targetName).AppendLine(_interfaceImplementation)
            .Append('{')
            .AppendLine("#region Nested Types")
            .Append(_nestedTypes)
            .AppendLine("#endregion")
            .AppendLine("#region Constructors")
            .Append(_constructors)
            .AppendLine("#endregion")
            .AppendLine("#region Fields")
            .Append(_fields)
            .AppendLine("#endregion")
            .AppendLine("#region Methods")
            .AppendLine(_downCastFunction)
            /*
            
    public readonly void Switch(
        Action<Int32> onInt32,
        Action<String?> onString,
        Action<Byte> onByte,
        Action<List<Double>> onList)
    {
        switch(_tag)
        {
            case Tag.String?:
                onString.Invoke((String?)_referenceTypeContainer);
                return;
            case Tag.List_Double:
                onList.Invoke((List<Double>)_referenceTypeContainer);
                return;
            case Tag.Int32:
                onInt32.Invoke(_valueTypeContainer.Int32Value);
                return;
            case Tag.Byte:
                onByte.Invoke(_valueTypeContainer.ByteValue);
                return;
            default:
                throw new InvalidOperationException();
        }
    }
    public readonly TResult Match<TResult>(
        Func<Int32, TResult> onInt32,
        Func<String?, TResult> onString,
        Func<Byte, TResult> onByte,
        Func<List<Double>, TResult> onList) =>
        _tag switch
        {
            Tag.String? => onString.Invoke((String?)_referenceTypeContainer),
            Tag.List_Double => onList.Invoke((List<Double>)_referenceTypeContainer),
            Tag.Int32 => onInt32.Invoke(_valueTypeContainer.Int32Value),
            Tag.Byte => onByte.Invoke(_valueTypeContainer.ByteValue),
            _ => throw new InvalidOperationException()
        };
            */
            //switch
            //match
            .AppendLine("#endregion")
            .AppendLine("#region Overrides & Equality")
            .Append(_toStringFunction)
            .Append(_getHashcodeFunction)
            .Append(_equalsFunctions)
            .AppendLine("#endregion")
            .AppendLine("#region Conversion Operators")
            .AppendLine(_conversionOperators)
            .AppendLine("#endregion")
            .Append('}');

        var source = builder.ToString();
        var formattedSource = CSharpSyntaxTree.ParseText(source)
                    .GetRoot()
                    .NormalizeWhitespace()
                    .SyntaxTree
                    .GetText()
                    .ToString();

        var hint = _targetName ?? String.Empty;

        var result = new BuiltModel(formattedSource, hint);

        return result;
    }

    public SourceModelBuilder Clone() => new()
    {
        _targetName = _targetName,
        _targetStructOrClass = _targetStructOrClass,
        _targetNamespace = _targetNamespace,
        _targetAccessibility = _targetAccessibility,
        _conversionOperators = _conversionOperators,
        _switchMethod = _switchMethod,
        _matchFunction = _matchFunction,
        _downCastFunction = _downCastFunction,
        _equalsFunctions = _equalsFunctions,
        _getHashcodeFunction = _getHashcodeFunction,
        _toStringFunction = _toStringFunction,
        _fields = _fields,
        _constructors = _constructors,
        _nestedTypes = _nestedTypes,
        _interfaceImplementation = _interfaceImplementation
    };
    public override Boolean Equals(Object obj) =>
        obj is SourceModelBuilder other && Equals(other);
    public Boolean Equals(SourceModelBuilder other) =>
        other is not null
        && _isInitialized == other._isInitialized
        && _targetName == other._targetName
        && _targetStructOrClass == other._targetStructOrClass
        && _targetNamespace == other._targetNamespace
        && _targetAccessibility == other._targetAccessibility
        && _conversionOperators == other._conversionOperators
        && _constructors == other._constructors
        && _nestedTypes == other._nestedTypes
        && _fields == other._fields
        && _toStringFunction == other._toStringFunction
        && _interfaceImplementation == other._interfaceImplementation
        && _getHashcodeFunction == other._getHashcodeFunction
        && _equalsFunctions == other._equalsFunctions
        && _switchMethod == other._switchMethod
        && _matchFunction == other._matchFunction
        && _downCastFunction == other._downCastFunction;

    public override Int32 GetHashCode()
    {
        var hashCode = -1573534906;
        hashCode = hashCode * -1521134295 + _isInitialized.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_targetName);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_targetStructOrClass);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_targetNamespace);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_targetAccessibility);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_conversionOperators);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_constructors);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_nestedTypes);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_fields);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_toStringFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_interfaceImplementation);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_getHashcodeFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_equalsFunctions);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_switchMethod);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_matchFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String?>.Default.GetHashCode(_downCastFunction);
        return hashCode;
    }

    public static Boolean operator ==(SourceModelBuilder left, SourceModelBuilder right) => EqualityComparer<SourceModelBuilder>.Default.Equals(left, right);
    public static Boolean operator !=(SourceModelBuilder left, SourceModelBuilder right) => !(left == right);
}
