using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Dtos;

public class PurchaseDispatchTransactionLineDto
{
    public PurchaseDispatchTransactionLineDto()
    {
        SeriLotTransactions = new List<SeriLotTransactionDto>();
    }

    public int? OrderReferenceId { get; set; }
    public short? TransactionType { get; set; } = default;
    public int? ProductReferenceId { get; set; }
    public string? ProductCode { get; set; } = string.Empty;
    public int? UnitsetReferenceId { get; set; }
    public string? UnitsetCode { get; set; } = string.Empty;
    public double? Quantity { get; set; }
    public double? UnitPrice { get; set; } = default;
    public int? SubUnitsetReferenceId { get; set; }
    public double? VatRate { get; set; } = default;
    public string? SubUnitsetCode { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.Now;
    public int? IOType { get; set; }
    public int? CurrentReferenceId { get; set; } = default;
    public string? CurrentCode { get; set; } = default;
    public int? WarehouseNumber { get; set; }
    public string? Description { get; set; } = string.Empty;
    public string? SpeCode { get; set; } = string.Empty;
    public double? ConversionFactor { get; set; }
    public double? OtherConversionFactor { get; set; }

    public IList<SeriLotTransactionDto> SeriLotTransactions { get; set; }
}