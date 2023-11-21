namespace RhoMicro.Unions.Generator;

using Microsoft.CodeAnalysis;

using System;

internal static partial class Diagnostics
{
    private const String _category = "RhoMicro.Unions.Generator";

    //IMPORTANT: append new values at the end to preserve error codes
    private enum Id
    {
        DuplicateUnionTypeAttributes = 1,
        GeneratorException = 2,
        MissingUnionTypeAttribute = 3,
        NonPartialDeclaration = 4,
        StaticTarget = 5,
        RecordTarget = 6,
        TooManyTypes = 7,
        ImplicitConversionOptionOnNonSolitary = 8,
        ImplicitConversionOptionOnSolitary = 9,
        InvalidAttributeTarget = 10,
        AliasCollision = 11,
        UnionTypeSettingsOnNonUnionType = 12,
        RepresentableTypeIsSupertype = 13,
        RepresentableTypeIsInterface = 14,
        ReservedGenericParameterName = 15,
        UnknownGenericParameterName = 16,
        PossibleBoxingStrategy = 17,
        BoxingStrategy = 18,
        PossibleTleStrategy = 19,
        TleStrategy = 20,
        SmallGenericUnion = 21,
        GenericViolationStrategy = 22
    }
    public static Diagnostic GenericViolationStrategy(Location location, String typeName) =>
        Create(
            Id.PossibleBoxingStrategy,
            location,
            DiagnosticSeverity.Warning,
            typeName);
    public static Diagnostic SmallGenericUnion(Location location) =>
        Create(
            Id.SmallGenericUnion,
            location,
            DiagnosticSeverity.Info);
    public static Diagnostic PossibleBoxingStrategy(Location location, String typeName) =>
        Create(
            Id.PossibleBoxingStrategy,
            location,
            DiagnosticSeverity.Warning,
            typeName);
    public static Diagnostic BoxingStrategy(Location location, String typeName) =>
        Create(
            Id.BoxingStrategy,
            location,
            DiagnosticSeverity.Warning,
            typeName);
    public static Diagnostic PossibleTleStrategy(Location location, String typeName) =>
        Create(
            Id.PossibleTleStrategy,
            location,
            DiagnosticSeverity.Warning,
            typeName);
    public static Diagnostic TleStrategy(Location location, String typeName) =>
        Create(
            Id.TleStrategy,
            location,
            DiagnosticSeverity.Warning,
            typeName);
    public static Diagnostic UnknownGenericParameterName(Location location, String name) =>
        Create(
            Id.UnknownGenericParameterName,
            location,
            DiagnosticSeverity.Error,
            name);
    public static Diagnostic ReservedGenericParameterName(Location location, String name) =>
        Create(
            Id.ReservedGenericParameterName,
            location,
            DiagnosticSeverity.Error,
            name);
    public static Diagnostic RepresentableTypeIsInterface(Location location, String representableTypeName) =>
        Create(
            Id.RepresentableTypeIsInterface,
            location,
            DiagnosticSeverity.Info,
            representableTypeName);
    public static Diagnostic RepresentableTypeIsSupertype(Location location, String representableTypeName) =>
        Create(
            Id.RepresentableTypeIsSupertype,
            location,
            DiagnosticSeverity.Info,
            representableTypeName);
    public static Diagnostic UnionTypeSettingsOnNonUnionType(Location location) =>
        Create(
            Id.UnionTypeSettingsOnNonUnionType,
            location,
            DiagnosticSeverity.Warning);
    public static Diagnostic AliasCollision(Location location, String representableTypeName) =>
        Create(
            Id.AliasCollision,
            location,
            DiagnosticSeverity.Error,
            representableTypeName);
    public static Diagnostic InvalidAttributeTarget(Location location) =>
        Create(
            Id.InvalidAttributeTarget,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic TooManyTypes(Location location) =>
        Create(
            Id.TooManyTypes,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic StaticTarget(Location location) =>
        Create(
            Id.StaticTarget,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic RecordTarget(Location location) =>
        Create(
            Id.RecordTarget,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic NonPartialDeclaration(Location location) =>
        Create(
            Id.NonPartialDeclaration,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic MissingUnionTypeAttribute(Location location) =>
        Create(
            Id.MissingUnionTypeAttribute,
            location,
            DiagnosticSeverity.Error);
    public static Diagnostic GeneratorException(Exception exception) =>
        Create(
            Id.GeneratorException,
            Location.None,
            DiagnosticSeverity.Error,
            exception.Message);
    public static Diagnostic DuplicateUnionTypeAttributes(String unionTypeName, Location location) =>
        Create(
            Id.DuplicateUnionTypeAttributes,
            location,
            DiagnosticSeverity.Error,
            unionTypeName);
    public static Diagnostic ImplicitConversionOptionOnNonSolitary(Location location) =>
        Create(
            Id.ImplicitConversionOptionOnNonSolitary,
            location,
            DiagnosticSeverity.Warning);
    public static Diagnostic ImplicitConversionOptionOnSolitary(String unionTypeName, String representableTypeName, Location location) =>
        Create(
            Id.ImplicitConversionOptionOnSolitary,
            location,
            DiagnosticSeverity.Info,
            $"ISuperset<{representableTypeName}, {unionTypeName}>");
    static Diagnostic Create(
        Id id,
        Location location,
        DiagnosticSeverity severity,
        params Object[] messageArgs) =>
        Diagnostic.Create(
            new DiagnosticDescriptor(
                $"RUG{(Int32)id:0000}",
                GetLocalized($"{id}_Title"),
                GetLocalized($"{id}_Message"),
                _category,
                severity,
                true),
            location,
            messageArgs);

    static LocalizableResourceString GetLocalized(String name) =>
        String.IsNullOrEmpty(Resources.Diagnostics.ResourceManager.GetString(name)) ?
            throw new ArgumentException($"No resource with name {name} could be located.", nameof(name)) :
            new LocalizableResourceString(
                name,
                Resources.Diagnostics.ResourceManager,
                typeof(Resources.Diagnostics));
}
