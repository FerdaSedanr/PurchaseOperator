using PurchaseOperator.Domain.Models.PortalProductModels;

namespace PurchaseOperator.Application.Services.PortalProductServices;

public interface IPortalProductService
{
    public Task<IEnumerable<Product>> GetObjectsAsync(HttpClient httpClient, string filter);
}