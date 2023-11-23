# Unions

Read about union types here: https://en.wikipedia.org/wiki/Union_type
Alternative union type implementations: 
- [OneOf](https://github.com/mcintyre321/OneOf)
- [ValueVariant](https://github.com/hikarin522/ValueVariant)
- [DiscriminatedUnion](https://github.com/sdedalus/DiscriminatedUnion)
- [CSharpDiscriminatedUnion](https://github.com/Galad/CSharpDiscriminatedUnion)
- [UnionType](https://github.com/Cricle/UnionType)
- [Funcky Discriminated Union](https://github.com/polyadic/funcky-discriminated-union)
- [N.SourceGenerators.UnionTypes](https://github.com/Ne4to/N.SourceGenerators.UnionTypes)

## Installation

Requirements: `net7` (due to `static abstract` members)

Package Reference:
```
<PackageReference Include="RhoMicro.Unions" Version="0.0.0-alpha.11">
  <PrivateAssets>all</PrivateAssets>
  <IncludeAssets>runtime; build; native; contentfiles; analyzers</IncludeAssets>
</PackageReference>
```
CLI:
```
dotnet add package RhoMicro.Unions
```

## Contrived Example

In our imaginary usecase, a user shall be retrieved from the infrastructure via a name query. The following types will be found throughout the example:

```cs
sealed record User(String Name);

enum ErrorCode
{
    NotFound,
    Unauthorized
}

readonly record struct MultipleUsersError(Int32 Count);
```

The `User` type represents a user. The `ErrorCode` represents an error that does not contain additional information, like `MultipleUsersError` does. It represents multiple users having been found while only one was requested.

We define a union type to represent our imaginary query:

```cs
[UnionType(typeof(ErrorCode))]
[UnionType(typeof(MultipleUsersError))]
[UnionType(typeof(User))]
readonly partial struct GetUserResult;
```
Instances of `GetUserResult` can represent *either* an instance of `ErrorCode`, `MultipleUsersError` or `User`.

It will be used in a service façade like so:
```cs
interface IUserService
{
    GetUserResult GetUserByName(String name);
}
```
A repository abstracts over the underlying infrastructure:
```cs
interface IUserRepository
{
    IQueryable<User> UsersByName(String name);
}
```
Access violations would be communicated through the repository using the following exception type: 
```cs
sealed class UnauthorizedDatabaseAccessException : Exception;
```
An implementation of the `IUserService` is provided as follows:
```cs
sealed class UserService : IUserService
{
    public UserService(IUserRepository repository) => _repository = repository;

    private readonly IUserRepository _repository;

    public GetUserResult GetUserByName(String name)
    {
        IQueryable<User> users;
        try
        {
            users = _repository.UsersByName(name);
        } catch(UnauthorizedDatabaseAccessException)
        {
            return ErrorCode.Unauthorized;
        }

        var reifiedUsers = users.ToArray();
        if(reifiedUsers.Length == 0)
        {
            return ErrorCode.NotFound;
        } else if(reifiedUsers.Length > 1)
        {
            return new MultipleUsersError(reifiedUsers.Length);
        }

        return reifiedUsers[0];
    }
}
```
As you can see, possible representations of `GetUserResult` are implicitly converted and returned by the service. Users of `OneOf` will be familiar with this.

On the consumer side of this api, a generated `Match` function helps with transforming the union instance to another type:

```cs
sealed class UserModel
{
    public UserModel(IUserService service) => _service = service;

    private readonly IUserService _service;

    public String ErrorMessage { get; private set; } = String.Empty;
    public User? User { get; private set; }
    public void SetUser(String name)
    {
        var getUserResult = _service.GetUserByName(name);
        User = getUserResult.Match(
            HandleErrorCode,
            HandleMultipleResult,
            user => user);
    }
    private User? HandleErrorCode(ErrorCode code)
    {
        ErrorMessage = code switch
        {
            ErrorCode.NotFound => "The user could not be located.",
            ErrorCode.Unauthorized => "You are not authorized to access users.",
            _ => throw new NotImplementedException()
        };
        return null;
    }
    private User? HandleMultipleResult(MultipleUsersError result)
    {
        ErrorMessage = $"{result.Count} users have been located. The name was not precise enough.";
        return null;
    }
}
```

Here is a list of some generated members on the `GetUserResult` union type (implementations have been elided):
```cs
/*Factories*/
public static GetUserResult Create(ErrorCode value);
public static GetUserResult Create(MultipleUsersResult value);
public static GetUserResult Create(User value);
public static Boolean TryCreate<TValue>(TValue value, out GetUserResult instance);
public static GetUserResult Create<TValue>(TValue value);

/*Handling Methods*/
public void Switch(
    Action<ErrorCode> onErrorCode, 
    Action<MultipleUsersResult> onMultipleUsersResult,
    Action<User> onUser);

public TResult Match<TResult>(
    Func<ErrorCode, TResult> onErrorCode,
    Func<MultipleUsersResult, TResult> onMultipleUsersResult,
    Func<User, TResult> onUser);

/*Casting & Conversion*/
public TResult DownCast<TResult>();

public Boolean Is<TValue>();

public Boolean Is(Type type);

public TValue As<TValue>();

public Boolean IsErrorCode;
public ErrorCode AsErrorCode;

public Boolean IsMultipleUsersResult;
public MultipleUsersResult AsMultipleUsersResult;

public Boolean IsUser;
public User AsUser;

public Type GetRepresentedType();

public static implicit operator GetUserResult(ErrorCode value);
public static explicit operator ErrorCode(GetUserResult union);

public static implicit operator GetUserResult(MultipleUsersResult value);
public static explicit operator MultipleUsersResult(GetUserResult union);

public static implicit operator GetUserResult(User value);
public static explicit operator User(GetUserResult union);
```

## Why Not OneOf?

Here are some issues that I have with `OneOf` that this generator aims to solve:
- the internal tag field is a byte instead of an int
- by default, not every representable type has a dedicated field generated for it
- generic value type unions are possible:
```cs
[UnionType(typeof(String))]
[UnionType(nameof(T), Alias = "Result")]
readonly partial struct Result<T>;
```
with helpful members like `IsResult` being generated for you.
- type order does not matter; convert to equivalent unions with ease:
```cs
Union u = DateTime.Now;
//Output: Union(<DateTime> | Double | String){23/11/2023 17:58:58}
Console.WriteLine(u);
EquivalentUnion eu = u.DownCast<EquivalentUnion>();
//Output: EquivalentUnion(<DateTime> | Double | String){23/11/2023 17:58:58}
Console.WriteLine(eu);

[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
[UnionType(typeof(Double))]
readonly partial struct Union;

[UnionType(typeof(Double))]
[UnionType(typeof(DateTime))]
[UnionType(typeof(String))]
sealed partial class EquivalentUnion;
```
- avoid `OneOf` invading your apis, instead rely on dedicated domain names for your union types
- use custom generated members for contextually semantic access to union data:
```cs
var r = Result<String>.CreateFromResult("Hello, World!");
if(r.IsErrorMessage)
{
    //handle error
} else if(r.IsResult)
{
    //handle result
}

//alternatively:
r.Switch(
    onErrorMessage: m =>/*handle error*/,
    onResult: r =>/*handle result*/);

[UnionType(typeof(String), Alias = "ErrorMessage")]
[UnionType(nameof(T), Alias = "Result")]
readonly partial struct Result<T>;
```
*Note that for generic union types, there will be no conversion operators generated for representable parameter types. That is why we are using the generated `CreateFromResult` factory method here.*
