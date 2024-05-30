using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PurchaseOperator.Domain.Models.QCNotification;

public class QCNotification
{
    [JsonPropertyName("@odata.context")]
    public string odatacontext { get; set; }

    [JsonPropertyName("Oid")]
    public string Oid { get; set; }

    [JsonPropertyName("DispatchOn")]
    public DateTime DispatchOn { get; set; }

    [JsonPropertyName("Status")]
    public string Status { get; set; }

    [JsonPropertyName("Code")]
    public string Code { get; set; }

    [JsonPropertyName("DispatchNumber")]
    public string DispatchNumber { get; set; }

    [JsonPropertyName("DispatchReferenceId")]
    public int DispatchReferenceId { get; set; }

    [JsonPropertyName("CompletedDate")]
    public object CompletedDate { get; set; }
}