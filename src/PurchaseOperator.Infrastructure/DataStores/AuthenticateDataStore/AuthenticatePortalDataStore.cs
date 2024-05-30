using PurchaseOperator.Application.Services.AuthenticateService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PurchaseOperator.Infrastructure.DataStores.AuthenticateDataStore
{
    public class AuthenticatePortalDataStore : IAuthenticatePortalService
    {
        private const string requestUri = "/api/Authentication/Authenticate";

        public AuthenticatePortalDataStore()
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
}