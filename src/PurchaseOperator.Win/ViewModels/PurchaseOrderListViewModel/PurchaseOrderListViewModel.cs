using DevExpress.XtraScheduler.Utils;
using Microsoft.Extensions.Logging;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.PurchaseOrderService;
using PurchaseOperator.Application.Services.SupplierService;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PurchaseOperator.Win.ViewModels.PurchaseOrderListViewModel;

public class PurchaseOrderListViewModel
{
    public List<PurchaseOrderLine> Items { get; set; } = new();

    public PurchaseOrderListViewModel()
    {
    }
}