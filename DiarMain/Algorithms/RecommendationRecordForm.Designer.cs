namespace Condenser
{
    partial class RecommendationRecordForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecommendationRecordForm));
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bSave = new DevExpress.XtraEditors.SimpleButton();
            this.panel = new DevExpress.XtraEditors.GroupControl();
            this.teKIn = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teConclusion = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.teRecommendation = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.panel)).BeginInit();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teKIn.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConclusion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRecommendation.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(486, 227);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Отменить";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bSave
            // 
            this.bSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSave.Appearance.Options.UseFont = true;
            this.bSave.Image = ((System.Drawing.Image)(resources.GetObject("bSave.Image")));
            this.bSave.Location = new System.Drawing.Point(359, 227);
            this.bSave.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(119, 33);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "Сохранить";
            this.bSave.Click += new System.EventHandler(this.bProtocol_Click);
            // 
            // panel
            // 
            this.panel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.panel.Appearance.Options.UseFont = true;
            this.panel.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.panel.AppearanceCaption.Options.UseFont = true;
            this.panel.Controls.Add(this.teKIn);
            this.panel.Controls.Add(this.labelControl3);
            this.panel.Controls.Add(this.labelControl1);
            this.panel.Controls.Add(this.teConclusion);
            this.panel.Controls.Add(this.labelControl2);
            this.panel.Controls.Add(this.teRecommendation);
            this.panel.Location = new System.Drawing.Point(9, 12);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(10);
            this.panel.Size = new System.Drawing.Size(597, 207);
            this.panel.TabIndex = 27;
            this.panel.Text = "КЦФ ≤ 80% РЕС";
            // 
            // teKIn
            // 
            this.teKIn.Location = new System.Drawing.Point(137, 39);
            this.teKIn.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.teKIn.Name = "teKIn";
            this.teKIn.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.teKIn.Properties.Appearance.Options.UseFont = true;
            this.teKIn.Properties.Mask.EditMask = "(\\d+|\\d+,\\d|\\d+,\\d\\d|\\d+,\\d\\d\\d|\\d+,\\d\\d\\d\\d)";
            this.teKIn.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.teKIn.Properties.Mask.ShowPlaceHolders = false;
            this.teKIn.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.teKIn.Size = new System.Drawing.Size(125, 26);
            this.teKIn.TabIndex = 32;
            this.teKIn.EditValueChanged += new System.EventHandler(this.teKIn_EditValueChanged);
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.labelControl3.Location = new System.Drawing.Point(13, 42);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(50, 20);
            this.labelControl3.TabIndex = 31;
            this.labelControl3.Text = "КЦn, %:";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl1.Location = new System.Drawing.Point(13, 141);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(106, 20);
            this.labelControl1.TabIndex = 30;
            this.labelControl1.Text = "Рекомендации:";
            // 
            // teConclusion
            // 
            this.teConclusion.EditValue = "";
            this.teConclusion.Location = new System.Drawing.Point(137, 73);
            this.teConclusion.Name = "teConclusion";
            this.teConclusion.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teConclusion.Properties.Appearance.Options.UseFont = true;
            this.teConclusion.Properties.Appearance.Options.UseTextOptions = true;
            this.teConclusion.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.teConclusion.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.teConclusion.Size = new System.Drawing.Size(445, 56);
            this.teConclusion.TabIndex = 27;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl2.Location = new System.Drawing.Point(13, 76);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 20);
            this.labelControl2.TabIndex = 28;
            this.labelControl2.Text = "Заключение:";
            // 
            // teRecommendation
            // 
            this.teRecommendation.EditValue = "";
            this.teRecommendation.Location = new System.Drawing.Point(137, 138);
            this.teRecommendation.Name = "teRecommendation";
            this.teRecommendation.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teRecommendation.Properties.Appearance.Options.UseFont = true;
            this.teRecommendation.Properties.Appearance.Options.UseTextOptions = true;
            this.teRecommendation.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.teRecommendation.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.teRecommendation.Size = new System.Drawing.Size(445, 57);
            this.teRecommendation.TabIndex = 29;
            // 
            // RecommendationRecordForm
            // 
            this.AcceptButton = this.bSave;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(617, 269);
            this.Controls.Add(this.panel);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecommendationRecordForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Заключение и рекомендации";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panel)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.teKIn.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConclusion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teRecommendation.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bSave;
        private DevExpress.XtraEditors.GroupControl panel;
        private DevExpress.XtraEditors.TextEdit teKIn;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit teConclusion;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.MemoEdit teRecommendation;
    }
}