using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Queries;

public class DetailPostQuery : IQueryDefinition<Post>
{
    public int PostId { get; }
    
    public int UserId { get; }

    public DetailPostQuery(int postId, int userId)
    {
        PostId = postId;
        UserId = userId;
    }
}