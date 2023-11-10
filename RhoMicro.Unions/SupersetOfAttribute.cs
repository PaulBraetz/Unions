#nullable enable
namespace RhoMicro.Unions;

using System;

/// <summary>
/// Marks the target type as a superset union type of the type passed to the constructor.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed partial class SupersetOfAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="subsetUnionType">
    /// The type to register as a subset of the target union type.
    /// </param>
    public SupersetOfAttribute(Type subsetUnionType) =>
        SubsetUnionType = subsetUnionType;

    /// <summary>
    /// Gets or sets the alias to use for members representing the subset union type represented by the union.
    /// For example, the subset union type <c>MySubsetUnion</c> would be represented using names like
    /// <c>mySubsetUnion</c>. Setting this property to <c>yourAlias</c> will instruct the generator to use
    /// member names like <c>yourAlias</c> instead of <c>mySubsetUnion</c>. Use this property to avoid
    /// name collisions in generated code.
    /// </summary>
    public String? Alias { get; set; }

    /// <summary>
    /// Gets the type to register as a subset of the target union type.
    /// </summary>
    public Type SubsetUnionType { get; }

    /// <summary>
    /// This property is not intended for use outside of the generator.
    /// </summary>
    public Object? SubsetUnionTypeSymbolContainer { get; private set; } = null;

    /// <summary>
    /// This method is not intended for use outside of the generator.
    /// </summary>
    public void SetTypeParameter(String parameterName, Object type)
    {
        if(parameterName == "subsetUnionType")
        {
            SubsetUnionTypeSymbolContainer = type;
        }
    }
}