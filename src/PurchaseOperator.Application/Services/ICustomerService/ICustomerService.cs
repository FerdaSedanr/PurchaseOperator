using PurchaseOperator.Domain.Models.CustomerModels;
using PurchaseOperator.Domain.Models.DataResultModel;

namespace PurchaseOperator.Application.Services.ICustomerService;

public interface ICustomerService
{
    public Task<IEnumerable<Customer>> GetObjectsAsync(HttpClient httpClient, string filter);
}