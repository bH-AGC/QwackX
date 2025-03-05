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
    public class ReplyService : BaseService, IReplyRepository
    {
        public ReplyService(DbConnection dbConnection) : base(dbConnection) { }
        
        public Result<IEnumerable<Reply?>> Execute(ListPostRepliesQuery query)
        {
            try
            {
                IEnumerable<Reply?> replies = DbConnection.ExecuteReader("[AppUserSchema].[ListPostReplies]", dr => dr.ToReply(), true, query);

                if (replies.Any())
                {
                    return Result<IEnumerable<Reply>>.Success(replies);
                }
                else
                {
                    return Result<IEnumerable<Reply?>>.Success(Enumerable.Empty<Reply>()); 
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<Reply?>>.Failure($"Code de retour : {ex.Message}");
            }
        }
        
        public Result Execute(AddReplyCommand command)
        {
            try
            {
                int responseMessage = DbConnection.ExecuteNonQuery("[AppUserSchema].[CreateReply]", true, command);

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
        
        public Result Execute(DeleteReplyCommand command)
        {
            try
            {
                int responseMessage = DbConnection.ExecuteNonQuery("[AppUserSchema].[DeleteReply]", true, command);

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
