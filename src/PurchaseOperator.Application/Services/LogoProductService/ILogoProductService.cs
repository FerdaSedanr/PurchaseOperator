using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.LogoProductModels;

namespace PurchaseOperator.Application.Services.LogoProductService;

public interface ILogoProductService
{
    public Task<DataResult<LogoProduct>> GetCustomObjectsAsync(HttpClient httpClient, int SupplierReferenceId, int FirmNumber);
}