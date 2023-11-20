﻿#if GENERATOR
#pragma warning disable CS8618
#endif

namespace RhoMicro.Unions;

using System;
using System.Collections.Generic;

/// <summary>
/// Defines options for generating union types.
/// </summary>
[Flags]
public enum UnionTypeOptions
{
    /// <summary>
    /// </summary>
    None = 0b0,
    /// <summary>
    /// Instructs the generator to emit an implicit conversion to the representable type if it is the only one.
    /// In effect, this option will enable the union type to act as an alias wrapper for the representable type.
    /// </summary>
    ImplicitConversionIfSolitary = 0b1
}

/// <summary>
/// Defines options for the storage implementation of a representable type.
/// In order for the generator to generate an efficient storage implementation, 
/// consumers should communicate whether the representable type is known to
/// be a struct, class or of unknown nature. This is mostly relevant for generic
/// type parameters, however an explicit strategy may be selected for any representable
/// type. Whether or not generic type parameters are known to be reference
/// or value types depends on their constraints. Parameters constrained to 
/// <see langword="struct"/> will be assumed to be value types. Conversely,
/// parameters constrained to <see langword="class"/> will be assumed to be reference types.
/// </summary>
/*
           | box |value| auto | field
    struct | rc! | vc  | vc   | cc
    class  | rc  | rc! | rc   | rc!
    none   | rc! | vc! | rc!  | cc
*/
public enum StorageOption
{
    /// <summary>
    /// The generator will automatically decide on a storage strategy.
    /// <para>
    /// If the representable type is <b>known to be a value type</b>,
    /// this will store values of that type inside a value type container.
    /// <b>Boxing will not occur.</b>
    /// </para>
    /// <para>
    /// If the representable type is <b>known to be a reference type</b>,
    /// this will store values of that type inside a reference type container.
    /// </para>
    /// <para>
    /// If the representable type is <b>neither known to be a reference type
    /// nor a value type</b>, this option will cause values of that type to 
    /// be stored inside a reference type container.
    /// <b>If the representable type is a generic type parameter,
    /// boxing will occur for value type arguments to that parameter.</b>
    /// </para>
    /// </summary>
    Auto,

    /// <summary>
    /// The generator will always store values of the representable type
    /// inside a reference type container.
    /// <para>
    /// If the representable type is <b>known to be a value type</b>,
    /// <b>boxing will occur</b>.
    /// </para>
    /// <para>
    /// If the representable type is a <b>generic type parameter</b>,
    /// <b>boxing will occur for value type arguments</b> to that parameter.
    /// </para>
    /// </summary>
    Reference,

    /// <summary>
    /// The generator will attempt to store values of the representable type
    /// inside a value type container.
    /// <para>
    /// If the representable type is <b>known to be a value type</b>,
    /// this will store values of that type inside a value type container.
    /// <b>Boxing will not occur.</b>
    /// </para>
    /// <para>
    /// If the representable type is <b>known to be a reference type</b>,
    /// this will store values of that type inside a reference type container.
    /// <b>Boxing will not occur.</b>
    /// </para>
    /// <para>
    /// If the representable type is <b>neither known to be a reference type
    /// nor a value type</b>, this option will cause values of that type to 
    /// be stored inside a value type container.
    /// <b>If the representable type is a generic type parameter,
    /// an exception of type <see cref="TypeLoadException"/> will occur for
    /// reference type arguments to that parameter.</b>
    /// </para>
    /// </summary>
    Value,

    /// <summary>
    /// The generator will attempt to store values of the representable type
    /// inside a dedicated field for that type.
    /// <para>
    /// If the representable type is <b>known to be a value type</b>,
    /// this will store values of that type inside a value type container.
    /// <b>Boxing will not occur.</b>
    /// </para>
    /// <para>
    /// If the representable type is <b>known to be a reference type</b>,
    /// this will store values of that type inside a reference type container.
    /// </para>
    /// <para>
    /// If the representable type is <b>neither known to be a reference type
    /// nor a value type</b>, this option will cause values of that type to 
    /// be stored inside a strongly typed container.
    /// <b>Boxing will not occur.</b>
    /// </para>
    /// </summary>
    Field
}

/// <summary>
/// Marks the target type as a union type being able to represent the type passed to the constructor.
/// </summary>
[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public sealed partial class UnionTypeAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="representableType">
    /// The type representable by the target union type.
    /// </param>
    public UnionTypeAttribute(Type representableType) =>
        RepresentableType = representableType;
    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    /// <param name="genericRepresentableTypeName">
    /// The name of the generic type parameter representable by the target union type.
    /// </param>
    public UnionTypeAttribute(String genericRepresentableTypeName)
    {
        GenericRepresentableTypeName = genericRepresentableTypeName;
        RepresentableTypeIsGenericParameter = true;
    }

    /// <summary>
    /// Gets or sets the alias to use for members representing the type represented by the union.
    /// For example, the represented type <see cref="List{T}"/> would be represented using names like
    /// <c>list</c>. Setting this property to <c>yourAlias</c> will instruct the generator to use
    /// member names like <c>yourAlias</c> instead of <c>list</c>. Use this property to avoid
    /// name collisions in generated code. Since the alias will be used for member names, it will
    /// only be taken into account if it is a valid identifier name.
    /// </summary>
    public String? Alias { get; set; }

    /// <summary>
    /// Gets or sets the generator options to use.
    /// </summary>
    public UnionTypeOptions Options { get; set; }

    /// <summary>
    /// Gets the conrete type representable by the target union type.
    /// </summary>
    public Type? RepresentableType { get; }
    /// <summary>
    /// Gets the generic type representable by the target union type.
    /// </summary>
    public String? GenericRepresentableTypeName { get; }
    /// <summary>
    /// Gets a value indicating whether the representable type is referring to a generic parameter of the union type.
    /// </summary>
    public Boolean RepresentableTypeIsGenericParameter { get; }
    /// <summary>
    /// Gets or sets the option defining storage generation.
    /// </summary>
    public StorageOption Storage { get; set; }
}