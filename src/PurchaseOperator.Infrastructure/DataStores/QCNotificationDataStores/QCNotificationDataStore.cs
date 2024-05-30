using PurchaseOperator.Application.Services.QCNotificationService;
using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.QCNotification;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.QCNotificationDataStores
{
    public class QCNotificationDataStore : IQCNotificationService
    {
        private const string requestUri = "api/odata/QCNotification";

        public async Task<QCNotification> InsertObjectAsync(HttpClient httpClient, QCNotificationDto dto)
        {
            QCNotification result = new QCNotification();

            string serilazeData = JsonSerializer.Serialize(dto);
            var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await httpClient.PostAsync(requestUri, content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(jsonData))
                {
                    result = JsonSerializer.Deserialize<QCNotification>(jsonData, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                }
            }

            return result;
        }
    }
}