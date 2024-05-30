using DevExpress.XtraPrinting.Native;
using PurchaseOperator.Domain.Models;
using PurchaseOperator.Infrastructure.DataStores.NotificationResultDataStore;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.Helpers
{
    public class NotificationResultHelper<T> where T : class
    {
        public async Task<NotificationDataResult<T>> GetNotification(HttpClient httpClient, Guid owner, double tryTime = 60)
        {
            var notificationResult = new NotificationDataResult<T>();
            notificationResult.IsSuccess = false;
            notificationResult.Message = "Bildirim alınamadı";
            try
            {
                bool isNotification = false;
                var notification = new NotificationResultDataStore<T>();

                var startTime = DateTime.UtcNow;

                while (DateTime.UtcNow - startTime < TimeSpan.FromSeconds(tryTime))
                {
                    notificationResult = await notification.GetObjectAsync(httpClient, owner);

                    if (notificationResult.IsNotification)
                    {
                        isNotification = notificationResult.IsSuccess;
                        return notificationResult;
                    }

                    await Task.Delay(3000);
                }

                return notificationResult;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}