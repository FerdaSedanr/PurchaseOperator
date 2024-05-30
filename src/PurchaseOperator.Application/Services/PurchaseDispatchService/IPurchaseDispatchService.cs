using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.ResponseModels;

namespace PurchaseOperator.Application.Services.PurchaseDispatchService
{
    public interface IPurchaseDispatchService
    {
        public Task<ResponseDataResult<ProductTransactionResult>> InsertAsync(HttpClient httpClient, PurchaseDispatchTransactionDto dto, int FirmNumber);
    }
}