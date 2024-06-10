using PurchaseOperator.Domain.Models.QCNotification;

namespace PurchaseOperator.Domain.Dtos.QCNotificationDtos;

public record QCNotificationDto(DateTime DispatchOn, string DispatchNumber, int DispatchReferenceId, Guid Customer,Guid OutWarehouse);
public record QCNotificationDetailDto(Guid QCNotification,Guid Product, Guid SubUnitset, double Quantity);