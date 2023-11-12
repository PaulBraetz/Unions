namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;
using System.Threading;

//it's a monad :DDD
readonly struct SourceCarry<T> : IEquatable<SourceCarry<T>>
{
    public readonly DiagnosticsModelBuilder Diagnostics;
    public readonly SourceModelBuilder Source;

    public readonly Boolean HasContext => _context.HasValue;
    public readonly T Context => _context.HasValue ?
        _context.Value :
        throw new InvalidOperationException("Unable to retrieve context data.");
    private readonly Optional<T> _context;

    public SourceCarry(
        Optional<T> context,
        DiagnosticsModelBuilder diagnostics,
        SourceModelBuilder builder)
    {
        Source = builder;
        Diagnostics = diagnostics;
        _context = context;
    }

    public SourceCarry<TResult> Project<TResult>(Func<T, DiagnosticsModelBuilder, SourceModelBuilder, TResult> project)
    {
        var diagnostics = Diagnostics.Clone();
        var builder = Source.Clone();
        var context = new Optional<TResult>();

        if (HasContext && !diagnostics.IsError)
        {
            try
            {
                var projection = project.Invoke(Context, diagnostics, builder);
                context = new(projection);
            }
            catch (Exception ex)
            {
                var diagnostic = Unions.Generator.Diagnostics.GeneratorException.Create(ex);
                _ = diagnostics.Add(diagnostic);
            }
        }

        var result = new SourceCarry<TResult>(context, diagnostics, builder);

        return result;
    }

    public void AddToContext(SourceProductionContext context)
    {
        var diagnostics = Diagnostics.Build();
        diagnostics.AddToContext(context);

        if (!diagnostics.IsError)
        {
            Source.Build().AddToContext(context);
        }
    }

    public override Boolean Equals(Object obj) => obj is SourceCarry<T> composite && Equals(composite);
    public Boolean Equals(SourceCarry<T> other) =>
        _context.Equals(other._context);
    public override Int32 GetHashCode() => -213244607 + _context.GetHashCode();

    public static Boolean operator ==(SourceCarry<T> left, SourceCarry<T> right) => left.Equals(right);
    public static Boolean operator !=(SourceCarry<T> left, SourceCarry<T> right) => !(left == right);
}
