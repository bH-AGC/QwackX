using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class EditUserCommand : ICommandDefinition
{
    public int Id { get; }
    public string Username { get; }
    public string Email { get; }
    public string PasswordHash { get; }

    public EditUserCommand(int id, string username, string email, string passwordHash)
    {
        Id = id;
        Username = username;
        Email = email;
        PasswordHash = passwordHash;
    }
}