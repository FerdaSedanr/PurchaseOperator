using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.PurchaseOrderModel
{
    public class PurchaseOrderLine
    {
        public int ReferenceId { get; set; }

        public short? TransactionType { get; set; }

        public string TransactionTypeName { get; set; } = string.Empty;

        public int ProductReferenceId { get; set; }

        public string ProductCode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;

        public int? UnitsetReferenceId { get; set; }

        public int? SubUnitsetReferenceId { get; set; }

        public string SubUnitsetCode { get; set; } = string.Empty;

        public double? Quantity { get; set; }

        public double? ShippedQuantity { get; set; }

        public double? WaitingQuantity { get; set; }

        public double DemandQuantity { get; set; } = default;

        public double? UnitPrice { get; set; }

        public int DivisionReferenceId { get; set; }

        public short? DivisionNumber { get; set; }

        public string DivisionCountry { get; set; } = string.Empty;

        public string DivisionCity { get; set; } = string.Empty;

        public string WarehouseName { get; set; } = string.Empty;

        public short? WarehouseNumber { get; set; }

        public DateTime? DueDate { get; set; }

        public double? Total { get; set; }

        public double? TotalVat { get; set; }

        public double? NetTotal { get; set; }

        public string Description { get; set; } = string.Empty;

        public int? OrderReferenceId { get; set; }

        public int? OrderTransactionType { get; set; }

        public string OrderTransactionTypeName { get; set; } = string.Empty;

        public string OrderCode { get; set; } = string.Empty;

        public int? CurrentReferenceId { get; set; }

        public string CurrentCode { get; set; } = string.Empty;

        public string CurrentName { get; set; } = string.Empty;
        public double? VatRate { get; set; } = default;

        public DateTime? Date { get; set; }

        public string ManufactureCode { get; set; } = string.Empty;

        // Yeni eklenen alanlar
        public string Code { get; set; } = string.Empty;  // Fatura/işlem kodu

        public string UnitsetCode { get; set; } = string.Empty;
    }
}