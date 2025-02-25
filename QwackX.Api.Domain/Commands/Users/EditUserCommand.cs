using CommandQuerySeparation.Commands;

namespace QwackX.Api.Domain.Commands;

public class EditUserCommand : ICommandDefinition
{
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }
    public string PasswordHash { get; }

    public EditUserCommand(int id, string username, string email, string passwordHash)
    {
        UserId = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}