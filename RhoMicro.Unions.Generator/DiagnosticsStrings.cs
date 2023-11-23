namespace RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;

internal static partial class Diagnostics
{
    readonly struct Item : IEquatable<Item>
    {
        public readonly String Title;
        public readonly String Message;

        public Item(String title, String message)
        {
            Title = title;
            Message = message;
        }

        public override Boolean Equals(Object? obj) => obj is Item item && Equals(item);
        public Boolean Equals(Item other) => Title == other.Title && Message == other.Message;

        public override Int32 GetHashCode()
        {
            var hashCode = -940627307;
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(Title);
            hashCode = hashCode * -1521134295 + EqualityComparer<String>.Default.GetHashCode(Message);
            return hashCode;
        }

        public static Boolean operator ==(Item left, Item right) => left.Equals(right);
        public static Boolean operator !=(Item left, Item right) => !(left == right);
    }

    private static readonly IReadOnlyDictionary<Id, Item> _items = new Dictionary<Id, Item>()
    {
{Id.AliasCollision , new Item(message:"An alias collision has been detected for the representable type `{0}`. Make sure that for colliding type names (excluding generic arguments) you provide a unique alias. If no explicit alias is provided, the simple type name of the representable type will be used, e.g.: `List` will be used for variables of the `List<T>` type. Aliae are used for parameter, variable, enum and field names.",title:"Alias Collision")},
{Id.BoxingStrategy , new Item(message:"Boxing will occur for the representable type {0}. Consider using a different storage option.",title:"Boxing Storage Detected")},
{Id.DuplicateUnionTypeAttributes , new Item(message:"Union types may not be declared using duplicate 'UnionType' attributes of the same representable type ({0}).",title:"Duplicate Union Type Declaration")},
{Id.GeneratorException , new Item(message:"The generator encountered an unexpected error: {0}",title:"Unexpected Generator Error")},
{Id.GenericViolationStrategy , new Item(message:"The selected storage option for {0} was ignored because the target union type is generic.",title:"Generic Storage Violation Detected")},
{Id.ImplicitConversionOptionOnNonSolitary , new Item(message:"The `ImplicitConversionIfSolitary` option will be ignored because the target union type may represent more than one type.",title:"Union Type Option Ignored")},
{Id.ImplicitConversionOptionOnSolitary , new Item(message:"The interface implementation for `{0}` was omitted because the `ImplicitConversionIfSolitary` option was used.",title:"Omitting Interface Implementation")},
{Id.InvalidAttributeTarget , new Item(message:"Only type declarations may be annotated with union type attributes.",title:"Invalid Attribute Target")},
{Id.MissingUnionTypeAttribute , new Item(message:"Union types must be declared using at least one instance of the 'UnionType' attribute.",title:"Missing 'UnionType' Attribute")},
{Id.NonPartialDeclaration , new Item(message:"Union types must be declared using the 'partial' keyword.",title:"Nonpartial Union Declaration")},
{Id.PossibleBoxingStrategy , new Item(message:"Boxing may occur for the representable type {0}. Consider constraining it to either `class` or `struct` in order to help the generator emit an efficient implementation. Alternatively, use the `StorageOption.Field` option to generate dedicated field for the type.",title:"Possible Boxing Storage Detected")},
{Id.PossibleTleStrategy , new Item(message:"The selected storage option for {0} was ignored because it could cause a `TypeLoadException` to be thrown.",title:"Possible TypeLoadException Detected")},
{Id.RecordTarget , new Item(message:"Union types may not be declared as 'record' types.",title:"Record Union Type Declaration")},
{Id.RepresentableTypeIsInterface , new Item(message:"No conversion operators will be generated for the representable type {0} because it is an interface.",title:"Conversion Operators Omitted")},
{Id.RepresentableTypeIsSupertype , new Item(message:"No conversion operators will be generated for the representable type {0} because it is a supertype of the target union type.",title:"Conversion Operators Omitted")},
{Id.ReservedGenericParameterName , new Item(message:"The targeted union type contains a generic parameter named `{0}` which is reserved for generated code.",title:"Reserved Generic Parameter Name")},
{Id.SmallGenericUnion , new Item(message:"The small layout setting was ignored because the target union type is generic.",title:"Ignoring Small Layout")},
{Id.StaticTarget , new Item(message:"Union types may not be declared using the 'static' modifier.",title:"Static Union Type Declaration")},
{Id.TleStrategy , new Item(message:"The selected storage option for {0} was ignored because it would cause a `TypeLoadException` to be thrown.",title:"TypeLoadException Prevented")},
{Id.UnionTypeSettingsOnNonUnionType , new Item(message:"The union type settings attribute will be ignored because the target type is not a union type.",title:"Union Type Settings Ignored")},
{Id.UnknownGenericParameterName , new Item(message:"The targeted union type contains an unknown parameter name `{0}`. It could not be located in the type parameter list.",title:"Unknown Generic Parameter Name") }
};

    static String MessageFor(Id id) => _items[id].Message;
    static String TitleFor(Id id) => _items[id].Title;
}
