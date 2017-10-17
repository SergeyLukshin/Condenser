namespace Condenser
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.bCancelFind = new DevExpress.XtraEditors.SimpleButton();
            this.bAcceptFind = new DevExpress.XtraEditors.SimpleButton();
            this.tFind = new DevExpress.XtraEditors.TextEdit();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.btnReport = new DevExpress.XtraEditors.SimpleButton();
            this.toolTip = new DevExpress.Utils.ToolTipController(this.components);
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.btnAddTest = new DevExpress.XtraEditors.SimpleButton();
            this.btnAllTest = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnPassport = new DevExpress.XtraEditors.SimpleButton();
            this.btnPassportAdd = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.MainGridControl = new DevExpress.XtraGrid.GridControl();
            this.qMainEquipmentsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSetQuery = new Condenser.DataSetQuery();
            this.MainGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCondenserState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colTestType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTestType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
            this.colNumber = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colVoltage = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCapacity = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colLiquidType = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.defaultSkin = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.qMainEquipmentsTableAdapter = new Condenser.DataSetQueryTableAdapters.QMainEquipmentsTableAdapter();
            this.panelExpertMode = new DevExpress.XtraEditors.PanelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.groupControl4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tFind.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.panelControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridControl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.qMainEquipmentsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTestType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).BeginInit();
            this.panelExpertMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.groupControl4);
            this.panelControl1.Controls.Add(this.groupControl2);
            this.panelControl1.Controls.Add(this.groupControl1);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl1.Location = new System.Drawing.Point(0, 28);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1138, 295);
            this.panelControl1.TabIndex = 1;
            // 
            // groupControl4
            // 
            this.groupControl4.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl4.AppearanceCaption.Options.UseFont = true;
            this.groupControl4.Controls.Add(this.bCancelFind);
            this.groupControl4.Controls.Add(this.bAcceptFind);
            this.groupControl4.Controls.Add(this.tFind);
            this.groupControl4.Location = new System.Drawing.Point(12, 204);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(694, 80);
            this.groupControl4.TabIndex = 18;
            this.groupControl4.Text = "Поиск конденсаторов";
            // 
            // bCancelFind
            // 
            this.bCancelFind.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bCancelFind.Appearance.Options.UseFont = true;
            this.bCancelFind.Image = ((System.Drawing.Image)(resources.GetObject("bCancelFind.Image")));
            this.bCancelFind.Location = new System.Drawing.Point(576, 36);
            this.bCancelFind.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bCancelFind.Name = "bCancelFind";
            this.bCancelFind.Size = new System.Drawing.Size(106, 33);
            this.bCancelFind.TabIndex = 16;
            this.bCancelFind.Text = "Отменить";
            this.bCancelFind.Click += new System.EventHandler(this.bCancelFind_Click);
            // 
            // bAcceptFind
            // 
            this.bAcceptFind.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.bAcceptFind.Appearance.Options.UseFont = true;
            this.bAcceptFind.Image = ((System.Drawing.Image)(resources.GetObject("bAcceptFind.Image")));
            this.bAcceptFind.Location = new System.Drawing.Point(487, 36);
            this.bAcceptFind.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.bAcceptFind.Name = "bAcceptFind";
            this.bAcceptFind.Size = new System.Drawing.Size(81, 33);
            this.bAcceptFind.TabIndex = 15;
            this.bAcceptFind.Text = "Найти";
            this.bAcceptFind.Click += new System.EventHandler(this.bAcceptFind_Click);
            // 
            // tFind
            // 
            this.tFind.Location = new System.Drawing.Point(13, 40);
            this.tFind.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tFind.Name = "tFind";
            this.tFind.Properties.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tFind.Properties.Appearance.Options.UseFont = true;
            this.tFind.Size = new System.Drawing.Size(466, 26);
            this.tFind.TabIndex = 14;
            this.tFind.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tFind_KeyPress);
            // 
            // groupControl2
            // 
            this.groupControl2.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl2.AppearanceCaption.Options.UseFont = true;
            this.groupControl2.Controls.Add(this.labelControl3);
            this.groupControl2.Controls.Add(this.btnReport);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Controls.Add(this.labelControl1);
            this.groupControl2.Controls.Add(this.btnAddTest);
            this.groupControl2.Controls.Add(this.btnAllTest);
            this.groupControl2.Location = new System.Drawing.Point(299, 11);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(407, 182);
            this.groupControl2.TabIndex = 17;
            this.groupControl2.Text = "Испытания";
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl3.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl3.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.labelControl3.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl3.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl3.Location = new System.Drawing.Point(273, 129);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(122, 48);
            this.labelControl3.TabIndex = 17;
            this.labelControl3.Text = "Протокол испытаний";
            // 
            // btnReport
            // 
            this.btnReport.AllowFocus = false;
            this.btnReport.Location = new System.Drawing.Point(273, 42);
            this.btnReport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReport.Name = "btnReport";
            this.btnReport.Size = new System.Drawing.Size(122, 84);
            this.btnReport.TabIndex = 16;
            this.btnReport.TabStop = false;
            this.btnReport.ToolTip = "Протокол испытаний";
            this.btnReport.ToolTipController = this.toolTip;
            this.btnReport.Click += new System.EventHandler(this.btnReport_Click);
            // 
            // toolTip
            // 
            this.toolTip.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.toolTip.Appearance.Options.UseFont = true;
            this.toolTip.Rounded = true;
            this.toolTip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTip_GetActiveObjectInfo);
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl2.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl2.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.labelControl2.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl2.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl2.Location = new System.Drawing.Point(143, 129);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(122, 48);
            this.labelControl2.TabIndex = 16;
            this.labelControl2.Text = "Результаты испытаний";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl1.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl1.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.labelControl1.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl1.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl1.Location = new System.Drawing.Point(14, 129);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(122, 48);
            this.labelControl1.TabIndex = 15;
            this.labelControl1.Text = "Добавить испытание";
            // 
            // btnAddTest
            // 
            this.btnAddTest.AllowFocus = false;
            this.btnAddTest.Location = new System.Drawing.Point(14, 42);
            this.btnAddTest.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnAddTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAddTest.Name = "btnAddTest";
            this.btnAddTest.Size = new System.Drawing.Size(122, 84);
            this.btnAddTest.TabIndex = 12;
            this.btnAddTest.TabStop = false;
            this.btnAddTest.ToolTip = "Добавить испытание";
            this.btnAddTest.ToolTipController = this.toolTip;
            this.btnAddTest.Click += new System.EventHandler(this.btnAddTest_Click);
            // 
            // btnAllTest
            // 
            this.btnAllTest.AllowFocus = false;
            this.btnAllTest.Location = new System.Drawing.Point(143, 42);
            this.btnAllTest.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnAllTest.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAllTest.Name = "btnAllTest";
            this.btnAllTest.Size = new System.Drawing.Size(122, 84);
            this.btnAllTest.TabIndex = 11;
            this.btnAllTest.TabStop = false;
            this.btnAllTest.ToolTip = "Результаты испытаний";
            this.btnAllTest.ToolTipController = this.toolTip;
            this.btnAllTest.Click += new System.EventHandler(this.btnAllTest_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl1.Appearance.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.labelControl7);
            this.groupControl1.Controls.Add(this.labelControl6);
            this.groupControl1.Controls.Add(this.btnPassport);
            this.groupControl1.Controls.Add(this.btnPassportAdd);
            this.groupControl1.Location = new System.Drawing.Point(12, 11);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(277, 182);
            this.groupControl1.TabIndex = 16;
            this.groupControl1.Text = "Конденсаторы";
            // 
            // labelControl7
            // 
            this.labelControl7.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl7.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl7.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.labelControl7.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl7.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl7.Location = new System.Drawing.Point(143, 129);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(122, 48);
            this.labelControl7.TabIndex = 19;
            this.labelControl7.Text = "Паспортные данные";
            // 
            // labelControl6
            // 
            this.labelControl6.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            this.labelControl6.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.labelControl6.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.labelControl6.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.labelControl6.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
            this.labelControl6.Location = new System.Drawing.Point(13, 129);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(122, 48);
            this.labelControl6.TabIndex = 18;
            this.labelControl6.Text = "Добавить конденсатор";
            // 
            // btnPassport
            // 
            this.btnPassport.AllowFocus = false;
            this.btnPassport.Location = new System.Drawing.Point(143, 42);
            this.btnPassport.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPassport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPassport.Name = "btnPassport";
            this.btnPassport.Size = new System.Drawing.Size(122, 84);
            this.btnPassport.TabIndex = 17;
            this.btnPassport.TabStop = false;
            this.btnPassport.ToolTip = "Паспортные данные";
            this.btnPassport.ToolTipController = this.toolTip;
            this.btnPassport.Click += new System.EventHandler(this.btnPassport_Click_1);
            // 
            // btnPassportAdd
            // 
            this.btnPassportAdd.AllowFocus = false;
            this.btnPassportAdd.Location = new System.Drawing.Point(13, 42);
            this.btnPassportAdd.LookAndFeel.UseDefaultLookAndFeel = false;
            this.btnPassportAdd.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPassportAdd.Name = "btnPassportAdd";
            this.btnPassportAdd.Size = new System.Drawing.Size(122, 84);
            this.btnPassportAdd.TabIndex = 16;
            this.btnPassportAdd.TabStop = false;
            this.btnPassportAdd.ToolTip = "Добавить конденсатор";
            this.btnPassportAdd.ToolTipController = this.toolTip;
            this.btnPassportAdd.Click += new System.EventHandler(this.btnPassportAdd_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.MainGridControl);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl2.Location = new System.Drawing.Point(0, 323);
            this.panelControl2.LookAndFeel.SkinName = "Caramel";
            this.panelControl2.LookAndFeel.UseDefaultLookAndFeel = false;
            this.panelControl2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(1138, 228);
            this.panelControl2.TabIndex = 2;
            // 
            // MainGridControl
            // 
            this.MainGridControl.DataSource = this.qMainEquipmentsBindingSource;
            this.MainGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridControl.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MainGridControl.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridControl.Location = new System.Drawing.Point(2, 2);
            this.MainGridControl.MainView = this.MainGridView;
            this.MainGridControl.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MainGridControl.Name = "MainGridControl";
            this.MainGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemTextEdit1,
            this.repTestType});
            this.MainGridControl.Size = new System.Drawing.Size(1134, 224);
            this.MainGridControl.TabIndex = 0;
            this.MainGridControl.ToolTipController = this.toolTip;
            this.MainGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.MainGridView});
            // 
            // qMainEquipmentsBindingSource
            // 
            this.qMainEquipmentsBindingSource.DataMember = "QMainEquipments";
            this.qMainEquipmentsBindingSource.DataSource = this.dataSetQuery;
            // 
            // dataSetQuery
            // 
            this.dataSetQuery.DataSetName = "DataSetQuery";
            this.dataSetQuery.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // MainGridView
            // 
            this.MainGridView.Appearance.ColumnFilterButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.ColumnFilterButton.Options.UseFont = true;
            this.MainGridView.Appearance.ColumnFilterButtonActive.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.ColumnFilterButtonActive.Options.UseFont = true;
            this.MainGridView.Appearance.CustomizationFormHint.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.CustomizationFormHint.Options.UseFont = true;
            this.MainGridView.Appearance.DetailTip.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.DetailTip.Options.UseFont = true;
            this.MainGridView.Appearance.FilterCloseButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.FilterCloseButton.Options.UseFont = true;
            this.MainGridView.Appearance.FilterPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.FilterPanel.Options.UseFont = true;
            this.MainGridView.Appearance.GroupButton.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.GroupButton.Options.UseFont = true;
            this.MainGridView.Appearance.GroupPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.GroupPanel.Options.UseFont = true;
            this.MainGridView.Appearance.GroupRow.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.GroupRow.Options.UseFont = true;
            this.MainGridView.Appearance.HeaderPanel.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.HeaderPanel.Options.UseFont = true;
            this.MainGridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.MainGridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.MainGridView.Appearance.Row.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.MainGridView.Appearance.Row.Options.UseFont = true;
            this.MainGridView.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.MainGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCondenserState,
            this.colTestType,
            this.colNumber,
            this.colType,
            this.colVoltage,
            this.colCapacity,
            this.colLiquidType});
            this.MainGridView.GridControl = this.MainGridControl;
            this.MainGridView.Name = "MainGridView";
            this.MainGridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.MainGridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.MainGridView.OptionsBehavior.Editable = false;
            this.MainGridView.OptionsBehavior.ReadOnly = true;
            this.MainGridView.OptionsFilter.AllowFilterEditor = false;
            this.MainGridView.OptionsHint.ShowColumnHeaderHints = false;
            this.MainGridView.OptionsMenu.EnableColumnMenu = false;
            this.MainGridView.OptionsMenu.EnableGroupPanelMenu = false;
            this.MainGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.MainGridView.OptionsSelection.EnableAppearanceHideSelection = false;
            this.MainGridView.OptionsView.ShowDetailButtons = false;
            this.MainGridView.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.MainGridView.OptionsView.ShowGroupPanel = false;
            this.MainGridView.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.MainGridView_RowCellClick);
            this.MainGridView.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.MainGridView_CustomDrawCell);
            this.MainGridView.ColumnWidthChanged += new DevExpress.XtraGrid.Views.Base.ColumnEventHandler(this.MainGridView_ColumnWidthChanged);
            this.MainGridView.ColumnFilterChanged += new System.EventHandler(this.MainGridView_ColumnFilterChanged);
            this.MainGridView.ShowFilterPopupListBox += new DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventHandler(this.MainGridView_ShowFilterPopupListBox);
            this.MainGridView.DoubleClick += new System.EventHandler(this.MainGridView_DoubleClick);
            // 
            // colCondenserState
            // 
            this.colCondenserState.Caption = " ";
            this.colCondenserState.FieldName = "CondenserState";
            this.colCondenserState.Name = "colCondenserState";
            this.colCondenserState.OptionsColumn.AllowGroup = DevExpress.Utils.DefaultBoolean.False;
            this.colCondenserState.OptionsColumn.AllowMove = false;
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
            this.colTestType.OptionsColumn.AllowMove = false;
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
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("VAL", "Name1")});
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
            this.colNumber.OptionsColumn.AllowMove = false;
            this.colNumber.OptionsFilter.AllowAutoFilter = false;
            this.colNumber.OptionsFilter.AllowFilter = false;
            this.colNumber.Visible = true;
            this.colNumber.VisibleIndex = 2;
            this.colNumber.Width = 164;
            // 
            // colType
            // 
            this.colType.Caption = "Тип конденсатора";
            this.colType.FieldName = "CondenserTypeName";
            this.colType.Name = "colType";
            this.colType.OptionsColumn.AllowMove = false;
            this.colType.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colType.Visible = true;
            this.colType.VisibleIndex = 3;
            this.colType.Width = 167;
            // 
            // colVoltage
            // 
            this.colVoltage.Caption = "Номинальное напряжение, кВ";
            this.colVoltage.FieldName = "NominalVoltage";
            this.colVoltage.Name = "colVoltage";
            this.colVoltage.OptionsColumn.AllowMove = false;
            this.colVoltage.OptionsFilter.AllowAutoFilter = false;
            this.colVoltage.OptionsFilter.AllowFilter = false;
            this.colVoltage.Visible = true;
            this.colVoltage.VisibleIndex = 4;
            this.colVoltage.Width = 167;
            // 
            // colCapacity
            // 
            this.colCapacity.Caption = "Номинальная емкость, мкФ";
            this.colCapacity.FieldName = "NominalCapacitance";
            this.colCapacity.Name = "colCapacity";
            this.colCapacity.OptionsColumn.AllowMove = false;
            this.colCapacity.OptionsFilter.AllowAutoFilter = false;
            this.colCapacity.OptionsFilter.AllowFilter = false;
            this.colCapacity.Visible = true;
            this.colCapacity.VisibleIndex = 5;
            this.colCapacity.Width = 217;
            // 
            // colLiquidType
            // 
            this.colLiquidType.Caption = "Марка изоляционной жидкости";
            this.colLiquidType.FieldName = "InsulatingLiquidTypeName";
            this.colLiquidType.Name = "colLiquidType";
            this.colLiquidType.OptionsColumn.AllowMove = false;
            this.colLiquidType.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.colLiquidType.Visible = true;
            this.colLiquidType.VisibleIndex = 6;
            this.colLiquidType.Width = 225;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.AutoHeight = false;
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // qMainEquipmentsTableAdapter
            // 
            this.qMainEquipmentsTableAdapter.ClearBeforeFill = true;
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
            this.panelExpertMode.Size = new System.Drawing.Size(1138, 28);
            this.panelExpertMode.TabIndex = 3;
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
            // MainForm
            // 
            this.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Appearance.Options.UseFont = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1138, 551);
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.panelExpertMode);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Отбраковка конденсаторов (версия 1.01 от 07.02.2017)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.groupControl4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tFind.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.panelControl2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGridControl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.qMainEquipmentsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSetQuery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTestType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelExpertMode)).EndInit();
            this.panelExpertMode.ResumeLayout(false);
            this.panelExpertMode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraGrid.GridControl MainGridControl;
        private DevExpress.XtraGrid.Views.Grid.GridView MainGridView;
        private DevExpress.XtraGrid.Columns.GridColumn colNumber;
        private DevExpress.XtraGrid.Columns.GridColumn colType;
        private DevExpress.XtraGrid.Columns.GridColumn colVoltage;
        private DevExpress.XtraGrid.Columns.GridColumn colLiquidType;
        private DevExpress.XtraGrid.Columns.GridColumn colCapacity;
        private DevExpress.Utils.ToolTipController toolTip;
        private DevExpress.XtraEditors.SimpleButton btnReport;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        private DevExpress.XtraEditors.SimpleButton btnAddTest;
        private DevExpress.XtraEditors.SimpleButton btnAllTest;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.SimpleButton btnPassportAdd;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SimpleButton bCancelFind;
        private DevExpress.XtraEditors.SimpleButton bAcceptFind;
        private DevExpress.XtraEditors.TextEdit tFind;
        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultSkin;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.SimpleButton btnPassport;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.LabelControl labelControl6;
        private System.Windows.Forms.BindingSource qMainEquipmentsBindingSource;
        private DataSetQuery dataSetQuery;
        private DataSetQueryTableAdapters.QMainEquipmentsTableAdapter qMainEquipmentsTableAdapter;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.PanelControl panelExpertMode;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraGrid.Columns.GridColumn colCondenserState;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraGrid.Columns.GridColumn colTestType;
        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit repTestType;
    }
}

