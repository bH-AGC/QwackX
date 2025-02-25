using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Queries;

public class LoginUserQuery : IQueryDefinition<User>
{
    public string Email { get; }
    public string Password { get; }

    public LoginUserQuery(string email, string passwd)
    {
        Email = email;
        Password = passwd;
    }
}