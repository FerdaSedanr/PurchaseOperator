using PurchaseOperator.Application.Services.EmployeeService;
using PurchaseOperator.Domain.Models.EmployeeModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace PurchaseOperator.Infrastructure.DataStores.EmployeeDataStore
{
    public class EmployeeDataStore : IEmployeeService
    {
        private const string requestUri = "api/odata/Employee";

        public async Task<IEnumerable<Employee>> GetObjectsAsync(HttpClient httpClient)
        {
            var jsonData = await httpClient.GetStringAsync(requestUri);
            JsonNode jsonNode = JsonNode.Parse(jsonData);
            return jsonNode["value"].Deserialize<IEnumerable<Employee>>();
        }

        public async Task<IEnumerable<Employee>> GetObjectsAsync(HttpClient httpClient, string filter)
        {
            var jsonData = await httpClient.GetStringAsync($"{requestUri}/{filter}");
            JsonNode jsonNode = JsonNode.Parse(jsonData);
            return jsonNode["value"].Deserialize<IEnumerable<Employee>>();
        }
    }
}