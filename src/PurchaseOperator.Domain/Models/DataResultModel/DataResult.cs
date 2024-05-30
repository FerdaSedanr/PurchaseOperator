using System.Text.Json.Serialization;

namespace PurchaseOperator.Domain.Models.DataResultModel;

public class DataResult<T> where T : class
{
    [JsonPropertyName("data")]
    public IEnumerable<T> Data { get; set; }

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; } = false;

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}