using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services
{
    public class AuthService : IAuthRepository
    {
        private readonly ILocalStorageService _localStorage;
        public HttpClient HttpClient { get; }
        private const string UserIdKey = "userId";
        private const string UsernameKey = "username";
        private const string UserTokenKey = "userToken";

        public AuthService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            this._localStorage = localStorage;
            HttpClient = httpClientFactory.CreateClient("Default");
        }
        
        public async Task<bool> IsAuthenticatedAsync()
        {
            string? token = await _localStorage.GetItemAsync<string>(UserTokenKey);
            return !string.IsNullOrEmpty(token);
        }

        public async Task<string?> GetUsernameAsync()
        {
            return await _localStorage.GetItemAsync<string>(UsernameKey);
        }

        public async Task SignIn(int userId, string username)
        {
            await _localStorage.SetItemAsync(UserIdKey, userId);
            await _localStorage.SetItemAsync(UsernameKey, username);
        }

        public async Task SignOut()
        {
            await _localStorage.RemoveItemAsync(UserIdKey);
            await _localStorage.RemoveItemAsync(UsernameKey);
            await _localStorage.RemoveItemAsync(UserTokenKey);
        }

        public async Task<(int? UserId, string? Username)> GetUser()
        {
            int? userId = await _localStorage.GetItemAsync<int?>(UserIdKey);
            string? username = await _localStorage.GetItemAsync<string>(UsernameKey);
            return (userId, username);
        }

        public async Task<Result> SetAuthorizationHeader()
        {
            string? token = await _localStorage.GetItemAsync<string>(UserTokenKey);
            if (!string.IsNullOrEmpty(token))
            {
                HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return Result.Success();
            }
            
            return Result.Failure($"Authentication Failed: Token is missing or invalid.");
        }

        public async Task<Result<User>> ExecuteAsync(LoginUserQuery query)
        {
            try
            {
                string jsonPayload = JsonSerializer.Serialize(query);
                HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/auth/login", httpContent))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                        return Result<User>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                    }

                    string json = await responseMessage.Content.ReadAsStringAsync();
                    User? user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;
                    string? token = JsonSerializer.Deserialize<JsonElement>(json).GetProperty("token").GetString()?.Trim('"');
                    
                    await _localStorage.SetItemAsync(UserTokenKey, token);
                    
                    return Result<User>.Success(user);
                }
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(ex.Message, ex);
            }
        }

        public async Task<Result> ExecuteAsync(RegisterUserCommand command)
        {
            try
            {
                HttpContent httpContent = JsonContent.Create(command);
                using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/auth/register", httpContent))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                        return Result.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                    }

                    return Result.Success();
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(ex.Message, ex);
            }
        }
    }
}
