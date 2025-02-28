using System.Net.Http.Json;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services;

public class LikeService : BaseService, ILikeRepository
{
    public LikeService(IAuthRepository authRepository) : base(authRepository) { }

    public async Task<Result> ExecuteAsync(LikeCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            HttpContent httpContent = JsonContent.Create(command);
            using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/likes", httpContent))
            {
                return await CommandResultMessageAsync(responseMessage);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ex);
        }
    }
}