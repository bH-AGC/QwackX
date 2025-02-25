using CommandQuerySeparation.Commands;
namespace QwackX.Blazor.Domain.Commands
{
    public class DeleteUserCommand : ICommandDefinition
    {
        public int Id { get; }

        public DeleteUserCommand(int id)
        {
            Id = id;
        }
    }
}