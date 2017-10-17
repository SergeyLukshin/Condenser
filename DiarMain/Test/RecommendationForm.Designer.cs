namespace Condenser
{
    partial class RecommendationForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecommendationForm));
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bProtocol = new DevExpress.XtraEditors.SimpleButton();
            this.teRecommendation = new DevExpress.XtraEditors.MemoEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.teConclusion = new DevExpress.XtraEditors.MemoEdit();
            ((System.ComponentModel.ISupportInitialize)(this.teRecommendation.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConclusion.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl2.Location = new System.Drawing.Point(21, 14);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(88, 20);
            this.labelControl2.TabIndex = 2;
            this.labelControl2.Text = "Заключение:";
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(485, 150);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 1;
            this.bCancel.Text = "Закрыть";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bProtocol
            // 
            this.bProtocol.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bProtocol.Appearance.Options.UseFont = true;
            this.bProtocol.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bProtocol.Image = ((System.Drawing.Image)(resources.GetObject("bProtocol.Image")));
            this.bProtocol.Location = new System.Drawing.Point(291, 150);
            this.bProtocol.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bProtocol.Name = "bProtocol";
            this.bProtocol.Size = new System.Drawing.Size(186, 33);
            this.bProtocol.TabIndex = 0;
            this.bProtocol.Text = "Вывести протокол";
            this.bProtocol.Click += new System.EventHandler(this.bProtocol_Click);
            // 
            // teRecommendation
            // 
            this.teRecommendation.EditValue = "";
            this.teRecommendation.Location = new System.Drawing.Point(145, 80);
            this.teRecommendation.Name = "teRecommendation";
            this.teRecommendation.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teRecommendation.Properties.Appearance.Options.UseFont = true;
            this.teRecommendation.Properties.Appearance.Options.UseTextOptions = true;
            this.teRecommendation.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.teRecommendation.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.teRecommendation.Properties.ReadOnly = true;
            this.teRecommendation.Size = new System.Drawing.Size(460, 57);
            this.teRecommendation.TabIndex = 3;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl1.Location = new System.Drawing.Point(21, 83);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(106, 20);
            this.labelControl1.TabIndex = 5;
            this.labelControl1.Text = "Рекомендации:";
            // 
            // teConclusion
            // 
            this.teConclusion.EditValue = "";
            this.teConclusion.Location = new System.Drawing.Point(145, 12);
            this.teConclusion.Name = "teConclusion";
            this.teConclusion.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.teConclusion.Properties.Appearance.Options.UseFont = true;
            this.teConclusion.Properties.Appearance.Options.UseTextOptions = true;
            this.teConclusion.Properties.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.teConclusion.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.teConclusion.Properties.ReadOnly = true;
            this.teConclusion.Size = new System.Drawing.Size(460, 56);
            this.teConclusion.TabIndex = 2;
            // 
            // RecommendationForm
            // 
            this.AcceptButton = this.bProtocol;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(617, 193);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.teConclusion);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bProtocol);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.teRecommendation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RecommendationForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Заключение и рекомендации";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.teRecommendation.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.teConclusion.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bProtocol;
        private DevExpress.XtraEditors.MemoEdit teRecommendation;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.MemoEdit teConclusion;
    }
}