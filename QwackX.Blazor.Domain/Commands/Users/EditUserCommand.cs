using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class EditUserCommand : ICommandDefinition
{
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }
    public string Password { get; }

    public EditUserCommand(int userId, string username, string email, string password)
    {
        UserId = userId;
        Username = username;
        Email = email;
        Password = password;
    }
}