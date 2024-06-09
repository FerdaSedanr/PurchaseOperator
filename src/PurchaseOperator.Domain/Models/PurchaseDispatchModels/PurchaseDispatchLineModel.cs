namespace PurchaseOperator.Domain.Models.PurchaseDispatchModels;

public class PurchaseDispatchLineModel
{
    public int ReferenceId { get; set; }
    public int DispatchReferenceId { get; set; }
    public string FicheNumber { get; set; } = string.Empty;
    public DateTime DispatchDate { get; set; }
    public int SupplierReferenceId { get; set; }
    public string SupplierCode { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public int ProductReferenceId { get; set; }
    public string ProductCode { get; set; } = string.Empty;
    public string ProductName { get; set; } = string.Empty;
    public string ProductSupplierCode { get; set; } = string.Empty;
    public int UnitsetReferenceId { get; set; }
    public string UnitsetCode { get; set; } = string.Empty;
    public string UnitsetName { get; set; } = string.Empty;
    public int SubUnitsetReferenceId { get; set; }
    public string SubUnitsetCode { get; set; } = string.Empty;
    public string SubUnitsetName { get; set; } = string.Empty;
    public double Quantity { get; set; }
    public int OrderReferenceId { get; set; }
    public string OrderNumber { get; set; } = string.Empty;

}
