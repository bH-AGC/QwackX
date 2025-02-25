using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class DeletePostCommand : ICommandDefinition
{
    public int Id { get; }

    public DeletePostCommand(int id)
    {
        Id = id;
    }
}