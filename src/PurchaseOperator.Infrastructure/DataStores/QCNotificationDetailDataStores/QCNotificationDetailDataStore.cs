using PurchaseOperator.Application.Services.QCNotificationDetailService;
using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.QCNotificationDetail;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.QCNotificationDetailDataStores;

public class QCNotificationDetailDataStore : IQCNotificationDetailService
{
    private const string requestUri = "api/odata/QCNotificationDetail";

    public async Task<QCNotificationDetail> InsertObjectAsync(HttpClient httpClient, QCNotificationDetailDto dto)
    {
        QCNotificationDetail result = new QCNotificationDetail();

        string serilazeData = JsonSerializer.Serialize(dto);
        var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUri, content);
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
            {
                result = JsonSerializer.Deserialize<QCNotificationDetail>(jsonData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            }
        }

        return result;
    }
}