using CommandQuerySeparation.Commands;

namespace QwackX.Api.Domain.Commands
{
    public class DeleteUserCommand : ICommandDefinition
    {
        public int UserId { get; }

        public DeleteUserCommand(int id)
        {
            UserId = id;
        }
    }
}