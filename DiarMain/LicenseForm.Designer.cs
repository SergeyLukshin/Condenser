namespace Condenser
{
    partial class LicenseForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LicenseForm));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.teCode = new DevExpress.XtraEditors.TextEdit();
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bActivation = new DevExpress.XtraEditors.SimpleButton();
            this.teActivationKey = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.teCode.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActivationKey.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.labelControl1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(67, 5);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(467, 90);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "Программа не активирована для данного пользователя. Для продолжения работы необхо" +
                "димо ввести ключ активации. Его можно узнать в службе поддержки.";
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl2.Location = new System.Drawing.Point(21, 116);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(126, 20);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Серийный номер:";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl3.Location = new System.Drawing.Point(21, 152);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(117, 20);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "Ключ активации:";
            // 
            // teCode
            // 
            this.teCode.Location = new System.Drawing.Point(166, 114);
            this.teCode.Name = "teCode";
            this.teCode.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teCode.Properties.Appearance.Options.UseFont = true;
            this.teCode.Properties.ReadOnly = true;
            this.teCode.Size = new System.Drawing.Size(437, 26);
            this.teCode.TabIndex = 1;
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(483, 248);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Отменить";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bActivation
            // 
            this.bActivation.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bActivation.Appearance.Options.UseFont = true;
            this.bActivation.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bActivation.Image = ((System.Drawing.Image)(resources.GetObject("bActivation.Image")));
            this.bActivation.Location = new System.Drawing.Point(335, 248);
            this.bActivation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bActivation.Name = "bActivation";
            this.bActivation.Size = new System.Drawing.Size(140, 33);
            this.bActivation.TabIndex = 2;
            this.bActivation.Text = "Активировать";
            this.bActivation.Click += new System.EventHandler(this.bActivation_Click);
            // 
            // teActivationKey
            // 
            this.teActivationKey.EditValue = "";
            this.teActivationKey.Location = new System.Drawing.Point(166, 150);
            this.teActivationKey.Name = "teActivationKey";
            this.teActivationKey.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teActivationKey.Properties.Appearance.Options.UseFont = true;
            this.teActivationKey.Properties.Appearance.Options.UseTextOptions = true;
            this.teActivationKey.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.teActivationKey.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.teActivationKey.Size = new System.Drawing.Size(437, 87);
            this.teActivationKey.TabIndex = 0;
            // 
            // LicenseForm
            // 
            this.AcceptButton = this.bActivation;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(617, 291);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bActivation);
            this.Controls.Add(this.teCode);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.teActivationKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Активация программы";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teCode.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teActivationKey.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit teCode;
        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bActivation;
        private DevExpress.XtraEditors.MemoEdit teActivationKey;
    }
}