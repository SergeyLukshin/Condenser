namespace Condenser
{
    partial class PswForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PswForm));
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bActivation = new DevExpress.XtraEditors.SimpleButton();
            this.tePsw = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tePsw.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(298, 60);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 22;
            this.bCancel.Text = "Отменить";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bActivation
            // 
            this.bActivation.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bActivation.Appearance.Options.UseFont = true;
            this.bActivation.Image = ((System.Drawing.Image)(resources.GetObject("bActivation.Image")));
            this.bActivation.Location = new System.Drawing.Point(150, 60);
            this.bActivation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bActivation.Name = "bActivation";
            this.bActivation.Size = new System.Drawing.Size(140, 33);
            this.bActivation.TabIndex = 21;
            this.bActivation.Text = "Продолжить";
            this.bActivation.Click += new System.EventHandler(this.bActivation_Click);
            // 
            // tePsw
            // 
            this.tePsw.Location = new System.Drawing.Point(150, 14);
            this.tePsw.Name = "tePsw";
            this.tePsw.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.tePsw.Properties.Appearance.Options.UseFont = true;
            this.tePsw.Properties.PasswordChar = '*';
            this.tePsw.Size = new System.Drawing.Size(268, 26);
            this.tePsw.TabIndex = 20;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl2.Location = new System.Drawing.Point(12, 17);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(114, 20);
            this.labelControl2.TabIndex = 19;
            this.labelControl2.Text = "Введите пароль:";
            // 
            // PswForm
            // 
            this.AcceptButton = this.bActivation;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(437, 104);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bActivation);
            this.Controls.Add(this.tePsw);
            this.Controls.Add(this.labelControl2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PswForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Вход в режим Эксперта";
            ((System.ComponentModel.ISupportInitialize)(this.tePsw.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bActivation;
        private DevExpress.XtraEditors.TextEdit tePsw;
        private DevExpress.XtraEditors.LabelControl labelControl2;

    }
}