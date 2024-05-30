using PurchaseOperator.Application.Services.ISubUnitsetServices;
using PurchaseOperator.Domain.Models.SubUnitsetModels;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace PurchaseOperator.Infrastructure.DataStores.SubUnitsetDataStore;

public class SubUnitsetDataStore : ISubUnitsetService
{
    private const string requestUri = "api/odata/SubUnitset";

    public async Task<IEnumerable<SubUnitset>> GetObjectsAsync(HttpClient httpClient, string filter)
    {
        var responseMessage = await httpClient.GetAsync($"{requestUri}{filter}");
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
            {
                JsonNode jsonNode = JsonNode.Parse(jsonData);
                return jsonNode["value"].Deserialize<IEnumerable<SubUnitset>>();
            }
            else
                return Enumerable.Empty<SubUnitset>();
        }
        else
            return Enumerable.Empty<SubUnitset>();
    }
}