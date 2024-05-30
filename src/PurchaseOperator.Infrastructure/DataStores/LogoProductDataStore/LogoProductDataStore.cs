using PurchaseOperator.Application.Services.LogoProductService;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.LogoProductModels;
using System.Text;
using System.Text.Json;

namespace PurchaseOperator.Infrastructure.DataStores.LogoProductDataStore
{
    public class LogoProductDataStore : ILogoProductService
    {
        private const string requestUri = "gateway/customQuery/CustomQuery";

        public async Task<DataResult<LogoProduct>> GetCustomObjectsAsync(HttpClient httpClient, int SupplierReferenceId, int FirmNumber)
        {
            DataResult<LogoProduct> result = new();
            try
            {
                string query = $@"SELECT
[ReferenceId] = ITEMS.LOGICALREF,
[Code] = ITEMS.CODE,
[Name] = ITEMS.NAME,
[ManufactureCode] = SUPPASGN.ICUSTSUPCODE,
[UnitsetReferenceId] = ITEMS.UNITSETREF,
[UnitsetCode] = UNITSETF.CODE,
[SubUnitsetReferenceId] = ISNULL((SELECT LOGICALREF FROM LG_{FirmNumber.ToString().PadLeft(3,'0')}_UNITSETL WHERE UNITSETREF = ITEMS.UNITSETREF AND MAINUNIT = 1),0),
[SubUnitsetCode] = ISNULL((SELECT CODE FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_UNITSETL WHERE UNITSETREF = ITEMS.UNITSETREF AND MAINUNIT = 1),0)
FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_SUPPASGN AS SUPPASGN WITH(NOLOCK)
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_CLCARD AS SUPPLIER WITH(NOLOCK) ON SUPPASGN.CLIENTREF = SUPPLIER.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3, '0')}_ITEMS AS ITEMS WITH(NOLOCK) ON SUPPASGN.ITEMREF = ITEMS.LOGICALREF
LEFT JOIN LG_{FirmNumber.ToString().PadLeft(3,'0')}_UNITSETF AS UNITSETF WITH(NOLOCK) ON ITEMS.UNITSETREF = UNITSETF.LOGICALREF
WHERE SUPPASGN.CLCARDTYPE = 1 AND ITEMS.SPECODE2 IN('A2','A3','B2','B3','S2','F1','F2','S3','P2','TT') AND ITEMS.CARDTYPE = 11 AND ITEMS.ACTIVE = 0 AND SUPPLIER.LOGICALREF = {SupplierReferenceId}";

                string serilazeData = JsonSerializer.Serialize(query);
                var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = await httpClient.PostAsync($"{requestUri}", content);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonData = await responseMessage.Content.ReadAsStringAsync();

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        result = JsonSerializer.Deserialize<DataResult<LogoProduct>>(jsonData);
                    }
                    else
                        result.Data = Enumerable.Empty<LogoProduct>();

                    result.IsSuccess = true;
                }
                else
                {
                    result.Data = Enumerable.Empty<LogoProduct>();
                    result.IsSuccess = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccess = false;
                result.Data = Enumerable.Empty<LogoProduct>();

                return result;
            }
        }
    }
}