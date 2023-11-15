namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelFactoryInvocationContext
{
    public readonly ModelFactoryParameters Parameters;
    public readonly CancellationToken CancellationToken;

    public ModelFactoryInvocationContext(ModelFactoryParameters parameters, CancellationToken cancellationToken)
    {
        Parameters = parameters;
        CancellationToken = cancellationToken;
    }
}
