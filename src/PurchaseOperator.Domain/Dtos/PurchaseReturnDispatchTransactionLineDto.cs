namespace PurchaseOperator.Domain.Dtos;
public record PurchaseReturnDispatchTransactionLineDto(
string ProductCode,
 string SubUnitsetCode,
 double Quantity,
 double UnitPrice,
 double VatRate,
 int WarehouseNumber,
 int OrderReferenceId,
 string Description,
 string SpeCode,
 double ConversionFactor,
 double OtherConversionFactor,
 List<SeriLotTransactionDto> SeriLotTransactions
 );
