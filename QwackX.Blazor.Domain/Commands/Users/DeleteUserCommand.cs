using CommandQuerySeparation.Commands;
namespace QwackX.Blazor.Domain.Commands
{
    public class DeleteUserCommand : ICommandDefinition
    {
        public int UserId { get; }

        public DeleteUserCommand(int userId)
        {
            UserId = userId;
        }
    }
}