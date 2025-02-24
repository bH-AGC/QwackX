using QwackX.Api.Controllers;

namespace QwackX.Api.Infrastructure;

public interface ITokenRepository
{
    UserDto? User { get; }
    void ApplyToken(UserDto user);
}