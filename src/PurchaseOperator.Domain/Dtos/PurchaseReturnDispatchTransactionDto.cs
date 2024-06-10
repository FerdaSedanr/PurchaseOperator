namespace PurchaseOperator.Domain.Dtos;
public record PurchaseReturnDispatchTransactionDto(
  DateTime TransactionDate,
 int WarehouseNumber,
 string CurrentCode,
 string Description,
 int DispatchType,
 string SpeCode,
 int DispatchStatus,
 int IsEDispatch,
 string DoCode,
 string DocTrackingNumber,
 int EDispatchProfileId,
 List<PurchaseReturnDispatchTransactionLineDto> Lines
   );


