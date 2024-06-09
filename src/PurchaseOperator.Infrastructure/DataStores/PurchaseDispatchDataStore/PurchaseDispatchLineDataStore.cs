using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseDispatchModels;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.PurchaseDispatchDataStore;

public class PurchaseDispatchLineDataStore : IPurchaseDispatchLineService
{
    private const string requestUri = "gateway/customQuery/CustomQuery";
    public async Task<DataResult<PurchaseDispatchLineModel>> GetObjectsAsync(HttpClient httpClient, int FirmNumber, int PeriodNumber, DateTime dateTime)
    {
        DataResult<PurchaseDispatchLineModel> result = new();

        try
        {
            string query = $@"SELECT 
[ReferenceId] = STLINE.LOGICALREF,
[DispatchReferenceId] = STLINE.STFICHEREF,
[FicheNumber] = STFICHE.FICHENO,
[DispatchDate] = STLINE.DATE_,
[SupplierReferenceId] = CLCARD.LOGICALREF,
[SupplierCode] = CLCARD.CODE,
[SupplierName] = CLCARD.DEFINITION_,
[ProductReferenceId] = ITEMS.LOGICALREF,
[ProductCode] = ITEMS.CODE,
[ProductName] = ITEMS.NAME,
[ProductSupplierCode] = ISNULL((SELECT ICUSTSUPCODE FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_SUPPASGN WHERE CLIENTREF = CLCARD.LOGICALREF AND ITEMREF = ITEMS.LOGICALREF),''),
[UnitsetReferenceId] = STLINE.USREF,
[UnitsetCode] = UNITSETF.CODE,
[UnitsetName] = UNITSETF.NAME,
[SubUnitsetReferenceId] = STLINE.UOMREF,
[SubUnitsetCode] = UNITSETL.CODE,
[SubUnitsetName] = UNITSETL.NAME,
[Quantity] = STLINE.AMOUNT,
[OrderReferenceId] = STLINE.ORDTRANSREF,
[OrderNumber] = ORFICHE.FICHENO

FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_STLINE AS STLINE WITH(NOLOCK)
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_STFICHE AS STFICHE WITH(NOLOCK) ON STLINE.STFICHEREF = STFICHE.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_ITEMS AS ITEMS WITH(NOLOCK) ON STLINE.STOCKREF = ITEMS.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_CLCARD AS CLCARD WITH(NOLOCK) ON STLINE.CLIENTREF = CLCARD.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_UNITSETF AS UNITSETF WITH(NOLOCK) ON STLINE.USREF = UNITSETF.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_UNITSETL AS UNITSETL WITH(NOLOCK) ON STLINE.UOMREF = UNITSETL.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_ORFLINE AS ORFLINE WITH(NOLOCK) ON STLINE.ORDTRANSREF = ORFLINE.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_ORFICHE AS ORFICHE WITH(NOLOCK) ON ORFLINE.ORDFICHEREF = ORFICHE.LOGICALREF
WHERE STLINE.TRCODE IN(7,8) AND STLINE.LINETYPE = 0 AND CONVERT (date,STLINE.DATE_, 101) = '{dateTime.Year}-{dateTime.Month.ToString().PadLeft(2,'0')}-{dateTime.Day.ToString().PadLeft(2, '0')}'";

            string serilazeData = JsonSerializer.Serialize(query);
            var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = await httpClient.PostAsync($"{requestUri}", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(jsonData))
                {
                    result = JsonSerializer.Deserialize<DataResult<PurchaseDispatchLineModel>>(jsonData);
                }
                else
                    result.Data = Enumerable.Empty<PurchaseDispatchLineModel>();

                result.IsSuccess = true;
            }
            else
            {
                result.Data = Enumerable.Empty<PurchaseDispatchLineModel>();
                result.IsSuccess = false;
            }

            return result;
        }
        catch (Exception ex)
        {
            result.Message = ex.Message;
            result.IsSuccess = false;
            result.Data = Enumerable.Empty<PurchaseDispatchLineModel>();

            return result;
        }
    }
}
