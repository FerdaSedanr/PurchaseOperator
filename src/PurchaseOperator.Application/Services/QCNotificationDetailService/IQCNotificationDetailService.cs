using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.QCNotificationDetail;

namespace PurchaseOperator.Application.Services.QCNotificationDetailService;

public interface IQCNotificationDetailService
{
    public Task<QCNotificationDetail> InsertObjectAsync(HttpClient httpClient, QCNotificationDetailDto dto);
}