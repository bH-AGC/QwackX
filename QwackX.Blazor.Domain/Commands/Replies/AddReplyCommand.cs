using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class AddReplyCommand : ICommandDefinition
{
    public int PostId { get; }
    public int UserId { get; }
    public string? Content { get; }

    public AddReplyCommand(int postId, int userId, string content)
    {
        PostId = postId;
        UserId = userId;
        Content = content;
    }
}