using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using PurchaseOperator.Domain.Dtos;
using PurchaseOperator.Domain.ResponseModels;
using PurchaseOperator.Win.ViewModels.ConfirmViewModels;
using PurchaseOperator.Win.Views.CustomPurchaseDispatchPreviewViews;
using PurchaseOperator.Win.Views.PurchaseDispatchDetailViews;
using PurchaseOperator.Win.Views.PurchaseDispatchPreviewViews;
using System;
using System.Threading;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.ConfirmViews;

public partial class ConfirmView : DevExpress.XtraEditors.XtraForm
{
    private readonly ConfirmViewModel _viewModel;

    public ConfirmView(ConfirmViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        dateEdit1.EditValue = DateTime.Now;
    }

    private void ConfirmView_FormClosing(object sender, FormClosingEventArgs e)
    {
    }

    private async void btnOk_Click(object sender, EventArgs e)
    {
        FlyoutAction action = new FlyoutAction() { Caption = "Mal Kabul İrsaliyesi", Description = "Mal Kabul İrsaliyesi Oluşturulacaktır. Devam etmek istiyor musunuz?" };
        FlyoutCommand command1 = new FlyoutCommand() { Text = "İrsaliye Kabul Et", Result = DialogResult.Yes };
        FlyoutCommand command2 = new FlyoutCommand() { Text = "Vazgeç", Result = System.Windows.Forms.DialogResult.No };
        action.Commands.Add(command1);
        action.Commands.Add(command2);
        FlyoutProperties properties = new FlyoutProperties();
        properties.ButtonSize = new System.Drawing.Size(100, 40);
        properties.Style = FlyoutStyle.MessageBox;
        if (FlyoutDialog.Show(this, action, properties) == DialogResult.Yes)
        {
            if (splashScreenManager1.IsSplashFormVisible)
                splashScreenManager1.CloseWaitForm();

            splashScreenManager1.ShowWaitForm();
            splashScreenManager1.SetWaitFormCaption("");
            splashScreenManager1.SetWaitFormDescription("Yükleniyor,Lütfen bekleyiniz..");

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            PurchaseDispatchTransactionDto dto;

            dto = _viewModel.TargetObjectType == 2 ? await _viewModel.CreateDispatchDTOWithOrderAsync(_viewModel.Items, textEdit1.Text, (DateTime)dateEdit1.EditValue).WaitAsync(cancellationTokenSource.Token) : await _viewModel.CreateDispatchDTOAsync(_viewModel.Items, textEdit1.Text, (DateTime)dateEdit1.EditValue).WaitAsync(cancellationTokenSource.Token);
            if (dto is not null)
            {
                var dataResult = await _viewModel.InsertPurchaseDispatchAsync(dto).WaitAsync(cancellationTokenSource.Token);
                if (dataResult.IsSuccess)
                {
                    //Fazla ürünler varsa
                    if (_viewModel.ExcessItems.Count > 0)
                    {
                        var excessDTO = await _viewModel.CreateDispatchDTOAsync(_viewModel.ExcessItems, textEdit1.Text, (DateTime)dateEdit1.EditValue).WaitAsync(cancellationTokenSource.Token);
                        await _viewModel.InsertPurchaseDispatchAsync(excessDTO).WaitAsync(cancellationTokenSource.Token);
                        _viewModel.ExcessItems.Clear();
                    }

                    //İade ürünleri varsa
                    if (_viewModel.ReturnItems.Count > 0)
                    {
                        await _viewModel.InsertPurchaseReturnDispatch(_viewModel.ReturnItems);
                        _viewModel.ReturnItems.Clear();
                    }

                    await _viewModel.InsertQCNotificationAsync(dataResult.Data.Code, dataResult.Data.ReferenceId, dto);
                }
                else
                    XtraMessageBox.Show(dataResult.Message);
            }

            splashScreenManager1.CloseWaitForm();
            this.Close();
            if (_viewModel.TargetObjectType == 2)
            {
                PurchaseDispatchPreview purchaseDispatchPreview = System.Windows.Forms.Application.OpenForms[nameof(PurchaseDispatchPreview)] as PurchaseDispatchPreview;
                if (purchaseDispatchPreview is not null)
                {
                    purchaseDispatchPreview.Close();

                    PurchaseDispatchProductView purchaseDispatchDetailView = System.Windows.Forms.Application.OpenForms[nameof(PurchaseDispatchProductView)] as PurchaseDispatchProductView;
                    if (purchaseDispatchDetailView is not null)
                    {
                        await purchaseDispatchDetailView.LoadDataAsync();
                        purchaseDispatchDetailView.gridControl1.RefreshDataSource();
                        purchaseDispatchDetailView.Show();
                    }
                }
            }
            else
            {
                CustomPurchaseDispatchPreviewView customPurchaseDispatchPreviewView = System.Windows.Forms.Application.OpenForms[nameof(CustomPurchaseDispatchPreviewView)] as CustomPurchaseDispatchPreviewView;
                if (customPurchaseDispatchPreviewView is not null)
                {
                    customPurchaseDispatchPreviewView.Close();
                }
            }
        }
        else { return; }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}