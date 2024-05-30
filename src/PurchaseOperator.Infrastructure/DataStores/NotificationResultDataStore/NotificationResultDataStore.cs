using PurchaseOperator.Domain;
using PurchaseOperator.Domain.Models;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.NotificationResultDataStore
{
    public class NotificationResultDataStore<T> where T : class
    {
        public async Task<NotificationDataResult<T>> GetObjectAsync(HttpClient httpClient, Guid owner)
        {
            HttpResponseMessage responseMessage = await httpClient.GetAsync($"gateway/notification/NotificationResult/owner/{owner}");
            var result = new NotificationDataResult<T>();

            if (responseMessage.IsSuccessStatusCode)
            {
                var json = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(json))
                {
                    var notificationResult = JsonSerializer.Deserialize<NotificationResult>(json, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });

                    if (notificationResult != null && notificationResult.Content != null)
                    {
                        var dataResult = JsonSerializer.Deserialize<NotificationDataResult<T>>(notificationResult.Content);

                        if (dataResult != null)
                        {
                            result.IsSuccess = dataResult.IsSuccess;
                            result.Message = dataResult.Message;
                            result.IsNotification = true;
                            result.Data = dataResult.Data;
                            return result;
                        }
                        else
                        {
                            result.IsSuccess = false;
                            result.Message = "Data result is null.";
                            return result;
                        }
                    }
                    else
                    {
                        result.IsSuccess = false;
                        result.Message = "Notification result or content is null.";
                        return result;
                    }
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "JSON content is null or empty.";
                    return result;
                }
            }
            else
            {
                result.IsSuccess = false;
                result.Message = $"Notification Service Status Code: {responseMessage.StatusCode}";
                return result;
            }
        }
    }
}