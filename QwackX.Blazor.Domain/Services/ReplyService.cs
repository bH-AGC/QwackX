using System.Net.Http.Json;
using System.Text.Json;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services;

public class ReplyService : BaseService, IReplyRepository
{
    public ReplyService(IAuthRepository authRepository) : base(authRepository) { }
    
    public async Task<Result<IEnumerable<Reply?>>> ExecuteAsync(ListPostRepliesQuery query)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await HttpClient.GetAsync($"api/replies/{query.PostId}"))
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    return Result<IEnumerable<Reply?>>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                }

                string json = await responseMessage.Content.ReadAsStringAsync();

                Reply[]? replies = JsonSerializer.Deserialize<Reply[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (replies is null)
                {
                    return Result<IEnumerable<Reply?>>.Success(Enumerable.Empty<Reply>());
                }

                return Result<IEnumerable<Reply?>>.Success(replies);
            }
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<Reply?>>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<Result> ExecuteAsync(AddReplyCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            HttpContent httpContent = JsonContent.Create(command);
            using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/replies", httpContent))
            {
                return await CommandResultMessageAsync(responseMessage);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ex);
        }
    }

    public async  Task<Result> ExecuteAsync(DeleteReplyCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await HttpClient.DeleteAsync($"api/replies/{command.ReplyId}"))
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