using CommandQuerySeparation.Queries;
using CommandQuerySeparation.Commands;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Entities;

namespace QwackX.Blazor.Domain.Repositories {

    public interface IUserRepository :
        IQueryAsyncHandler<ListUsersQuery, IEnumerable<User>>,
        IQueryAsyncHandler<DetailUserQuery, User>,
        ICommandAsyncHandler<AddUserCommand>,
        ICommandAsyncHandler<EditUserCommand>,
        ICommandAsyncHandler<DeleteUserCommand>
    {}
}