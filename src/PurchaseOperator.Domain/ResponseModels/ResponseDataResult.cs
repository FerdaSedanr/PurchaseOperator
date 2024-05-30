using System.Text.Json.Serialization;

namespace PurchaseOperator.Domain.ResponseModels;

public class ResponseDataResult<T> where T : class
{
    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("isSuccess")]
    public bool IsSuccess { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
}