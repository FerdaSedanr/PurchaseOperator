using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.PurchaseOrderModel;

public class PurchaseOrder
{
    public int ReferenceId { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan? Time { get; set; }
    public int? TransactionType { get; set; }
    public string? TransactionTypeName { get; set; }
    public short? OrderType { get; set; }

    public string? Code { get; set; }

    public int? WarehouseReferenceId { get; set; }

    public string? WarehouseName { get; set; }

    public short? WarehouseNumber { get; set; }

    public int? DivisionReferenceId { get; set; }

    public short? DivisionNumber { get; set; }

    public string? DivisionCountry { get; set; }

    public string? DivisionCity { get; set; }

    public int? CurrentReferenceId { get; set; }

    public string? CurrentCode { get; set; }

    public string? CurrentName { get; set; }

    public double? Total { get; set; }

    public double? TotalVat { get; set; }

    public double? NetTotal { get; set; }

    public string? Description { get; set; }

    public short? Status { get; set; }
}