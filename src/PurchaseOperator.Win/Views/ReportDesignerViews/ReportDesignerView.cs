using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.XtraReports.UserDesigner;
using PurchaseOperator.Domain.Models.PurchaseDispatchTransactionModels;
using PurchaseOperator.Domain.Models.ReportModels;
using PurchaseOperator.Win.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PurchaseOperator.Win.Views.ReportDesignerViews;

public partial class ReportDesignerView : DevExpress.XtraEditors.XtraForm
{
    private List<ReportProductBarcodeModel> Items { get; set; }
    public ReportDesignerView(List<DispatchItem> dispatchItems)
    {
        InitializeComponent();
        Items = dispatchItems.Select(x => new ReportProductBarcodeModel
        {
            Name = x.Name,
            Code = x.Code,
            CustomQuantity = x.CustomQuantity
        }).ToList();
    }

    private void ReportDesignerView_Load(object sender, EventArgs e)
    {
        // Enables form skins in the application (if required).
        DevExpress.Skins.SkinManager.EnableFormSkins();
        DevExpress.LookAndFeel.LookAndFeelHelper.ForceDefaultLookAndFeelChanged();

        // Create an End-User Report Designer form with a ribbon UI.
        XRDesignRibbonForm designForm = new XRDesignRibbonForm();

        ProductBarcode productBarcode = null;

        


        if (File.Exists($"{Environment.CurrentDirectory}/Barcodes/ProductBarcode.repx"))
            productBarcode = XtraReport.FromFile($"{Environment.CurrentDirectory}/Barcodes/ProductBarcode.repxpx", true) as ProductBarcode;
        else
        {
            Directory.CreateDirectory($"{Environment.CurrentDirectory}/Barcodes");
            productBarcode = new ProductBarcode();
        }
           

        // Create a new blank report.
        productBarcode.DataSource = Items;
        designForm.OpenReport(productBarcode);

        // Display the Report Designer form.
        //designForm.Show();

        // Display the Report Designer form, modally.
        designForm.ShowDialog();
    }
}