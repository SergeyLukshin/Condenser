namespace Condenser
{
    partial class CondenserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CondenserForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.controlNavigator1 = new DevExpress.XtraEditors.ControlNavigator();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.GridGC = new DevExpress.XtraGrid.GridControl();
            this.qCondensersBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetQuery = new Condenser.DataSetQuery();
            this.GridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCondenserState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTestType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTestType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVoltage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLiquidType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.toolTip = new DevExpress.Utils.ToolTipController(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.panelExpertMode = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.qCondensersTableAdapter = new Condenser.DataSetQueryTableAdapters.QCondensersTableAdapter();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCondensersBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTestType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).BeginInit();
            this.panelExpertMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.controlNavigator1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1164, 59);
            this.panelControl1.TabIndex = 1;
            // 
            // controlNavigator1
            // 
            this.controlNavigator1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.controlNavigator1.Appearance.Options.UseFont = true;
            this.controlNavigator1.Buttons.Append.Hint = "Добавить конденсатор";
            this.controlNavigator1.Buttons.Append.ImageIndex = 0;
            this.controlNavigator1.Buttons.CancelEdit.Hint = "Отменить изменения";
            this.controlNavigator1.Buttons.CancelEdit.ImageIndex = 3;
            this.controlNavigator1.Buttons.CancelEdit.Visible = false;
            this.controlNavigator1.Buttons.Edit.Hint = "Изменить паспортные данные";
            this.controlNavigator1.Buttons.Edit.ImageIndex = 4;
            this.controlNavigator1.Buttons.Edit.Visible = false;
            this.controlNavigator1.Buttons.EndEdit.Hint = "Подтвердить изменения";
            this.controlNavigator1.Buttons.EndEdit.ImageIndex = 2;
            this.controlNavigator1.Buttons.EndEdit.Visible = false;
            this.controlNavigator1.Buttons.First.Visible = false;
            this.controlNavigator1.Buttons.ImageList = this.imageList1;
            this.controlNavigator1.Buttons.Last.Visible = false;
            this.controlNavigator1.Buttons.Next.Visible = false;
            this.controlNavigator1.Buttons.NextPage.Visible = false;
            this.controlNavigator1.Buttons.Prev.Visible = false;
            this.controlNavigator1.Buttons.PrevPage.Visible = false;
            this.controlNavigator1.Buttons.Remove.Hint = "Удалить конденсатор";
            this.controlNavigator1.Buttons.Remove.ImageIndex = 1;
            this.controlNavigator1.CustomButtons.AddRange(new DevExpress.XtraEditors.NavigatorCustomButton[] {
            new DevExpress.XtraEditors.NavigatorCustomButton(8, 4, "Изменить паспортные данные")});
            this.controlNavigator1.Location = new System.Drawing.Point(7, 6);
            this.controlNavigator1.LookAndFeel.SkinName = "Caramel";
            this.controlNavigator1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.controlNavigator1.Name = "controlNavigator1";
            this.controlNavigator1.NavigatableControl = this.GridGC;
            this.controlNavigator1.ShowToolTips = true;
            this.controlNavigator1.Size = new System.Drawing.Size(232, 48);
            this.controlNavigator1.TabIndex = 2;
            this.controlNavigator1.Text = "controlNavigator1";
            this.controlNavigator1.ToolTipController = this.toolTip;
            this.controlNavigator1.ButtonClick += new DevExpress.XtraEditors.NavigatorButtonClickEventHandler(this.controlNavigator1_ButtonClick);
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
            this.GridGC.DataSource = this.qCondensersBindingSource;
            this.GridGC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridGC.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridGC.Location = new System.Drawing.Point(0, 87);
            this.GridGC.MainView = this.GridView;
            this.GridGC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Name = "GridGC";
            this.GridGC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repTestType});
            this.GridGC.Size = new System.Drawing.Size(1164, 498);
            this.GridGC.TabIndex = 2;
            this.GridGC.ToolTipController = this.toolTip;
            this.GridGC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            // 
            // qCondensersBindingSource
            // 
            this.qCondensersBindingSource.DataMember = "QCondensers";
            this.qCondensersBindingSource.DataSource = this.dataSetQuery;
            // 
            // dataSetQuery
            // 
            this.dataSetQuery.DataSetName = "DataSetQuery";
            this.dataSetQuery.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GridView
            // 
            this.GridView.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.GridView.Appearance.ColumnFilterButtonActive.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
            this.GridView.Appearance.CustomizationFormHint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.CustomizationFormHint.Options.UseFont = true;
            this.GridView.Appearance.DetailTip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.DetailTip.Options.UseFont = true;
            this.GridView.Appearance.FilterCloseButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.FilterCloseButton.Options.UseFont = true;
            this.GridView.Appearance.FilterPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.FilterPanel.Options.UseFont = true;
            this.GridView.Appearance.GroupButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.GroupButton.Options.UseFont = true;
            this.GridView.Appearance.GroupPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.GroupPanel.Options.UseFont = true;
            this.GridView.Appearance.GroupRow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.GroupRow.Options.UseFont = true;
            this.GridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.GridView.Appearance.Row.Options.UseFont = true;
            this.GridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCondenserState,
            this.colTestType,
            this.colNumber,
            this.colType,
            this.colVoltage,
            this.colCapacity,
            this.colLiquidType});
            this.GridView.GridControl = this.GridGC;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this.GridView.OptionsDetail.EnableMasterViewMode = false;
            this.GridView.OptionsFilter.AllowFilterEditor = false;
            this.GridView.OptionsHint.ShowColumnHeaderHints = false;
            this.GridView.OptionsMenu.EnableColumnMenu = false;
            this.GridView.OptionsMenu.EnableGroupPanelMenu = false;
            this.GridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridView.OptionsView.ShowDetailButtons = false;
            this.GridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.GridView.OptionsView.ShowGroupPanel = false;
            this.GridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GridView_CustomDrawCell);
            this.GridView.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.GridView_ColumnWidthChanged);
            this.GridView.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
            this.GridView.ShowFilterPopupListBox += new DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventHandler(this.GridView_ShowFilterPopupListBox);
            this.GridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridViewView_KeyDown);
            this.GridView.DoubleClick += new System.EventHandler(this.GridView_DoubleClick);
            // 
            // colCondenserState
            // 
            this.colCondenserState.Caption = " ";
            this.colCondenserState.FieldName = "CondenserState";
            this.colCondenserState.Name = "colCondenserState";
            this.colCondenserState.OptionsColumn.AllowEdit = false;
            this.colCondenserState.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colCondenserState.OptionsColumn.AllowMove = false;
            this.colCondenserState.OptionsColumn.ReadOnly = true;
            this.colCondenserState.OptionsFilter.AllowAutoFilter = false;
            this.colCondenserState.OptionsFilter.AllowFilter = false;
            this.colCondenserState.Visible = true;
            this.colCondenserState.VisibleIndex = 0;
            this.colCondenserState.Width = 28;
            // 
            // colTestType
            // 
            this.colTestType.Caption = "Текущий вид испытаний";
            this.colTestType.ColumnEdit = this.repTestType;
            this.colTestType.FieldName = "CondenserTestType";
            this.colTestType.Name = "colTestType";
            this.colTestType.OptionsColumn.AllowEdit = false;
            this.colTestType.OptionsColumn.AllowMove = false;
            this.colTestType.OptionsColumn.ReadOnly = true;
            this.colTestType.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colTestType.Visible = true;
            this.colTestType.VisibleIndex = 1;
            this.colTestType.Width = 150;
            // 
            // repTestType
            // 
            this.repTestType.AutoHeight = false;
            this.repTestType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repTestType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VAL", "Name2")});
            this.repTestType.Name = "repTestType";
            this.repTestType.NullText = "";
            this.repTestType.ShowFooter = false;
            this.repTestType.ShowHeader = false;
            // 
            // colNumber
            // 
            this.colNumber.Caption = "Идентификационный номер";
            this.colNumber.FieldName = "CondenserNumber";
            this.colNumber.Name = "colNumber";
            this.colNumber.OptionsColumn.AllowEdit = false;
            this.colNumber.OptionsColumn.AllowMove = false;
            this.colNumber.OptionsColumn.ReadOnly = true;
            this.colNumber.OptionsFilter.AllowAutoFilter = false;
            this.colNumber.OptionsFilter.AllowFilter = false;
            this.colNumber.Visible = true;
            this.colNumber.VisibleIndex = 2;
            this.colNumber.Width = 171;
            // 
            // colType
            // 
            this.colType.Caption = "Тип конденсатора";
            this.colType.FieldName = "CondenserTypeName";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowEdit = false;
            this.colType.OptionsColumn.AllowMove = false;
            this.colType.OptionsColumn.ReadOnly = true;
            this.colType.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 3;
            this.colType.Width = 174;
            // 
            // colVoltage
            // 
            this.colVoltage.Caption = "Номинальное напряжение, кВ";
            this.colVoltage.FieldName = "NominalVoltage";
            this.colVoltage.Name = "colVoltage";
            this.colVoltage.OptionsColumn.AllowEdit = false;
            this.colVoltage.OptionsColumn.AllowMove = false;
            this.colVoltage.OptionsColumn.ReadOnly = true;
            this.colVoltage.OptionsFilter.AllowAutoFilter = false;
            this.colVoltage.OptionsFilter.AllowFilter = false;
            this.colVoltage.Visible = true;
            this.colVoltage.VisibleIndex = 4;
            this.colVoltage.Width = 174;
            // 
            // colCapacity
            // 
            this.colCapacity.Caption = "Номинальная емкость, мкФ";
            this.colCapacity.FieldName = "NominalCapacitance";
            this.colCapacity.Name = "colCapacity";
            this.colCapacity.OptionsColumn.AllowEdit = false;
            this.colCapacity.OptionsColumn.AllowMove = false;
            this.colCapacity.OptionsColumn.ReadOnly = true;
            this.colCapacity.OptionsFilter.AllowAutoFilter = false;
            this.colCapacity.OptionsFilter.AllowFilter = false;
            this.colCapacity.Visible = true;
            this.colCapacity.VisibleIndex = 5;
            this.colCapacity.Width = 224;
            // 
            // colLiquidType
            // 
            this.colLiquidType.Caption = "Марка изоляционной жидкости";
            this.colLiquidType.FieldName = "InsulatingLiquidTypeName";
            this.colLiquidType.Name = "colLiquidType";
            this.colLiquidType.OptionsColumn.AllowEdit = false;
            this.colLiquidType.OptionsColumn.AllowMove = false;
            this.colLiquidType.OptionsColumn.ReadOnly = true;
            this.colLiquidType.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colLiquidType.Visible = true;
            this.colLiquidType.VisibleIndex = 6;
            this.colLiquidType.Width = 227;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // toolTip
            // 
            this.toolTip.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolTip.Appearance.Options.UseFont = true;
            this.toolTip.Rounded = true;
            this.toolTip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTip_GetActiveObjectInfo);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "visual.png");
            this.imageList2.Images.SetKeyName(1, "FHA.png");
            this.imageList2.Images.SetKeyName(2, "HARG.png");
            this.imageList2.Images.SetKeyName(3, "warm.png");
            this.imageList2.Images.SetKeyName(4, "vibro.png");
            this.imageList2.Images.SetKeyName(5, "parameter_small.jpg");
            this.imageList2.Images.SetKeyName(6, "electrical_small.jpg");
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
            this.panelExpertMode.Size = new System.Drawing.Size(1164, 28);
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
            // qCondensersTableAdapter
            // 
            this.qCondensersTableAdapter.ClearBeforeFill = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CondenserForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1164, 585);
            this.Controls.Add(this.GridGC);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelExpertMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Caramel";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "CondenserForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Перечень конденсаторов";
            this.Load += new System.EventHandler(this.CondenserForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCondensersBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTestType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).EndInit();
            this.panelExpertMode.ResumeLayout(false);
            this.panelExpertMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private System.Windows.Forms.ImageList imageList1;
        private DevExpress.XtraEditors.ControlNavigator controlNavigator1;
        private DevExpress.Utils.ToolTipController toolTip;
        private System.Windows.Forms.ImageList imageList2;
        private DevExpress.XtraGrid.GridControl GridGC;
        private DevExpress.XtraGrid.Views.Grid.GridView GridView;
        private DevExpress.XtraGrid.Columns.GridColumn colCondenserState;
        private DevExpress.XtraGrid.Columns.GridColumn colNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
        private DevExpress.XtraGrid.Columns.GridColumn colVoltage;
        private DevExpress.XtraGrid.Columns.GridColumn colCapacity;
        private DevExpress.XtraGrid.Columns.GridColumn colLiquidType;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.PanelControl panelExpertMode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DataSetQuery dataSetQuery;
        private System.Windows.Forms.BindingSource qCondensersBindingSource;
        private DataSetQueryTableAdapters.QCondensersTableAdapter qCondensersTableAdapter;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repTestType;
        private DevExpress.XtraGrid.Columns.GridColumn colTestType;
    }
}