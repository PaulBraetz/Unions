//TODO: continue here

namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator.Models;

using System;
using System.Threading;

internal sealed class UnionTypeModel
{
    public static ISourceProductionModel Create(GeneratorSyntaxContext context, CancellationToken token)
    {
        ISourceProductionModel result;
        try
        {
            result = CreateUnsafe(context, token);
        } catch(OperationCanceledException)
        when(token.IsCancellationRequested)
        {
            result = NullModel.Instance;
        } catch(Exception ex)
        {
            var diagnostic = Diagnostics.GeneratorException.Create(ex);
            result = new DiagnosticsModel(diagnostic);
        }

        return result;
    }
    private static ISourceProductionModel CreateUnsafe(GeneratorSyntaxContext context, CancellationToken token)
    {
        //if(!NullModel.IsTypeDeclaration(context.Node, token, out var errorModel, out var typeDeclaration) ||
        //   !DiagnosticsModel.IsTypeDeclarationValid(typeDeclaration, context.SemanticModel, token, out errorModel))
        //{
        //    return errorModel;
        //}

        //var attributes = typeDeclaration.AttributeLists
        //    .SelectMany(a => a.Attributes)
        //    .Select(a => (
        //        IsUnionAttribute: AttributeFactories.AggregateFactory.TryBuild(a, context.SemanticModel, out var r),
        //        Syntax: a,
        //        Attribute: r))
        //    .Where(t => t.IsUnionAttribute)
        //    .Select(t => t.Attribute)
        //    .GroupBy(a => a.GetType())
        //    .ToDictionary(g => g.Key);

        //var result = new UnionTypeModel(
        //    typeDeclaration,
        //    attributes[typeof(UnionTypeAttribute)].Cast<UnionTypeAttribute>().ToArray(),
        //    attributes[typeof(SubsetOfAttribute)].Cast<SubsetOfAttribute>().ToArray(),
        //    attributes[typeof(SupersetOfAttribute)].Cast<SupersetOfAttribute>().ToArray());

        //return result; TODO

        throw new NotImplementedException();
    }

    #region Equality & Hashing
    //public override Boolean Equals(Object obj) => obj is UnionTypeModel model && Equals(model);
    //public Boolean Equals(UnionTypeModel other) => HintName == other.HintName && SourceText == other.SourceText;

    //public override Int32 GetHashCode()
    //{
    //    var hashCode = -989924654;
    //    hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(HintName);
    //    hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(SourceText);
    //    return hashCode;
    //}

    //public static Boolean operator ==(UnionTypeModel left, UnionTypeModel right) => left.Equals(right);
    //public static Boolean operator !=(UnionTypeModel left, UnionTypeModel right) => !(left == right); TODO
    #endregion
}
