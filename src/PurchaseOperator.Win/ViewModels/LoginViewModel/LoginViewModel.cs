using Microsoft.Extensions.Configuration;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.EmployeeService;
using PurchaseOperator.Application.Services.OperatorService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PurchaseOperator.Win.ViewModels.LoginViewModel
{
    public class LoginViewModel
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IAuthenticatePortalService _authenticateService;
        private readonly IOperatorService _operatorService;
        private readonly IEmployeeService _employeeService;
        private readonly IConfiguration _configuration;

        public LoginViewModel(IHttpClientFactory httpClientFactory, IAuthenticatePortalService authenticateService, IOperatorService operatorService, IEmployeeService employeeService, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _authenticateService = authenticateService;
            _operatorService = operatorService;
            _employeeService = employeeService;
            _configuration = configuration;
        }

        public async Task<bool> UserLoginAsync(string operatorCode)
        {
            bool result = false;
            var httpClient = _httpClientFactory.CreateClient("Portal");

            var userName = _configuration["PortalAuthenticateInformation:UserName"];

            var token = await _authenticateService.AuthenticateAsync(httpClient, _configuration["PortalAuthenticateInformation:UserName"], _configuration["PortalAuthenticateInformation:Password"]);
            if (!string.IsNullOrEmpty(token))
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                string filter = $"?$filter=Code eq '{operatorCode}'&$expand=OperatorGroup";
                var data = await _operatorService.GetObjectsAsync(httpClient, filter);

                if (data != null)
                {
                    if (data.Any())
                    {
                        var currentOperator = data.FirstOrDefault();
                        if (currentOperator != null)
                        {
                            Program.CurrentOperator = currentOperator;
                            string currentUserfilter = $"?$filter=Operator/Code eq '{currentOperator.Code}'&$expand=Operator,Department";
                            Program.CurrentUser = (await _employeeService.GetObjectsAsync(httpClient, currentUserfilter)).FirstOrDefault();
                            result = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}