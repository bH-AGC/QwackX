using CommandQuerySeparation.Commands;
using CommandQuerySeparation.Queries;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Queries;

namespace QwackX.Api.Domain.Repositories;

public interface IPostRepository : 
    IQueryHandler<ListeTitlePostsQuery, IEnumerable<PostTitle?>>,
    IQueryHandler<DetailPostQuery, Post?>,
    ICommandHandler<AddPostCommand>,
    ICommandHandler<DeletePostCommand>
{
    
}