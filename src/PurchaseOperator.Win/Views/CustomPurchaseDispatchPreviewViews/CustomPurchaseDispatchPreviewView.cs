using DevExpress.Data.Utils;
using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using Microsoft.Extensions.Configuration;
using PurchaseOperator.Application.Services.AuthenticateService;
using PurchaseOperator.Application.Services.ICustomerService;
using PurchaseOperator.Application.Services.ISubUnitsetServices;
using PurchaseOperator.Application.Services.PortalProductServices;
using PurchaseOperator.Application.Services.QCNotificationDetailService;
using PurchaseOperator.Application.Services.QCNotificationService;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.Dtos.QCNotificationDtos;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Win.Helpers;
using PurchaseOperator.Win.ViewModels.ConfirmViewModels;
using PurchaseOperator.Win.ViewModels.CustomPurchaseDispatchPreviewViewModels;
using PurchaseOperator.Win.ViewModels.ProductListViewModels;
using PurchaseOperator.Win.Views.ConfirmViews;
using PurchaseOperator.Win.Views.ProductListViews;
using PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;
using PurchaseOperator.Win.Views.SupplierViews;
using System;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews
{
    public partial class CustomPurchaseDispatchPreviewView : DevExpress.XtraEditors.XtraForm
    {
        public CustomPurchaseDispatchPreviewViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly HttpClient httpClient;

        public CustomPurchaseDispatchPreviewView(CustomPurchaseDispatchPreviewViewModel viewModel, IServiceProvider serviceProvider, IHttpClientFactory httpClientFactory)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            gridControl1.DataSource = _viewModel.Items;
            gridView1.BestFitColumns();
        }

        private void PurchaseDispatchPreviewView_Load(object sender, EventArgs e)
        {
            gridControl1.RefreshDataSource();
            gridView1.BestFitColumns();
            txtSupplierCode.Text = _viewModel.Supplier.Code;
            txtSupplierName.Text = _viewModel.Supplier.Name;
        }

        private async void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            DispatchItem focusedItem = gridView1.GetRow(e.FocusedRowHandle) as DispatchItem;
            if (focusedItem != null)
            {
                txtProductCode.Text = focusedItem.Code;
                txtProductName.Text = focusedItem.Name;
                txtTotalQuantity.Text = focusedItem.TotalQuantity.ToString();
                txtTotalShippedQuantity.Text = focusedItem.TotalShippedQuantity.ToString();
                txtQuantity.Text = focusedItem.CustomQuantity.ToString();

                var portalProduct = await _viewModel.GetProductAsync(focusedItem.ReferenceId);
                if (portalProduct != null)
                {
                    pictureEdit2.EditValue = portalProduct.MainImage;
                }
            }
        }

        private Predicate<DialogResult> predicate = canCloseFunc;

        private static bool canCloseFunc(DialogResult parameter)
        {
            return parameter != DialogResult.Cancel;
        }

        private bool CloseMessage()
        {
            FlyoutAction action = new FlyoutAction()
            {
                Caption = "Uyarı",
                Description = "Form ekranında yaptığınız değişiklikler kaydedilmeden kapatılacaktır."
            };

            Predicate<DialogResult> predicate = canCloseFunc;
            FlyoutCommand command1 = new FlyoutCommand()
            {
                Text = "Kapat",
                Result = DialogResult.Yes
            };
            FlyoutCommand command2 = new FlyoutCommand()
            {
                Text = "Vazgeç",
                Result = DialogResult.No
            };

            action.Commands.Add(command1);
            action.Commands.Add(command2);

            FlyoutProperties properties = new FlyoutProperties
            {
                ButtonSize = new Size(100, 40),
                Style = FlyoutStyle.MessageBox
            };

            return FlyoutDialog.Show(this, action, properties, predicate) == DialogResult.Yes;
        }

        private void ShowWarningMessage(string description)
        {
            FlyoutAction action = new FlyoutAction() { Caption = "Uyarı", Description = description };
            //Predicate<DialogResult> predicate = canCloseFunc;
            FlyoutCommand command1 = new FlyoutCommand() { Text = "Pardon, Unuttum..!", Result = DialogResult.OK };
            action.Commands.Add(command1);
            FlyoutProperties properties = new FlyoutProperties();
            properties.ButtonSize = new Size(100, 40);
            properties.Style = FlyoutStyle.MessageBox;
            if (FlyoutDialog.Show(this, action, properties) == System.Windows.Forms.DialogResult.OK)
                return;
        }

        private DispatchItem ControlShipQuantity()
        {
            DispatchItem dispatchItem = null;

            foreach (var item in _viewModel.Items)
            {
                if (item.CustomQuantity == 0)
                {
                    dispatchItem = item;
                    break;
                }
            }

            return dispatchItem;
        }

        private DispatchItem ControlCountQuantity()
        {
            DispatchItem dispatchItem = null;

            foreach (var item in _viewModel.Items)
            {
                if (item.CountAmount == 0)
                {
                    dispatchItem = item;
                    break;
                }
            }

            return dispatchItem;
        }

        private DispatchItem ControlShipCountQuantity()
        {
            DispatchItem dispatchItem = null;

            foreach (var item in _viewModel.Items)
            {
                if (item.CountAmount > item.CustomQuantity)
                {
                    dispatchItem = item;
                    break;
                }
            }

            return dispatchItem;
        }

        private void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (e.Button.Properties.Caption)
            {
                case "Geri":
                    SupplierListView supplierListView = _serviceProvider.GetService<SupplierListView>(); //(SupplierListView)System.Windows.Forms.Application.OpenForms[nameof(SupplierListView)];
                    supplierListView.Show();
                    this.Close();
                    break;

                case "Tamam":

                    var errorShipQuantityControl = ControlShipQuantity();
                    if (errorShipQuantityControl is not null)
                    {
                        //XtraMessageBox.Show($"{errorShipQuantityControl.Code} kodlu ürün için miktar girişi yapmadınız..");
                        ShowWarningMessage($"{errorShipQuantityControl.Code} kodlu ürün için miktar girişi yapmadınız..");
                        break;
                    }

                    var errorCountQuantityControl = ControlCountQuantity();
                    if (errorCountQuantityControl is not null)
                    {
                        //XtraMessageBox.Show($"{errorCountQuantityControl.Code} kodlu ürün için sayım miktarı girmediniz..");
                        ShowWarningMessage($"{errorCountQuantityControl.Code} kodlu ürün için sayım miktarı girmediniz..");
                        break;
                    }

                    var errorShipCountQuantityControl = ControlShipCountQuantity();
                    if (errorShipCountQuantityControl is not null)
                    {
                        //XtraMessageBox.Show($"{errorCountQuantityControl.Code} kodlu ürün için sayım miktarı girmediniz..");
                        ShowWarningMessage($"{errorShipCountQuantityControl.Code} kodlu ürün için sayım miktarı, giriş miktarından büyük olamaz..");
                        break;
                    }

                    ConfirmViewModel confirmViewModel = _serviceProvider.GetService<ConfirmViewModel>();
                    confirmViewModel.Items.Clear();
                    confirmViewModel.Items.AddRange(_viewModel.Items);
                    confirmViewModel.Supplier = _viewModel.Supplier;
                    confirmViewModel.TargetObjectType = 1;

                    ConfirmView confirmView = _serviceProvider.GetService<ConfirmView>();
                    confirmView.ShowDialog();

                    break;

                case "Ekle":

                    ProductListViewModel productListViewModel = _serviceProvider.GetService<ProductListViewModel>();
                    productListViewModel.Supplier = _viewModel.Supplier;
                    productListViewModel.TargetObjectType = 1;
                    productListViewModel.Items.ForEach(x => x.Quantity = 1);

                    ProductListView productListView = _serviceProvider.GetService<ProductListView>();
                    productListView.ShowDialog();
                    break;

                case "Çıkar":
                    DispatchItem removeItem = gridView1.GetFocusedRow() as DispatchItem;
                    if (removeItem is not null)
                        RemoveObject(removeItem);
                    break;
            }
        }

        private void RemoveObject(DispatchItem removeItem)
        {
            FlyoutAction action = new FlyoutAction() { Caption = "Uyarı", Description = $"{removeItem.Code} kodlu ürün irsaliye listesinden çıkarılacaktır. Devam etmek istiyor musunuz?" };
            //Predicate<DialogResult> predicate = canCloseFunc;
            FlyoutCommand command1 = new FlyoutCommand() { Text = "Evet, Listeden Çıkar..!", Result = DialogResult.Yes };
            FlyoutCommand command2 = new FlyoutCommand() { Text = "Hayır, Kalsın..!", Result = DialogResult.No };
            action.Commands.Add(command1);
            action.Commands.Add(command2);
            FlyoutProperties properties = new FlyoutProperties();
            properties.ButtonSize = new Size(100, 40);
            properties.Style = FlyoutStyle.MessageBox;
            if (FlyoutDialog.Show(this, action, properties) == System.Windows.Forms.DialogResult.Yes)
            {
                _viewModel.Items.Remove(removeItem);
                gridControl1.RefreshDataSource();
            }
            else
                return;
        }

        private void windowsuıButtonPanel1_Click(object sender, EventArgs e)
        {
        }

        private void PurchaseDispatchPreviewView_Activated(object sender, EventArgs e)
        {
        }

        private void btnIncrement_Click(object sender, EventArgs e)
        {
            DispatchItem dispatchItem = gridView1.GetFocusedRow() as DispatchItem;
            if (dispatchItem is not null)
            {
                if (dispatchItem.CustomQuantity >= 1)
                {
                    dispatchItem.CustomQuantity += 1;
                    txtQuantity.Text = dispatchItem.CustomQuantity.ToString();
                }
            }
        }

        private void btnDecrement_Click(object sender, EventArgs e)
        {
            DispatchItem dispatchItem = gridView1.GetFocusedRow() as DispatchItem;
            if (dispatchItem is not null)
            {
                if (dispatchItem.CustomQuantity >= 1)
                {
                    dispatchItem.CustomQuantity -= 1;
                    txtQuantity.Text = dispatchItem.CustomQuantity.ToString();
                }
            }
        }

        private void txtQuantity_EditValueChanged(object sender, EventArgs e)
        {
            //DispatchItem dispatchItem = gridView1.GetFocusedRow() as DispatchItem;
            //if (dispatchItem is not null)
            //{
            //    //XtraMessageBox.Show("Miktar, Bekleyen Miktardan Büyük Olamaz..!");
            //    dispatchItem.CustomQuantity = double.Parse(txtQuantity.Text); ;
            //    txtQuantity.Text = dispatchItem.TotalWaitingQuantity.ToString();
            //}
        }

        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
        {
        }

        private void gridView1_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                DispatchItem dispatchItem = View.GetRow(e.RowHandle) as DispatchItem;
                if (dispatchItem is not null)
                {
                    if (!dispatchItem.Lines.Any() || dispatchItem.Lines is null)
                    {
                        e.Appearance.BackColor = Color.Gold;
                        e.Appearance.BackColor2 = Color.LightYellow;
                        e.HighPriority = true;
                    }
                }
            }
        }

        private void CustomPurchaseDispatchPreviewView_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
        }
    }
}