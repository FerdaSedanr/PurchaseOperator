using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseDispatchModels;

namespace PurchaseOperator.Application.Services.PurchaseDispatchService;

public interface IPurchaseDispatchLineService
{
    public Task<DataResult<PurchaseDispatchLineModel>> GetObjectsAsync(HttpClient httpClient, int FirmNumber, int PeriodNumber,DateTime dateTime);

}
