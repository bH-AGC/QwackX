using CommandQuerySeparation.Results;
using QwackX.Blazor.Domain.Repositories;

namespace QwackX.Blazor.Domain.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient HttpClient;
        protected readonly IAuthRepository AuthRepository;

        protected BaseService(IAuthRepository authRepository)
        {
            if (authRepository is AuthService authService)
            {
                HttpClient = authService.HttpClient;
            }
            else
            {
                throw new ArgumentNullException(nameof(authRepository), "L'instance fournie n'est pas un AuthService.");
            }

            AuthRepository = authRepository;
        }


        protected static async Task<Result> CommandResultMessageAsync(HttpResponseMessage responseMessage)
        {
            if (!responseMessage.IsSuccessStatusCode)
            {
                string errorResponse = await responseMessage.Content.ReadAsStringAsync();
                return Result.Failure($"Code de l'api : {(int)responseMessage.StatusCode}, Réponse : {errorResponse}");
            }

            return Result.Success();
        }
    }
}