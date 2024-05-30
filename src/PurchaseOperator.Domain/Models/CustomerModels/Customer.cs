using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.CustomerModels
{
    public class Customer
    {
        public int ReferenceId { get; set; }

        public Guid Oid { get; set; }
    }
}