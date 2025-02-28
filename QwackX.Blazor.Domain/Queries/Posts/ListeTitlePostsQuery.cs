using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Commands;

public class ListeTitlePostsQuery : IQueryDefinition<IEnumerable<PostTitle?>>
{
    public int UserId { get; }

    public ListeTitlePostsQuery(int userId)
    {
        UserId = userId;
    }
}