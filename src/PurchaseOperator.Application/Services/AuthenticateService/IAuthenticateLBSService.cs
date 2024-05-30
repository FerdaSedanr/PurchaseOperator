namespace PurchaseOperator.Application.Services.AuthenticateService;

public interface IAuthenticateLBSService
{
    public Task<string> AuthenticateAsync(HttpClient httpClient, string username, string password);
}