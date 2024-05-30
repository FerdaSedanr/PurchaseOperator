using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Domain.Models.CustomerModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PurchaseOperator.Infrastructure.DataStores.CustomerDataStores
{
    public class CustomerDataStore : ICustomerService
    {
        private const string requestUri = "api/odata/Customer";

        public async Task<IEnumerable<Customer>> GetObjectsAsync(HttpClient httpClient, string filter)
        {
            var responseMessage = await httpClient.GetAsync($"{requestUri}{filter}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonData))
                {
                    JsonNode jsonNode = JsonNode.Parse(jsonData);
                    return jsonNode["value"].Deserialize<IEnumerable<Customer>>();
                }
                else
                    return Enumerable.Empty<Customer>();
            }
            else
                return Enumerable.Empty<Customer>();
        }
    }
}