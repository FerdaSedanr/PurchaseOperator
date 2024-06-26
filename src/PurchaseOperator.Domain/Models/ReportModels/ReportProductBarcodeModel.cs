using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;

namespace PurchaseOperator.Domain.Models.ReportModels;

public class ReportProductBarcodeModel : DispatchItem
{
    public string DispatchNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; } = default;
}
