using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;

namespace PurchaseOperator.Application.Services.PurchaseOrderService;

public interface IPurchaseOrderService
{
    public Task<DataResult<PurchaseOrderLine>> GetObjectsAsync(HttpClient httpClient, int SupplierReferenceId, int FirmNumber, int PeriodNumber);
}