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
    public PostService(IHttpClientFactory httpClientFactory, AuthService authService)
        : base(httpClientFactory, authService) { }
    
    public async Task<Result<IEnumerable<PostTitle>>> ExecuteAsync(ListeTitlePostsQuery query)
    {
        try
        {
            await _authService.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/posts"))
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return Result<IEnumerable<PostTitle>>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}");
                }

                string json = await responseMessage.Content.ReadAsStringAsync();

                PostTitle[]? postTitles = JsonSerializer.Deserialize<PostTitle[]>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

                if (postTitles is null)
                    return Result<IEnumerable<PostTitle>>.Success(Enumerable.Empty<PostTitle>());

                return Result<IEnumerable<PostTitle>>.Success(postTitles);
            }
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<PostTitle>>.Failure($"Error: {ex.Message}");
        }
    }

    public async Task<Result<Post>> ExecuteAsync(DetailPostQuery query)
    {
        try
        {
            await _authService.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/posts/{query.Id}"))
            {
                if (!responseMessage.IsSuccessStatusCode)
                {
                    return Result<Post>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}");
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
            await _authService.SetAuthorizationHeader();
            HttpContent httpContent = JsonContent.Create(command);
            using (HttpResponseMessage responseMessage = await _httpClient.PostAsync("api/posts", httpContent))
            {
                return responseMessage.IsSuccessStatusCode 
                    ? Result.Success() 
                    : Result.Failure($"Code de l'api : {responseMessage.StatusCode}");
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
            await _authService.SetAuthorizationHeader();
            using (HttpResponseMessage responseMessage = await _httpClient.DeleteAsync($"api/posts/{command.Id}"))
            {
                return responseMessage.IsSuccessStatusCode 
                    ? Result.Success() 
                    : Result.Failure($"Code de l'api : {(int)responseMessage.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            return Result.Failure(ex.Message, ex);
        }
    }
}