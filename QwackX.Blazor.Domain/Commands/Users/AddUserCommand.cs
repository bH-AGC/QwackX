using CommandQuerySeparation.Commands;
namespace QwackX.Blazor.Domain.Commands
{
    public class AddUserCommand : ICommandDefinition
    {
        public string Username { get; }
        public string Email { get; }
        public string Password { get; }

        public AddUserCommand(string username, string email, string password)
        {
            Username = username;
            Email = email;
            Password = password;
        }
    }
}