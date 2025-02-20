using System.Data.Common;
using BStorm.Tools.Database;
using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Mappers;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services;

public class AuthService : BaseRepository, IAuthRepository
{
    public AuthService(DbConnection dbConnection) : base(dbConnection) { }
    
    public Result<User> Execute(LoginUserQuery userQuery)
    {
        try
        {
            User? utilisateur = DbConnection.ExecuteReader("[AppUserSchema].[LoginUser]", dr => dr.ToUser(), true, userQuery).SingleOrDefault();

            if (utilisateur is null)
                return Result<User>.Failure("Email incorrect");

            if (!BCrypt.Net.BCrypt.Verify(userQuery.PasswordHash, utilisateur.PasswordHash))
                return Result<User>.Failure("Mot de passe incorrect");
            
            return Result<User>.Success(utilisateur);            
        }
        catch (Exception ex)
        {
            return Result<User>.Failure(ex.Message, ex);
        }
    }
}