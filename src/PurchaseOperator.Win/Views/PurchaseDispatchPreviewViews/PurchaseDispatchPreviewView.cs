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
using PurchaseOperator.Win.ViewModels.ProductListViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchPreviewViewModels;
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
using static DevExpress.DataProcessing.InMemoryDataProcessor.AddSurrogateOperationAlgorithm;

namespace PurchaseOperator.Win.Views.PurchaseDispatchPreviewViews
{
    public partial class PurchaseDispatchPreview : DevExpress.XtraEditors.XtraForm
    {
        public PurchaseDispatchPreviewViewModel _viewModel;
        private readonly IServiceProvider _serviceProvider;

        //private IAuthenticatePortalService _authenticatePortalService;        
        //private readonly IConfiguration _configuration;
        //private readonly IPortalProductService _portalProductService;
        //private readonly ICustomerService _customerService;
        //private IHttpClientFactory _httpClientFactory;
        //private IQCNotificationService _notificationService;
        //private IQCNotificationDetailService _notificationDetailService;
        //private ISubUnitsetService _subUnitsetService;
        //private HttpClient httpClient;

        public PurchaseDispatchPreview(PurchaseDispatchPreviewViewModel viewModel, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = viewModel;
            gridControl1.DataSource = _viewModel.Items;
            _serviceProvider = serviceProvider;
            //_configuration = configuration;
            //_portalProductService = portalProductService;
            //_customerService = customerService;
            //_authenticatePortalService = authenticatePortalService;
            //_notificationService = qCNotificationService;
            //_notificationDetailService = qCNotificationDetailService;
            //_subUnitsetService = subUnitsetService;
            //_httpClientFactory = httpClientFactory;
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
                txtDemandQuantity.Text = focusedItem?.DemandQuantity.ToString();
                txtWaitingQuantity.Text = focusedItem.TotalWaitingQuantity.ToString();

                var portalProduct = await _viewModel.GetProductAsync(focusedItem.ReferenceId);

                if (portalProduct != null)
                {
                    pictureEdit2.EditValue = portalProduct.MainImage;
                }
            }
        }

        private async void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
        {
            switch (e.Button.Properties.Caption)
            {
                case "Geri":

                    FlyoutAction action = new FlyoutAction() { Caption = "Uyarı", Description = "Form ekranında yaptığınız değişiklikler kaydedilmeden kapatılacaktır. " };
                    // Predicate<DialogResult> predicate = canCloseFunc;
                    FlyoutCommand command1 = new FlyoutCommand() { Text = "Kapat", Result = DialogResult.Yes };
                    FlyoutCommand command2 = new FlyoutCommand() { Text = "Vazgeç", Result = System.Windows.Forms.DialogResult.No };
                    action.Commands.Add(command1);
                    action.Commands.Add(command2);
                    FlyoutProperties properties = new FlyoutProperties();
                    properties.ButtonSize = new Size(100, 40);
                    properties.Style = FlyoutStyle.MessageBox;
                    if (FlyoutDialog.Show(this, action, properties) == System.Windows.Forms.DialogResult.Yes)
                    {
                        PurchaseDispatchProductView purchaseDispatchDetailView = (PurchaseDispatchProductView)System.Windows.Forms.Application.OpenForms[nameof(PurchaseDispatchProductView)];

                        var selectedRows = ((GridView)purchaseDispatchDetailView.gridControl1.Views[0]).GetSelectedRows();

                        for (int i = 0; i < selectedRows.Length; i++)
                        {
                            DispatchItem item = ((GridView)purchaseDispatchDetailView.gridControl1.Views[0]).GetRow(selectedRows[i]) as DispatchItem;
                            item.LineNumber = 0;

                            ((GridView)purchaseDispatchDetailView.gridControl1.Views[0]).UnselectRow(selectedRows[i]);
                        }

                        purchaseDispatchDetailView.gridControl1.RefreshDataSource();
                        this.Close();
                        purchaseDispatchDetailView.Show();
                    }
                    else
                    {
                        return;
                    }
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
                    confirmViewModel.TargetObjectType = 2;
                    ConfirmView confirmView = new ConfirmView(confirmViewModel); //_serviceProvider.GetService<ConfirmView>();
                    confirmView.ShowDialog();

                    break;

                case "Ekle":
                    ProductListViewModel productListViewModel = _serviceProvider.GetService<ProductListViewModel>();
                    productListViewModel.Supplier = _viewModel.Supplier;
                    productListViewModel.TargetObjectType = 2;
                    productListViewModel.Items.ForEach(x => x.Quantity = 1);

                    ProductListView productListView = new ProductListView(productListViewModel, _serviceProvider); // _serviceProvider.GetService<ProductListView>();
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

        private void windowsuıButtonPanel1_Click(object sender, EventArgs e)
        {
        }

        private void PurchaseDispatchPreviewView_Activated(object sender, EventArgs e)
        {
            //await AuthenticateAsync();
        }

        [Obsolete]
        private async Task CreateDispatch()
        {
            //PurchaseDispatchTransactionDto dto = new();
            //dto.WarehouseNumber = 1;
            //dto.CurrentCode = _viewModel.Supplier.Code;
            //var owner = Guid.NewGuid();
            //dto.Owner = owner;
            //dto.TransactionType = 1;
            //dto.IOType = 1;
            //dto.GroupType = 1;
            //dto.IsEDispatch = _viewModel.Supplier.DispatchType;

            //foreach (var item in _viewModel.Items)
            //{
            //    var remainingQuantity = item.CustomQuantity;
            //    foreach (var line in item.Lines.OrderBy(x => x.Date))
            //    {
            //        if (remainingQuantity > 0)
            //        {
            //            if ((remainingQuantity - line.WaitingQuantity) < 0)
            //            {
            //                var lineDto = new PurchaseDispatchTransactionLineDto()
            //                {
            //                    WarehouseNumber = int.Parse(_configuration.GetSection("DefaultERP")["Warehouse"]),
            //                    ProductCode = line.ProductCode,
            //                    UnitsetCode = line.SubUnitsetCode,
            //                    SubUnitsetReferenceId = line.SubUnitsetReferenceId,
            //                    SubUnitsetCode = line.SubUnitsetCode,
            //                    CurrentCode = line.CurrentCode,
            //                    IOType = 1,
            //                    Description = line.Description,
            //                    Quantity = remainingQuantity,
            //                    ConversionFactor = remainingQuantity,
            //                    OtherConversionFactor = remainingQuantity,
            //                    TransactionDate = DateTime.Now,
            //                    UnitPrice = line.UnitPrice,
            //                    TransactionType = 1,
            //                    VatRate = line.VatRate,
            //                    OrderReferenceId = line.ReferenceId
            //                };
            //                lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(
            //                    StockLocationCode : _configuration.GetSection("DefaultERP")["Location"],
            //                    DestinationStockLocationCode : _configuration.GetSection("DefaultERP")["Location"],
            //                    InProductTransactionLineReferenceId : 0,
            //                    OutProductTransactionLineReferenceId : 0,
            //                    SerilotType:0,
            //                    Quantity:remainingQuantity,
            //                    SubUnitsetCode:line.SubUnitsetCode,
            //                    ConversionFactor:1,
            //                    OtherConversionFactor:1));

            //                dto.Lines.Add(lineDto);
            //                remainingQuantity = 0;
            //            }
            //            else
            //            {
            //                var lineDto = new PurchaseDispatchTransactionLineDto()
            //                {
            //                    WarehouseNumber = int.Parse(_configuration.GetSection("DefaultERP")["Warehouse"]),
            //                    ProductCode = line.ProductCode,
            //                    UnitsetCode = line.SubUnitsetCode,
            //                    SubUnitsetReferenceId = line.SubUnitsetReferenceId,
            //                    SubUnitsetCode = line.SubUnitsetCode,
            //                    CurrentCode = line.CurrentCode,
            //                    IOType = 1,
            //                    Description = line.Description,
            //                    //remaninig alanını ekledim
            //                    Quantity = line.Quantity - line.ShippedQuantity,
            //                    ConversionFactor = line.Quantity - line.ShippedQuantity,
            //                    OtherConversionFactor = line.Quantity - line.ShippedQuantity,
            //                    TransactionDate = DateTime.Now,
            //                    UnitPrice = line.UnitPrice,
            //                    TransactionType = 1,
            //                    VatRate = line.VatRate,
            //                    OrderReferenceId = line.ReferenceId
            //                };

            //                lineDto.SeriLotTransactions.Add(new SeriLotTransactionDto(                            
            //                    StockLocationCode : _configuration.GetSection("DefaultERP")["Location"],
            //                    DestinationStockLocationCode : string.Empty,
            //                    InProductTransactionLineReferenceId : 0,
            //                    OutProductTransactionLineReferenceId : 0,
            //                    SerilotType:0,
            //                    Quantity:Convert.ToDouble(line.Quantity - line.ShippedQuantity),
            //                    SubUnitsetCode:line.SubUnitsetCode,
            //                    ConversionFactor:1,
            //                    OtherConversionFactor:1));

            //                dto.Lines.Add(lineDto);
            //                remainingQuantity -= (double)line.Quantity;
            //            }
            //        }
            //    }
            //}

            //await _viewModel.CreatePurchaseDispatch(dto);

            //string dispatchNumber = string.Empty;
            ////var data = _notifyService get Notification
            //var httpClientService = _httpClientFactory.CreateClient("LBS");
            //var data = await new NotificationResultHelper<PurchaseDispatchTransactionDto>().GetNotification(httpClientService, dto.Owner);
            //if (data.IsSuccess)
            //{
            //    dispatchNumber = data.Data.Code;
            //    string customerFilter = $"?$filter=ReferenceId eq {_viewModel.Supplier.ReferenceId}";
            //    var customer = await _customerService.GetObjectsAsync(httpClient, customerFilter);
            //    if (customer is not null)
            //    {
            //        var qcNotificationResult = await _notificationService.InsertObjectAsync(httpClient, new QCNotificationDto(DateTime.Now, dispatchNumber, 0, customer.FirstOrDefault().Oid));
            //        if (qcNotificationResult is not null)
            //        {
            //            foreach (var item in dto.Lines)
            //            {
            //                var product = await _portalProductService.GetObjectsAsync(httpClient, $"?$filter=Code eq '{item.ProductCode}'");
            //                var subUnitset = await _subUnitsetService.GetObjectsAsync(httpClient, $"?$filter=ReferenceId eq {item.SubUnitsetReferenceId}");
            //                if (product.Any() && subUnitset.Any())
            //                {
            //                    QCNotificationDetailDto detailDto = new QCNotificationDetailDto(Guid.Parse(qcNotificationResult.Oid), product.FirstOrDefault().Oid, subUnitset.FirstOrDefault().Oid, (double)item.Quantity);
            //                    await _notificationDetailService.InsertObjectAsync(httpClient, detailDto);
            //                }
            //            }

            //            this.Close();
            //            SupplierListView supplierListView = System.Windows.Forms.Application.OpenForms[nameof(SupplierListView)] as SupplierListView;
            //            if (supplierListView is not null)
            //            {
            //                PurchaseDispatchProductView purchaseDispatchDetailView = System.Windows.Forms.Application.OpenForms[nameof(PurchaseDispatchProductView)] as PurchaseDispatchProductView;
            //                if (purchaseDispatchDetailView is not null)
            //                    purchaseDispatchDetailView.Close();

            //                supplierListView.Show();
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    XtraMessageBox.Show(data.Message);
            //}
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

        private void gridView1_RowStyle(object sender, RowStyleEventArgs e)
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

        private void PurchaseDispatchPreview_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FlyoutAction action = new FlyoutAction() { Caption = "Uyarı", Description = "Form ekranında yaptığınız değişiklikler kaydedilmeden kapatılacaktır. " };
            //Predicate<DialogResult> predicate = canCloseFunc;
            //FlyoutCommand command1 = new FlyoutCommand() { Text = "Kapat", Result = DialogResult.Yes };
            //FlyoutCommand command2 = new FlyoutCommand() { Text = "Vazgeç", Result = System.Windows.Forms.DialogResult.No };
            //action.Commands.Add(command1);
            //action.Commands.Add(command2);
            //FlyoutProperties properties = new FlyoutProperties();
            //properties.ButtonSize = new Size(100, 40);
            //properties.Style = FlyoutStyle.MessageBox;
            //if (FlyoutDialog.Show(this, action, properties, predicate) == System.Windows.Forms.DialogResult.Yes) e.Cancel = false;
            //else e.Cancel = true;
        }
    }
}