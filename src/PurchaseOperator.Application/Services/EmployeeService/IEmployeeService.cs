using PurchaseOperator.Domain.Models.EmployeeModels;

namespace PurchaseOperator.Application.Services.EmployeeService;

public interface IEmployeeService
{
    public Task<IEnumerable<Employee>> GetObjectsAsync(HttpClient httpClient);

    public Task<IEnumerable<Employee>> GetObjectsAsync(HttpClient httpClient, string filter);
}