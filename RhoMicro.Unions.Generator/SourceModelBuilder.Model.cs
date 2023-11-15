namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

sealed partial class SourceModelBuilder
{
    public class Model
    {
        protected Model() { }
        public static readonly Model Instance = new();
        public virtual void AddToContext(SourceProductionContext context) { }
    }
}