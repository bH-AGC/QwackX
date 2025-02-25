using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;

namespace QwackX.Blazor.Domain.Repositories;

public interface IAuthRepository :
    IQueryAsyncHandler<LoginUserQuery, User>,
    ICommandAsyncHandler<RegisterUserCommand>
{
    
}