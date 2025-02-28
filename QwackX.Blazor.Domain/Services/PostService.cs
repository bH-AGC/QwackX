using System.Net.Http.Json;
using System.Text.Json;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services;

public class PostService : BaseService, IPostRepository
{
    public PostService(IAuthRepository authRepository) : base(authRepository) { }
    
    public async Task<Result<IEnumerable<PostTitle?>>> ExecuteAsync(ListeTitlePostsQuery query)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await HttpClient.GetAsync("api/posts/titles"))
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    return Result<IEnumerable<PostTitle?>>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                }

                string json = await responseMessage.Content.ReadAsStringAsync();

                PostTitle[]? postTitles = JsonSerializer.Deserialize<PostTitle[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (postTitles is null)
                {
                    return Result<IEnumerable<PostTitle?>>.Success(Enumerable.Empty<PostTitle>());
                }

                return Result<IEnumerable<PostTitle?>>.Success(postTitles);
            }
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PostTitle?>>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<Result<Post>> ExecuteAsync(DetailPostQuery query)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await HttpClient.GetAsync($"api/posts/{query.PostId}"))
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                    return Result<Post>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                }

                string json = await responseMessage.Content.ReadAsStringAsync();

                Post post = JsonSerializer.Deserialize<Post>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

                return Result<Post>.Success(post);
            }
        }
        catch (Exception ex)
        {
            return Result<Post>.Failure(ex.Message, ex);
        }
    }

    public async Task<Result> ExecuteAsync(AddPostCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            HttpContent httpContent = JsonContent.Create(command);
            using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/posts", httpContent))
            {
                return await CommandResultMessageAsync(responseMessage);
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ex);
        }
    }
    
    public async Task<Result> ExecuteAsync(DeletePostCommand command)
    {
        try
        {
            await AuthRepository.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await HttpClient.DeleteAsync($"api/posts/{command.PostId}"))
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