using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using System.Collections.ObjectModel;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PurchaseOperator.Domain.Models.SupplierModels;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using System.Net.Http;
using System.Linq;
using Microsoft.Extensions.Logging;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.Extensions.Configuration;

namespace PurchaseOperator.Win.ViewModels.PurchaseDispatchDetailViewModel;

public class PurchaseDispatchProductViewModel
{
    private ILogger<PurchaseDispatchProductViewModel> _logger;
    private IHttpClientFactory _httpClientFactory;
    private IAuthenticateLBSService _authenticateLBSService;
    private IPurchaseOrderService _purchaseOrderService;
    private IConfiguration _configuration;
    public List<DispatchItem> Items { get; } = new();
    public Supplier Supplier { get; set; }

    public int FirmNumber { get; set; }
    public int PeriodNumber { get; set; }

    public PurchaseDispatchProductViewModel(IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, IPurchaseOrderService purchaseOrderService, ILogger<PurchaseDispatchProductViewModel> logger, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _authenticateLBSService = authenticateLBSService;
        _purchaseOrderService = purchaseOrderService;
        _logger = logger;
        _configuration = configuration;
        FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
        PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
    }

    /// <summary>
    /// Bekleyen Satınalma Siparişlerini Getirir
    /// </summary>
    /// <param name="supplier">Seçili Tedarikçi</param>
    /// <returns>Satınalma Siparişleri</returns>
    private async Task<IEnumerable<PurchaseOrderLine>> GetPurchaseOrderLinesAsync(Supplier supplier)
    {
        var httpClient = _httpClientFactory.CreateClient("LBS");
        var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            var result = await _purchaseOrderService.GetObjectsAsync(httpClient, supplier.ReferenceId, FirmNumber, PeriodNumber);
            return (IEnumerable<PurchaseOrderLine>)result.Data;
        }
        else
            return Enumerable.Empty<PurchaseOrderLine>();
    }

    public async Task LoadDataAsync()
    {
        try
        {
            var orders = await GetPurchaseOrderLinesAsync(Supplier);

            if (orders.Any())
            {
                Items.Clear();
                var gropingData = orders.Where(x => x.WaitingQuantity > 0).GroupBy(x => x.ProductCode);
                foreach (var item in gropingData)
                {
                    DispatchItem dispatchItem = new DispatchItem();
                    dispatchItem.Code = item.Key;
                    dispatchItem.ReferenceId = item.ToList().First().ProductReferenceId;
                    dispatchItem.Name = item.ToList().First().ProductName;
                    dispatchItem.ManufactureCode = item.ToList().First().ManufactureCode;
                    dispatchItem.DemandQuantity = item.ToList().Sum(x => x.DemandQuantity);
                    dispatchItem.SupplyChainQuantity = item.ToList().Sum(x=> x.SupplyChainQuantity);
                    dispatchItem.TotalQuantity = item.ToList().Sum(x => x.Quantity);
                    dispatchItem.TotalShippedQuantity = item.ToList().Sum(x => x.ShippedQuantity);
                    dispatchItem.TotalWaitingQuantity = item.ToList().Sum(x => x.WaitingQuantity);

                    dispatchItem.CustomQuantity = default;
                    dispatchItem.Lines.AddRange(item.ToList());

                    Items.Add(dispatchItem);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public async Task ReloadDataAsync()
    {
        try
        {
            var orders = await GetPurchaseOrderLinesAsync(Supplier);

            if (orders.Any())
            {
                Items.Clear();
                var gropingData = orders.GroupBy(x => x.ProductCode);
                foreach (var item in gropingData)
                {
                    DispatchItem dispatchItem = new DispatchItem();
                    dispatchItem.Code = item.Key;
                    dispatchItem.ReferenceId = item.ToList().First().ReferenceId;
                    dispatchItem.Name = item.ToList().First().ProductName;
                    dispatchItem.DemandQuantity = item.ToList().Sum(x => x.DemandQuantity);
                    dispatchItem.SupplyChainQuantity = item.ToList().Sum(x => x.SupplyChainQuantity);
                    dispatchItem.TotalQuantity = item.ToList().Sum(x => x.Quantity);
                    dispatchItem.TotalShippedQuantity = item.ToList().Sum(x => x.ShippedQuantity);
                    dispatchItem.TotalWaitingQuantity = item.ToList().Sum(x => x.WaitingQuantity);

                    dispatchItem.CustomQuantity = (double)dispatchItem.TotalWaitingQuantity;
                    dispatchItem.Lines.AddRange(item.ToList());

                    Items.Add(dispatchItem);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            throw;
        }
    }

    public DialogResult ShowCloseMessage(Form form, Predicate<DialogResult> canCloseFunc)
    {
        FlyoutAction action = new FlyoutAction()
        {
            Caption = "Uyarı",
            Description = "Uygulamayı kapatmak istiyor musunuz?"
        };

        FlyoutCommand command1 = new FlyoutCommand()
        {
            Text = "Kapat",
            Result = DialogResult.Yes
        };

        FlyoutCommand command2 = new FlyoutCommand()
        {
            Text = "İptal",
            Result = DialogResult.No
        };

        action.Commands.Add(command1);
        action.Commands.Add(command2);

        Predicate<DialogResult> predicate = canCloseFunc;

        FlyoutProperties properties = new FlyoutProperties();

        properties.ButtonSize = new Size(100, 40);
        properties.Style = FlyoutStyle.MessageBox;

        return FlyoutDialog.Show(form, action, properties, predicate);
    }
}