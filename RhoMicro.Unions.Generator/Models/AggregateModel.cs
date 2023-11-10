namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using System.Collections.Generic;

internal sealed class AggregateModel : ISourceProductionModel
{
    public AggregateModel(IEnumerable<ISourceProductionModel> models) => _models = models;
    private readonly IEnumerable<ISourceProductionModel> _models;

    public void AddToContext(SourceProductionContext context)
    {
        foreach(var model in _models)
        {
            model.AddToContext(context);
        }
    }
}
