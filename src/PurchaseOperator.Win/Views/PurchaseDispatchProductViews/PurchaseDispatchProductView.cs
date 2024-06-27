using DevExpress.Data;
using DevExpress.DirectX.NativeInterop.DXGI;
using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.PurchaseOrderModel;
using PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.ProductListViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchDetailViewModel;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseOrderListViewModel;
using PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.ProductListViews;
using PurchaseOperator.Win.Views.PurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.PurchaseOrderListViews;
using PurchaseOperator.Win.Views.SupplierViews;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;

public partial class PurchaseDispatchProductView : DevExpress.XtraEditors.XtraForm
{
    private PurchaseDispatchProductViewModel _viewModel;
    private readonly IServiceProvider _serviceProvider;
    public List<DispatchItem> SelectedRows { get; } = new();
    private int count = 0;

    public PurchaseDispatchProductView(PurchaseDispatchProductViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
        gridControl1.DataSource = _viewModel.Items;
        lblSupplierName.Text = _viewModel.Supplier.Name;
        //gridView1.SelectionChanged += gridView1_SelectionChanged;
    }

    protected async override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        if (splashScreenManager1.IsSplashFormVisible)
            splashScreenManager1.CloseWaitForm();

        splashScreenManager1.ShowWaitForm();
        splashScreenManager1.SetWaitFormCaption("Ürünler Listesi");
        splashScreenManager1.SetWaitFormDescription("Yükleniyor, Lütfen bekleyiniz..");

        await Task.Delay(1000);
        await _viewModel.LoadDataAsync();
        gridControl1.RefreshDataSource();
        gridView1.BestFitColumns();
        splashScreenManager1.CloseWaitForm();
    }

    private async void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        switch (e.Button.Properties.Caption)
        {
            case "Geri":
                SupplierListView supplierListView = _serviceProvider.GetService<SupplierListView>(); //(SupplierListView)System.Windows.Forms.Application.OpenForms[nameof(SupplierListView)];
                supplierListView.Show();
                this.Close();

                break;

            case "Siparişler":
                if (gridView1.SelectedRowsCount > 1)
                {
                    XtraMessageBox.Show("Birden fazla ürün seçimi yapamazsınız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DispatchItem selectedItem = gridView1.GetFocusedRow() as DispatchItem;
                    if (selectedItem is not null)
                    {
                        PurchaseOrderListViewModel purchaseOrderListViewModel = _serviceProvider.GetService<PurchaseOrderListViewModel>();
                        purchaseOrderListViewModel.Items.AddRange(selectedItem.Lines);

                        PurchaseOrderListView purchaseOrderListView = new PurchaseOrderListView(purchaseOrderListViewModel, _serviceProvider);
                        purchaseOrderListView.ShowDialog();
                    }
                }
                

                break;

            case "Aşağı":
                gridView1.MoveNext();
                break;

            case "Yukarı":
                gridView1.MovePrev();
                break;

            case "Yenile":
                await LoadDataAsync();
                break;

            case "İleri":

                List<DispatchItem> items = new List<DispatchItem>();
                var selectedRows = gridView1.GetSelectedRows();
                for (int i = 0; i < selectedRows.Length; i++)
                {
                    DispatchItem dispatchItem = gridView1.GetRow(selectedRows[i]) as DispatchItem;
                    items.Add(dispatchItem);
                }

                if (items.Any())
                {
                    PurchaseDispatchPreviewViewModel purchaseDispatchPreviewViewModel = _serviceProvider.GetService<PurchaseDispatchPreviewViewModel>();
                    purchaseDispatchPreviewViewModel.Items.Clear();

                    foreach (var item in items)
                        purchaseDispatchPreviewViewModel.Items.Add(item);


                    purchaseDispatchPreviewViewModel.Supplier = _viewModel.Supplier;
                    this.Hide();
                    PurchaseDispatchPreview purchaseDispatchPreviewView = new PurchaseDispatchPreview(purchaseDispatchPreviewViewModel, _serviceProvider); //_serviceProvider.GetService<PurchaseDispatchPreview>();
                    purchaseDispatchPreviewView.Show();
                }

                break;

            case "Kapat":
                this.Close();
                break;
        }
    }

    private void PurchaseDispatchDetailView_Load(object sender, EventArgs e)
    {
        //if (splashScreenManager1.IsSplashFormVisible)
        //    splashScreenManager1.CloseWaitForm();

        //splashScreenManager1.ShowWaitForm();
        //splashScreenManager1.SetWaitFormCaption("Ürünler Listesi");
        //splashScreenManager1.SetWaitFormDescription("Yükleniyor, Lütfen bekleyiniz..");

        //await Task.Delay(1000);
        //await _viewModel.LoadDataAsync();
        //gridControl1.RefreshDataSource();
        //gridView1.BestFitColumns();
        //splashScreenManager1.CloseWaitForm();
    }

    public async Task LoadDataAsync()
    {
        try
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Veri Yenileme");
            splashScreenManager1.SetWaitFormDescription("Veriler yenileniyor,lütfen bekleyiniz..");

            await Task.Delay(1000);
            await _viewModel.ReloadDataAsync();
            gridControl1.RefreshDataSource();

            splashScreenManager1.CloseWaitForm();
        }
        catch (Exception ex)
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            XtraMessageBox.Show(ex.Message, "Tedarikçi listesi Load Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PurchaseDispatchDetailView_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
    {
    }

    private void gridView1_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        DispatchItem selectedItem = gridView1.GetFocusedRow() as DispatchItem;
        if (selectedItem != null)
        {
            switch (e.Action)
            {
                case CollectionChangeAction.Add:
                    selectedItem.LineNumber = gridView1.GetSelectedRows().Count();
                    gridControl1.RefreshDataSource();
                    break;

                case CollectionChangeAction.Remove:
                    selectedItem.LineNumber = 0;
                    gridControl1.RefreshDataSource();
                    break;

                case CollectionChangeAction.Refresh:
                    break;

                default:
                    break;
            }
        }
    }
}