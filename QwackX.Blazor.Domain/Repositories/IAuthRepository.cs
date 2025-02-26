using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;

namespace QwackX.Blazor.Domain.Repositories;

public interface IAuthRepository :
    IQueryAsyncHandler<LoginUserQuery, User>,
    ICommandAsyncHandler<RegisterUserCommand>
{
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetUsernameAsync();
    Task SignIn(int userId, string username);
    Task SignOut();
    Task<(int? UserId, string? Username)> GetUser();
    Task<Result> SetAuthorizationHeader();
}
