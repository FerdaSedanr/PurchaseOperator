using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Domain.Models.PortalProductModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PurchaseOperator.Infrastructure.DataStores.PortalProductDataStore;

public class PortalProductDataStore : IPortalProductService
{
    private const string requestUri = "api/odata/Product";

    public async Task<IEnumerable<Product>> GetObjectsAsync(HttpClient httpClient, string filter)
    {
        var responseMessage = await httpClient.GetAsync($"{requestUri}{filter}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
            {
                JsonNode jsonNode = JsonNode.Parse(jsonData);
                return jsonNode["value"].Deserialize<IEnumerable<Product>>();
            }
            else
                return Enumerable.Empty<Product>();
        }
        else
            return Enumerable.Empty<Product>();
    }
}