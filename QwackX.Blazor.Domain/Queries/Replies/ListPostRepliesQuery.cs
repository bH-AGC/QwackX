using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Queries;

public class ListPostRepliesQuery : IQueryDefinition<IEnumerable<Reply?>>
{
    public int PostId { get; }

    public ListPostRepliesQuery(int postId)
    {
        PostId = postId;
    }
}