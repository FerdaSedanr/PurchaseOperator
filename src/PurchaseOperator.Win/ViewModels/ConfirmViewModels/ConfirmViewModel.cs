using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Application.Services.ISubUnitsetServices;
using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Application.Services.PurchaseDispatchService;
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
using System.Threading.Tasks;
using System.Windows.Threading;

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
        private IQCNotificationService _notificationService;
        private IQCNotificationDetailService _notificationDetailService;
        private ISubUnitsetService _subUnitsetService;

        public ConfirmViewModel(ILogger<ConfirmViewModel> logger, IConfiguration configuration, IPurchaseDispatchService purchaseDispatchService, IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, ICustomerService customerService, IPortalProductService portalProductService, IAuthenticatePortalService authenticatePortalService, IQCNotificationService notificationService, IQCNotificationDetailService notificationDetailService, ISubUnitsetService subUnitsetService)
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

            WarehouseNumber = _configuration.GetSection("DefaultERP")["Warehouse"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["Warehouse"]) : 0;
            LocationCode = string.IsNullOrEmpty(_configuration.GetSection("DefaultERP")["Location"]) ? string.Empty : _configuration.GetSection("DefaultERP")["Location"];
            FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
            PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
        }

        public List<DispatchItem> Items { get; } = new();
        public Supplier Supplier { get; set; }

        public int WarehouseNumber { get; set; }
        public string LocationCode { get; set; }

        public int FirmNumber { get; set; }

        public int PeriodNumber { get; set; }

        public int TargetObjectType { get; set; }

        public async Task<ResponseDataResult<ProductTransactionResult>> InsertPurchaseDispatchAsync(PurchaseDispatchTransactionDto dto)
        {
            ResponseDataResult<ProductTransactionResult> productTransactionResult = new ResponseDataResult<ProductTransactionResult>();

            try
            {
                var httpClient = _httpClientFactory.CreateClient("LBS");
                var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
                if (!string.IsNullOrEmpty(token))
                {
                    httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    productTransactionResult = await _purchaseDispatchService.InsertAsync(httpClient, dto, FirmNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return await Task.FromResult(productTransactionResult);
        }

        public async Task InsertQCNotificationAsync(string DispatchNumber, PurchaseDispatchTransactionDto dto)
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
                        var qcNotificationResult = await _notificationService.InsertObjectAsync(httpClient, new QCNotificationDto(DateTime.Now, DispatchNumber, 0, customer.Oid));
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
                        QCNotificationDetailDto detailDto = new QCNotificationDetailDto(Guid.Parse(qCNotification.Oid), product.FirstOrDefault().Oid, subUnitset.FirstOrDefault().Oid, (double)item.Quantity);
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
                lineDto.SeriLotTransactions.Add(new()
                {
                    WarehouseNumber = WarehouseNumber,
                    Quantity = line.CustomQuantity,
                    Date = DateTime.Now,
                    IOCode = 1,
                    StockLocationCode = LocationCode,
                    ConversionFactor = line.CustomQuantity,
                    OtherConversionFactor = line.CustomQuantity,
                });

                dto.Lines.Add(lineDto);
            }

            return await Task.FromResult(dto);
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

            foreach (var item in items.Where(x=> x.Lines.Count > 0))
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
                            lineDto.SeriLotTransactions.Add(new()
                            {
                                WarehouseNumber = WarehouseNumber,
                                Quantity = remainingQuantity,
                                Date = DateTime.Now,
                                IOCode = 1,
                                StockLocationCode = LocationCode,
                                ConversionFactor = (double)remainingQuantity,
                                OtherConversionFactor = (double)remainingQuantity,
                            });

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
                            lineDto.SeriLotTransactions.Add(new()
                            {
                                WarehouseNumber = WarehouseNumber,
                                Quantity = line.Quantity - line.ShippedQuantity,
                                Date = DateTime.Now,
                                IOCode = 1,
                                StockLocationCode = LocationCode,
                                ConversionFactor = line.Quantity - line.ShippedQuantity,
                                OtherConversionFactor = line.Quantity - line.ShippedQuantity,
                            });

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
                lineDto.SeriLotTransactions.Add(new()
                {
                    WarehouseNumber = WarehouseNumber,
                    Quantity = line.CustomQuantity,
                    Date = DateTime.Now,
                    IOCode = 1,
                    StockLocationCode = LocationCode,
                    ConversionFactor = line.CustomQuantity,
                    OtherConversionFactor = line.CustomQuantity,
                });

                dto.Lines.Add(lineDto);
            }

            return await Task.FromResult(dto);
        }
    }
}