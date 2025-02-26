using QwackX.Api.Controllers;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Models.Mappers;

public static class Mappers
{
    public static UserDto ToUserDto(this User entity)
    {
        return new UserDto()
        {
            UserId = entity.UserId,
            Username = entity.Username,
            Email = entity.Email,
        };
    }
}