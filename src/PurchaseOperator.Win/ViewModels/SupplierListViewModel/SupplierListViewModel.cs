using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.SupplierService;
using PurchaseOperator.Domain.Models.SupplierModels;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.ViewModels.SupplierListViewModel;

public class SupplierListViewModel
{
    private readonly IAuthenticateLBSService _authenticateLBSService;
    private readonly ISupplierService _supplierService;
    private readonly ILogger<SupplierListViewModel> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    public Supplier SelectedSupplier { get; set; }
    public IEnumerable<Supplier> Items { get; set; } = Enumerable.Empty<Supplier>();

    public int FirmNumber { get; set; }
    public int PeriodNumber { get; set; }

    public SupplierListViewModel(ISupplierService supplierService, ILogger<SupplierListViewModel> logger, IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, IConfiguration configuration)
    {
        _authenticateLBSService = authenticateLBSService;
        _supplierService = supplierService;
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
        PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
    }

    public async Task SupplierListAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("LBS");
            var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                Items.ToList().Clear();
                var result = await _supplierService.GetObjectsAsync(httpClient, FirmNumber, PeriodNumber);
                Items = (IEnumerable<Supplier>)result.Data;
            }
            else
                Items = Enumerable.Empty<Supplier>();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public async Task ReloadListAsync()
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("LBS");
            var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = await _supplierService.GetObjectsAsync(httpClient, FirmNumber, PeriodNumber);
                Items = (IEnumerable<Supplier>)result.Data;
            }
            else
                Items = Enumerable.Empty<Supplier>();
        }
        catch (System.Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}