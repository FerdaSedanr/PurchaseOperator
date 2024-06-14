using DevExpress.XtraEditors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Application.Services.ISubUnitsetServices;
using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Application.Services.PurchaseReturnDispatchService;
using PurchaseOperator.Application.Services.QCNotificationDetailService;
using PurchaseOperator.Application.Services.QCNotificationService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.DataResultModel;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.QCNotification;
using PurchaseOperator.Domain.Models.SupplierModels;
using PurchaseOperator.Domain.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using PurchaseOperator.Domain.Models.SeriLotInfoModels;
using System.Text.Json.Nodes;

namespace PurchaseOperator.Win.ViewModels.ConfirmViewModels
{
    public class ConfirmViewModel
    {
        private readonly ILogger<ConfirmViewModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly IPurchaseDispatchService _purchaseDispatchService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticateLBSService _authenticateLBSService;
        private readonly IAuthenticatePortalService _authenticatePortalService;
        private readonly ICustomerService _customerService;
        private readonly IPortalProductService _portalProductService;
        private readonly IQCNotificationService _notificationService;
        private readonly IQCNotificationDetailService _notificationDetailService;
        private readonly ISubUnitsetService _subUnitsetService;
        private readonly IPurchaseReturnDispatchService _purchaseReturnDispatchService;

        public ConfirmViewModel(ILogger<ConfirmViewModel> logger, IConfiguration configuration, IPurchaseDispatchService purchaseDispatchService, IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, ICustomerService customerService, IPortalProductService portalProductService, IAuthenticatePortalService authenticatePortalService, IQCNotificationService notificationService, IQCNotificationDetailService notificationDetailService, ISubUnitsetService subUnitsetService, IPurchaseReturnDispatchService purchaseReturnDispatchService)
        {
            _logger = logger;
            _configuration = configuration;
            _purchaseDispatchService = purchaseDispatchService;
            _httpClientFactory = httpClientFactory;
            _authenticateLBSService = authenticateLBSService;
            _customerService = customerService;
            _portalProductService = portalProductService;
            _authenticatePortalService = authenticatePortalService;
            _notificationService = notificationService;
            _notificationDetailService = notificationDetailService;
            _subUnitsetService = subUnitsetService;
            _purchaseReturnDispatchService = purchaseReturnDispatchService;

            WarehouseNumber = _configuration.GetSection("DefaultERP")["Warehouse"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["Warehouse"]) : 0;
            LocationCode = string.IsNullOrEmpty(_configuration.GetSection("DefaultERP")["Location"]) ? string.Empty : _configuration.GetSection("DefaultERP")["Location"];
            FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
            PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
        }

        public List<DispatchItem> Items { get; } = new();
        public List<DispatchItem> ReturnItems { get; set; } = new();
        public List<DispatchItem> ExcessItems { get; set; } = new();
        public Supplier Supplier { get; set; }

        public int WarehouseNumber { get; set; }
        public string LocationCode { get; set; }

        public int FirmNumber { get; set; }

        public int PeriodNumber { get; set; }

        public int TargetObjectType { get; set; }

        public async Task<HttpClient> LBSAuthenticateAsync()
        {
            return await Task.Run(async () =>
            {
                var httpClient = _httpClientFactory.CreateClient("LBS");
                var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    return httpClient;
                }
                return null;
            });
        }

        public async Task<ResponseDataResult<ProductTransactionResult>> InsertPurchaseDispatchAsync(HttpClient httpClient ,PurchaseDispatchTransactionDto dto)
        {
            ResponseDataResult<ProductTransactionResult> productTransactionResult = new ResponseDataResult<ProductTransactionResult>();

            try
            {
                //var httpClient = _httpClientFactory.CreateClient("LBS");
                //var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
                //if (!string.IsNullOrEmpty(token))
                //{
                //    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                //    productTransactionResult = await _purchaseDispatchService.InsertAsync(httpClient, dto, FirmNumber);
                //}

                productTransactionResult = await _purchaseDispatchService.InsertAsync(httpClient, dto, FirmNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return await Task.FromResult(productTransactionResult);
        }

        public async Task InsertQCNotificationAsync(string DispatchNumber, int DispatchReferenceId, PurchaseDispatchTransactionDto dto)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("Portal");
                var token = await _authenticatePortalService.AuthenticateAsync(httpClient, "Administrator", "V0lkansun!*#16730000");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    var customer = await GetPortalSupplier(httpClient);
                    if (customer is not null)
                    {
                        Guid warehouseOid = _configuration.GetSection("DefaultERP")["WarehouseOid"] is not null ? Guid.Parse(_configuration.GetSection("DefaultERP")["WarehouseOid"]) : Guid.Empty;
                        var qcNotificationResult = await _notificationService.InsertObjectAsync(httpClient, new QCNotificationDto(DateTime.Now, DispatchNumber, DispatchReferenceId, customer.Oid, warehouseOid));
                        if (qcNotificationResult is not null)
                            await InsertQCNotificationDetailAsync(httpClient, qcNotificationResult, dto.Lines.ToList());
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task<Domain.Models.CustomerModels.Customer> GetPortalSupplier(HttpClient httpClient)
        {
            string customerFilter = $"?$filter=ReferenceId eq {Supplier.ReferenceId}";
            var customers = await _customerService.GetObjectsAsync(httpClient, customerFilter);
            return customers.FirstOrDefault();
        }

        private async Task InsertQCNotificationDetailAsync(HttpClient httpClient, QCNotification qCNotification, List<PurchaseDispatchTransactionLineDto> dispatchLines)
        {
            try
            {
                foreach (var item in dispatchLines)
                {
                    var product = await _portalProductService.GetObjectsAsync(httpClient, $"?$filter=Code eq '{item.ProductCode}'");
                    var subUnitset = await _subUnitsetService.GetObjectsAsync(httpClient, $"?$filter=ReferenceId eq {item.SubUnitsetReferenceId}");
                    if (product.Any() && subUnitset.Any())
                    {
                        QCNotificationDetailDto detailDto = new QCNotificationDetailDto(Guid.Parse(qCNotification.Oid), product.FirstOrDefault().Oid, subUnitset.FirstOrDefault().Oid, Items.FirstOrDefault(x=> x.Code == item.ProductCode)?.CountAmount ?? 0);
                        await _notificationDetailService.InsertObjectAsync(httpClient, detailDto);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Sipariş Bağımsız İrsaliye Oluşturur
        /// </summary>
        /// <param name="items">İrsaliye Satırları</param>
        /// <param name="WarehouseNumber">Ambar Numarası</param>
        /// <param name="LocationCode">Raf Yeri Kodu</param>
        /// <returns></returns>
        public async Task<PurchaseDispatchTransactionDto> CreateDispatchDTOAsync(List<DispatchItem> items, string number, DateTime dispatchOn)
        {
            PurchaseDispatchTransactionDto dto = new();
            dto.WarehouseNumber = WarehouseNumber;
            dto.CurrentCode = Supplier.Code;
            var owner = Guid.NewGuid();
            dto.Owner = owner;
            dto.TransactionType = 1;
            dto.IOType = 1;
            dto.GroupType = 1;
            dto.TransactionDate = dispatchOn;
            dto.Code = number;
            dto.IsEDispatch = Supplier.DispatchType;

            foreach (var line in items)
            {
                var lineDto = new PurchaseDispatchTransactionLineDto()
                {
                    WarehouseNumber = WarehouseNumber,
                    ProductCode = line.Code,
                    UnitsetCode = line.UnitsetCode,
                    SubUnitsetReferenceId = line.SubUnitsetReferenceId,
                    SubUnitsetCode = line.SubUnitsetCode,
                    CurrentCode = Supplier.Code,
                    IOType = 1,
                    Description = line.Description,
                    Quantity = line.CustomQuantity,
                    ConversionFactor = line.CustomQuantity,
                    OtherConversionFactor = line.CustomQuantity,
                    TransactionDate = DateTime.Now,
                    UnitPrice = 0,
                    TransactionType = 1,
                    VatRate = 20,
                };
                lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(

                StockLocationCode: LocationCode,
                DestinationStockLocationCode: string.Empty,
                InProductTransactionLineReferenceId: 0,
                OutProductTransactionLineReferenceId: 0,
                SerilotType: 0,
                Quantity: Convert.ToDouble(line.CustomQuantity),
                SubUnitsetCode: line.SubUnitsetCode,
                ConversionFactor: 1,
                OtherConversionFactor: 1));



                dto.Lines.Add(lineDto);
            }

            return await Task.FromResult(dto);
        }

        private static DispatchItem ConvertItem(DispatchItem line)
        {
            return new DispatchItem { CustomQuantity = line.CustomQuantity, ManufactureCode = line.ManufactureCode, Code = line.Code, CountAmount = line.CountAmount, TotalWaitingQuantity = line.TotalWaitingQuantity, DemandQuantity = line.DemandQuantity, Name = line.Name, ReferenceId = line.ReferenceId, SubUnitsetCode = line.SubUnitsetCode, SupplyChainQuantity = line.SupplyChainQuantity, SubUnitsetReferenceId = line.SubUnitsetReferenceId, TotalQuantity = line.TotalQuantity, Description = line.Description, LineNumber = line.LineNumber, TotalShippedQuantity = line.TotalShippedQuantity, UnitsetCode = line.UnitsetCode, UnıtsetReferenceId = line.UnıtsetReferenceId, Lines = new() };
        }

        /// <summary>
        /// Sipariş Bağımlı İrsaliye Nesnesi Oluşturur
        /// </summary>
        /// <param name="items">İrsaliye Satırları</param>
        /// <param name="WarehouseNumber">Ambar Numarası</param>
        /// <param name="LocationCode">Raf Yeri Kodu</param>
        /// <returns></returns>
        public async Task<PurchaseDispatchTransactionDto> CreateDispatchDTOWithOrderAsync(List<DispatchItem> items, string number, DateTime dispatchOn)
        {
            PurchaseDispatchTransactionDto dto = new();            
            dto.WarehouseNumber = WarehouseNumber;
            dto.CurrentCode = Supplier.Code;
            var owner = Guid.NewGuid();
            dto.Owner = owner;
            dto.TransactionType = 1;
            dto.IOType = 1;
            dto.GroupType = 1;
            dto.TransactionDate = dispatchOn;
            dto.Code = number;
            dto.IsEDispatch = Supplier.DispatchType;

            foreach (var item in items.Where(x => x.Lines.Count > 0))
            {
                var remainingQuantity = item.CustomQuantity;
                foreach (var line in item.Lines.OrderBy(x => x.Date))
                {
                    if (remainingQuantity > 0)
                    {
                        if ((remainingQuantity - line.WaitingQuantity) < 0)
                        {
                            var lineDto = new PurchaseDispatchTransactionLineDto()
                            {
                                WarehouseNumber = WarehouseNumber,
                                ProductCode = line.ProductCode,
                                UnitsetCode = line.SubUnitsetCode,
                                SubUnitsetReferenceId = line.SubUnitsetReferenceId,
                                SubUnitsetCode = line.SubUnitsetCode,
                                CurrentCode = line.CurrentCode,
                                IOType = 1,
                                Description = line.Description,
                                Quantity = remainingQuantity,
                                ConversionFactor = remainingQuantity,
                                OtherConversionFactor = remainingQuantity,
                                TransactionDate = DateTime.Now,
                                UnitPrice = line.UnitPrice,
                                TransactionType = 1,
                                VatRate = line.VatRate,
                                OrderReferenceId = line.ReferenceId
                            };
                            lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(
                                StockLocationCode: LocationCode,
                                DestinationStockLocationCode: string.Empty,
                                InProductTransactionLineReferenceId: 0,
                                OutProductTransactionLineReferenceId: 0,
                                SerilotType: 0,
                                Quantity: Convert.ToDouble(remainingQuantity),
                                SubUnitsetCode: line.SubUnitsetCode,
                                ConversionFactor: 1,
                                OtherConversionFactor: 1));

                            dto.Lines.Add(lineDto);
                            remainingQuantity = 0;
                        }
                        else
                        {
                            var lineDto = new PurchaseDispatchTransactionLineDto()
                            {
                                WarehouseNumber = WarehouseNumber,
                                ProductCode = line.ProductCode,
                                UnitsetCode = line.SubUnitsetCode,
                                SubUnitsetReferenceId = line.SubUnitsetReferenceId,
                                SubUnitsetCode = line.SubUnitsetCode,
                                CurrentCode = line.CurrentCode,
                                IOType = 1,
                                Description = line.Description,
                                //remaninig alanını ekledim
                                Quantity = line.Quantity - line.ShippedQuantity,
                                ConversionFactor = line.Quantity - line.ShippedQuantity,
                                OtherConversionFactor = line.Quantity - line.ShippedQuantity,
                                TransactionDate = DateTime.Now,
                                UnitPrice = line.UnitPrice,
                                TransactionType = 1,
                                VatRate = line.VatRate,
                                OrderReferenceId = line.ReferenceId
                            };
                            lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(
                                StockLocationCode: LocationCode,
                                DestinationStockLocationCode: string.Empty,
                                InProductTransactionLineReferenceId: 0,
                                OutProductTransactionLineReferenceId: 0,
                                SerilotType: 0,
                                Quantity: Convert.ToDouble(line.Quantity - line.ShippedQuantity),
                                SubUnitsetCode: line.SubUnitsetCode,
                                ConversionFactor: 1,
                                OtherConversionFactor: 1));

                            dto.Lines.Add(lineDto);
                            remainingQuantity -= (double)line.Quantity;
                        }

                    }
                }
            }

            foreach (var line in items.Where(x => x.Lines.Count == 0))
            {
                var lineDto = new PurchaseDispatchTransactionLineDto()
                {
                    WarehouseNumber = WarehouseNumber,
                    ProductCode = line.Code,
                    UnitsetCode = line.UnitsetCode,
                    SubUnitsetReferenceId = line.SubUnitsetReferenceId,
                    SubUnitsetCode = line.SubUnitsetCode,
                    CurrentCode = Supplier.Code,
                    IOType = 1,
                    Description = line.Description,
                    Quantity = line.CustomQuantity,
                    ConversionFactor = line.CustomQuantity,
                    OtherConversionFactor = line.CustomQuantity,
                    TransactionDate = DateTime.Now,
                    UnitPrice = 0,
                    TransactionType = 1,
                    VatRate = 20
                };
                lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(
                                StockLocationCode: LocationCode,
                                DestinationStockLocationCode: string.Empty,
                                InProductTransactionLineReferenceId: 0,
                                OutProductTransactionLineReferenceId: 0,
                                SerilotType: 0,
                                Quantity: Convert.ToDouble(line.CustomQuantity),
                                SubUnitsetCode: line.SubUnitsetCode,
                                ConversionFactor: 1,
                                OtherConversionFactor: 1));

                dto.Lines.Add(lineDto);
            }

            return await Task.FromResult(dto);
        }

        public async Task InsertPurchaseReturnDispatch(HttpClient httpClient, int DispatchReferenceId, List<DispatchItem> items,int QuantityType)
        {
            var dto = new PurchaseReturnDispatchTransactionDto(
                    Code: null,
                    TransactionDate: DateTime.Now,
                    WarehouseNumber: WarehouseNumber,
                    CurrentCode: Supplier.Code,
                    Description: string.Empty,
                    DispatchType: 0,
                    SpeCode: string.Empty,
                    DispatchStatus: 1,
                    IsEDispatch: (int)Supplier.DispatchType,
                    DoCode: string.Empty,
                    DocTrackingNumber: string.Empty,
                    EDispatchProfileId: (int)Supplier.DispatchType,
                    Lines: GetReturnItems(httpClient,DispatchReferenceId, items, QuantityType));



            //var httpClient = _httpClientFactory.CreateClient("LBS");
            //var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            //if (!string.IsNullOrEmpty(token))
            //{
            //    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            //    var result = await _purchaseReturnDispatchService.InsertAsync(httpClient, dto, FirmNumber);
            //}

            var result = await _purchaseReturnDispatchService.InsertAsync(httpClient, dto, FirmNumber);
        }
        private List<PurchaseReturnDispatchTransactionLineDto> GetReturnItems(HttpClient httpClient,int DispatchReferenceId, List<DispatchItem> items, int QuantityType)
        {
            var lines = new List<PurchaseReturnDispatchTransactionLineDto>();

            if (QuantityType == 1)
            {
                foreach (var item in items)
                {
                    lines.Add(new(
                        ProductCode: item.Code,
                        SubUnitsetCode: item.SubUnitsetCode,
                        Quantity: item.UnderCountQuantity,
                        UnitPrice: 0,
                        VatRate: 20,
                        WarehouseNumber: WarehouseNumber,
                        OrderReferenceId: 0,
                        Description: string.Empty,
                        SpeCode: "Sayım Eksiği İade",
                        ConversionFactor: 1,
                        OtherConversionFactor: 1,
                        GetReturnItemSeriLots(httpClient, DispatchReferenceId, item.ReferenceId, item.SubUnitsetCode, item.UnderCountQuantity)));

                }
            }
            else if (QuantityType == 2)
            {
                foreach (var item in items)
                {
                    lines.Add(new(
                        ProductCode: item.Code,
                        SubUnitsetCode: item.SubUnitsetCode,
                        Quantity: item.OverOrderQuantity,
                        UnitPrice: 0,
                        VatRate: 20,
                        WarehouseNumber: WarehouseNumber,
                        OrderReferenceId: 0,
                        Description: string.Empty,
                        SpeCode: "Sipariş Fazlası İade",
                        ConversionFactor: 1,
                        OtherConversionFactor: 1,
                        GetReturnItemSeriLots(httpClient, DispatchReferenceId, item.ReferenceId, item.SubUnitsetCode, item.OverOrderQuantity)));

                }
            }
            else
            {
                foreach (var item in items)
                {


                    lines.Add(new(
                        ProductCode: item.Code,
                        SubUnitsetCode: item.SubUnitsetCode,
                        Quantity: item.CustomQuantity,
                        UnitPrice: 0,
                        VatRate: 20,
                        WarehouseNumber: WarehouseNumber,
                        OrderReferenceId: 0,
                        Description: string.Empty,
                        SpeCode: "Diğer",
                        ConversionFactor: 1,
                        OtherConversionFactor: 1,
                        GetReturnItemSeriLots(httpClient, DispatchReferenceId, item.ReferenceId, item.SubUnitsetCode, item.CustomQuantity)));

                }
            }


            return lines;
        }
        public List<SeriLotTransactionDto> GetReturnItemSeriLots(HttpClient httpClient,int DispatchReferenceId, int ProductReferenceId, string SubUnitsetCode, double Quantity)
        {
            var seriLots = new List<SeriLotTransactionDto>();
            var seriLotInfo = CustomQuery(httpClient, DispatchReferenceId, ProductReferenceId, WarehouseNumber);
            if (seriLotInfo is not null)
            {
                seriLots.Add(new SeriLotTransactionDto(
                    StockLocationCode: LocationCode,
                    DestinationStockLocationCode: string.Empty,
                    InProductTransactionLineReferenceId: seriLotInfo.InProductTransactionReferenceId,
                    OutProductTransactionLineReferenceId: seriLotInfo.OutProductTransacationReferenceId,
                    SerilotType: 0,
                    Quantity: Quantity,
                    SubUnitsetCode: SubUnitsetCode,
                    ConversionFactor: 1,
                    OtherConversionFactor: 1));
            }

            return seriLots;
        }
        private SeriLotInfo CustomQuery(HttpClient httpClient, int dispatchReferenceId, int productReferenceId, int warehouseNumber)
        {
            SeriLotInfo seriLotInfo = null;

            string query = $@"SELECT  
[InProductTransactionReferenceId] = LGMAIN.STTRANSREF,
[OutProductTransacationReferenceId] = LGMAIN.LOGICALREF

 FROM 
LG_001_02_SLTRANS LGMAIN WITH(NOLOCK)    
LEFT OUTER JOIN LG_001_ITEMS ITEMS WITH(NOLOCK) ON (LGMAIN.ITEMREF  =  ITEMS.LOGICALREF) 
LEFT OUTER JOIN LG_001_LOCATION INVLOC WITH(NOLOCK) ON (LGMAIN.LOCREF  =  INVLOC.LOGICALREF) 
LEFT OUTER JOIN LG_001_02_STFICHE STFIC WITH(NOLOCK) ON (LGMAIN.STFICHEREF  =  STFIC.LOGICALREF) 
LEFT OUTER JOIN LG_001_UNITSETL USLINE WITH(NOLOCK) ON (LGMAIN.UOMREF  =  USLINE.LOGICALREF) 
LEFT OUTER JOIN LG_001_UNITSETF UNITSET WITH(NOLOCK) ON (USLINE.UNITSETREF  =  UNITSET.LOGICALREF)
LEFT OUTER JOIN L_CAPIWHOUSE WHOUSE WITH(NOLOCK) ON (LGMAIN.INVENNO = WHOUSE.NR AND WHOUSE.FIRMNR = 1)
 WHERE 
(LGMAIN.CANCELLED = 0) AND 
(LGMAIN.LPRODSTAT = 0) AND 
(LGMAIN.ITEMREF = {productReferenceId}) AND
(LGMAIN.INVENNO = {warehouseNumber}) AND
(LGMAIN.STFICHEREF = {dispatchReferenceId}) AND
(LGMAIN.EXIMFCTYPE IN ( 0 , 4 , 5 , 3 , 2 , 7 )) AND 
(LGMAIN.STATUS = 0) AND 
(LGMAIN.REMAMOUNT > 0) AND 
(LGMAIN.IOCODE IN (1,2))

GROUP BY LGMAIN.LOGICALREF,LGMAIN.STTRANSREF";

            string requestUri = "gateway/customQuery/CustomQuery";

            string serilazeData = JsonSerializer.Serialize(query);
            var content = new StringContent(serilazeData, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = httpClient.PostAsync($"{requestUri}", content).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                var result = responseMessage.Content.ReadAsStringAsync().Result;

                if (!string.IsNullOrEmpty(result))                
                    seriLotInfo = JsonNode.Parse(result)["data"].Deserialize<IEnumerable<SeriLotInfo>>().First();
                
                
            }

            return seriLotInfo;

        }
    }
}