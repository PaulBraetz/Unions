namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Options;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Xml.Linq;

readonly struct SourceDiagnosticsCarry<T> : IEquatable<SourceDiagnosticsCarry<T>>
    where T : class
{
    public readonly DiagnosticsModelBuilder Diagnostics;
    public readonly Boolean HasContext => _context.HasValue;
    public readonly T Context => _context.HasValue ?
        _context.Value :
        throw new InvalidOperationException("Unable to retrieve context data.");
    private readonly Optional<T> _context;

    public SourceDiagnosticsCarry(Optional<T> context, DiagnosticsModelBuilder diagnostics)
    {
        Diagnostics = diagnostics;
        _context = context;
    }
    public SourceDiagnosticsCarry(DiagnosticsModelBuilder diagnostics)
        : this(new(), diagnostics)
    {
    }
    public SourceDiagnosticsCarry(Optional<T> context)
        : this(context, new())
    {
    }
    public SourceDiagnosticsCarry()
        : this(new(), new())
    {
    }

    public SourceDiagnosticsCarry<TResult> Project<TResult>(Func<T, DiagnosticsModelBuilder, TResult> projection)
        where TResult : class
    {
        if(!_context.HasValue)
        {
            return new SourceDiagnosticsCarry<TResult>(Diagnostics);
        }

        var diagnostics = Diagnostics.Clone();
        var context = projection.Invoke(Context, diagnostics);
        var result = new SourceDiagnosticsCarry<TResult>(context, diagnostics);

        return result;
    }

    public override Boolean Equals(Object obj) => obj is SourceDiagnosticsCarry<T> composite && Equals(composite);
    public Boolean Equals(SourceDiagnosticsCarry<T> other) =>
        _context.Equals(other._context);
    public override Int32 GetHashCode() => -213244607 + _context.GetHashCode();

    public static Boolean operator ==(SourceDiagnosticsCarry<T> left, SourceDiagnosticsCarry<T> right) => left.Equals(right);
    public static Boolean operator !=(SourceDiagnosticsCarry<T> left, SourceDiagnosticsCarry<T> right) => !(left == right);
}
