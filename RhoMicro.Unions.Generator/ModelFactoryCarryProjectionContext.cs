namespace RhoMicro.Unions.Generator;

using System.Threading;

readonly struct ModelCreationContext
{
    public readonly ModelFactoryParameters Parameters;
    public readonly CancellationToken CancellationToken;

    public ModelCreationContext(ModelFactoryParameters parameters, CancellationToken cancellationToken)
    {
        Parameters = parameters;
        CancellationToken = cancellationToken;
    }
}
