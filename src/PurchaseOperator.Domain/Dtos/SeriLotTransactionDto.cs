namespace PurchaseOperator.Domain.Dtos
{
    public record SeriLotTransactionDto(string StockLocationCode,
 string DestinationStockLocationCode,
 int InProductTransactionLineReferenceId,
 int OutProductTransactionLineReferenceId,
 int SerilotType,
 double Quantity,
 string SubUnitsetCode,
 double ConversionFactor,
 double OtherConversionFactor
   )
    {

    }
}