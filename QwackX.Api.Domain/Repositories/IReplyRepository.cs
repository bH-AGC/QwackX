using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;

namespace QwackX.Api.Domain.Repositories;

public interface IReplyRepository : 
    IQueryHandler<ListPostRepliesQuery, IEnumerable<Reply?>>,
    ICommandHandler<AddReplyCommand>,
    ICommandHandler<DeleteReplyCommand>
{
}