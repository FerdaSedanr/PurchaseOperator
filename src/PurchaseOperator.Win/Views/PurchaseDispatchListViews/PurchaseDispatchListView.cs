using DevExpress.XtraEditors;
using PurchaseOperator.Win.ViewModels.PurchaseDispatchListViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.PurchaseDispatchListViews;

public partial class PurchaseDispatchListView : DevExpress.XtraEditors.XtraForm
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PurchaseDispatchListViewModel _viewModel;
    public PurchaseDispatchListView(IServiceProvider serviceProvider, PurchaseDispatchListViewModel viewModel)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _viewModel = viewModel;
        
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        await Databind();
    }

    private async Task Databind()
    {
        if (_viewModel.Items is not null)
            _viewModel.Items.Clear();

        await _viewModel.LoadDataAsync(dateTimePicker1.Value);
        gridControl1.DataSource = _viewModel.Items;
        gridControl1.RefreshDataSource();
        gridView1.ExpandAllGroups();
        gridView1.BestFitColumns();
    }

    private async void windowsuıButtonPanel1_ButtonClick(object sender, DevExpress.XtraBars.Docking2010.ButtonEventArgs e)
    {
        switch (e.Button.Properties.Caption)
        {
            case "Yenile":
                await Databind();
                break;
            case "Kapat":
                Close();
                break;
            default:
                break;
        }
    }

    private async void btnPrevDate_Click(object sender, EventArgs e)
    {
        dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
        if (_viewModel.Items is not null)
            _viewModel.Items.Clear();

        await _viewModel.LoadDataAsync(dateTimePicker1.Value);
        gridControl1.DataSource = _viewModel.Items;
        gridControl1.RefreshDataSource();
        gridView1.ExpandAllGroups();
        gridView1.BestFitColumns();
    }

    private async void btnNextDate_Click(object sender, EventArgs e)
    {
        dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
        if (_viewModel.Items is not null)
            _viewModel.Items.Clear();

        await _viewModel.LoadDataAsync(dateTimePicker1.Value);
        gridControl1.DataSource = _viewModel.Items;
        gridControl1.RefreshDataSource();
        gridView1.ExpandAllGroups();
        gridView1.BestFitColumns();
    }
}