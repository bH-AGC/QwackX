using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Queries;

public class DetailUserQuery : IQueryDefinition <User?>
{
    public int UserId { get; }

    public DetailUserQuery(int userId)
    {
        UserId = userId;
    }
}