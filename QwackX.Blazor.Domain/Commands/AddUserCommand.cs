using CommandQuerySeparation.Commands;
namespace QwackX.Blazor.Domain.Commands
{
    public class AddUserCommand : ICommandDefinition
    {
        public string Username { get; }
        public string Email { get; }
        public string PasswordHash { get; }

        public AddUserCommand(string username, string email, string passwordHash)
        {
            Username = username;
            Email = email;
            PasswordHash = passwordHash;
        }
    }
}