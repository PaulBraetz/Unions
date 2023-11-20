namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelCreationContext
{
    public readonly TargetDataModel TargetData;
    public readonly CancellationToken CancellationToken;

    public ModelCreationContext(TargetDataModel parameters, CancellationToken cancellationToken)
    {
        TargetData = parameters;
        CancellationToken = cancellationToken;
    }
}
