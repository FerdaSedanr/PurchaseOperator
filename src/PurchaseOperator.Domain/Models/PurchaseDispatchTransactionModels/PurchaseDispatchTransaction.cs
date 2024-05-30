using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.PurchaseDispatchTransaction;

public class PurchaseDispatchTransaction
{
    public int ReferenceId { get; set; }

    public DateTime? TransactionDate { get; set; }

    public TimeSpan? TransactionTime { get; set; }

    public int? ConvertedTime { get; set; }

    public int? OrderReference { get; set; }

    public string? Code { get; set; }

    public short? GroupType { get; set; }

    public short? IOType { get; set; }

    public short? TransactionType { get; set; }

    public string? TransactionTypeName { get; set; }

    public int DivisionReferenceId { get; set; }

    public short? DivisionNumber { get; set; }

    public string? DivisionCountry { get; set; }

    public string? DivisionCity { get; set; }

    public int WarehouseReferenceId { get; set; }

    public string? WarehouseName { get; set; }
}