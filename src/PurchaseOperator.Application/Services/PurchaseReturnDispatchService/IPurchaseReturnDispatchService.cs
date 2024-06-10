using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.ResponseModels;

namespace PurchaseOperator.Application.Services.PurchaseReturnDispatchService;

public interface IPurchaseReturnDispatchService
{
    public Task<ResponseDataResult<PurchaseTransactionResult>> InsertAsync(HttpClient httpClient, PurchaseReturnDispatchTransactionDto dto, int FirmNumber);
}
