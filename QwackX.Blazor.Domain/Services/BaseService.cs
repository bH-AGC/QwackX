namespace QwackX.Blazor.Domain.Services
{
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly AuthService _authService;

        protected BaseService(IHttpClientFactory httpClientFactory, AuthService authService)
        {
            _httpClient = authService.HttpClient;
            _authService = authService;
        }
    }
}