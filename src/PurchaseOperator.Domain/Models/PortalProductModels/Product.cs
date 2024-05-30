namespace PurchaseOperator.Domain.Models.PortalProductModels;

public class Product
{
    public int ReferenceId { get; set; }
    public Guid Oid { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public byte[]? MainImage { get; set; }
}