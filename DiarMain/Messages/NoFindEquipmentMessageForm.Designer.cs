namespace Condenser
{
    partial class NoFindEquipmentMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoFindEquipmentMessageForm));
            this.defaultSkin = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.lMessage = new DevExpress.XtraEditors.LabelControl();
            this.bAdd = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(484, 83);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 19;
            this.bCancel.Text = "Закрыть";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // lMessage
            // 
            this.lMessage.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lMessage.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.lMessage.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
            this.lMessage.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.lMessage.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.lMessage.Location = new System.Drawing.Point(14, 21);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(590, 54);
            this.lMessage.TabIndex = 21;
            this.lMessage.Text = "Оборудование по поиску \"___________\" отсутствует в базе данных. Добавить оборудов" +
                "ание?";
            // 
            // bAdd
            // 
            this.bAdd.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAdd.Appearance.Options.UseFont = true;
            this.bAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bAdd.Image = ((System.Drawing.Image)(resources.GetObject("bAdd.Image")));
            this.bAdd.Location = new System.Drawing.Point(356, 83);
            this.bAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bAdd.Name = "bAdd";
            this.bAdd.Size = new System.Drawing.Size(120, 33);
            this.bAdd.TabIndex = 22;
            this.bAdd.Text = "Добавить";
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // NoFindEquipmentMessageForm
            // 
            this.AcceptButton = this.bAdd;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(617, 127);
            this.Controls.Add(this.bAdd);
            this.Controls.Add(this.lMessage);
            this.Controls.Add(this.bCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NoFindEquipmentMessageForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Сообщение";
            this.Load += new System.EventHandler(this.NoFindEquipmentMessageForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultSkin;
        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.LabelControl lMessage;
        private DevExpress.XtraEditors.SimpleButton bAdd;
    }
}