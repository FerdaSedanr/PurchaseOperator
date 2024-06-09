using Microsoft.Extensions.Configuration;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Domain.Models.PurchaseDispatchModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.ViewModels.PurchaseDispatchListViewModels;

public class PurchaseDispatchListViewModel
{
    readonly IHttpClientFactory _httpClientFactory;
    readonly IPurchaseDispatchLineService _purchaseDispatchLineService;
    readonly IConfiguration _configuration;
    readonly IAuthenticateLBSService _authenticateLBSService;

    int FirmNumber = 1;
    int PeriodNumber = 1;

    public IList<PurchaseDispatchLineModel> Items { get; set; }
    public PurchaseDispatchListViewModel(IHttpClientFactory httpClientFactory, IPurchaseDispatchLineService purchaseDispatchLineService, IConfiguration configuration, IAuthenticateLBSService authenticateLBSService)
    {
        _httpClientFactory = httpClientFactory;
        _purchaseDispatchLineService = purchaseDispatchLineService;
        _configuration = configuration;

        FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
        PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
        _authenticateLBSService = authenticateLBSService;

    }

    public async Task LoadDataAsync(DateTime dateTime)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("LBS");
            var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                var result = await _purchaseDispatchLineService.GetObjectsAsync(httpClient, FirmNumber, PeriodNumber, dateTime);
                if (result.IsSuccess)
                    Items = result.Data.ToList();
                else
                    Items = new List<PurchaseDispatchLineModel>();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}
