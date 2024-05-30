using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Domain.Models.LogoProductModels;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.ProductListViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.PurchaseDispatchPreviewViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.ProductListViews
{
    public partial class ProductListView : DevExpress.XtraEditors.XtraForm
    {
        private IServiceProvider _serviceProvider;
        private ProductListViewModel _viewModel;
        public List<DispatchItem> SelectedRows { get; } = new();
        public object PurchaseDispatchPreviewView { get; private set; }

        public ProductListView(ProductListViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _serviceProvider = serviceProvider;
            gridControl1.DataSource = _viewModel.Items;
        }

        private async void ProductListView_Load(object sender, EventArgs e)
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("Ürün Listesi");
            splashScreenManager1.SetWaitFormDescription("Yükleniyor,Lütfen bekleyiniz..");

            _viewModel.Items.Clear();
            await _viewModel.GetCustomProductAsync();
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
            splashScreenManager1.CloseWaitForm();
        }

        private void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (e.Button.Properties.Caption)
            {
                case "Kapat":
                    this.Close();
                    break;

                case "Tamam":
                    var items = new List<LogoProduct>();
                    var selectedRows = gridView1.GetSelectedRows();

                    for (int i = 0; i < selectedRows.Length; i++)
                        items.Add(gridView1.GetRow(selectedRows[i]) as LogoProduct);

                    if (items.Any())
                    {
                        switch (_viewModel.TargetObjectType)
                        {
                            case 1:

                                CustomPurchaseDispatchPreviewViewModel customPurchaseDispatchPreviewViewModel = _serviceProvider.GetService<CustomPurchaseDispatchPreviewViewModel>();

                                int customLineNumber = customPurchaseDispatchPreviewViewModel.Items.Count;
                                foreach (LogoProduct item in items)
                                {
                                    customLineNumber++;
                                    if (customPurchaseDispatchPreviewViewModel.Items.Where(x => x.Lines.Count == 0).ToList().Exists(x => x.Code == item.Code))
                                    {
                                        DispatchItem existingItem = customPurchaseDispatchPreviewViewModel.Items.FirstOrDefault(x => x.Code == item.Code && x.Lines.Count == 0);
                                        if (existingItem is not null)
                                        {
                                            existingItem.TotalQuantity = item.Quantity;
                                            existingItem.CustomQuantity = item.Quantity;
                                            existingItem.ManufactureCode = item.ManufactureCode;
                                            existingItem.TotalShippedQuantity = 0;
                                            existingItem.TotalWaitingQuantity = 0;
                                            existingItem.CountAmount = item.Quantity;
                                        }
                                    }
                                    else
                                    {
                                        customPurchaseDispatchPreviewViewModel.Items.Add(new DispatchItem
                                        {
                                            ReferenceId = item.ReferenceId,
                                            Code = item.Code,
                                            Name = item.Name,
                                            SubUnitsetCode = item.SubUnitsetCode,
                                            SubUnitsetReferenceId = item.SubUnitsetReferenceId,
                                            LineNumber = customLineNumber,
                                            TotalQuantity = item.Quantity,
                                            CustomQuantity = item.Quantity,
                                            ManufactureCode = item.ManufactureCode,
                                            TotalShippedQuantity = 0,
                                            TotalWaitingQuantity = 0,
                                            CountAmount = item.Quantity,

                                            Lines = new List<Domain.Models.PurchaseOrderModel.PurchaseOrderLine>()
                                        });
                                    }
                                }

                                CustomPurchaseDispatchPreviewView customPurchaseDispatchPreviewView = System.Windows.Forms.Application.OpenForms[nameof(CustomPurchaseDispatchPreviewView)] as CustomPurchaseDispatchPreviewView;
                                customPurchaseDispatchPreviewView.gridControl1.RefreshDataSource();
                                customPurchaseDispatchPreviewView.gridControl1.Refresh();

                                this.Close();

                                break;

                            case 2:
                                PurchaseDispatchPreviewViewModel purchaseDispatchPreviewViewModel = _serviceProvider.GetService<PurchaseDispatchPreviewViewModel>();

                                int lineNumber = purchaseDispatchPreviewViewModel.Items.Count;
                                foreach (LogoProduct item in items)
                                {
                                    lineNumber++;
                                    if (purchaseDispatchPreviewViewModel.Items.Where(x => x.Lines.Count == 0).ToList().Exists(x => x.Code == item.Code))
                                    {
                                        DispatchItem existingItem = purchaseDispatchPreviewViewModel.Items.FirstOrDefault(x => x.Code == item.Code && x.Lines.Count == 0);
                                        if (existingItem is not null)
                                        {
                                            existingItem.TotalQuantity = item.Quantity;
                                            existingItem.CustomQuantity = item.Quantity;
                                            existingItem.ManufactureCode = item.ManufactureCode;
                                            existingItem.TotalShippedQuantity = 0;
                                            existingItem.TotalWaitingQuantity = 0;
                                            existingItem.CountAmount = item.Quantity;
                                        }
                                    }
                                    else
                                    {
                                        purchaseDispatchPreviewViewModel.Items.Add(new DispatchItem
                                        {
                                            ReferenceId = item.ReferenceId,
                                            Code = item.Code,
                                            Name = item.Name,
                                            SubUnitsetCode = item.SubUnitsetCode,
                                            SubUnitsetReferenceId = item.SubUnitsetReferenceId,
                                            LineNumber = lineNumber,
                                            TotalQuantity = item.Quantity,
                                            CustomQuantity = item.Quantity,
                                            ManufactureCode = item.ManufactureCode,
                                            TotalShippedQuantity = 0,
                                            TotalWaitingQuantity = 0,
                                            CountAmount = item.Quantity,

                                            Lines = new List<Domain.Models.PurchaseOrderModel.PurchaseOrderLine>()
                                        });
                                    }
                                }

                                PurchaseDispatchPreview purchaseDispatchPreview = System.Windows.Forms.Application.OpenForms[nameof(PurchaseDispatchPreview)] as PurchaseDispatchPreview;
                                purchaseDispatchPreview.gridControl1.RefreshDataSource();
                                purchaseDispatchPreview.gridControl1.Refresh();

                                this.Close();
                                break;

                            default:
                                break;
                        }
                    }
                    break;
            }
        }
    }
}