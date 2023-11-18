namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator.Models;

using System;

sealed partial class SourceModelBuilder
{
    public class Model
    {
        protected Model() { }
        public static readonly Model Instance = new();
        public virtual void AddToContext(SourceProductionContext context) { }
    }
}