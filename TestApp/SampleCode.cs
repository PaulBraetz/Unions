namespace RhoMicro.Unions.TestApp.SampleCode;

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
    private User? HandleMultipleResult(MultipleUsersResult result)
    {
        ErrorMessage = $"{result.Count} users have been located. The name was not precise enough.";
        return null;
    }
}

sealed class UnauthorizedDatabaseAccessException : Exception;
interface IUserRepository
{
    IQueryable<User> UsersByName(String name);
}
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
            return new MultipleUsersResult(reifiedUsers.Length);
        }

        return reifiedUsers[0];
    }
}

interface IUserService
{
    GetUserResult GetUserByName(String name);
}

enum ErrorCode
{
    NotFound,
    Unauthorized
}
sealed record User(String Name);
readonly record struct MultipleUsersResult(Int32 Count);

[UnionType(typeof(ErrorCode))]
[UnionType(typeof(MultipleUsersResult))]
[UnionType(typeof(User))]
readonly partial struct GetUserResult;