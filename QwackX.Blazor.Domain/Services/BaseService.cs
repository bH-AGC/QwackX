using CommandQuerySeparation.Results;

namespace QwackX.Blazor.Domain.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient HttpClient;
        protected readonly AuthService AuthService;

        protected BaseService(AuthService authService)
        {
            HttpClient = authService.HttpClient;
            AuthService = authService;
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