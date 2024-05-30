namespace PurchaseOperator.Win.Views.LoginView
{
    partial class LoginView
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginView));
            DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions windowsuıButtonImageOptions1 = new DevExpress.XtraBars.Docking2010.WindowsUIButtonImageOptions();
            tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            pictureEdit2 = new DevExpress.XtraEditors.PictureEdit();
            txtOperatorCode = new DevExpress.XtraEditors.TextEdit();
            windowsuıButtonPanel1 = new DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel();
            splashScreenManager1 = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(WaitForm1), true, true);
            alertControl1 = new DevExpress.XtraBars.Alerter.AlertControl(components);
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).BeginInit();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureEdit2.Properties).BeginInit();
            ((System.ComponentModel.ISupportInitialize)txtOperatorCode.Properties).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(pictureEdit1, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 1);
            tableLayoutPanel1.Controls.Add(windowsuıButtonPanel1, 0, 2);
            tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65F));
            tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15F));
            tableLayoutPanel1.Size = new System.Drawing.Size(1458, 868);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // pictureEdit1
            // 
            pictureEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureEdit1.EditValue = resources.GetObject("pictureEdit1.EditValue");
            pictureEdit1.Location = new System.Drawing.Point(367, 3);
            pictureEdit1.Name = "pictureEdit1";
            pictureEdit1.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pictureEdit1.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pictureEdit1.Size = new System.Drawing.Size(723, 167);
            pictureEdit1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 80F));
            tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            tableLayoutPanel2.Controls.Add(pictureEdit2, 0, 1);
            tableLayoutPanel2.Controls.Add(txtOperatorCode, 1, 1);
            tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutPanel2.Location = new System.Drawing.Point(367, 176);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 41.75627F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.5448027F));
            tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.698925F));
            tableLayoutPanel2.Size = new System.Drawing.Size(723, 558);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // pictureEdit2
            // 
            pictureEdit2.Dock = System.Windows.Forms.DockStyle.Fill;
            pictureEdit2.EditValue = resources.GetObject("pictureEdit2.EditValue");
            pictureEdit2.Location = new System.Drawing.Point(3, 236);
            pictureEdit2.Name = "pictureEdit2";
            pictureEdit2.Properties.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            pictureEdit2.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            pictureEdit2.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            pictureEdit2.Size = new System.Drawing.Size(66, 64);
            pictureEdit2.TabIndex = 0;
            // 
            // txtOperatorCode
            // 
            txtOperatorCode.Dock = System.Windows.Forms.DockStyle.Fill;
            txtOperatorCode.Location = new System.Drawing.Point(75, 236);
            txtOperatorCode.Name = "txtOperatorCode";
            txtOperatorCode.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            txtOperatorCode.Properties.Appearance.Options.UseFont = true;
            txtOperatorCode.Properties.AutoHeight = false;
            txtOperatorCode.Properties.NullValuePrompt = "Kullanıcı Kodu Giriniz..";
            txtOperatorCode.Properties.PasswordChar = '*';
            txtOperatorCode.Size = new System.Drawing.Size(572, 64);
            txtOperatorCode.TabIndex = 1;
            txtOperatorCode.EditValueChanged += txtOperatorCode_EditValueChanged;
            txtOperatorCode.KeyDown += txtOperatorCode_KeyDown;
            // 
            // windowsuıButtonPanel1
            // 
            windowsuıButtonPanel1.AppearanceButton.Hovered.BackColor = System.Drawing.Color.FromArgb(224, 224, 224);
            windowsuıButtonPanel1.AppearanceButton.Hovered.BorderColor = System.Drawing.Color.FromArgb(224, 224, 224);
            windowsuıButtonPanel1.AppearanceButton.Hovered.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanel1.AppearanceButton.Hovered.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            windowsuıButtonPanel1.AppearanceButton.Hovered.Options.UseBackColor = true;
            windowsuıButtonPanel1.AppearanceButton.Hovered.Options.UseBorderColor = true;
            windowsuıButtonPanel1.AppearanceButton.Hovered.Options.UseFont = true;
            windowsuıButtonPanel1.AppearanceButton.Hovered.Options.UseForeColor = true;
            windowsuıButtonPanel1.AppearanceButton.Normal.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanel1.AppearanceButton.Normal.FontStyleDelta = System.Drawing.FontStyle.Bold;
            windowsuıButtonPanel1.AppearanceButton.Normal.ForeColor = System.Drawing.Color.White;
            windowsuıButtonPanel1.AppearanceButton.Normal.Options.UseFont = true;
            windowsuıButtonPanel1.AppearanceButton.Normal.Options.UseForeColor = true;
            windowsuıButtonPanel1.AppearanceButton.Pressed.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            windowsuıButtonPanel1.AppearanceButton.Pressed.ForeColor = System.Drawing.Color.FromArgb(224, 224, 224);
            windowsuıButtonPanel1.AppearanceButton.Pressed.Options.UseFont = true;
            windowsuıButtonPanel1.AppearanceButton.Pressed.Options.UseForeColor = true;
            windowsuıButtonPanel1.BackColor = System.Drawing.Color.FromArgb(227, 6, 19);
            windowsuıButtonImageOptions1.Image = (System.Drawing.Image)resources.GetObject("windowsuıButtonImageOptions1.Image");
            windowsuıButtonPanel1.Buttons.AddRange(new DevExpress.XtraEditors.ButtonPanel.IBaseButton[] { new DevExpress.XtraBars.Docking2010.WindowsUIButton("Kapat", true, windowsuıButtonImageOptions1, DevExpress.XtraBars.Docking2010.ButtonStyle.PushButton, "", -1, true, null, true, false, true, null, -1, false) });
            tableLayoutPanel1.SetColumnSpan(windowsuıButtonPanel1, 3);
            windowsuıButtonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            windowsuıButtonPanel1.Location = new System.Drawing.Point(3, 740);
            windowsuıButtonPanel1.Name = "windowsuıButtonPanel1";
            windowsuıButtonPanel1.Size = new System.Drawing.Size(1452, 125);
            windowsuıButtonPanel1.TabIndex = 2;
            windowsuıButtonPanel1.Text = "windowsuıButtonPanel1";
            windowsuıButtonPanel1.UseWaitCursor = true;
            windowsuıButtonPanel1.ButtonClick += windowsuıButtonPanel1_ButtonClick;
            // 
            // splashScreenManager1
            // 
            splashScreenManager1.ClosingDelay = 500;
            // 
            // LoginView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1458, 868);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Name = "LoginView";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "LoginView";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureEdit1.Properties).EndInit();
            tableLayoutPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureEdit2.Properties).EndInit();
            ((System.ComponentModel.ISupportInitialize)txtOperatorCode.Properties).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private DevExpress.XtraEditors.PictureEdit pictureEdit2;
        private DevExpress.XtraEditors.TextEdit txtOperatorCode;
        private DevExpress.XtraBars.Docking2010.WindowsUIButtonPanel windowsuıButtonPanel1;
        private DevExpress.XtraSplashScreen.SplashScreenManager splashScreenManager1;
        private DevExpress.XtraBars.Alerter.AlertControl alertControl1;
    }
}