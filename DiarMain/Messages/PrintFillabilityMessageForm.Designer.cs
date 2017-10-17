namespace Condenser
{
    partial class PrintFillabilityMessageForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintFillabilityMessageForm));
            this.defaultSkin = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.bContinue = new DevExpress.XtraEditors.SimpleButton();
            this.lMessage = new DevExpress.XtraEditors.LabelControl();
            this.bFillData = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // bContinue
            // 
            this.bContinue.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bContinue.Appearance.Options.UseFont = true;
            this.bContinue.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bContinue.Location = new System.Drawing.Point(200, 68);
            this.bContinue.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bContinue.Name = "bContinue";
            this.bContinue.Size = new System.Drawing.Size(203, 33);
            this.bContinue.TabIndex = 19;
            this.bContinue.Text = "Сформировать протокол";
            this.bContinue.Click += new System.EventHandler(this.bContinue_Click);
            // 
            // lMessage
            // 
            this.lMessage.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lMessage.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.lMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lMessage.Location = new System.Drawing.Point(14, 21);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(389, 30);
            this.lMessage.TabIndex = 21;
            this.lMessage.Text = "Не заполнено xx% данных.";
            // 
            // bFillData
            // 
            this.bFillData.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bFillData.Appearance.Options.UseFont = true;
            this.bFillData.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bFillData.Location = new System.Drawing.Point(14, 68);
            this.bFillData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bFillData.Name = "bFillData";
            this.bFillData.Size = new System.Drawing.Size(178, 33);
            this.bFillData.TabIndex = 22;
            this.bFillData.Text = "Заполнить данные";
            this.bFillData.Click += new System.EventHandler(this.bFillData_Click);
            // 
            // PrintFillabilityMessageForm
            // 
            this.AcceptButton = this.bFillData;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bContinue;
            this.ClientSize = new System.Drawing.Size(418, 114);
            this.Controls.Add(this.bFillData);
            this.Controls.Add(this.lMessage);
            this.Controls.Add(this.bContinue);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrintFillabilityMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сообщение";
            this.Load += new System.EventHandler(this.PrintFillabilityMessageForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultSkin;
        private DevExpress.XtraEditors.SimpleButton bContinue;
        private DevExpress.XtraEditors.LabelControl lMessage;
        private DevExpress.XtraEditors.SimpleButton bFillData;
    }
}