using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Queries;

public class DetailPostQuery : IQueryDefinition<Post>
{
    public int Id { get; }

    public DetailPostQuery(int id)
    {
        Id = id;
    }
}