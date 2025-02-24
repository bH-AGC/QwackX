using System.Net.Http.Headers;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;
using System.Net.Http.Json;
using System.Text.Json;
using Blazored.LocalStorage;
using CommandQuerySeparation.Results;

namespace QwackX.Blazor.Domain.Services
{
    public class UserService : IUserRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;

        public UserService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("Default");
            _localStorage = localStorage;
        }

        public async Task<Result<IEnumerable<User>>> ExecuteAsync(ListUsersQuery query)
        {
            try
            {
                await SetAuthorizationHeader();
                Console.WriteLine($"Authorization: {_httpClient.DefaultRequestHeaders.Authorization}");

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
                await SetAuthorizationHeader();
                
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
                await SetAuthorizationHeader();
                
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
                await SetAuthorizationHeader();
                
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
                await SetAuthorizationHeader();
                
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
        
        private async Task<Result> SetAuthorizationHeader()
        {
            string? token = await _localStorage.GetItemAsync<string>("userToken");
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Result.Success();
            }

            return Result.Failure("Autentication Failed");
        }
    }
}
