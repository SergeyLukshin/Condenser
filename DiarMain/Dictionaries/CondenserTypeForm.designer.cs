namespace Condenser
{
    partial class CondenserTypeForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CondenserTypeForm));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.cbCanEdit = new DevExpress.XtraEditors.CheckEdit();
            this.controlNavigator1 = new DevExpress.XtraEditors.ControlNavigator();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.GridGC = new DevExpress.XtraGrid.GridControl();
            this.qCondenserTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetQuery = new Condenser.DataSetQuery();
            this.GridView = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCondenserTypeName = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colStatus = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repStatus = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand5 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colInsulatingLiquidType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repInsulatingLiquidType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.qInsulatingLiquidTypesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridBand6 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colDielectricType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repDielectricType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridBand7 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colDielectricThickness = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repDigit4 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridBand8 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colTangentAngle = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repDecimal3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridBand9 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colDielectricInductiveCapacity = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridBand10 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCasingType = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.repCasingType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.gridBand11 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.colCasingThickness = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.toolTip = new DevExpress.Utils.ToolTipController(this.components);
            this.panelSelect = new DevExpress.XtraEditors.PanelControl();
            this.bSelect = new DevExpress.XtraEditors.SimpleButton();
            this.qCondenserTypesTableAdapter = new Condenser.DataSetQueryTableAdapters.QCondenserTypesTableAdapter();
            this.qInsulatingLiquidTypesTableAdapter = new Condenser.DataSetQueryTableAdapters.QInsulatingLiquidTypesTableAdapter();
            this.panelExpertMode = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cbCanEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCondenserTypesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repInsulatingLiquidType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qInsulatingLiquidTypesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDielectricType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDigit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDecimal3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCasingType)).BeginInit();
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
            this.panelControl1.Size = new System.Drawing.Size(1237, 58);
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
            this.GridGC.DataSource = this.qCondenserTypesBindingSource;
            this.GridGC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridGC.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Location = new System.Drawing.Point(0, 86);
            this.GridGC.MainView = this.GridView;
            this.GridGC.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.GridGC.Name = "GridGC";
            this.GridGC.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repDigit4,
            this.repDecimal3,
            this.repInsulatingLiquidType,
            this.repDielectricType,
            this.repCasingType,
            this.repStatus});
            this.GridGC.Size = new System.Drawing.Size(1237, 451);
            this.GridGC.TabIndex = 2;
            this.GridGC.ToolTipController = this.toolTip;
            this.GridGC.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridView});
            this.GridGC.Click += new System.EventHandler(this.GridGC_Click);
            // 
            // qCondenserTypesBindingSource
            // 
            this.qCondenserTypesBindingSource.DataMember = "QCondenserTypes";
            this.qCondenserTypesBindingSource.DataSource = this.dataSetQuery;
            this.qCondenserTypesBindingSource.AddingNew += new System.ComponentModel.AddingNewEventHandler(this.qCondenserTypesBindingSource_AddingNew);
            // 
            // dataSetQuery
            // 
            this.dataSetQuery.DataSetName = "DataSetQuery";
            this.dataSetQuery.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GridView
            // 
            this.GridView.Appearance.BandPanel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.GridView.Appearance.BandPanel.Options.UseFont = true;
            this.GridView.Appearance.BandPanel.Options.UseTextOptions = true;
            this.GridView.Appearance.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.GridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.GridView.Appearance.Row.Options.UseFont = true;
            this.GridView.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand4,
            this.gridBand2,
            this.gridBand3});
            this.GridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GridView.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.colCondenserTypeName,
            this.colStatus,
            this.colInsulatingLiquidType,
            this.colDielectricType,
            this.colDielectricThickness,
            this.colTangentAngle,
            this.colDielectricInductiveCapacity,
            this.colCasingType,
            this.colCasingThickness});
            this.GridView.GridControl = this.GridGC;
            this.GridView.Name = "GridView";
            this.GridView.OptionsBehavior.Editable = false;
            this.GridView.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
            this.GridView.OptionsCustomization.AllowBandMoving = false;
            this.GridView.OptionsDetail.EnableMasterViewMode = false;
            this.GridView.OptionsHint.ShowColumnHeaderHints = false;
            this.GridView.OptionsMenu.EnableColumnMenu = false;
            this.GridView.OptionsNavigation.AutoFocusNewRow = true;
            this.GridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.GridView.OptionsSelection.InvertSelection = true;
            this.GridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GridView.OptionsView.ColumnAutoWidth = false;
            this.GridView.OptionsView.ShowColumnHeaders = false;
            this.GridView.OptionsView.ShowGroupPanel = false;
            this.GridView.BandWidthChanged += new DevExpress.XtraGrid.Views.BandedGrid.BandEventHandler(this.GridView_BandWidthChanged);
            this.GridView.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.GridView_ShowingEditor);
            this.GridView.InitNewRow += new DevExpress.XtraGrid.Views.Grid.InitNewRowEventHandler(this.GridView_InitNewRow);
            this.GridView.InvalidRowException += new DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventHandler(this.GridView_InvalidRowException_1);
            this.GridView.ValidateRow += new DevExpress.XtraGrid.Views.Base.ValidateRowEventHandler(this.GridView_ValidateRow);
            this.GridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GridView_KeyDown);
            this.GridView.ValidatingEditor += new DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventHandler(this.GridView_ValidatingEditor);
            // 
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand1.Caption = "Наименование";
            this.gridBand1.Columns.Add(this.colCondenserTypeName);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.RowCount = 2;
            this.gridBand1.Width = 173;
            // 
            // colCondenserTypeName
            // 
            this.colCondenserTypeName.Caption = "Наименование";
            this.colCondenserTypeName.FieldName = "CondenserTypeName";
            this.colCondenserTypeName.Name = "colCondenserTypeName";
            this.colCondenserTypeName.OptionsFilter.AllowAutoFilter = false;
            this.colCondenserTypeName.OptionsFilter.AllowFilter = false;
            this.colCondenserTypeName.Visible = true;
            this.colCondenserTypeName.Width = 173;
            // 
            // gridBand4
            // 
            this.gridBand4.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.gridBand4.AppearanceHeader.Options.UseFont = true;
            this.gridBand4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand4.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand4.Caption = "Статус";
            this.gridBand4.Columns.Add(this.colStatus);
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.RowCount = 2;
            this.gridBand4.Width = 200;
            // 
            // colStatus
            // 
            this.colStatus.AppearanceCell.Options.UseTextOptions = true;
            this.colStatus.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colStatus.Caption = "Статус";
            this.colStatus.ColumnEdit = this.repStatus;
            this.colStatus.FieldName = "Status";
            this.colStatus.Name = "colStatus";
            this.colStatus.OptionsColumn.AllowEdit = false;
            this.colStatus.OptionsColumn.ReadOnly = true;
            this.colStatus.OptionsFilter.AllowAutoFilter = false;
            this.colStatus.OptionsFilter.AllowFilter = false;
            this.colStatus.Visible = true;
            this.colStatus.Width = 200;
            // 
            // repStatus
            // 
            this.repStatus.AutoHeight = false;
            this.repStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repStatus.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VAL", "Name2")});
            this.repStatus.DisplayMember = "VAL";
            this.repStatus.Name = "repStatus";
            this.repStatus.NullText = "";
            this.repStatus.ShowHeader = false;
            this.repStatus.ShowLines = false;
            this.repStatus.ValueMember = "KEY";
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand2.Caption = "Изоляция";
            this.gridBand2.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand5,
            this.gridBand6,
            this.gridBand7,
            this.gridBand8,
            this.gridBand9});
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.OptionsBand.AllowMove = false;
            this.gridBand2.Width = 820;
            // 
            // gridBand5
            // 
            this.gridBand5.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand5.AppearanceHeader.Options.UseFont = true;
            this.gridBand5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand5.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand5.Caption = "Марка изоляционной жидкости";
            this.gridBand5.Columns.Add(this.colInsulatingLiquidType);
            this.gridBand5.Name = "gridBand5";
            this.gridBand5.Width = 200;
            // 
            // colInsulatingLiquidType
            // 
            this.colInsulatingLiquidType.AppearanceCell.Options.UseTextOptions = true;
            this.colInsulatingLiquidType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colInsulatingLiquidType.Caption = "Марка изоляционной жидкости";
            this.colInsulatingLiquidType.ColumnEdit = this.repInsulatingLiquidType;
            this.colInsulatingLiquidType.FieldName = "InsulatingLiquidTypeID";
            this.colInsulatingLiquidType.Name = "colInsulatingLiquidType";
            this.colInsulatingLiquidType.OptionsFilter.AllowAutoFilter = false;
            this.colInsulatingLiquidType.OptionsFilter.AllowFilter = false;
            this.colInsulatingLiquidType.Visible = true;
            this.colInsulatingLiquidType.Width = 200;
            // 
            // repInsulatingLiquidType
            // 
            this.repInsulatingLiquidType.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.repInsulatingLiquidType.AppearanceDropDown.Options.UseFont = true;
            this.repInsulatingLiquidType.AutoHeight = false;
            this.repInsulatingLiquidType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo),
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis, "", -1, true, true, false, DevExpress.XtraEditors.ImageLocation.MiddleCenter, null, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject2, "Открыть справочник \"Марка изоляционной жидкости\"", null, null, true)});
            this.repInsulatingLiquidType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("InsulatingLiquidTypeName", "Insulating Liquid Type Name", 144, DevExpress.Utils.FormatType.None, "", true, DevExpress.Utils.HorzAlignment.Near)});
            this.repInsulatingLiquidType.DataSource = this.qInsulatingLiquidTypesBindingSource;
            this.repInsulatingLiquidType.DisplayMember = "InsulatingLiquidTypeName";
            this.repInsulatingLiquidType.Name = "repInsulatingLiquidType";
            this.repInsulatingLiquidType.NullText = "";
            this.repInsulatingLiquidType.ShowFooter = false;
            this.repInsulatingLiquidType.ShowHeader = false;
            this.repInsulatingLiquidType.ValueMember = "InsulatingLiquidTypeID";
            this.repInsulatingLiquidType.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.repInsulatingLiquidType_ButtonClick);
            this.repInsulatingLiquidType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rep_KeyUp);
            // 
            // qInsulatingLiquidTypesBindingSource
            // 
            this.qInsulatingLiquidTypesBindingSource.DataMember = "QInsulatingLiquidTypes";
            this.qInsulatingLiquidTypesBindingSource.DataSource = this.dataSetQuery;
            // 
            // gridBand6
            // 
            this.gridBand6.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand6.AppearanceHeader.Options.UseFont = true;
            this.gridBand6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand6.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand6.Caption = "Тип диэлектрика";
            this.gridBand6.Columns.Add(this.colDielectricType);
            this.gridBand6.Name = "gridBand6";
            this.gridBand6.Width = 300;
            // 
            // colDielectricType
            // 
            this.colDielectricType.AppearanceCell.Options.UseTextOptions = true;
            this.colDielectricType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colDielectricType.Caption = "Тип диэлектрика";
            this.colDielectricType.ColumnEdit = this.repDielectricType;
            this.colDielectricType.FieldName = "DielectricType";
            this.colDielectricType.Name = "colDielectricType";
            this.colDielectricType.OptionsFilter.AllowAutoFilter = false;
            this.colDielectricType.OptionsFilter.AllowFilter = false;
            this.colDielectricType.Visible = true;
            this.colDielectricType.Width = 300;
            // 
            // repDielectricType
            // 
            this.repDielectricType.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.repDielectricType.AppearanceDropDown.Options.UseFont = true;
            this.repDielectricType.AutoHeight = false;
            this.repDielectricType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repDielectricType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VAL", "Name6")});
            this.repDielectricType.DisplayMember = "VAL";
            this.repDielectricType.Name = "repDielectricType";
            this.repDielectricType.NullText = "";
            this.repDielectricType.ShowFooter = false;
            this.repDielectricType.ShowHeader = false;
            this.repDielectricType.ValueMember = "KEY";
            this.repDielectricType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rep_KeyUp);
            // 
            // gridBand7
            // 
            this.gridBand7.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand7.AppearanceHeader.Options.UseFont = true;
            this.gridBand7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand7.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand7.Caption = "Толщина диэлектрика, мкм";
            this.gridBand7.Columns.Add(this.colDielectricThickness);
            this.gridBand7.Name = "gridBand7";
            this.gridBand7.Width = 100;
            // 
            // colDielectricThickness
            // 
            this.colDielectricThickness.AppearanceCell.Options.UseTextOptions = true;
            this.colDielectricThickness.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colDielectricThickness.Caption = "Толщина диэлектрика, мкм";
            this.colDielectricThickness.ColumnEdit = this.repDigit4;
            this.colDielectricThickness.FieldName = "DielectricThickness";
            this.colDielectricThickness.Name = "colDielectricThickness";
            this.colDielectricThickness.OptionsFilter.AllowAutoFilter = false;
            this.colDielectricThickness.OptionsFilter.AllowFilter = false;
            this.colDielectricThickness.Visible = true;
            this.colDielectricThickness.Width = 100;
            // 
            // repDigit4
            // 
            this.repDigit4.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.repDigit4.AutoHeight = false;
            this.repDigit4.Mask.EditMask = "\\d+";
            this.repDigit4.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.repDigit4.Mask.ShowPlaceHolders = false;
            this.repDigit4.MaxLength = 6;
            this.repDigit4.Name = "repDigit4";
            // 
            // gridBand8
            // 
            this.gridBand8.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand8.AppearanceHeader.Options.UseFont = true;
            this.gridBand8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand8.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand8.Caption = "Тангенс угла диэлектрических потерь";
            this.gridBand8.Columns.Add(this.colTangentAngle);
            this.gridBand8.Name = "gridBand8";
            this.gridBand8.Width = 100;
            // 
            // colTangentAngle
            // 
            this.colTangentAngle.AppearanceCell.Options.UseTextOptions = true;
            this.colTangentAngle.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colTangentAngle.Caption = "Тангенс угла диэлектрических потерь";
            this.colTangentAngle.ColumnEdit = this.repDecimal3;
            this.colTangentAngle.FieldName = "TangentAngle";
            this.colTangentAngle.Name = "colTangentAngle";
            this.colTangentAngle.OptionsFilter.AllowAutoFilter = false;
            this.colTangentAngle.OptionsFilter.AllowFilter = false;
            this.colTangentAngle.Visible = true;
            this.colTangentAngle.Width = 100;
            // 
            // repDecimal3
            // 
            this.repDecimal3.AllowNullInput = DevExpress.Utils.DefaultBoolean.True;
            this.repDecimal3.AutoHeight = false;
            this.repDecimal3.Mask.EditMask = "(\\d+|\\d+,\\d|\\d+,\\d\\d|\\d+,\\d\\d\\d)";
            this.repDecimal3.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.repDecimal3.Mask.ShowPlaceHolders = false;
            this.repDecimal3.Name = "repDecimal3";
            this.repDecimal3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.repDecimal3_KeyPress);
            // 
            // gridBand9
            // 
            this.gridBand9.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand9.AppearanceHeader.Options.UseFont = true;
            this.gridBand9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand9.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand9.Caption = "Относительная диэлектрическая проницаемость";
            this.gridBand9.Columns.Add(this.colDielectricInductiveCapacity);
            this.gridBand9.Name = "gridBand9";
            this.gridBand9.Width = 120;
            // 
            // colDielectricInductiveCapacity
            // 
            this.colDielectricInductiveCapacity.AppearanceCell.Options.UseTextOptions = true;
            this.colDielectricInductiveCapacity.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colDielectricInductiveCapacity.Caption = "Относительная диэлектрическая проницаемость";
            this.colDielectricInductiveCapacity.ColumnEdit = this.repDecimal3;
            this.colDielectricInductiveCapacity.FieldName = "DielectricInductiveCapacity";
            this.colDielectricInductiveCapacity.Name = "colDielectricInductiveCapacity";
            this.colDielectricInductiveCapacity.OptionsFilter.AllowAutoFilter = false;
            this.colDielectricInductiveCapacity.OptionsFilter.AllowFilter = false;
            this.colDielectricInductiveCapacity.Visible = true;
            this.colDielectricInductiveCapacity.Width = 120;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.gridBand3.AppearanceHeader.Options.UseFont = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand3.Caption = "Обкладки";
            this.gridBand3.Children.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand10,
            this.gridBand11});
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.OptionsBand.AllowMove = false;
            this.gridBand3.Width = 450;
            // 
            // gridBand10
            // 
            this.gridBand10.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand10.AppearanceHeader.Options.UseFont = true;
            this.gridBand10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand10.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand10.Caption = "Тип обкладок";
            this.gridBand10.Columns.Add(this.colCasingType);
            this.gridBand10.Name = "gridBand10";
            this.gridBand10.Width = 350;
            // 
            // colCasingType
            // 
            this.colCasingType.AppearanceCell.Options.UseTextOptions = true;
            this.colCasingType.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colCasingType.Caption = "Тип обкладок";
            this.colCasingType.ColumnEdit = this.repCasingType;
            this.colCasingType.FieldName = "CasingType";
            this.colCasingType.Name = "colCasingType";
            this.colCasingType.OptionsFilter.AllowAutoFilter = false;
            this.colCasingType.OptionsFilter.AllowFilter = false;
            this.colCasingType.Visible = true;
            this.colCasingType.Width = 350;
            // 
            // repCasingType
            // 
            this.repCasingType.AppearanceDropDown.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.repCasingType.AppearanceDropDown.Options.UseFont = true;
            this.repCasingType.AutoHeight = false;
            this.repCasingType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repCasingType.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VAL", "Name7")});
            this.repCasingType.DisplayMember = "VAL";
            this.repCasingType.Name = "repCasingType";
            this.repCasingType.NullText = "";
            this.repCasingType.ShowHeader = false;
            this.repCasingType.ShowLines = false;
            this.repCasingType.ValueMember = "KEY";
            this.repCasingType.KeyUp += new System.Windows.Forms.KeyEventHandler(this.rep_KeyUp);
            // 
            // gridBand11
            // 
            this.gridBand11.AppearanceHeader.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.gridBand11.AppearanceHeader.Options.UseFont = true;
            this.gridBand11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand11.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridBand11.Caption = "Толщина обкладки, мкм";
            this.gridBand11.Columns.Add(this.colCasingThickness);
            this.gridBand11.Name = "gridBand11";
            this.gridBand11.Width = 100;
            // 
            // colCasingThickness
            // 
            this.colCasingThickness.AppearanceCell.Options.UseTextOptions = true;
            this.colCasingThickness.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.colCasingThickness.Caption = "Толщина обкладки, мкм";
            this.colCasingThickness.ColumnEdit = this.repDigit4;
            this.colCasingThickness.FieldName = "CasingThickness";
            this.colCasingThickness.Name = "colCasingThickness";
            this.colCasingThickness.OptionsFilter.AllowAutoFilter = false;
            this.colCasingThickness.OptionsFilter.AllowFilter = false;
            this.colCasingThickness.Visible = true;
            this.colCasingThickness.Width = 100;
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
            this.panelSelect.Size = new System.Drawing.Size(1237, 48);
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
            // qCondenserTypesTableAdapter
            // 
            this.qCondenserTypesTableAdapter.ClearBeforeFill = true;
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
            this.panelExpertMode.Size = new System.Drawing.Size(1237, 28);
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
            // CondenserTypeForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1237, 585);
            this.Controls.Add(this.GridGC);
            this.Controls.Add(this.panelSelect);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelExpertMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Caramel";
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimizeBox = false;
            this.Name = "CondenserTypeForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Тип конденсатора";
            this.Load += new System.EventHandler(this.CondenserTypeForm_Load);
            this.SizeChanged += new System.EventHandler(this.CondenserTypeForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cbCanEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridGC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qCondenserTypesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repInsulatingLiquidType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qInsulatingLiquidTypesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDielectricType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDigit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repDecimal3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repCasingType)).EndInit();
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
        private DevExpress.XtraEditors.ControlNavigator controlNavigator1;
        private DevExpress.XtraEditors.CheckEdit cbCanEdit;
        private DevExpress.Utils.ToolTipController toolTip;
        private DevExpress.XtraEditors.PanelControl panelSelect;
        private DevExpress.XtraEditors.SimpleButton bSelect;
        private DataSetQuery dataSetQuery;
        private System.Windows.Forms.BindingSource qCondenserTypesBindingSource;
        private DataSetQueryTableAdapters.QCondenserTypesTableAdapter qCondenserTypesTableAdapter;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GridView;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCondenserTypeName;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colInsulatingLiquidType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDielectricType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDielectricThickness;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colTangentAngle;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colDielectricInductiveCapacity;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCasingType;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colCasingThickness;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repDigit4;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repDecimal3;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repInsulatingLiquidType;
        private System.Windows.Forms.BindingSource qInsulatingLiquidTypesBindingSource;
        private DataSetQueryTableAdapters.QInsulatingLiquidTypesTableAdapter qInsulatingLiquidTypesTableAdapter;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repDielectricType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repCasingType;
        private DevExpress.XtraEditors.PanelControl panelExpertMode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn colStatus;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand5;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand6;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand7;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand8;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand9;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand10;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand11;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repStatus;
    }
}