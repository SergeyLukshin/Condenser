namespace Condenser
{
    partial class InsulatingLiquidTypeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsulatingLiquidTypeForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbCanEdit = new DevExpress.XtraEditors.CheckEdit();
            this.controlNavigator1 = new DevExpress.XtraEditors.ControlNavigator();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.GridGC = new DevExpress.XtraGrid.GridControl();
            this.qInsulatingLiquidTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetQuery = new Condenser.DataSetQuery();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colInsulatingLiquidTypeID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colInsulatingLiquidTypeName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repYesNo = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.toolTip = new DevExpress.Utils.ToolTipController(this.components);
            this.panelSelect = new DevExpress.XtraEditors.PanelControl();
            this.bSelect = new DevExpress.XtraEditors.SimpleButton();
            this.qInsulatingLiquidTypesTableAdapter = new Condenser.DataSetQueryTableAdapters.QInsulatingLiquidTypesTableAdapter();
            this.panelExpertMode = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbCanEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qInsulatingLiquidTypesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repYesNo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSelect)).BeginInit();
            this.panelSelect.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).BeginInit();
            this.panelExpertMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.cbCanEdit);
            this.panelControl1.Controls.Add(this.controlNavigator1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(793, 58);
            this.panelControl1.TabIndex = 1;
            // 
            // cbCanEdit
            // 
            this.cbCanEdit.Location = new System.Drawing.Point(413, 18);
            this.cbCanEdit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCanEdit.Name = "cbCanEdit";
            this.cbCanEdit.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbCanEdit.Properties.Appearance.Options.UseFont = true;
            this.cbCanEdit.Properties.Caption = "разрешить исправление";
            this.cbCanEdit.Size = new System.Drawing.Size(256, 25);
            this.cbCanEdit.TabIndex = 3;
            this.cbCanEdit.CheckedChanged += new System.EventHandler(this.cbCanEdit_CheckedChanged);
            this.cbCanEdit.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubjectView_KeyDown);
            // 
            // controlNavigator1
            // 
            this.controlNavigator1.Buttons.Append.Hint = "Добавить запись";
            this.controlNavigator1.Buttons.Append.ImageIndex = 0;
            this.controlNavigator1.Buttons.CancelEdit.Hint = "Отменить изменения";
            this.controlNavigator1.Buttons.CancelEdit.ImageIndex = 3;
            this.controlNavigator1.Buttons.Edit.Hint = "Редактировать запись";
            this.controlNavigator1.Buttons.Edit.ImageIndex = 4;
            this.controlNavigator1.Buttons.EndEdit.Hint = "Подтвердить изменения";
            this.controlNavigator1.Buttons.EndEdit.ImageIndex = 2;
            this.controlNavigator1.Buttons.First.Visible = false;
            this.controlNavigator1.Buttons.ImageList = this.imageList1;
            this.controlNavigator1.Buttons.Last.Visible = false;
            this.controlNavigator1.Buttons.Next.Visible = false;
            this.controlNavigator1.Buttons.NextPage.Visible = false;
            this.controlNavigator1.Buttons.Prev.Visible = false;
            this.controlNavigator1.Buttons.PrevPage.Visible = false;
            this.controlNavigator1.Buttons.Remove.Hint = "Удалить запись";
            this.controlNavigator1.Buttons.Remove.ImageIndex = 1;
            this.controlNavigator1.Location = new System.Drawing.Point(7, 6);
            this.controlNavigator1.LookAndFeel.SkinName = "Caramel";
            this.controlNavigator1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.controlNavigator1.Name = "controlNavigator1";
            this.controlNavigator1.NavigatableControl = this.GridGC;
            this.controlNavigator1.ShowToolTips = true;
            this.controlNavigator1.Size = new System.Drawing.Size(384, 47);
            this.controlNavigator1.TabIndex = 2;
            this.controlNavigator1.Text = "controlNavigator1";
            this.controlNavigator1.ToolTipController = this.toolTip;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "03.png");
            this.imageList1.Images.SetKeyName(1, "04.png");
            this.imageList1.Images.SetKeyName(2, "02_.png");
            this.imageList1.Images.SetKeyName(3, "01_.png");
            this.imageList1.Images.SetKeyName(4, "19_.png");
            this.imageList1.Images.SetKeyName(5, "34.png");
            // 
            // GridGC
            // 
            this.GridGC.DataSource = this.qInsulatingLiquidTypesBindingSource;
            this.GridGC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridGC.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Location = new System.Drawing.Point(0, 86);
            this.GridGC.MainView = this.GridView;
            this.GridGC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Name = "GridGC";
            this.GridGC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repYesNo});
            this.GridGC.Size = new System.Drawing.Size(793, 451);
            this.GridGC.TabIndex = 2;
            this.GridGC.ToolTipController = this.toolTip;
            this.GridGC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            // 
            // qInsulatingLiquidTypesBindingSource
            // 
            this.qInsulatingLiquidTypesBindingSource.DataMember = "QInsulatingLiquidTypes";
            this.qInsulatingLiquidTypesBindingSource.DataSource = this.dataSetQuery;
            // 
            // dataSetQuery
            // 
            this.dataSetQuery.DataSetName = "DataSetQuery";
            this.dataSetQuery.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GridView
            // 
            this.GridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.Row.Options.UseFont = true;
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colInsulatingLiquidTypeID,
            this.colInsulatingLiquidTypeName});
            this.GridView.GridControl = this.GridGC;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this.GridView.OptionsDetail.EnableMasterViewMode = false;
            this.GridView.OptionsMenu.EnableColumnMenu = false;
            this.GridView.OptionsNavigation.AutoFocusNewRow = true;
            this.GridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridView.OptionsSelection.InvertSelection = true;
            this.GridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GridView.OptionsView.ShowGroupPanel = false;
            this.GridView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.GridView_InvalidRowException);
            this.GridView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView_ValidateRow);
            this.GridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SubjectView_KeyDown);
            // 
            // colInsulatingLiquidTypeID
            // 
            this.colInsulatingLiquidTypeID.FieldName = "InsulatingLiquidTypeID";
            this.colInsulatingLiquidTypeID.Name = "colInsulatingLiquidTypeID";
            // 
            // colInsulatingLiquidTypeName
            // 
            this.colInsulatingLiquidTypeName.Caption = "Наименование";
            this.colInsulatingLiquidTypeName.FieldName = "InsulatingLiquidTypeName";
            this.colInsulatingLiquidTypeName.Name = "colInsulatingLiquidTypeName";
            this.colInsulatingLiquidTypeName.OptionsFilter.AllowAutoFilter = false;
            this.colInsulatingLiquidTypeName.OptionsFilter.AllowFilter = false;
            this.colInsulatingLiquidTypeName.Visible = true;
            this.colInsulatingLiquidTypeName.VisibleIndex = 0;
            this.colInsulatingLiquidTypeName.Width = 500;
            // 
            // repYesNo
            // 
            this.repYesNo.AutoHeight = false;
            this.repYesNo.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repYesNo.Name = "repYesNo";
            this.repYesNo.NullText = "";
            // 
            // toolTip
            // 
            this.toolTip.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolTip.Appearance.Options.UseFont = true;
            this.toolTip.Rounded = true;
            // 
            // panelSelect
            // 
            this.panelSelect.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelSelect.Controls.Add(this.bSelect);
            this.panelSelect.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelSelect.Location = new System.Drawing.Point(0, 537);
            this.panelSelect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelSelect.Name = "panelSelect";
            this.panelSelect.Size = new System.Drawing.Size(793, 48);
            this.panelSelect.TabIndex = 4;
            // 
            // bSelect
            // 
            this.bSelect.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bSelect.Appearance.Options.UseFont = true;
            this.bSelect.Image = ((System.Drawing.Image)(resources.GetObject("bSelect.Image")));
            this.bSelect.Location = new System.Drawing.Point(5, 8);
            this.bSelect.LookAndFeel.SkinName = "Caramel";
            this.bSelect.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.bSelect.Name = "bSelect";
            this.bSelect.Size = new System.Drawing.Size(105, 33);
            this.bSelect.TabIndex = 22;
            this.bSelect.Text = "Выбрать";
            this.bSelect.Click += new System.EventHandler(this.bSelect_Click);
            // 
            // qInsulatingLiquidTypesTableAdapter
            // 
            this.qInsulatingLiquidTypesTableAdapter.ClearBeforeFill = true;
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
            this.panelExpertMode.Size = new System.Drawing.Size(793, 28);
            this.panelExpertMode.TabIndex = 5;
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
            // InsulatingLiquidTypeForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 585);
            this.Controls.Add(this.GridGC);
            this.Controls.Add(this.panelSelect);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelExpertMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Caramel";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "InsulatingLiquidTypeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Марка изоляционной жидкости";
            this.Load += new System.EventHandler(this.InsulatingLiquidTypeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbCanEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qInsulatingLiquidTypesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repYesNo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelSelect)).EndInit();
            this.panelSelect.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).EndInit();
            this.panelExpertMode.ResumeLayout(false);
            this.panelExpertMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraGrid.GridControl GridGC;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn colInsulatingLiquidTypeID;
        private DevExpress.XtraGrid.Columns.GridColumn colInsulatingLiquidTypeName;
        private DevExpress.XtraEditors.ControlNavigator controlNavigator1;
        private DevExpress.XtraEditors.CheckEdit cbCanEdit;
        private DevExpress.Utils.ToolTipController toolTip;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repYesNo;
        private DevExpress.XtraEditors.PanelControl panelSelect;
        private DevExpress.XtraEditors.SimpleButton bSelect;
        private DataSetQuery dataSetQuery;
        private System.Windows.Forms.BindingSource qInsulatingLiquidTypesBindingSource;
        private DataSetQueryTableAdapters.QInsulatingLiquidTypesTableAdapter qInsulatingLiquidTypesTableAdapter;
        private DevExpress.XtraEditors.PanelControl panelExpertMode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}