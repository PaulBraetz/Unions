namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator.Models;

using System.Threading;

sealed class DiagnosticsCarryContextTerminal<T> : ISourceProductionModel
    where T : class, ISourceProductionModel
{
    private readonly SourceDiagnosticsCarry<T> _composite;
    public DiagnosticsCarryContextTerminal(SourceDiagnosticsCarry<T> composite) => _composite = composite;
    public void AddToContext(SourceProductionContext context)
    {
        if(_composite.HasContext)
        {
            _composite.Context.AddToContext(context);
        }

        _composite.Diagnostics.Build().AddToContext(context);
    }
    public static ISourceProductionModel Terminate(SourceDiagnosticsCarry<T> context, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        return new DiagnosticsCarryContextTerminal<T>(context);
    }
}
static class DiagnosticsCarryContextTerminal
{
    public static ISourceProductionModel Create<T>(SourceDiagnosticsCarry<T> context, CancellationToken token)
        where T : class, ISourceProductionModel
    {
        token.ThrowIfCancellationRequested();

        return new DiagnosticsCarryContextTerminal<T>(context);
    }
}