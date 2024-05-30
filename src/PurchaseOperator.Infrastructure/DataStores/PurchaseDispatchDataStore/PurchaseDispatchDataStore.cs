using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransaction;
using PurchaseOperator.Domain.ResponseModels;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.PurchaseDispatchDataStore;

public class PurchaseDispatchDataStore : IPurchaseDispatchService
{
    public string postUrl = $"gateway/purchase/" + nameof(PurchaseDispatchTransaction) + "/Tiger";

    public async Task<ResponseDataResult<ProductTransactionResult>> InsertAsync(HttpClient httpClient, PurchaseDispatchTransactionDto dto, int FirmNumber)
    {
        ResponseDataResult<ProductTransactionResult> dataResult = null;

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await httpClient.PostAsync($"{postUrl}?firmNumber={FirmNumber}", content);

        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
                dataResult = JsonSerializer.Deserialize<ResponseDataResult<ProductTransactionResult>>(jsonData);
        }

        return await Task.FromResult(dataResult);
    }
}