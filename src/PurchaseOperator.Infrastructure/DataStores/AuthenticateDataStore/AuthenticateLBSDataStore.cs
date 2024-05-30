using PurchaseOperator.Application.Services.AuthenticateService;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.AuthenticateDataStore;

public class AuthenticateLBSDataStore : IAuthenticateLBSService
{
    private const string requestUri = "/gateway/identity/Authentication/Authenticate";

    public AuthenticateLBSDataStore()
    {
    }

    public async Task<string> AuthenticateAsync(HttpClient httpClient, string username, string password)
    {
        string token = string.Empty;
        string UserName = username;
        string Password = password;
        var serializeData = JsonSerializer.Serialize(new { UserName, Password });
        StringContent content = new StringContent(serializeData, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUri, content);
        if (responseMessage.IsSuccessStatusCode)
        {
            var data = await responseMessage.Content.ReadAsStringAsync();
            token = data;
        }

        return token;
    }
}