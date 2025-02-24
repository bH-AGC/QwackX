using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Queries;

public class DetailPostQuery : IQueryDefinition<Post>
{
    public int PostId { get; }

    public DetailPostQuery(int postId)
    {
        PostId = postId;
    }
}