namespace PurchaseOperator.Application.Services.AuthenticateService;

public interface IAuthenticatePortalService
{
    public Task<string> AuthenticateAsync(HttpClient httpClient, string username, string password);
}