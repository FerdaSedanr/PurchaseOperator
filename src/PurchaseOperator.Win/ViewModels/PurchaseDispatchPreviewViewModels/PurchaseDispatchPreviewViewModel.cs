using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using Microsoft.Extensions.Configuration;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Models.PortalProductModels;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.SupplierModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.ViewModels.PurchaseDispatchPreviewViewModels;

public class PurchaseDispatchPreviewViewModel
{
    private readonly IPurchaseDispatchService _purchaseDispatchService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAuthenticateLBSService _authenticateLBSService;
    private readonly IAuthenticatePortalService _authenticatePortalService;
    private readonly ICustomerService _customerService;
    private readonly IPortalProductService _portalProductService;
    private readonly IConfiguration _configuration;

    public PurchaseDispatchPreviewViewModel(IPurchaseDispatchService purchaseDispatchService, IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, ICustomerService customerService, IPortalProductService portalProductService, IAuthenticatePortalService authenticatePortalService, IConfiguration configuration)
    {
        _purchaseDispatchService = purchaseDispatchService;
        _httpClientFactory = httpClientFactory;
        _authenticateLBSService = authenticateLBSService;
        _customerService = customerService;
        _portalProductService = portalProductService;
        _authenticatePortalService = authenticatePortalService;
        _configuration = configuration;
        FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
        PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
    }

    public int FirmNumber { get; set; }
    public int PeriodNumber { get; set; }

    public BindingList<DispatchItem> Items { get; } = new();
    public Supplier Supplier { get; set; }

    public async Task CreatePurchaseDispatch(PurchaseDispatchTransactionDto dto)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("LBS");
            var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var result = _purchaseDispatchService.InsertAsync(httpClient, dto, FirmNumber);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private async Task AuthenticateAsync()
    {

        var httpClient = _httpClientFactory.CreateClient("Portal");
        var token = await _authenticatePortalService.AuthenticateAsync(httpClient, _configuration["PortalAuthenticateInformation:UserName"], _configuration["PortalAuthenticateInformation:Password"]);
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

    }

    public async Task<Product> GetProductAsync(int ProductReferenceId)
    {
        Product product = null;

        var httpClient = _httpClientFactory.CreateClient("Portal");
        var token = await _authenticatePortalService.AuthenticateAsync(httpClient, _configuration["PortalAuthenticateInformation:UserName"], _configuration["PortalAuthenticateInformation:Password"]);
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            string filter = $"?$filter=ReferenceId eq {ProductReferenceId}";
            var result = await _portalProductService.GetObjectsAsync(httpClient, filter);
            if (result != null)            
                product = result.FirstOrDefault();
            
        }

        return product;
    }

    public async Task GetObjectsAsync(string Code)
    {
        //var httpClient = _httpClientFactory.CreateClient("Portal");
        //var token = await _authenticatePortalService.AuthenticateAsync(http)
    }
}