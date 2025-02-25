using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Queries;

public class ListPostRepliesQuery : IQueryDefinition<IEnumerable<Reply?>>
{
    public int PostId { get; }

    public ListPostRepliesQuery(int postId)
    {
        PostId = postId;
    }
}