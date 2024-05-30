using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain
{
    public class NotificationResult
    {
        public string Id { get; set; } = string.Empty;
        public Guid? Owner { get; set; }
        public string? Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}