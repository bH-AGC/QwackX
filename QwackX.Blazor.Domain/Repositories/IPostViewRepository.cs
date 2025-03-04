using CommandQuerySeparation.Commands;
using QwackX.Blazor.Domain.Commands;

namespace QwackX.Blazor.Domain.Repositories;

public interface IPostViewRepository :
    ICommandAsyncHandler<IncrementPostViewsCommand>
{
    
}