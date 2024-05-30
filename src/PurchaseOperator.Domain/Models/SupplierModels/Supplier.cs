using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.SupplierModels
{
    public class Supplier
    {
        public int ReferenceId { get; set; }

        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string OtherTelephone { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string WebAddress { get; set; } = string.Empty;

        public string TaxOffice { get; set; } = string.Empty;

        public short CardType { get; set; }

        public string County { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        public int ReferenceCount { get; set; }

        public double? NetTotal { get; set; }

        public DateTime? LastTransactionDate { get; set; }

        public TimeSpan? lastTransactionTime { get; set; }

        public short? DispatchType { get; set; } = default;

        public double OrderQuantity { get; set; }
    }
}