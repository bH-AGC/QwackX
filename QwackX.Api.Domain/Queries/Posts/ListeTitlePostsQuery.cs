using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Queries;

public class ListeTitlePostsQuery : IQueryDefinition<IEnumerable<PostTitle?>>
{
    public int UserId { get; }

    public ListeTitlePostsQuery(int userId)
    {
        UserId = userId;
    }
}