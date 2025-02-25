using CommandQuerySeparation.Commands;

namespace QwackX.Api.Domain.Commands;

public class RegisterUserCommand : ICommandDefinition
{
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public RegisterUserCommand(string username, string email, string password)
    {
        Username = username;
        Email = email;
        Password = password;
    }
}