using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Blazored.LocalStorage;
using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services
{
    public class AuthService : IAuthRepository
    {
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient _httpClient;

        private const string UserIdKey = "userId";
        private const string UsernameKey = "username";

        public AuthService(ILocalStorageService localStorage, IHttpClientFactory httpClientFactory)
        {
            _localStorage = localStorage;
            _httpClient = httpClientFactory.CreateClient("Default");
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
        }

        public async Task<(int? UserId, string? Username)> GetUser()
        {
            var userId = await _localStorage.GetItemAsync<int?>(UserIdKey);
            var username = await _localStorage.GetItemAsync<string>(UsernameKey);
            return (userId, username);
        }
        
        public async Task<Result<User>> ExecuteAsync(LoginUserQuery query)
        {
            try
            {
                string jsonPayload = JsonSerializer.Serialize(query);
                Console.WriteLine($"JSON envoyé: {jsonPayload}"); // Ajoute ce log pour débugger

                HttpContent httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (HttpResponseMessage responseMessage = await _httpClient.PostAsync("api/auth/login", httpContent))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                        return Result<User>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                    }

                    string json = await responseMessage.Content.ReadAsStringAsync();
                    User _user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

                    // Récupérer le token depuis la réponse JSON et le stocker dans localStorage
                    var token = json.Substring(json.LastIndexOf("token") + 7); // Extraire le token
                    await _localStorage.SetItemAsync("authToken", token);  // Stockage du token dans localStorage

                    return Result<User>.Success(_user);
                }
            }
            catch (Exception ex)
            {
                return Result<User>.Failure(ex.Message, ex);
            }
        }


        public async Task<Result<User>> ValidateUserCredentialsAsync(string email, string password)
        {
            var saltResponse = await _httpClient.GetFromJsonAsync<SaltResponse>($"api/auth/salt/{email}");

            if (saltResponse is null || string.IsNullOrEmpty(saltResponse.Salt))
                return Result<User>.Failure("Impossible de récupérer le sel.");

            string salt = saltResponse.Salt;
            
            string saltedPasswordHash = BCrypt.Net.BCrypt.HashPassword(password, salt);
            
            var loginQuery = new LoginUserQuery(email, saltedPasswordHash);
            
            var result = await ExecuteAsync(loginQuery);

            if (result.IsSuccess)
            {
                return Result<User>.Success(result.Content);
            }
            else
            {
                return Result<User>.Failure("Nom d'utilisateur ou mot de passe incorrect");
            }
        }

        private class SaltResponse
        {
            public string Salt { get; set; } = default!;
        }
    }
}