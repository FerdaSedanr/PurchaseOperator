using PurchaseOperator.Domain.Models.SubUnitsetModels;

namespace PurchaseOperator.Application.Services.ISubUnitsetServices;

public interface ISubUnitsetService
{
    public Task<IEnumerable<SubUnitset>> GetObjectsAsync(HttpClient httpClient, string filter);
}