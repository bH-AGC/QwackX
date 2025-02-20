using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;
using System.Net.Http.Json;
using System.Text.Json;
using CommandQuerySeparation;
using CommandQuerySeparation.Results;

namespace QwackX.Blazor.Domain.Services
{
    public class UserService : IUserRepository
    {
        private readonly HttpClient _httpClient;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Default");
        }

        public async Task<Result<IEnumerable<User>>> ExecuteAsync(ListUsersQuery query)
        {
            try
            {
                using (HttpResponseMessage responseMessage = await _httpClient.GetAsync("api/user"))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return Result<IEnumerable<User>>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}");
                    }

                    string json = await responseMessage.Content.ReadAsStringAsync();
                    User[]? users = JsonSerializer.Deserialize<User[]>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    return users is null 
                        ? Result<IEnumerable<User>>.Success(Enumerable.Empty<User>()) 
                        : Result<IEnumerable<User>>.Success(users);
                }
            }
            catch (Exception ex)
            {
                return Result<IEnumerable<User>>.Failure(ex.Message, ex);
            }
        }
        
        public async Task<Result<User>> ExecuteAsync(DetailUserQuery query)
        {
            try
            {
                using (HttpResponseMessage responseMessage = await _httpClient.GetAsync($"api/user/{query.Id}"))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        return Result<User>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}");
                    }

                    string json = await responseMessage.Content.ReadAsStringAsync();

                    User _user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

                    return Result<User>.Success(_user);
                }
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(ex.Message, ex);
            }
        }

        public async Task<Result> ExecuteAsync(AddUserCommand command)
        {
            try
            {
                HttpContent httpContent = JsonContent.Create(command);
                using (HttpResponseMessage responseMessage = await _httpClient.PostAsync("api/user", httpContent))
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
        
        public async Task<Result> ExecuteAsync(EditUserCommand command)
        {
            try
            {
                HttpContent httpContent = JsonContent.Create(command);

                using (HttpResponseMessage responseMessage = await _httpClient.PutAsync($"api/user", httpContent))
                {
                    if (responseMessage.IsSuccessStatusCode)
                    {
                        return Result.Success();
                    }
                    return Result.Failure($"Code de l'api : {responseMessage.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, ex);
            }
        }

        public async Task<Result> ExecuteAsync(DeleteUserCommand command)
        {
            try
            {
                using (HttpResponseMessage responseMessage = await _httpClient.DeleteAsync($"api/user/{command.Id}"))
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
}
