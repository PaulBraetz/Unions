namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

interface ISourceProductionModel
{
    void AddToContext(SourceProductionContext context);
}
