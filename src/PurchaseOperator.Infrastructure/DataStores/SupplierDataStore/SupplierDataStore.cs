using PurchaseOperator.Application.Services.SupplierService;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.SupplierDataStore;

public class SupplierDataStore : ISupplierService
{
    private const string requestUri = "gateway/purchase/Supplier";

    public async Task<DataResult<Supplier>> GetObjectsAsync(HttpClient httpClient, int FirmNumber, int PeriodNumber)
    {
        DataResult<Supplier> result = new DataResult<Supplier>();

        try
        {
            var responseMessage = await httpClient.GetAsync($"{requestUri}?firmNumber={FirmNumber}?pageSize=10000");

            if (responseMessage.IsSuccessStatusCode)
            {
                var json = await responseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(json))
                {
                    result = JsonSerializer.Deserialize<DataResult<Supplier>>(json);
                }
                else
                    result.Data = Enumerable.Empty<Supplier>();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

        return result;
    }
}