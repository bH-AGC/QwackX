using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Queries;

public class LoginUserQuery : IQueryDefinition<User>
{
    public string Email { get; }
    public string PasswordHash { get; }

    public LoginUserQuery(string email, string passwd)
    {
        Email = email;
        PasswordHash = passwd;
    }
}