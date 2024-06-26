using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchDetailViewModel;
using PurchaseOperator.Win.ViewModels.PurchaseOrderListViewModel;
using PurchaseOperator.Win.ViewModels.SupplierListViewModel;
using PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;
using PurchaseOperator.Win.Views.SupplierViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.PurchaseOrderListViews;

public partial class PurchaseOrderListView : DevExpress.XtraEditors.XtraForm
{
    private PurchaseOrderListViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public PurchaseOrderListView(PurchaseOrderListViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    public async Task LoadDataAsync()
    {
        try
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Ürün Listesi");
            splashScreenManager1.SetWaitFormDescription("Yükleniyor,Lütfen bekleyiniz..");

            //gridView1.RowHeight = 50;
            
            gridControl1.DataSource = _viewModel.Items;
            gridView1.BestFitColumns();
            gridControl1.RefreshDataSource();

            splashScreenManager1.CloseWaitForm();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void PurchaseOrderListView_Load(object sender, EventArgs e)
    {
        
        await LoadDataAsync();
    }

    private void windowsuıButtonPanelPOList_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        switch (e.Button.Properties.Caption)
        {
            case "Kapat":
                this.Close();
                break;
        }
    }

    private void labelControl1_Click(object sender, EventArgs e)
    {
    }

    private void gridControl1_Click(object sender, EventArgs e)
    {
    }
}