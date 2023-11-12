namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

sealed class SourceModelBuilder : IEquatable<SourceModelBuilder>
{
    private sealed class BuiltModel : Model
    {
        private readonly String _source;
        private readonly String _hint;

        public BuiltModel(String source, String hint)
        {
            _source = source;
            _hint = hint;
        }

        public override void AddToContext(SourceProductionContext context) => context.AddSource(_hint, _source);
        public override Boolean Equals(Object obj) => Equals(obj as BuiltModel);
        public Boolean Equals(BuiltModel other) => other is not null && _source == other._source && _hint == other._hint;

        public override Int32 GetHashCode()
        {
            var hashCode = 557685766;
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_source);
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_hint);
            return hashCode;
        }
    }
    public class Model
    {
        protected Model() { }
        public static readonly Model Instance = new();
        public virtual void AddToContext(SourceProductionContext context) { }
    }

    private Boolean _isInitialized;

    private String _targetName;
    private String _targetStructOrClass;
    private String _targetNamespace;
    private String _targetAccessibility;

    public void SetTarget(ITypeSymbol symbol)
    {
        _isInitialized = true;

        _targetName = symbol.Name;
        _targetStructOrClass = symbol.IsValueType ?
            "struct" :
            "class";
        _targetNamespace = symbol.ContainingNamespace.IsGlobalNamespace ?
            String.Empty :
            $"namespace {symbol.ContainingNamespace.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat)};";
        _targetAccessibility = SyntaxFacts.GetText(symbol.DeclaredAccessibility);
    }

    private String _conversionOperators;

    public void SetOperators(IEnumerable<ConversionOperatorModel> models)
    {
        _isInitialized = true;

        _conversionOperators = String.Join("\n", models.Select(m => m.SourceText));
    }

#pragma warning disable IDE0044 // Add readonly modifier
    private String _switchMethod;
    private String _matchFunction;
    private String _downCastFunction;
    private String _equalsFunction;
    private String _getHashcodeFunction;
    private String _toStringFunction;
    private String _fields;
    private String _constructors;
    private String _nestedTypes;
#pragma warning restore IDE0044 // Add readonly modifier

    public Model Build()
    {
        if (!_isInitialized)
        {
            return Model.Instance;
        }

        var builder = new StringBuilder()
            .AppendLine("#pragma warning disable")
            .AppendLine("//using todo")
            .Append(_targetNamespace)
            .Append(_targetAccessibility).Append(" partial ").Append(_targetStructOrClass).Append(' ').AppendLine(_targetName)
            .Append('{')
            .AppendLine("#region Nested Types")
            //nested types
            .AppendLine("#endregion")
            .AppendLine("#region Constructors")
            //ctors
            .AppendLine("#endregion")
            .AppendLine("#region Fields")
            //fields
            .AppendLine("#endregion")
            .AppendLine("#region Methods")
            //switch
            //match
            //downcast
            .AppendLine("#endregion")
            .AppendLine("#region Overrides & Equality")
            //tostring
            //equality & hashing
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

        var hint = $"{_targetName}_UnionImplementation.cs";
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
        _equalsFunction = _equalsFunction,
        _getHashcodeFunction = _getHashcodeFunction,
        _toStringFunction = _toStringFunction,
        _fields = _fields,
        _constructors = _constructors,
        _nestedTypes = _nestedTypes
    };
    public override Boolean Equals(Object obj) => Equals(obj as SourceModelBuilder);
    public Boolean Equals(SourceModelBuilder other) => other is not null && _isInitialized == other._isInitialized && _targetName == other._targetName && _targetStructOrClass == other._targetStructOrClass && _targetNamespace == other._targetNamespace && _targetAccessibility == other._targetAccessibility && _conversionOperators == other._conversionOperators && _switchMethod == other._switchMethod && _matchFunction == other._matchFunction && _downCastFunction == other._downCastFunction && _equalsFunction == other._equalsFunction && _getHashcodeFunction == other._getHashcodeFunction && _toStringFunction == other._toStringFunction && _fields == other._fields && _constructors == other._constructors && _nestedTypes == other._nestedTypes;

    public override Int32 GetHashCode()
    {
        var hashCode = 916049698;
        hashCode = hashCode * -1521134295 + _isInitialized.GetHashCode();
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_targetName);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_targetStructOrClass);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_targetNamespace);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_targetAccessibility);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_conversionOperators);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_switchMethod);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_matchFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_downCastFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_equalsFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_getHashcodeFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_toStringFunction);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_fields);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_constructors);
        hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_nestedTypes);
        return hashCode;
    }

    public static Boolean operator ==(SourceModelBuilder left, SourceModelBuilder right) => EqualityComparer<SourceModelBuilder>.Default.Equals(left, right);
    public static Boolean operator !=(SourceModelBuilder left, SourceModelBuilder right) => !(left == right);
}