﻿namespace PurchaseOperator.Win.Views.PurchaseOrderListViews
{
    partial class PurchaseOrderListView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PurchaseOrderListView));
            DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions windowsuıButtonImageOptions2 = new DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            gridControl1 = new DevExpress.XtraGrid.GridControl();
            gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            windowsuıButtonPanelPOList = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(WaitForm1), true, true);
            gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(pictureEdit1, 0, 0);
            tableLayoutPanel1.Controls.Add(gridControl1, 0, 1);
            tableLayoutPanel1.Controls.Add(windowsuıButtonPanelPOList, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1445, 734);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pictureEdit1
            // 
            pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureEdit1.EditValue = resources.GetObject("pictureEdit1.EditValue");
            pictureEdit1.Location = new System.Drawing.Point(3, 2);
            pictureEdit1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            pictureEdit1.Name = "pictureEdit1";
            pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pictureEdit1.Size = new System.Drawing.Size(355, 106);
            pictureEdit1.TabIndex = 0;
            // 
            // gridControl1
            // 
            tableLayoutPanel1.SetColumnSpan(gridControl1, 3);
            gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            gridControl1.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            gridControl1.Location = new System.Drawing.Point(3, 112);
            gridControl1.MainView = gridView1;
            gridControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            gridControl1.Name = "gridControl1";
            gridControl1.Size = new System.Drawing.Size(1439, 509);
            gridControl1.TabIndex = 1;
            gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] { gridView1 });
            gridControl1.Click += gridControl1_Click;
            // 
            // gridView1
            // 
            gridView1.Appearance.FocusedRow.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            gridView1.Appearance.FocusedRow.Options.UseFont = true;
            gridView1.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            gridView1.Appearance.HeaderPanel.Options.UseFont = true;
            gridView1.Appearance.Row.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            gridView1.Appearance.Row.Options.UseFont = true;
            gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] { gridColumn2, gridColumn3, gridColumn5, gridColumn4, gridColumn6, gridColumn1, gridColumn7, gridColumn8 });
            gridView1.DetailHeight = 284;
            gridView1.GridControl = gridControl1;
            gridView1.Name = "gridView1";
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsBehavior.ReadOnly = true;
            gridView1.OptionsSelection.MultiSelect = true;
            gridView1.OptionsView.HeaderFilterButtonShowMode = DevExpress.XtraEditors.Controls.FilterButtonShowMode.Button;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.OptionsView.ShowGroupPanel = false;
            // 
            // gridColumn2
            // 
            gridColumn2.Caption = "Adı";
            gridColumn2.FieldName = "ProductName";
            gridColumn2.MinWidth = 21;
            gridColumn2.Name = "gridColumn2";
            gridColumn2.Visible = true;
            gridColumn2.VisibleIndex = 3;
            gridColumn2.Width = 179;
            // 
            // gridColumn3
            // 
            gridColumn3.Caption = "Miktar";
            gridColumn3.FieldName = "Quantity";
            gridColumn3.MinWidth = 21;
            gridColumn3.Name = "gridColumn3";
            gridColumn3.Visible = true;
            gridColumn3.VisibleIndex = 4;
            gridColumn3.Width = 184;
            // 
            // gridColumn5
            // 
            gridColumn5.Caption = "Bekleyen Miktar";
            gridColumn5.FieldName = "WaitingQuantity";
            gridColumn5.MinWidth = 21;
            gridColumn5.Name = "gridColumn5";
            gridColumn5.Visible = true;
            gridColumn5.VisibleIndex = 6;
            gridColumn5.Width = 200;
            // 
            // gridColumn4
            // 
            gridColumn4.Caption = "Termin Tarihi";
            gridColumn4.FieldName = "DueDate";
            gridColumn4.MinWidth = 21;
            gridColumn4.Name = "gridColumn4";
            gridColumn4.Visible = true;
            gridColumn4.VisibleIndex = 7;
            gridColumn4.Width = 203;
            // 
            // gridColumn6
            // 
            gridColumn6.Caption = "Kabul Edilen";
            gridColumn6.FieldName = "ShippedQuantity";
            gridColumn6.MinWidth = 21;
            gridColumn6.Name = "gridColumn6";
            gridColumn6.Visible = true;
            gridColumn6.VisibleIndex = 5;
            gridColumn6.Width = 232;
            // 
            // windowsuıButtonPanelPOList
            // 
            windowsuıButtonPanelPOList.AppearanceButton.Hovered.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanelPOList.AppearanceButton.Hovered.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            windowsuıButtonPanelPOList.AppearanceButton.Hovered.Options.UseFont = true;
            windowsuıButtonPanelPOList.AppearanceButton.Hovered.Options.UseForeColor = true;
            windowsuıButtonPanelPOList.AppearanceButton.Normal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanelPOList.AppearanceButton.Normal.ForeColor = System.Drawing.Color.White;
            windowsuıButtonPanelPOList.AppearanceButton.Normal.Options.UseFont = true;
            windowsuıButtonPanelPOList.AppearanceButton.Normal.Options.UseForeColor = true;
            windowsuıButtonPanelPOList.AppearanceButton.Pressed.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanelPOList.AppearanceButton.Pressed.ForeColor = System.Drawing.Color.White;
            windowsuıButtonPanelPOList.AppearanceButton.Pressed.Options.UseFont = true;
            windowsuıButtonPanelPOList.AppearanceButton.Pressed.Options.UseForeColor = true;
            windowsuıButtonPanelPOList.BackColor = System.Drawing.Color.FromArgb(227, 6, 19);
            windowsuıButtonImageOptions2.Image = (System.Drawing.Image)resources.GetObject("windowsuıButtonImageOptions2.Image");
            windowsuıButtonPanelPOList.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] { new DevExpress.XtraBars.Docking2010.WindowsUIButton("Kapat", true, windowsuıButtonImageOptions2, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, null, -1, false) });
            tableLayoutPanel1.SetColumnSpan(windowsuıButtonPanelPOList, 3);
            windowsuıButtonPanelPOList.Dock = System.Windows.Forms.DockStyle.Fill;
            windowsuıButtonPanelPOList.Location = new System.Drawing.Point(3, 625);
            windowsuıButtonPanelPOList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            windowsuıButtonPanelPOList.Name = "windowsuıButtonPanelPOList";
            windowsuıButtonPanelPOList.Size = new System.Drawing.Size(1439, 107);
            windowsuıButtonPanelPOList.TabIndex = 2;
            windowsuıButtonPanelPOList.Text = "windowsuıButtonPanel1";
            windowsuıButtonPanelPOList.ButtonClick += windowsuıButtonPanelPOList_ButtonClick;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.79331F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.20669F));
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(364, 2);
            tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.3333321F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.6666679F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            tableLayoutPanel2.Size = new System.Drawing.Size(716, 106);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // splashScreenManager1
            // 
            splashScreenManager1.ClosingDelay = 500;
            // 
            // gridColumn1
            // 
            gridColumn1.Caption = "Kodu";
            gridColumn1.FieldName = "ProductCode";
            gridColumn1.Name = "gridColumn1";
            gridColumn1.Visible = true;
            gridColumn1.VisibleIndex = 2;
            gridColumn1.Width = 64;
            // 
            // gridColumn7
            // 
            gridColumn7.Caption = "Sipariş No";
            gridColumn7.FieldName = "Code";
            gridColumn7.Name = "gridColumn7";
            gridColumn7.Visible = true;
            gridColumn7.VisibleIndex = 1;
            gridColumn7.Width = 107;
            // 
            // gridColumn8
            // 
            gridColumn8.Caption = "Sipariş Tarihi";
            gridColumn8.FieldName = "Date";
            gridColumn8.Name = "gridColumn8";
            gridColumn8.Visible = true;
            gridColumn8.VisibleIndex = 0;
            gridColumn8.Width = 134;
            // 
            // PurchaseOrderListView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1445, 734);
            Controls.Add(tableLayoutPanel1);
            FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            Name = "PurchaseOrderListView";
            Text = "PurchaseOrderListView";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            Load += PurchaseOrderListView_Load;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridControl1).EndInit();
            ((System.ComponentModel.ISupportInitialize)gridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel windowsuıButtonPanelPOList;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
    }
}