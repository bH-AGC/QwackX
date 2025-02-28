using System.Data.Common;
using BStorm.Tools.Database;
using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Mappers;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services
{
    public class PostService : BaseService, IPostRepository
    {
        public PostService(DbConnection dbConnection) : base(dbConnection) { }
        
        public Result<IEnumerable<PostTitle?>> Execute(ListeTitlePostsQuery query)
        {
            try
            {
                IEnumerable<PostTitle?> postTitles = DbConnection.ExecuteReader("[dbo].[ListPostsTitles]", dr => dr.ToPostTitle(), true, query).ToList();
                
                if (postTitles.Any())
                {
                    return Result<IEnumerable<PostTitle?>>.Success(postTitles);
                }
                else
                {
                    return Result<IEnumerable<PostTitle?>>.Failure("No Posts Found");
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<PostTitle?>>.Failure($"Code de retour : {ex.Message}");
            }
        }
        
        public Result<Post?> Execute(DetailPostQuery query)
        {
            try
            {
                Post? post = DbConnection.ExecuteReader("[dbo].[Detailpost]", dr => dr.ToPost(), true, query).SingleOrDefault();

                if (post is null)
                {
                    return Result<Post?>.Failure("No Post Found");
                }
                else
                {
                    return Result<Post?>.Success(post);
                }
            }
            catch (Exception ex)
            {
                return Result<Post?>.Failure($"Code de retour : {ex.Message}");
            }
        }
        
        public Result Execute(AddPostCommand command)
        {
            try
            {
                int responseMessage = DbConnection.ExecuteNonQuery("[dbo].[CreatePost]", true, command);

                if (responseMessage == 1)
                {
                    return Result.Success();
                }

                return Result.Failure($"Code de retour : {responseMessage}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Code de retour : {ex.Message}");
            }
        }

        public Result Execute(DeletePostCommand command)
        {
            try
            {
                int responseMessage = DbConnection.ExecuteNonQuery("[dbo].[DeletePost]", true, command);

                if (responseMessage == 1)
                {
                    return Result.Success();
                }

                return Result.Failure($"Code de retour : {responseMessage}");
            }
            catch (Exception ex)
            {
                return Result.Failure($"Code de retour : {ex.Message}");
            }
        }
    }
}