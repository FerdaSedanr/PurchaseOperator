using Microsoft.Extensions.Configuration;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.LogoProductService;
using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Domain.Models.LogoProductModels;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.ViewModels.ProductListViewModels
{
    public class ProductListViewModel
    {
        private IHttpClientFactory _httpClientFactory;
        private IAuthenticateLBSService _authenticateLBSService;
        private ILogoProductService _logoProductService;
        private IConfiguration _configuration;

        public Supplier Supplier { get; set; }
        public List<LogoProduct> Items { get; } = new();

        public int TargetObjectType { get; set; }

        public ProductListViewModel(IHttpClientFactory httpClientFactory, IAuthenticateLBSService authenticateLBSService, ILogoProductService logoProductService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _authenticateLBSService = authenticateLBSService;
            _logoProductService = logoProductService;
            _configuration = configuration;
            FirmNumber = _configuration.GetSection("DefaultERP")["DefaultFirmNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultFirmNumber"]) : 0;
            PeriodNumber = _configuration.GetSection("DefaultERP")["DefaultPeriodNumber"] is not null ? int.Parse(_configuration.GetSection("DefaultERP")["DefaultPeriodNumber"]) : 0;
        }

        public int FirmNumber { get; set; }
        public int PeriodNumber { get; set; }

        public async Task GetCustomProductAsync()
        {
            var httpClient = _httpClientFactory.CreateClient("LBS");
            var token = await _authenticateLBSService.AuthenticateAsync(httpClient, "Admin", "");
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                var result = await _logoProductService.GetCustomObjectsAsync(httpClient, Supplier.ReferenceId, FirmNumber);

                var data = result.Data as IEnumerable<LogoProduct>;

                if (data.Any())
                {
                    Items.Clear();
                    Items.AddRange(data.ToList());
                }
            }
        }
    }
}