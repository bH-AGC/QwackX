using System.Data.Common;
using BStorm.Tools.Database;
using CommandQuerySeparation.Results;
using QwackX.Api.Domain.Commands;
using QwackX.Api.Domain.Entities;
using QwackX.Api.Domain.Mappers;
using QwackX.Api.Domain.Queries;
using QwackX.Api.Domain.Repositories;

namespace QwackX.Api.Domain.Services;

public class UserServices : IUserRepository
{
    private readonly DbConnection _dbConnection;
    private IUserRepository _userRepositoryImplementation;

    public UserServices (DbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Open();
    }
    
    public Result<IEnumerable<User>> Execute(ListUsersQuery query)
    {
        try
        {
            IEnumerable<User> users = _dbConnection.ExecuteReader("[AppUserSchema].[ListUsers]", dr => dr.ToUser(), true);

            if (users.Any())
            {
                return Result<IEnumerable<User>>.Success(users);
            }
            else
            {
                return Result<IEnumerable<User>>.Failure("No Users Found");
            }
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<User>>.Failure(($"Code de retour : {ex.Message}"));
        }
    }
    
    public Result<User?> Execute(DetailUserQuery query)
    {
        try
        {
            User? user = _dbConnection.ExecuteReader("[AppUserSchema].[DetailUser]", dr => dr.ToUser(), true, query).SingleOrDefault();

            if (user is null)
            {
                return Result<User?>.Failure("No Users Found");
            }
            else
            {
                return Result<User?>.Success(user);
            }
        }
        catch (Exception ex)
        {
            return Result<User?>.Failure($"Code de retour : {ex.Message}");
        }
    }

    public Result Execute(AddUserCommand command)
    {
        try
        {
            int responseMessage = _dbConnection.ExecuteNonQuery("[AppUserSchema].[CreateUser]", true, command);

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
    
    public Result Execute(EditUserCommand command)
    {
        try
        {
            int responseMessage = _dbConnection.ExecuteNonQuery("[AppUserSchema].[EditUser]", true, command);

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

    public Result Execute(DeleteUserCommand command)
    {
        try
        {
            int responseMessage = _dbConnection.ExecuteNonQuery("[AppUserSchema].[DeleteUser]", true, command);

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