using System.Data;
using QwackX.Api.Domain.Entities;

namespace QwackX.Api.Domain.Mappers;

internal static partial class Mappers
{
    public static User ToUser(this IDataRecord record)
    {
        return new User
        {
            Id = (int)record["Id"],
            Username = (string)record["Username"],
            Email = (string)record["Email"],
            PasswordHash = (string)record["PasswordHash"],
            CreatedAt = (DateTime)record["CreatedAt"]
        };
    }
}
