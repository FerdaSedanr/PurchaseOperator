using PurchaseOperator.Application.Services.PurchaseReturnDispatchService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.ResponseModels;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.PurchaseReturnDispatchDataStores;

public class PurchaseReturnDispatchDataStore : IPurchaseReturnDispatchService
{
    public string postUrl = $"gateway/purchase/PurchaseReturnDispatchTransaction/Tiger";
    public async Task<ResponseDataResult<PurchaseTransactionResult>> InsertAsync(HttpClient httpClient, PurchaseReturnDispatchTransactionDto dto, int FirmNumber)
    {
        ResponseDataResult<PurchaseTransactionResult> dataResult = null;

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await httpClient.PostAsync($"{postUrl}?firmNumber={FirmNumber}", content);

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
                dataResult = JsonSerializer.Deserialize<ResponseDataResult<PurchaseTransactionResult>>(jsonData);
        }

        return await Task.FromResult(dataResult);
    }
}
