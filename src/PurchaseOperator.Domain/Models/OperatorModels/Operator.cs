using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.OperatorModels
{
    public class Operator
    {
        public Guid Oid { get; set; }
        public string Code { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public byte[] Image { get; set; }
        public OperatorGroup? OperatorGroup { get; set; }
    }
}