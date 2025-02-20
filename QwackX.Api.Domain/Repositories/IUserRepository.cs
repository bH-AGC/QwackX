using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;

namespace QwackX.Api.Domain.Repositories;

public interface IUserRepository : 
    IQueryHandler<ListUsersQuery, IEnumerable<User>>,
    IQueryHandler<DetailUserQuery, User>,
    ICommandHandler<AddUserCommand>,
    ICommandHandler<EditUserCommand>,
    ICommandHandler<DeleteUserCommand>
{
    
}