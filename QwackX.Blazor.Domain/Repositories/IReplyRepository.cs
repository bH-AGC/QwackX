using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;

namespace QwackX.Blazor.Domain.Repositories;

public interface IReplyRepository :
    IQueryAsyncHandler<ListPostRepliesQuery, IEnumerable<Reply?>>,
    ICommandAsyncHandler<AddReplyCommand>,
    ICommandAsyncHandler<DeleteReplyCommand>
{
    
}