namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Threading;

sealed class NullModel : ISourceProductionModel
{
    private NullModel() { }
    public static readonly ISourceProductionModel Instance = new NullModel();
    public void AddToContext(SourceProductionContext context) { }
}
