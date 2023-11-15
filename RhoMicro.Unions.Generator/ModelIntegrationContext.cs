namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelIntegrationContext<TModel>
{
    public readonly DiagnosticsModelBuilder Diagnostics;
    public readonly SourceModelBuilder Source;
    public readonly CancellationToken CancellationToken;
    public readonly TModel Model;

    public ModelIntegrationContext(DiagnosticsModelBuilder diagnostics, SourceModelBuilder source, CancellationToken cancellationToken, TModel model)
    {
        Diagnostics = diagnostics;
        Source = source;
        CancellationToken = cancellationToken;
        Model = model;
    }
}