using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.QCNotification;
using PurchaseOperator.Domain.Models.QCNotificationDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Application.Services.QCNotificationService
{
    public interface IQCNotificationService
    {
        public Task<QCNotification> InsertObjectAsync(HttpClient httpClient, QCNotificationDto dto);
    }
}