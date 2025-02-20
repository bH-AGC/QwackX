using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;

namespace QwackX.Api.Domain.Repositories;

public interface IAuthRepository : 
    IQueryHandler<LoginUserQuery, User>
{
    
}