using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Commands;

namespace QwackX.Api.Domain.Repositories;

public interface ILikeRepository :
    ICommandHandler<LikeCommand>
{
    
}