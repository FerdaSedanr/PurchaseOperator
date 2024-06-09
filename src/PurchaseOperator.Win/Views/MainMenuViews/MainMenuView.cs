using DevExpress.XtraBars.Docking2010.Customization;
using DevExpress.XtraBars.Docking2010.Views.WindowsUI;
using DevExpress.XtraEditors;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Win.ViewModels.MainMenuViewModels;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchListViewModels;
using PurchaseOperator.Win.ViewModels.SupplierListViewModel;
using PurchaseOperator.Win.Views.PurchaseDispatchListViews;
using PurchaseOperator.Win.Views.SupplierViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.MainMenuViews;

public partial class MainMenuView : DevExpress.XtraEditors.XtraForm
{
    private readonly IServiceProvider _serviceProvider;
    private readonly MainMenuViewModel _viewModel;
    public MainMenuView(MainMenuViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    private void tileControl1_ItemClick(object sender, TileItemEventArgs e)
    {
        switch (e.Item.Text)
        {
            case "İrsaliyeler":
                var purchaseDispatchListViewModel = _serviceProvider.GetService<PurchaseDispatchListViewModel>();
                PurchaseDispatchListView purchaseDispatchListView = new PurchaseDispatchListView(_serviceProvider, purchaseDispatchListViewModel);
                purchaseDispatchListView.ShowDialog();
                break;
            case "Mal Kabul":
                this.Hide();
                var supplierListViewModel = _serviceProvider.GetService<SupplierListViewModel>();
                SupplierListView supplierListView = new SupplierListView(supplierListViewModel, _serviceProvider);
                supplierListView.Show();
                break;
            default:
                break;
        }
    }

    private void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        switch (e.Button.Properties.Caption)
        {
            case "Kapat":
                FlyoutAction action = new FlyoutAction() { Caption = "Uyarı", Description = "Uygulama Kapatılacaktır. Devam etmek istiyor musunuz?" };
                //Predicate<DialogResult> predicate = canCloseFunc;
                FlyoutCommand command1 = new FlyoutCommand() { Text = "Uygulamayı Kapat", Result = DialogResult.Yes };
                FlyoutCommand command2 = new FlyoutCommand() { Text = "Vazgeç", Result = System.Windows.Forms.DialogResult.No };
                action.Commands.Add(command1);
                action.Commands.Add(command2);
                FlyoutProperties properties = new FlyoutProperties();
                properties.ButtonSize = new Size(100, 40);
                properties.Style = FlyoutStyle.MessageBox;
                if (FlyoutDialog.Show(this, action, properties) == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Windows.Forms.Application.Exit();
                }
                else return;
                break;
            default:
                break;
        }
    }
}