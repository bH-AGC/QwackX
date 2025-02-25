using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Queries;

public class DetailUserQuery : IQueryDefinition<User>
{
    public int Id { get; }

    public DetailUserQuery(int id)
    {
        Id = id;
    }
}