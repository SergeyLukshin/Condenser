namespace Condenser
{
    partial class ServicePackForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServicePackForm));
            this.progress = new DevExpress.XtraEditors.ProgressBarControl();
            this.lCaptionVersion = new DevExpress.XtraEditors.LabelControl();
            this.lCaption = new DevExpress.XtraEditors.LabelControl();
            this.pictureEdit1 = new DevExpress.XtraEditors.PictureEdit();
            this.bCancel = new DevExpress.XtraEditors.SimpleButton();
            this.bRefresh = new DevExpress.XtraEditors.SimpleButton();
            this.worker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.progress.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // progress
            // 
            this.progress.Location = new System.Drawing.Point(12, 62);
            this.progress.Name = "progress";
            this.progress.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.progress.Properties.ShowTitle = true;
            this.progress.Properties.Step = 1;
            this.progress.Size = new System.Drawing.Size(553, 34);
            this.progress.TabIndex = 1;
            // 
            // lCaptionVersion
            // 
            this.lCaptionVersion.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lCaptionVersion.Location = new System.Drawing.Point(68, 23);
            this.lCaptionVersion.Name = "lCaptionVersion";
            this.lCaptionVersion.Size = new System.Drawing.Size(262, 20);
            this.lCaptionVersion.TabIndex = 2;
            this.lCaptionVersion.Text = "Доступна версия базы 1 от 01.01.2001";
            // 
            // lCaption
            // 
            this.lCaption.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.lCaption.Location = new System.Drawing.Point(12, 102);
            this.lCaption.Name = "lCaption";
            this.lCaption.Size = new System.Drawing.Size(207, 20);
            this.lCaption.TabIndex = 3;
            this.lCaption.Text = "<нажмите кнопку Обновить>";
            // 
            // pictureEdit1
            // 
            this.pictureEdit1.EditValue = ((object)(resources.GetObject("pictureEdit1.EditValue")));
            this.pictureEdit1.Location = new System.Drawing.Point(12, 13);
            this.pictureEdit1.Name = "pictureEdit1";
            this.pictureEdit1.Properties.ReadOnly = true;
            this.pictureEdit1.Properties.ShowMenu = false;
            this.pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Zoom;
            this.pictureEdit1.Size = new System.Drawing.Size(44, 43);
            this.pictureEdit1.TabIndex = 4;
            // 
            // bCancel
            // 
            this.bCancel.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancel.Appearance.Options.UseFont = true;
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Image = ((System.Drawing.Image)(resources.GetObject("bCancel.Image")));
            this.bCancel.Location = new System.Drawing.Point(445, 137);
            this.bCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(120, 33);
            this.bCancel.TabIndex = 20;
            this.bCancel.Text = "Отменить";
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bRefresh.Appearance.Options.UseFont = true;
            this.bRefresh.Image = ((System.Drawing.Image)(resources.GetObject("bRefresh.Image")));
            this.bRefresh.Location = new System.Drawing.Point(315, 137);
            this.bRefresh.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(122, 33);
            this.bRefresh.TabIndex = 19;
            this.bRefresh.Text = "Обновить";
            this.bRefresh.Click += new System.EventHandler(this.bActivation_Click);
            // 
            // worker
            // 
            this.worker.WorkerReportsProgress = true;
            this.worker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.worker_DoWork);
            this.worker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.worker_ProgressChanged);
            this.worker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.worker_RunWorkerCompleted);
            // 
            // ServicePackForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(577, 182);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.pictureEdit1);
            this.Controls.Add(this.lCaption);
            this.Controls.Add(this.lCaptionVersion);
            this.Controls.Add(this.progress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServicePackForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обновление базы данных";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServicePackForm_FormClosing);
            this.Load += new System.EventHandler(this.ServicePackForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.progress.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureEdit1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.ProgressBarControl progress;
        private DevExpress.XtraEditors.LabelControl lCaptionVersion;
        private DevExpress.XtraEditors.LabelControl lCaption;
        private DevExpress.XtraEditors.PictureEdit pictureEdit1;
        private DevExpress.XtraEditors.SimpleButton bCancel;
        private DevExpress.XtraEditors.SimpleButton bRefresh;
        private System.ComponentModel.BackgroundWorker worker;
    }
}