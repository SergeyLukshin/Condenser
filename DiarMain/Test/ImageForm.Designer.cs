namespace Condenser
{
    partial class ImageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.peImage = new DevExpress.XtraEditors.PictureEdit();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.btnLoad = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl4 = new DevExpress.XtraEditors.PanelControl();
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bSave = new DevExpress.XtraEditors.SimpleButton();
            this.panelExpertMode = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).BeginInit();
            this.panelControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).BeginInit();
            this.panelExpertMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.peImage);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Padding = new System.Windows.Forms.Padding(7, 8, 7, 8);
            this.panelControl1.Size = new System.Drawing.Size(631, 537);
            this.panelControl1.TabIndex = 0;
            // 
            // peImage
            // 
            this.peImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peImage.Location = new System.Drawing.Point(9, 10);
            this.peImage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.peImage.Name = "peImage";
            this.peImage.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.peImage.Properties.Appearance.Options.UseFont = true;
            this.peImage.Properties.AppearanceDisabled.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.peImage.Properties.AppearanceDisabled.Options.UseFont = true;
            this.peImage.Properties.AppearanceFocused.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.peImage.Properties.AppearanceFocused.Options.UseFont = true;
            this.peImage.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.peImage.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.peImage.Properties.NullText = "нет изображения";
            this.peImage.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.peImage.Size = new System.Drawing.Size(613, 517);
            this.peImage.TabIndex = 23;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.btnLoad);
            this.panelControl2.Controls.Add(this.panelControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl2.Location = new System.Drawing.Point(0, 565);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(631, 49);
            this.panelControl2.TabIndex = 1;
            // 
            // btnLoad
            // 
            this.btnLoad.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnLoad.Appearance.Options.UseFont = true;
            this.btnLoad.Image = ((System.Drawing.Image)(resources.GetObject("btnLoad.Image")));
            this.btnLoad.Location = new System.Drawing.Point(9, 9);
            this.btnLoad.LookAndFeel.SkinName = "Caramel";
            this.btnLoad.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.btnLoad.MinimumSize = new System.Drawing.Size(122, 33);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(122, 33);
            this.btnLoad.TabIndex = 23;
            this.btnLoad.Text = "Загрузить";
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // panelControl4
            // 
            this.panelControl4.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl4.Controls.Add(this.bCancel);
            this.panelControl4.Controls.Add(this.bSave);
            this.panelControl4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelControl4.Location = new System.Drawing.Point(357, 2);
            this.panelControl4.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.panelControl4.Name = "panelControl4";
            this.panelControl4.Size = new System.Drawing.Size(272, 45);
            this.panelControl4.TabIndex = 22;
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(146, 7);
            this.bCancel.LookAndFeel.SkinName = "Caramel";
            this.bCancel.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(117, 33);
            this.bCancel.TabIndex = 22;
            this.bCancel.Text = "Отменить";
            // 
            // bSave
            // 
            this.bSave.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSave.Appearance.Options.UseFont = true;
            this.bSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.bSave.Image = ((System.Drawing.Image)(resources.GetObject("bSave.Image")));
            this.bSave.Location = new System.Drawing.Point(16, 7);
            this.bSave.LookAndFeel.SkinName = "Caramel";
            this.bSave.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(120, 33);
            this.bSave.TabIndex = 21;
            this.bSave.Text = "Сохранить";
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // panelExpertMode
            // 
            this.panelExpertMode.Appearance.BackColor = System.Drawing.Color.LimeGreen;
            this.panelExpertMode.Appearance.Options.UseBackColor = true;
            this.panelExpertMode.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelExpertMode.Controls.Add(this.labelControl4);
            this.panelExpertMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelExpertMode.Location = new System.Drawing.Point(0, 0);
            this.panelExpertMode.Name = "panelExpertMode";
            this.panelExpertMode.Size = new System.Drawing.Size(631, 28);
            this.panelExpertMode.TabIndex = 4;
            this.panelExpertMode.Visible = false;
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl4.Location = new System.Drawing.Point(12, 3);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(120, 20);
            this.labelControl4.TabIndex = 0;
            this.labelControl4.Text = "Режим Эксперта";
            // 
            // ImageForm
            // 
            this.AcceptButton = this.bSave;
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(631, 614);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelExpertMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(629, 641);
            this.Name = "ImageForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Изображение";
            this.Load += new System.EventHandler(this.ImageForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.peImage.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl4)).EndInit();
            this.panelControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).EndInit();
            this.panelExpertMode.ResumeLayout(false);
            this.panelExpertMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.PanelControl panelControl4;
        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bSave;
        private DevExpress.XtraEditors.PictureEdit peImage;
        private DevExpress.XtraEditors.SimpleButton btnLoad;
        private DevExpress.XtraEditors.PanelControl panelExpertMode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}