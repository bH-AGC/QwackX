using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;

namespace QwackX.Blazor.Domain.Repositories;

public interface IPostRepository : 
    IQueryAsyncHandler<ListeTitlePostsQuery, IEnumerable<PostTitle>>,
    IQueryAsyncHandler<DetailPostQuery, Post>,
    ICommandAsyncHandler<AddPostCommand>,
    ICommandAsyncHandler<DeletePostCommand>
{
    
}