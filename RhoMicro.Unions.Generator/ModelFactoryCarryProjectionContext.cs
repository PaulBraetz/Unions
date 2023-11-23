namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelCreationContext(TargetDataModel parameters, CancellationToken cancellationToken)
{
    public readonly TargetDataModel TargetData = parameters;
    public readonly CancellationToken CancellationToken = cancellationToken;
}
