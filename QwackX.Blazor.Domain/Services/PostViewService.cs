using System.Net.Http.Json;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services;

public class PostViewService : BaseService, IPostViewRepository
{
    public PostViewService(IAuthRepository authRepository) : base(authRepository) { }
    
    public async Task<Result> ExecuteAsync(IncrementPostViewsCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            HttpContent httpContent = JsonContent.Create(command);
            using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/PostView/increment", httpContent))
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