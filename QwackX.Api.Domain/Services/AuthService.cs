using System.Data.Common;
using BStorm.Tools.Database;
using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Mappers;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services;

public class AuthService : BaseService, IAuthRepository
{
    public AuthService(DbConnection dbConnection) : base(dbConnection) { }
    
    public Result<User> Execute(LoginUserQuery userQuery)
    {
        try
        {
            User? utilisateur = DbConnection.ExecuteReader("[AppUserSchema].[LoginUser]", dr => dr.ToUser(), true, userQuery).SingleOrDefault();

            if (utilisateur is null)
                return Result<User>.Failure("Email et/ou mot de passe incorrecte");
            
            return Result<User>.Success(utilisateur);            
        }
        catch (Exception ex)
        {
            return Result<User>.Failure(ex.Message, ex);
        }
    }

    public Result Execute(RegisterUserCommand command)
    {
        try
        {
            int responseMessage = DbConnection.ExecuteNonQuery("[AppUserSchema].[RegisterUser]", true, command);

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