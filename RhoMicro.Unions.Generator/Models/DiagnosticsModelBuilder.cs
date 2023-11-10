namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using System;
using System.Collections.Generic;
using System.Linq;

sealed class DiagnosticsModelBuilder
{
    private readonly ICollection<Diagnostic> _diagnostics = new List<Diagnostic>();
    private readonly Object _syncRoot = new();

    private DiagnosticsModelBuilder(ICollection<Diagnostic> diagnostics) => _diagnostics = diagnostics;
    public DiagnosticsModelBuilder() : this(new List<Diagnostic>()) { }

    public DiagnosticsModelBuilder Add(Diagnostic diagnostic)
    {
        lock(_syncRoot)
        {
            _diagnostics.Add(diagnostic);
        }

        return this;
    }
    public DiagnosticsModelBuilder Clone() => new(_diagnostics.ToList());
    public ISourceProductionModel Build() =>
        _diagnostics.Count > 0 ?
            new AggregateModel(_diagnostics.Select(d => new DiagnosticsModel(d)).ToArray()) :
            NullModel.Instance;
}
