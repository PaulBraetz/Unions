namespace RhoMicro.Unions.Abstractions;

/// <summary>
/// Represents a superset union type.
/// </summary>
/// <typeparam name="TSubset">The subset union type that this union type is a superset of.</typeparam>
/// <typeparam name="TSuperset">This type (akin to <c>TSelf</c>).</typeparam>
public interface ISuperset<TSubset, TSuperset>
    where TSuperset : ISuperset<TSubset, TSuperset>
{
    /// <summary>
    /// Implicitly converts an instance of the subset union type to this superset union type.
    /// </summary>
    /// <param name="subset">The subset union type instance to convert.</param>
    static abstract implicit operator TSuperset(TSubset subset);
    /// <summary>
    /// Explicitly converts an instance of this superset union type to the subset union type.
    /// </summary>
    /// <param name="superset">The superset union type instance to convert.</param>
    static abstract explicit operator TSubset(TSuperset superset);
}
/// <summary>
/// Represents a union type that is able to represent 1 type
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
public interface IUnion<T1>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 2 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 3 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 4 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 5 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 6 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 7 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 8 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 9 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 10 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 11 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 12 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 13 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 14 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 15 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 16 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 17 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 18 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 19 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 20 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 21 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 22 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 23 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 24 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 25 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 26 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 27 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 28 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T28">The 28th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>,
		ISuperset<T28, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 29 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T28">The 28th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T29">The 29th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28,T29>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>,
		ISuperset<T28, TSupersetUnion>,
		ISuperset<T29, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 30 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T28">The 28th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T29">The 29th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T30">The 30th type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28,T29,T30>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>,
		ISuperset<T28, TSupersetUnion>,
		ISuperset<T29, TSupersetUnion>,
		ISuperset<T30, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 31 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T28">The 28th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T29">The 29th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T30">The 30th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T31">The 31st type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28,T29,T30,T31>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>,
		ISuperset<T28, TSupersetUnion>,
		ISuperset<T29, TSupersetUnion>,
		ISuperset<T30, TSupersetUnion>,
		ISuperset<T31, TSupersetUnion>;
}/// <summary>
/// Represents a union type that is able to represent 32 types
/// </summary>
/// <typeparam name="T1">The first type unions of this type are able to represent.</typeparam>
/// <typeparam name="T2">The second type unions of this type are able to represent.</typeparam>
/// <typeparam name="T3">The third type unions of this type are able to represent.</typeparam>
/// <typeparam name="T4">The fourth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T5">The fifth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T6">The sixth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T7">The seventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T8">The eigth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T9">The ninth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T10">The tenth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T11">The eleventh type unions of this type are able to represent.</typeparam>
/// <typeparam name="T12">The twelfth type unions of this type are able to represent.</typeparam>
/// <typeparam name="T13">The 13th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T14">The 14th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T15">The 15th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T16">The 16th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T17">The 17th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T18">The 18th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T19">The 19th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T20">The 20th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T21">The 21st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T22">The 22nd type unions of this type are able to represent.</typeparam>
/// <typeparam name="T23">The 23th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T24">The 24th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T25">The 25th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T26">The 26th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T27">The 27th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T28">The 28th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T29">The 29th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T30">The 30th type unions of this type are able to represent.</typeparam>
/// <typeparam name="T31">The 31st type unions of this type are able to represent.</typeparam>
/// <typeparam name="T32">The 32nd type unions of this type are able to represent.</typeparam>
public interface IUnion<T1,T2,T3,T4,T5,T6,T7,T8,T9,T10,T11,T12,T13,T14,T15,T16,T17,T18,T19,T20,T21,T22,T23,T24,T25,T26,T27,T28,T29,T30,T31,T32>
{
	/// <summary>
	/// Safely converts this instance to a union type that is either a superset of or congruent to this one.
	/// </summary>
	/// <typeparam name="TSupersetUnion">The type of union to convert this instance to.</typeparam>
	TSupersetUnion DownCast<TSupersetUnion>()
	where TSupersetUnion : 
		ISuperset<T1, TSupersetUnion>,
		ISuperset<T2, TSupersetUnion>,
		ISuperset<T3, TSupersetUnion>,
		ISuperset<T4, TSupersetUnion>,
		ISuperset<T5, TSupersetUnion>,
		ISuperset<T6, TSupersetUnion>,
		ISuperset<T7, TSupersetUnion>,
		ISuperset<T8, TSupersetUnion>,
		ISuperset<T9, TSupersetUnion>,
		ISuperset<T10, TSupersetUnion>,
		ISuperset<T11, TSupersetUnion>,
		ISuperset<T12, TSupersetUnion>,
		ISuperset<T13, TSupersetUnion>,
		ISuperset<T14, TSupersetUnion>,
		ISuperset<T15, TSupersetUnion>,
		ISuperset<T16, TSupersetUnion>,
		ISuperset<T17, TSupersetUnion>,
		ISuperset<T18, TSupersetUnion>,
		ISuperset<T19, TSupersetUnion>,
		ISuperset<T20, TSupersetUnion>,
		ISuperset<T21, TSupersetUnion>,
		ISuperset<T22, TSupersetUnion>,
		ISuperset<T23, TSupersetUnion>,
		ISuperset<T24, TSupersetUnion>,
		ISuperset<T25, TSupersetUnion>,
		ISuperset<T26, TSupersetUnion>,
		ISuperset<T27, TSupersetUnion>,
		ISuperset<T28, TSupersetUnion>,
		ISuperset<T29, TSupersetUnion>,
		ISuperset<T30, TSupersetUnion>,
		ISuperset<T31, TSupersetUnion>,
		ISuperset<T32, TSupersetUnion>;
}