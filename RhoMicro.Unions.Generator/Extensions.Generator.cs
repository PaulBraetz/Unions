namespace RhoMicro.Unions.Generator;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;

internal static partial class Extensions
{
    public static IncrementalValuesProvider<SourceCarry<TResult>> SelectCarry<TSource, TResult>(
        this IncrementalValuesProvider<SourceCarry<TSource>> provider,
        Func<TSource, DiagnosticsModelBuilder, SourceModelBuilder, CancellationToken, TResult> project) =>
        provider.Select((c, t) => c.Project((s, d, b) => project.Invoke(s, d, b, t)));
    public static IncrementalValuesProvider<SourceCarry<TargetDataModel>> SelectCarry<TModel>(
        this IncrementalValuesProvider<SourceCarry<TargetDataModel>> provider,
        Func<ModelCreationContext, TModel> modelFactory,
        Action<ModelIntegrationContext<TModel>> modelIntegration) =>
        provider.SelectCarry((c, d, s, t) =>
        {
            t.ThrowIfCancellationRequested();

            var invocationContext = new ModelCreationContext(c, t);
            var model = modelFactory.Invoke(invocationContext);

            var integrationContext = new ModelIntegrationContext<TModel>(d, s, t, model);
            modelIntegration.Invoke(integrationContext);

            return c;
        });

}
