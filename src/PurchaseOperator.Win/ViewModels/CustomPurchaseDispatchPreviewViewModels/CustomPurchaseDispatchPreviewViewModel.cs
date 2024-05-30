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
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;

public class CustomPurchaseDispatchPreviewViewModel
{
    private readonly IPurchaseDispatchService _purchaseDispatchService;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IAuthenticateLBSService _authenticateLBSService;
    private readonly IAuthenticatePortalService _authenticatePortalService;
    private readonly ICustomerService _customerService;
    private readonly IPortalProductService _portalProductService;
    private readonly IConfiguration _configuration;

    public CustomPurchaseDispatchPreviewViewModel(IPurchaseDispatchService purchaseDispatchService, IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, ICustomerService customerService, IPortalProductService portalProductService, IAuthenticatePortalService authenticatePortalService, IConfiguration configuration)
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

    public List<DispatchItem> Items { get; } = new();
    public Supplier Supplier { get; set; }
    public int FirmNumber { get; set; }
    public int PeriodNumber { get; set; }

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

    private async Task<HttpClient> PortalAuthenticateAsync()
    {
        HttpClient httpClient;

        httpClient = _httpClientFactory.CreateClient("Portal");
        var token = await _authenticatePortalService.AuthenticateAsync(httpClient, _configuration["PortalAuthenticateInformation:UserName"], _configuration["PortalAuthenticateInformation:Password"]);
        if (!string.IsNullOrEmpty(token))
        {
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
        }

        return await Task.FromResult(httpClient);
    }

    public async Task<Product> GetProductAsync(int ProductReferenceId)
    {
        var httpClient = await PortalAuthenticateAsync();

        string filter = $"?$filter=ReferenceId eq {ProductReferenceId}";
        var data = await _portalProductService.GetObjectsAsync(httpClient, filter);
        return data.FirstOrDefault();
    }

    public async Task GetObjectsAsync(string Code)
    {
        //var httpClient = _httpClientFactory.CreateClient("Portal");
        //var token = await _authenticatePortalService.AuthenticateAsync(http)
    }
}