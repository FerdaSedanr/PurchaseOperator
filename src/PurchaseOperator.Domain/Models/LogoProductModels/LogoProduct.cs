using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.LogoProductModels
{
    public class LogoProduct
    {
        public LogoProduct()
        {
            Quantity = 1;
        }

        public int ReferenceId { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string ManufactureCode { get; set; } = string.Empty;
        public int SubUnitsetReferenceId { get; set; }
        public string SubUnitsetCode { get; set; } = string.Empty;
        public double Quantity { get; set; }
    }
}