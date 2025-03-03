using CommandQuerySeparation.Commands;
using QwackX.Api.Domain.Commands;

namespace QwackX.Api.Domain.Repositories;

public interface IPostViewRepository : 
    ICommandHandler<BulkInsertPostsViews>
{
}