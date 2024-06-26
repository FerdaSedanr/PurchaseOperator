using PurchaseOperator.Application.Services.SupplierService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PurchaseOperator.Infrastructure.DataStores.SupplierDataStore;

public class CustomSupplierDataStore : ISupplierService
{
    private const string requestUri = "gateway/customQuery/CustomQuery";

    public async Task<DataResult<Supplier>> GetObjectsAsync(HttpClient httpClient, int FirmNumber, int PeriodNumber)
    {
        DataResult<Supplier> result = new();

        string query = $@"

SELECT * FROM (SELECT

[ReferenceId] = SUPPLIER.LOGICALREF,
[Code] = SUPPLIER.CODE,
[Title] = SUPPLIER.DEFINITION_,
[Name] = SUPPLIER.DEFINITION_,
[City] = SUPPLIER.CITY,
[County] = SUPPLIER.TOWN,
[OrderQuantity] = ISNULL((SELECT SUM(AMOUNT - SHIPPEDAMOUNT) FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_{PeriodNumber.ToString().PadLeft(2, '0')}_ORFLINE AS ORFLINE WITH(NOLOCK) WHERE ORFLINE.CLIENTREF = SUPPLIER.LOGICALREF AND ORFLINE.TRCODE = 2 AND ORFLINE.CLOSED = 0),0),
[DispatchType] = SUPPLIER.ACCEPTEDESP

FROM LG_{FirmNumber.ToString().PadLeft(3, '0')}_CLCARD AS SUPPLIER
WHERE SUPPLIER.ACTIVE = 0) DD WHERE DD.[OrderQuantity] > 0
";

        string serilazeData = JsonSerializer.Serialize(query);
        var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
        HttpResponseMessage responseMessage = await httpClient.PostAsync($"{requestUri}", content);
        if (responseMessage.IsSuccessStatusCode)
        {
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(jsonData))
                result = JsonSerializer.Deserialize<DataResult<Supplier>>(jsonData);
            else
                result.Data = Enumerable.Empty<Supplier>();

            result.IsSuccess = true;
        }
        else
        {
            result.Data = Enumerable.Empty<Supplier>();
            result.IsSuccess = false;
        }

        return result;
    }
}