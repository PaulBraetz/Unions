namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelCreationContext
{
    public readonly TargetDataModel Parameters;
    public readonly CancellationToken CancellationToken;

    public ModelCreationContext(TargetDataModel parameters, CancellationToken cancellationToken)
    {
        Parameters = parameters;
        CancellationToken = cancellationToken;
    }
}
