using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.PurchaseOrderDataStore;

public class PurchaseOrderDataStoreV2 : IPurchaseOrderService
{
    private const string requestUri = "gateway/customQuery/CustomQuery";

    public async Task<DataResult<PurchaseOrderLine>> GetObjectsAsync(HttpClient httpClient, int SupplierReferenceId, int FirmNumber, int PeriodNumber)
    {
        DataResult<PurchaseOrderLine> result = new();

        try
        {
            string query = $@"SELECT
			[ReferenceId] = ORFLINE.LOGICALREF,
			[Date] = ORFICHE.DATE_,
			[DueDate] = ORFLINE.DUEDATE,
			[Code] = ORFICHE.FICHENO,
			[TransactionType] = ORFICHE.TRCODE,
			[Description] = ORFLINE.LINEEXP,
			[UnitPrice] = ORFLINE.PRICE,
			[VatRate] = ORFLINE.VAT,
			[ProductReferenceId] = ORFLINE.STOCKREF,
			[ProductCode] = ISNULL(ITEMS.CODE,''),
			[ProductName] = ISNULL(ITEMS.NAME,''),
			[WarehouseNumber] = ISNULL(WHOUSE.NR,NULL),
			[WarehouseName] = WHOUSE.NAME,
			[CurrentReferenceId] = CLCARD.LOGICALREF,
			[CurrentCode] = CLCARD.CODE,
			[CurrentName] = CLCARD.DEFINITION_,
			[SubUnitsetCode] = SUBUNITSET.CODE,
			[SubUnitsetReferenceId] = SUBUNITSET.LOGICALREF,
			[UnitsetCode] = UNITSET.CODE,
			[UnitsetReferenceId] = UNITSET.LOGICALREF,
			[Quantity] = ORFLINE.AMOUNT,
			[ShippedQuantity] = ORFLINE.SHIPPEDAMOUNT,
			[WaitingQuantity] = (ORFLINE.AMOUNT - ORFLINE.SHIPPEDAMOUNT),
            [DemandQuantity] = ISNULL((SELECT SUM((AMOUNT - (MEETAMNT + CANCAMOUNT))) AS ONHAND FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_DEMANDLINE WITH(NOLOCK) WHERE ITEMREF = ITEMS.LOGICALREF AND STATUS <> 4),0),

			[NetTotal] = ORFLINE.LINENET,
            [ManufactureCode] =ISNULL((SELECT TOP 1 SUPPASGN.ICUSTSUPCODE FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_SUPPASGN AS SUPPASGN WHERE SUPPASGN.CLIENTREF = CLCARD.LOGICALREF AND SUPPASGN.ITEMREF = ITEMS.LOGICALREF AND SUPPASGN.CLCARDTYPE = 1),'')

			FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_ORFLINE AS ORFLINE
			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_ORFICHE AS ORFICHE ON ORFLINE.ORDFICHEREF = ORFICHE.LOGICALREF
			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_ITEMS AS ITEMS ON ORFLINE.STOCKREF = ITEMS.LOGICALREF
			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_CLCARD AS CLCARD ON ORFICHE.CLIENTREF = CLCARD.LOGICALREF

			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_UNITSETL AS SUBUNITSET ON ORFLINE.UOMREF = SUBUNITSET.LOGICALREF AND MAINUNIT = 1
			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_UNITSETF AS UNITSET ON ORFLINE.USREF = UNITSET.LOGICALREF
			LEFT JOIN L_CAPIWHOUSE AS WHOUSE ON ORFLINE.SOURCEINDEX = WHOUSE.NR AND WHOUSE.FIRMNR = {FirmNumber}
			LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_CLCARD AS Current_ ON ORFLINE.CLIENTREF = Current_.LOGICALREF
			WHERE ORFLINE.CLOSED = 0 AND (ORFLINE.AMOUNT - ORFLINE.SHIPPEDAMOUNT) > 0 AND ORFLINE.TRCODE = 2 AND CLCARD.LOGICALREF = {SupplierReferenceId}";

            string serilazeData = JsonSerializer.Serialize(query);
            var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await httpClient.PostAsync($"{requestUri}", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(jsonData))
                {
                    result = JsonSerializer.Deserialize<DataResult<PurchaseOrderLine>>(jsonData);
                }
                else
                    result.Data = Enumerable.Empty<PurchaseOrderLine>();

                result.IsSuccess = true;
            }
            else
            {
                result.Data = Enumerable.Empty<PurchaseOrderLine>();
                result.IsSuccess = false;
            }

            return result;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.IsSuccess = false;
            result.Data = Enumerable.Empty<PurchaseOrderLine>();

            return result;
        }
    }
}