using PurchaseOperator.Domain.Models.OperatorModels;

namespace PurchaseOperator.Application.Services.OperatorService;

public interface IOperatorService
{
    public Task<Operator> GetObjectAsync(HttpClient httpClient);

    public Task<IEnumerable<Operator>> GetObjectsAsync(HttpClient httpClient);

    public Task<Operator> GetObjectByCodeAsync(HttpClient httpClient, string filter);

    public Task<IEnumerable<Operator>> GetObjectsAsync(HttpClient httpClient, string filter);
}