using QwackX.Blazor.Domain.Commands;
using QwackX.Blazor.Domain.Entities;
using QwackX.Blazor.Domain.Queries;
using QwackX.Blazor.Domain.Repositories;
using System.Net.Http.Json;
using System.Text.Json;
using CommandQuerySeparation.Results;

namespace QwackX.Blazor.Domain.Services
{
    public class UserService : BaseService, IUserRepository
    {
        public UserService(IAuthRepository authRepository) : base(authRepository) { }
        public async Task<Result<IEnumerable<User>>> ExecuteAsync(ListUsersQuery query)
        {
            try
            {
                await AuthRepository.SetAuthorizationHeader();

                using (HttpResponseMessage responseMessage = await HttpClient.GetAsync("api/users"))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                        return Result<IEnumerable<User>>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
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
                await AuthRepository.SetAuthorizationHeader();
                using (HttpResponseMessage responseMessage = await HttpClient.GetAsync($"api/users/{query.Id}"))
                {
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                        return Result<User>.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
                    }

                    string json = await responseMessage.Content.ReadAsStringAsync();
                    User user = JsonSerializer.Deserialize<User>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true })!;

                    return Result<User>.Success(user);
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
                await AuthRepository.SetAuthorizationHeader();
                HttpContent httpContent = JsonContent.Create(command);
                using (HttpResponseMessage responseMessage = await HttpClient.PostAsync("api/users", httpContent))
                {
                    return await CommandResultMessageAsync(responseMessage);
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
                await AuthRepository.SetAuthorizationHeader();
                HttpContent httpContent = JsonContent.Create(command);
                using (HttpResponseMessage responseMessage = await HttpClient.PutAsync($"api/users", httpContent))
                {
                    return await CommandResultMessageAsync(responseMessage);
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
                await AuthRepository.SetAuthorizationHeader();
                using (HttpResponseMessage responseMessage = await HttpClient.DeleteAsync($"api/users/{command.UserId}"))
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
}
