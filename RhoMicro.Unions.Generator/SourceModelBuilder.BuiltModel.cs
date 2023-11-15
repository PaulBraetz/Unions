namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;

sealed partial class SourceModelBuilder
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
        public override Boolean Equals(Object? obj) =>
            obj is BuiltModel other && Equals(other);
        public Boolean Equals(BuiltModel other) =>
            other is not null && _source == other._source && _hint == other._hint;

        public override Int32 GetHashCode()
        {
            var hashCode = 557685766;
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_source);
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(_hint);
            return hashCode;
        }
    }
}