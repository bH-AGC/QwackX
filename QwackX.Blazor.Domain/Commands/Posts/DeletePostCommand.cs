using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class DeletePostCommand : ICommandDefinition
{
    public int PostId { get; }

    public DeletePostCommand(int postId)
    {
        PostId = postId;
    }
}