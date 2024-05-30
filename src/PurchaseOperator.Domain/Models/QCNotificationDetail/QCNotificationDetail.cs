using PurchaseOperator.Domain.Models.PortalProductModels;

namespace PurchaseOperator.Domain.Models.QCNotificationDetail;

public class QCNotificationDetail
{
    public Guid Oid { get; set; }
    public QCNotification.QCNotification? QCNotification { get; set; }
    public Product? Product { get; set; }
    public double Quantity { get; set; }
}