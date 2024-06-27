using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraRichEdit.Layout.Engine;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Domain.Models.SupplierModels;
using PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchDetailViewModel;
using PurchaseOperator.Win.ViewModels.PurchaseOrderListViewModel;
using PurchaseOperator.Win.ViewModels.SupplierListViewModel;
using PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.MainMenuViews;
using PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;
using PurchaseOperator.Win.Views.PurchaseOrderListViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.SupplierViews;

public partial class SupplierListView : DevExpress.XtraEditors.XtraForm
{
    private readonly SupplierListViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;

    public SupplierListView(SupplierListViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
        gridView1.RowHeight = 50;
        gridView1.BestFitColumns();
        gridControl1.DataSource = _viewModel.Items;
        gridView1.FocusedRowChanged += GridView1_FocusedRowChanged;
    }

    private void GridView1_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
    {

    }

    public async Task LoadDataAsync()
    {
        try
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Tedarikçi Listesi");
            splashScreenManager1.SetWaitFormDescription("Yükleniyor, Lütfen bekleyiniz..");

            await Task.Delay(100);
            await _viewModel.SupplierListAsync();
            gridControl1.RefreshDataSource();
            gridControl1.DataSource = _viewModel.Items;
            gridView1.BestFitColumns();

            splashScreenManager1.CloseWaitForm();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    public async Task ReloadDataAsync()
    {
        try
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Veri Yenileme");
            splashScreenManager1.SetWaitFormDescription("Yeniden yükleniyor, Lütfen bekleyiniz..");

            await Task.Delay(100);
            await _viewModel.ReloadListAsync();
            gridControl1.RefreshDataSource();
            gridControl1.DataSource = _viewModel.Items;
            gridView1.BestFitColumns();

            splashScreenManager1.CloseWaitForm();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
    }

    private async void SupplierListView_Load(object sender, EventArgs e)
    {
        await LoadDataAsync();
    }

    private static bool canCloseFunc(DialogResult parameter)
    {
        return parameter != DialogResult.Cancel;
    }

    private async void windowsuıButtonPanelSupplierList_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        Supplier selectedItem = null;

        switch (e.Button.Properties.Caption)
        {
            case "İleri":

                selectedItem = gridView1.GetFocusedRow() as Supplier;
                if (selectedItem is not null)
                {
                    PurchaseDispatchProductViewModel purchaseDispatchDetailViewModel = _serviceProvider.GetService<PurchaseDispatchProductViewModel>();
                    purchaseDispatchDetailViewModel.Supplier = selectedItem;
                    PurchaseDispatchProductView purchaseDispatchDetailView = new PurchaseDispatchProductView(purchaseDispatchDetailViewModel, _serviceProvider); //_serviceProvider.GetService<PurchaseDispatchDetailView>();
                    this.Hide();
                    purchaseDispatchDetailView.Show();
                }
                else
                    MessageBox.Show("Lütfen tedarikçi seçiniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                break;

            case "Yenile":
                await ReloadDataAsync();
                break;

            case "Özel Kabul":

                selectedItem = gridView1.GetFocusedRow() as Supplier;
                if (selectedItem is not null)
                {
                    CustomPurchaseDispatchPreviewViewModel customPurchaseDispatchPreviewViewModel = _serviceProvider.GetService<CustomPurchaseDispatchPreviewViewModel>();
                    customPurchaseDispatchPreviewViewModel.Supplier = selectedItem;
                    CustomPurchaseDispatchPreviewView customPurchaseDispatchPreviewView = new CustomPurchaseDispatchPreviewView(customPurchaseDispatchPreviewViewModel, _serviceProvider);
                    customPurchaseDispatchPreviewView.ShowDialog();
                }
                else
                    MessageBox.Show("Lütfen tedarikçi seçiniz..", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);


                break;

            case "Geri":
                this.Close();
                var mainMenuView = System.Windows.Forms.Application.OpenForms[nameof(MainMenuView)] as MainMenuView;
                mainMenuView.Show();
                break;
        }
    }

    private Predicate<DialogResult> predicate = canCloseFunc;

    private void SupplierListView_FormClosing(object sender, FormClosingEventArgs e)
    {

    }
}