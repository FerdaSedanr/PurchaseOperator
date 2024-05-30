using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.SupplierModels;

namespace PurchaseOperator.Application.Services.SupplierService;

public interface ISupplierService
{
    public Task<DataResult<Supplier>> GetObjectsAsync(HttpClient httpClient, int FirmNumber, int PeriodNumber);
}