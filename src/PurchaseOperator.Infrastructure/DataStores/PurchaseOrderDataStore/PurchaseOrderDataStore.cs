using Microsoft.IdentityModel.Tokens;
using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.PurchaseOrderDataStore;

public class PurchaseOrderDataStore : IPurchaseOrderService
{
    private const string requestUri = "gateway/purchase/PurchaseOrderLine";

    public async Task<DataResult<PurchaseOrderLine>> GetObjectsAsync(HttpClient httpClient, int SupplierReferenceId)
    {
        DataResult<PurchaseOrderLine> result = new DataResult<PurchaseOrderLine>();

        var responseMessage = await httpClient.GetAsync($"{requestUri}/Current/Id/{SupplierReferenceId}?includeWaiting=true&orderBy=datedesc&page=0&pageSize=20000");
        try
        {
            if (responseMessage.IsSuccessStatusCode)
            {
                var json = await responseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    result = JsonSerializer.Deserialize<DataResult<PurchaseOrderLine>>(json, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
                else
                    result.Data = Enumerable.Empty<PurchaseOrderLine>();
            }
        }
        catch (Exception)
        {
            throw;
        }
        return result;
    }

    public Task<DataResult<PurchaseOrderLine>> GetObjectsAsync(HttpClient httpClient, int SupplierReferenceId, int FirmNumber, int PeriodNumber)
    {
        throw new NotImplementedException();
    }
}