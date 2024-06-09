using PurchaseOperator.Domain.Models.PurchaseOrderModel;

namespace PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;

public class DispatchItem
{
    public DispatchItem()
    {
        Lines = new();
    }

    public int ReferenceId { get; set; }

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public int UnıtsetReferenceId { get; set; }
    public string UnitsetCode { get; set; } = string.Empty;
    public int SubUnitsetReferenceId { get; set; }
    public string SubUnitsetCode { get; set; } = string.Empty;
    public double DemandQuantity { get; set; } = default;

    public double SupplyChainQuantity { get; set; } = default;

    public double? TotalQuantity { get; set; } = default;

    public double? TotalWaitingQuantity { get; set; } = default;

    public double? TotalShippedQuantity { get; set; } = default;//Shipped (Satış Olduğunda Sevkedilenn, Satınalma  olduğunda Kabul edilendir

    public double CustomQuantity { get; set; } = default;

    public double CountAmount { get; set; } = default;

    public string ManufactureCode { get; set; } = string.Empty;

    public int LineNumber { get; set; }
    public string Description { get; set; } = string.Empty;

    public List<PurchaseOrderLine> Lines { get; set; }
}