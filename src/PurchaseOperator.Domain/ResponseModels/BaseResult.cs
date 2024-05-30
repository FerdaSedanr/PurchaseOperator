using System.Text.Json.Serialization;

namespace PurchaseOperator.Domain.ResponseModels;

public class BaseResult
{
    [JsonPropertyName("referenceId")]
    public int ReferenceId { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;
}