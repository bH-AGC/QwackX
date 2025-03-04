using CommandQuerySeparation.Commands;

namespace QwackX.Blazor.Domain.Commands;

public class IncrementPostViewsCommand : ICommandDefinition
{
    public int PostId { get; set; }
    public int UserId { get; set; }
    
    public IncrementPostViewsCommand(int postId, int userId)
    {
        PostId = postId;
        UserId = userId;
    }
}