using DevExpress.XtraEditors;
using PurchaseOperator.Win.ViewModels.LoginViewModel;
using PurchaseOperator.Win.ViewModels.SupplierListViewModel;
using PurchaseOperator.Win.Views.SupplierViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Mvvm;
using Microsoft.Extensions.DependencyInjection;
using PurchaseOperator.Win.Views.MainMenuViews;
using PurchaseOperator.Win.ViewModels.MainMenuViewModels;

namespace PurchaseOperator.Win.Views.LoginView;

public partial class LoginView : DevExpress.XtraEditors.XtraForm
{
    private readonly LoginViewModel _viewModel;
    private IServiceProvider _serviceProvider;

    public LoginView(LoginViewModel viewModel, IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = viewModel;
        _serviceProvider = serviceProvider;
    }

    private async void txtOperatorCode_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            try
            {
                if (splashScreenManager1.IsSplashFormVisible)
                    splashScreenManager1.CloseWaitForm();

                splashScreenManager1.ShowWaitForm();
                splashScreenManager1.SetWaitFormCaption("Oturum Aç");
                splashScreenManager1.SetWaitFormDescription("Bilgileriniz doğrulanıyor,lütfen bekleyiniz...");

                var result = await _viewModel.UserLoginAsync(txtOperatorCode.Text);
                if (result)
                {
                    splashScreenManager1.CloseWaitForm();
                    this.Hide();
                    var mainMenuViewModel = _serviceProvider.GetService<MainMenuViewModel>();
                    var mainMenuView = new MainMenuView(mainMenuViewModel, _serviceProvider);
                    mainMenuView.Show();
                }
                else
                {
                    splashScreenManager1.CloseWaitForm();
                    alertControl1.Show(this, new DevExpress.XtraBars.Alerter.AlertInfo("Uyarı", "Kullanıcı Bilgileri Geçersizdir..."));
                }
            }
            catch (Exception ex)
            {
                if (splashScreenManager1.IsSplashFormVisible)
                    splashScreenManager1.CloseWaitForm();

                alertControl1.Show(this, new DevExpress.XtraBars.Alerter.AlertInfo("Hata", ex.Message));
            }
        }
    }

    private void txtOperatorCode_EditValueChanged(object sender, EventArgs e)
    {
    }

    private void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        switch (e.Button.Properties.Caption)
        {
            case "Kapat":
                this.Close();
                break;
        }
    }

    private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
    {
    }
}