using System.Data.Common;
using BStorm.Tools.Database;
using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services;

public class LikeService : BaseService, ILikeRepository
{
    public LikeService(DbConnection dbConnection) : base(dbConnection) { }
    
    public Result Execute(LikeCommand command)
    {
        try
        {
            int responseMessage = DbConnection.ExecuteNonQuery("[dbo].[Like" + command.EntityType + "]", true, command);

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