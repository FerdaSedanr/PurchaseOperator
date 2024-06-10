using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.EmployeeService;
using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Application.Services.ISubUnitsetServices;
using PurchaseOperator.Application.Services.LogoProductService;
using PurchaseOperator.Application.Services.OperatorService;
using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Application.Services.PurchaseDispatchService;
using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Application.Services.PurchaseReturnDispatchService;
using PurchaseOperator.Application.Services.QCNotificationDetailService;
using PurchaseOperator.Application.Services.QCNotificationService;
using PurchaseOperator.Application.Services.SupplierService;
using PurchaseOperator.Domain.Models.EmployeeModels;
using PurchaseOperator.Domain.Models.OperatorModels;
using PurchaseOperator.Infrastructure.DataStores.AuthenticateDataStore;
using PurchaseOperator.Infrastructure.DataStores.CustomerDataStores;
using PurchaseOperator.Infrastructure.DataStores.EmployeeDataStore;
using PurchaseOperator.Infrastructure.DataStores.LogoProductDataStore;
using PurchaseOperator.Infrastructure.DataStores.OperatorDataStore;
using PurchaseOperator.Infrastructure.DataStores.PortalProductDataStore;
using PurchaseOperator.Infrastructure.DataStores.PurchaseDispatchDataStore;
using PurchaseOperator.Infrastructure.DataStores.PurchaseOrderDataStore;
using PurchaseOperator.Infrastructure.DataStores.PurchaseReturnDispatchDataStores;
using PurchaseOperator.Infrastructure.DataStores.QCNotificationDataStores;
using PurchaseOperator.Infrastructure.DataStores.QCNotificationDetailDataStores;
using PurchaseOperator.Infrastructure.DataStores.SubUnitsetDataStore;
using PurchaseOperator.Infrastructure.DataStores.SupplierDataStore;
using PurchaseOperator.Win.ViewModels.ConfirmViewModels;
using PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.LoginViewModel;
using PurchaseOperator.Win.ViewModels.MainMenuViewModels;
using PurchaseOperator.Win.ViewModels.ProductListViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchDetailViewModel;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchListViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseOrderListViewModel;
using PurchaseOperator.Win.ViewModels.SupplierListViewModel;
using PurchaseOperator.Win.Views.ConfirmViews;
using PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.LoginView;
using PurchaseOperator.Win.Views.ProductListViews;
using PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;
using PurchaseOperator.Win.Views.PurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.PurchaseOrderListViews;
using PurchaseOperator.Win.Views.SupplierViews;
using System;

namespace PurchaseOperator.Win;

internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
        System.Windows.Forms.Application.EnableVisualStyles();
        System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
        var host = CreateHostBuilder().Build();
        ServiceProvider = host.Services;
        System.Windows.Forms.Application.Run(ServiceProvider.GetRequiredService<LoginView>());
    }

    public static IServiceProvider ServiceProvider { get; private set; }

    private static IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder().ConfigureServices((context, services) =>
        {
            //services.AddSingleton<ICustomerService<CustomerModel>, CustomerDataStore>();
            services.AddLogging();
            services.AddHttpClient("Portal", v =>
            {
                v.BaseAddress = new Uri("http://172.16.1.3:1096");
            });
            services.AddHttpClient("LBS", s =>
            {
                s.BaseAddress = new Uri("http://172.16.1.25:52789");
            });

            services.AddTransient<IAuthenticatePortalService, AuthenticatePortalDataStore>();
            services.AddTransient<IAuthenticateLBSService, AuthenticateLBSDataStore>();
            services.AddTransient<IOperatorService, OperatorDataStore>();
            services.AddTransient<IEmployeeService, EmployeeDataStore>();
            services.AddTransient<ISupplierService, CustomSupplierDataStore>();
            services.AddTransient<IPurchaseOrderService, PurchaseOrderDataStoreV2>();
            services.AddTransient<IPortalProductService, PortalProductDataStore>();
            services.AddTransient<IPurchaseDispatchService, PurchaseDispatchDataStore>();
            services.AddTransient<IPortalProductService, PortalProductDataStore>();
            services.AddTransient<ICustomerService, CustomerDataStore>();
            services.AddTransient<IQCNotificationService, QCNotificationDataStore>();
            services.AddTransient<IQCNotificationDetailService, QCNotificationDetailDataStore>();
            services.AddTransient<ISubUnitsetService, SubUnitsetDataStore>();
            services.AddTransient<ILogoProductService, LogoProductDataStore>();
            services.AddTransient<IPurchaseDispatchLineService, PurchaseDispatchLineDataStore>();
            services.AddTransient<IPurchaseReturnDispatchService, PurchaseReturnDispatchDataStore>();

            services.AddTransient<LoginViewModel>();
            services.AddTransient<LoginView>();
            services.AddTransient<MainMenuViewModel>();
            services.AddTransient<PurchaseDispatchListViewModel>();
            services.AddTransient<SupplierListViewModel>();
            services.AddTransient<SupplierListView>();
            services.AddTransient<PurchaseOrderListViewModel>();
            services.AddTransient<PurchaseOrderListView>();
            services.AddTransient<PurchaseDispatchProductViewModel>();
            //services.AddScoped<PurchaseDispatchProductView>();
            services.AddTransient<PurchaseDispatchPreviewViewModel>();
            services.AddTransient<PurchaseDispatchPreview>();
            services.AddTransient<CustomPurchaseDispatchPreviewViewModel>();
            services.AddTransient<CustomPurchaseDispatchPreviewView>();
            services.AddTransient<ConfirmViewModel>();
            services.AddTransient<ConfirmView>();
            services.AddTransient<ProductListViewModel>();
            services.AddTransient<ProductListView>();
        });
    }

    public static Employee CurrentUser { get; set; }
    public static Operator CurrentOperator { get; set; }
}