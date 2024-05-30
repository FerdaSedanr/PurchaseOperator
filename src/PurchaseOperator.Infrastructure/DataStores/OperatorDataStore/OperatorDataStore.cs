using PurchaseOperator.Application.Services.OperatorService;
using PurchaseOperator.Domain.Models.OperatorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PurchaseOperator.Infrastructure.DataStores.OperatorDataStore;

public class OperatorDataStore : IOperatorService
{
    private const string requestUri = "api/odata/Operator";

    public async Task<Operator> GetObjectAsync(HttpClient httpClient)
    {
        try
        {
            var data = await httpClient.GetStringAsync(requestUri);
            JsonNode jsonNode = JsonNode.Parse(data);
            return jsonNode["value"].Deserialize<Operator>();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<Operator> GetObjectByCodeAsync(HttpClient httpClient, string filter)
    {
        var data = await httpClient.GetStringAsync($"{requestUri}/{filter}");
        JsonNode jsonNode = JsonNode.Parse(data);
        return jsonNode["value"].Deserialize<Operator>();
    }

    public async Task<IEnumerable<Operator>> GetObjectsAsync(HttpClient httpClient)
    {
        var data = await httpClient.GetStringAsync(requestUri);
        JsonNode jsonNode = JsonNode.Parse(data);
        return jsonNode["value"].Deserialize<IEnumerable<Operator>>();
    }

    public async Task<IEnumerable<Operator>> GetObjectsAsync(HttpClient httpClient, string filter)
    {
        IEnumerable<Operator> result = new List<Operator>();

        var responseMessage = await httpClient.GetAsync($"{requestUri}{filter}");
        if (responseMessage.IsSuccessStatusCode)
        {
            if (responseMessage.StatusCode == System.Net.HttpStatusCode.OK)
            {
                JsonNode jsonNode = JsonNode.Parse(await responseMessage.Content.ReadAsStringAsync());
                result = jsonNode["value"].Deserialize<IEnumerable<Operator>>();
            }
        }

        return result;
    }
}