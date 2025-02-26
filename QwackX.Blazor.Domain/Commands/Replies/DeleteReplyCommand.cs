using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class DeleteReplyCommand : ICommandDefinition
{
    public int ReplyId { get; }

    public DeleteReplyCommand(int replyId)
    {
        ReplyId = replyId;
    }
}