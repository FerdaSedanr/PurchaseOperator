namespace PurchaseOperator.Domain.Models.SubUnitsetModels;

public class SubUnitset
{
    public Guid Oid { get; set; }
    public int ReferenceId { get; set; }
    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
}