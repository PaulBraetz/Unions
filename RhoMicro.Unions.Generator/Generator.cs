namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using RhoMicro.Unions.Generator.Models;

using System.Collections.Generic;
using System.Linq;

[Generator(LanguageNames.CSharp)]
public sealed class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var handledTargets = new HashSet<ITypeSymbol>(SymbolEqualityComparer.Default);
        var models = context.SyntaxProvider.CreateSyntaxProvider(
            GeneratorUtilities.IsUnionDeclaration,
            SourceCarry<ModelFactoryParameters>.Create)
            .SelectCarry(
                c => c.Parameters,
                c => c.Diagnostics.DiagnoseAll(c.Model, c.CancellationToken))
            .SelectCarry((c, d, s, t) => (Context: c, IsFirst: handledTargets.Add(c.TargetSymbol)))
            .Where(c => !c.HasContext || c.Context.IsFirst)
            .SelectCarry((c, d, s, t) => c.Context)
            .SelectCarry(
                c => c.Parameters.TargetSymbol,
                c => c.Source.SetTarget(c.Model))
            .SelectCarry(
                c => c.Parameters.Attributes.AllUnionTypeAttributes
                    .Select(a => ConversionOperatorModel.Create(c.Parameters.TargetSymbol, a, c.Parameters.Attributes.AllUnionTypeAttributes)),
                c => c.Source.SetOperators(c.Model))
            .SelectCarry(
                ConstructorsModel.Create,
                ConstructorsModel.Integrate)
            .SelectCarry(
                NestedTypesModel.Create,
                NestedTypesModel.Integrate)
            .SelectCarry(
                FieldsModel.Create,
                FieldsModel.Integrate)
            .SelectCarry(
                ToStringFunctionModel.Create,
                ToStringFunctionModel.Integrate)
            .SelectCarry(
                InterfaceImplementationModel.Create,
                InterfaceImplementationModel.Integrate)
            .SelectCarry(
                GetHashcodeFunctionModel.Create,
                GetHashcodeFunctionModel.Integrate)
            .SelectCarry(
                EqualsFunctionsModel.Create,
                EqualsFunctionsModel.Integrate)
            .SelectCarry(
                DownCastFunctionModel.Create,
                DownCastFunctionModel.Integrate);

        context.RegisterSourceOutput(models, (c, sc) => sc.AddToContext(c));
    }
}
