using System;
using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraBars;
using DevExpress.Utils;

namespace Condenser
{
    public partial class MainForm : DevExpress.XtraEditors.XtraForm
    {
        [SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "COMPARE_STR")]
        class COMPARE_STR : SQLiteFunction
        {
            public override object Invoke(object[] args)
            {
                string s1 = (args[0] as string).ToLower();
                string s2 = (args[1] as string).ToLower();
                return s1.IndexOf(s2);
            }
        }

        [SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "EQUAL_STR")]
        class EQUAL_STR : SQLiteFunction
        {
            public override object Invoke(object[] args)
            {
                string s1 = (args[0] as string).ToLower();
                string s2 = (args[1] as string).ToLower();
                return s1 == s2 ? 0 : 1;
            }
        }

        bool m_bShowCloseMsg;
        string m_strFilterString = "";
        string m_strIDsString = "";
        public bool m_bAddPassportMessage = false;
        public string m_strLicenseCode;
        private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string,int>();
        BarSubItem subSettingsMenu = null;
        BindingList<DataSourceString> listTestType = new BindingList<DataSourceString>();

        float DpiXRel;
        float DpiYRel;

        public MainForm()
        {
            InitializeComponent();
            defaultSkin.LookAndFeel.SetSkinStyle("Black");
            CreateMenu();
        }

        public void CreateMenu()
        {
            BarManager barManager = new BarManager();
            barManager.Form = this;
            // Prevent excessive updates while adding and customizing bars and bar items.
            // The BeginUpdate must match the EndUpdate method.
            barManager.BeginUpdate();
            // Create two bars and dock them to the top of the form.
            // Bar1 - is a main menu, which is stretched to match the form's width.
            // Bar2 - is a regular bar.
            Bar bar1 = new Bar(barManager, "Главное меню");
            bar1.DockStyle = BarDockStyle.Top;
            bar1.DockRow = 0;
            barManager.MainMenu = bar1;
            bar1.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            bar1.Appearance.Options.UseFont = true;
            bar1.OptionsBar.MultiLine = true;
            bar1.OptionsBar.UseWholeRow = true;
            bar1.OptionsBar.AllowQuickCustomization = false;
            bar1.OptionsBar.AllowCollapse = false;

            // Create bar items for the bar1 and bar2
            BarSubItem subDictMenu = new BarSubItem(barManager, "Справочники");
            subSettingsMenu = new BarSubItem(barManager, "Настройки");
            BarSubItem subAboutMenu = new BarSubItem(barManager, "О программе");

            BarButtonItem btnCondenserType = new BarButtonItem(barManager, "Тип конденсатора");
            btnCondenserType.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            btnCondenserType.Appearance.Options.UseFont = true;

            BarButtonItem btnInsulatingLiquidType = new BarButtonItem(barManager, "Марка изоляционной жидкости");
            btnInsulatingLiquidType.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            btnInsulatingLiquidType.Appearance.Options.UseFont = true;

            BarButtonItem btnCondensers = new BarButtonItem(barManager, "Перечень конденсаторов");
            btnCondensers.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            btnCondensers.Appearance.Options.UseFont = true;

            subDictMenu.AddItems(new BarItem[] { btnCondenserType, btnInsulatingLiquidType, btnCondensers });
            subDictMenu.ItemLinks[2].BeginGroup = true;

            //btnAdminMode = new BarButtonItem(barManager, "Режим Администратора");
            //btnAdminMode.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            //btnAdminMode.Appearance.Options.UseFont = true;

            //btnParameters = new BarButtonItem(barManager, "Параметры");
            //btnParameters.Appearance.Font = new System.Drawing.Font("Segoe UI", 11.25F);
            //btnParameters.Appearance.Options.UseFont = true;

            //subSettingsMenu.AddItems(new BarItem[] { btnParameters });
            if (Program.m_bExpertMode)
                subSettingsMenu.Visibility = BarItemVisibility.Always;
            else
                subSettingsMenu.Visibility = BarItemVisibility.Never;

            //Add the sub-menus to the bar1
            bar1.AddItems(new BarItem[] { subDictMenu, subSettingsMenu, subAboutMenu });

            btnCondenserType.ItemClick += new ItemClickEventHandler(CondenserType_ItemClick);
            btnInsulatingLiquidType.ItemClick += new ItemClickEventHandler(InsulatingLiquidType_ItemClick);
            subAboutMenu.ItemClick += new ItemClickEventHandler(AboutSubMenu_ItemClick);
            subSettingsMenu.ItemClick += new ItemClickEventHandler(subSettingsMenu_ItemClick);
            btnCondensers.ItemClick += new ItemClickEventHandler(btnCondensers_ItemClick);
            //btnAdminMode.ItemClick += btnAdminMode_ItemClick;

            //btnLimitC2H2.ItemClick += btnLimitC2H2_ItemClick;
            //btnLimitCO2_CO.ItemClick += btnLimitCO2_CO_ItemClick;
            //btnKoefDegreeAproxCO2_CO.ItemClick += btnKoefDegreeAproxCO2_CO_ItemClick;
            //btnParameters.ItemClick += btnParameters_ItemClick;

            barManager.EndUpdate();
        }

        void btnCondensers_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                CondenserForm f = new CondenserForm();
                f.ShowDialog();
                m_strFilterString = tFind.Text;
                FindEquipments(-1);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void subSettingsMenu_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ParameterForm f = new ParameterForm();
                f.ShowDialog();
            }
            catch (Exception ex)
            {
            }
        }

        /*void btnAdminMode_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                PswForm f = new PswForm();
                if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    btnAdminMode.Visibility = BarItemVisibility.Never;
                    btnLimitC2H2.Visibility = BarItemVisibility.Always;
                    btnLimitCO2_CO.Visibility = BarItemVisibility.Always;
                    btnKoefDegreeAproxCO2_CO.Visibility = BarItemVisibility.Always;
                    btnParameters.Visibility = BarItemVisibility.Always;

                    MyLocalizer.XtraMessageBoxShow("Режим Администратора активирован.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void MainForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            try
            {
                Localizer.Active = new MyLocalizer();

                this.Text = "Отбраковка конденсаторов (версия " + Program.m_strVersion + " от " + Program.m_strDateVersion + ")";

                listTestType.Add(new DataSourceString(1, "ресурсные"));
                listTestType.Add(new DataSourceString(2, "приемо-сдаточные"));
                listTestType.Add(new DataSourceString(3, "эксплуатация"));
                repTestType.DataSource = listTestType;
                repTestType.DisplayMember = "VAL";
                repTestType.ValueMember = "KEY";
                repTestType.DropDownRows = listTestType.Count;

                m_bShowCloseMsg = true;
                SQLiteConnection con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                try
                {
                    con.Open();
                    con.Close();


                    m_strFilterString = "###";
                    FindEquipments(-1);

                    btnAllTest.LookAndFeel.SetSkinStyle("MySkin_Result");
                    btnAddTest.LookAndFeel.SetSkinStyle("MySkin_Test");
                    btnPassport.LookAndFeel.SetSkinStyle("MySkin_Passport");
                    btnPassportAdd.LookAndFeel.SetSkinStyle("MySkin_AddPassport");
                    btnReport.LookAndFeel.SetSkinStyle("MySkin_Protocol");
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_bShowCloseMsg = false;
                    Close();
                }

                Graphics gr = this.CreateGraphics();

                DpiXRel = gr.DpiX / 96.0f;
                DpiYRel = gr.DpiY / 96.0f;

                colCondenserState.MaxWidth = (int)(30 * DpiXRel);
                colCondenserState.MinWidth = (int)(30 * DpiXRel);

                foreach (GridColumn col in MainGridView.Columns)
                {
                    GetColumnBestHeight(col);
                }
                SetMaxColumnHeights();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SetMaxColumnHeights()
        {
            try
            {
                int maxHeight = 0;
                foreach (KeyValuePair<string, int> val in m_dictColumnHeight)
                {
                    if (maxHeight < val.Value) maxHeight = val.Value;
                }
                MainGridView.ColumnPanelRowHeight = maxHeight;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /*void bi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                foreach (DevExpress.XtraBars.BarCheckItemLink bi in StyleSubMenu.ItemLinks)
                {
                    ((DevExpress.XtraBars.BarCheckItem)bi.Item).Checked = false;
                    if (bi.Caption == e.Item.Caption) ((DevExpress.XtraBars.BarCheckItem)bi.Item).Checked = true;
                }
                defaultSkin.LookAndFeel.SetSkinStyle(e.Item.Caption);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void FindEquipments(long id)
        {
            try
            {
                if (id <= 0)
                {
                    m_strIDsString = "";

                    this.qMainEquipmentsTableAdapter.SetCommandText("SELECT c.CondenserID, c.CondenserNumber, ct.CondenserTypeName, " +
                        "ilt.InsulatingLiquidTypeName, c.NominalVoltage, c.NominalCapacitance, c.CondenserState, c.CondenserTestType " +
                        "FROM Condensers AS c " +
                        "INNER JOIN CondenserTypes AS ct ON ct.CondenserTypeID = c.CondenserTypeID " +
                        "INNER JOIN InsulatingLiquidTypes AS ilt ON ilt.InsulatingLiquidTypeID = ct.InsulatingLiquidTypeID " +
                        "WHERE  " +
                        "COMPARE_STR(c.CondenserNumber, ?) >= 0 " +
                        "OR COMPARE_STR(ct.CondenserTypeName, ?) >= 0 " +
                        "OR COMPARE_STR(ilt.InsulatingLiquidTypeName, ?) >= 0");
                }
                else
                {
                    if (m_strIDsString == "") m_strIDsString = id.ToString();
                    else m_strIDsString = m_strIDsString + "," + id.ToString();

                    this.qMainEquipmentsTableAdapter.SetCommandText("SELECT c.CondenserID, c.CondenserNumber, ct.CondenserTypeName, " +
                        "ilt.InsulatingLiquidTypeName, c.NominalVoltage, c.NominalCapacitance, c.CondenserState, c.CondenserTestType " +
                        "FROM Condensers AS c " +
                        "INNER JOIN CondenserTypes AS ct ON ct.CondenserTypeID = c.CondenserTypeID " +
                        "INNER JOIN InsulatingLiquidTypes AS ilt ON ilt.InsulatingLiquidTypeID = ct.InsulatingLiquidTypeID " +
                        "WHERE c.CondenserID IN (" + m_strIDsString + ")" +
                        "OR COMPARE_STR(c.CondenserNumber, ?) >= 0 " +
                        "OR COMPARE_STR(ct.CondenserTypeName, ?) >= 0 " +
                        "OR COMPARE_STR(ilt.InsulatingLiquidTypeName, ?) >= 0");
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                if (m_strFilterString != "")
                {
                    string strFind = m_strFilterString;
                    this.qMainEquipmentsTableAdapter.Fill(this.dataSetQuery.QMainEquipments, strFind, strFind, strFind);
                }
                else
                    this.qMainEquipmentsTableAdapter.Fill(this.dataSetQuery.QMainEquipments, "", "", "");
                this.Cursor = System.Windows.Forms.Cursors.Default;

                RefreshButtons();
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public void RefreshButtons()
        {
            try
            {
                if (this.dataSetQuery.QMainEquipments.Count == 0)
                {
                    btnPassport.Enabled = false;
                    //btnPassport.LookAndFeel.SetSkinStyle(this.LookAndFeel.ActiveSkinName);
                    btnAddTest.Enabled = false;
                    btnAllTest.Enabled = false;
                    btnReport.Enabled = false;
                }
                else
                {
                    btnPassport.Enabled = true;
                    //btnPassport.LookAndFeel.SetSkinStyle("MySkin_StylePassport");
                    btnAddTest.Enabled = true;
                    btnAllTest.Enabled = true;
                    btnReport.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bAcceptFind_Click(object sender, EventArgs e)
        {
            try
            {
                m_strFilterString = tFind.Text;
                FindEquipments(-1);
                MainGridView.ExpandAllGroups();

                if (MainGridView.RowCount == 0)
                {
                    NoFindEquipmentMessageForm f = new NoFindEquipmentMessageForm();

                    f.m_strMessage = "Конденсатор по поиску \"" + m_strFilterString + "\" отсутствует в базе данных. Добавить конденсатор?";

                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        long id = -1;

                        PassportDataForm rf = new PassportDataForm(id, 0);
                        rf.m_bShowContinueMsg = true;
                        System.Windows.Forms.DialogResult dr = rf.ShowDialog(this);
                        id = rf.m_id;
                        if (dr == System.Windows.Forms.DialogResult.OK)
                            RefreshGridPos(id);

                        if (rf.m_bContinueNext)
                        {
                            ShowTestForm(id, rf.m_CondenserTestID);
                        }
                    }
                    else
                    {
                        tFind.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void tFind_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void ClearMenuChecked()
        {
        }

        private void FilterForSelect_Click(object sender, EventArgs e)
        {
        }

        private void MainGridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    //mFilterForSelect.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainGridView_ColumnFilterChanged(object sender, EventArgs e)
        {
        }

        private void bCancelFind_Click(object sender, EventArgs e)
        {
            try
            {
                tFind.Text = "";
                m_strFilterString = "###";
                FindEquipments(-1);
                MainGridView.ExpandAllGroups();
                MainGridView.ClearColumnsFilter();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                /*if (m_bShowCloseMsg && MyLocalizer.XtraMessageBoxShow("Вы действительно хотите выйти из программы?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.Cancel = true;
                }*/
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshGridPos(long id)
        {
            try
            {
                int f_row = MainGridView.FocusedRowHandle;
                if (id <= 0)
                {
                    return;
                }
                else
                {
                    FindEquipments(id);

                    for (int i = 0; i < MainGridView.RowCount; i++)
                    {
                        long id_ = Convert.ToInt64(MainGridView.GetRowCellValue(i, "CondenserID"));
                        if (id_ == id)
                        {
                            MainGridView.ClearSelection();
                            MainGridView.SelectRow(i);
                            MainGridView.FocusedRowHandle = i;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VerifyEnd(long id)
        {
            try
            {
                /*List<ReportInfo.Equipment> m_listEquipments = new List<ReportInfo.Equipment>();
                Dictionary<Inspection.InspectionType, List<ReportInfo.InspectionSubType_>> m_list_sub_types = new Dictionary<Inspection.InspectionType, List<ReportInfo.InspectionSubType_>>();
                Dictionary<Inspection.InspectionType, List<long?>> m_dictCommonSubTypes = new Dictionary<Inspection.InspectionType, List<long?>>();
                Dictionary<Inspection.InspectionType, double> m_InspectionTypeFillability = new Dictionary<Inspection.InspectionType, double>();
                double fCommonFillability = 0;

                m_listEquipments.Add(new ReportInfo.Equipment(id, EquipmentKindID));

                if (!ReportInfo.GetData(m_CheckID, m_listEquipments, m_dictCommonSubTypes, m_list_sub_types, 0))
                    return;

                fCommonFillability = ReportInfo.GetFillability(EquipmentKindID, m_listEquipments, m_dictCommonSubTypes, 0, m_InspectionTypeFillability);

                if (Math.Abs(1.0 - fCommonFillability) > 0.0009)
                {
                    //PrintFillabilityMessageForm f = new PrintFillabilityMessageForm();
                    //f.m_fProcent = fCommonFillability;

                    fCommonFillability = 1.0 - fCommonFillability;
                    //if (fCommonFillability < 0.01) fCommonFillability = 0.01;
                    DialogResult res = MyLocalizer.XtraMessageBoxShow("Не заполнено " + fCommonFillability.ToString("0.#%") + " данных", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, "Заполнить данные", "Завершить ввод данных");

                    if (res == System.Windows.Forms.DialogResult.No)
                    {
                        //PrintProtocol(id);
                    }
                    else
                    {
                        // ищем 
                        //foreach (Inspection.InspectionType type in Enum.GetValues(typeof(Inspection.InspectionType)))
                        for (int i = 0; i < Inspection.m_listEquipmentInspections[(Equipment.EquipmentKind)EquipmentKindID].Count; i++)
                        {
                            Inspection.InspectionType type = Inspection.m_listEquipmentInspections[(Equipment.EquipmentKind)EquipmentKindID][i];
                            if (Math.Abs(1.0 - m_InspectionTypeFillability[type]) > 0.0009)
                            {
                                switch (type)
                                {
                                    case Inspection.InspectionType.Vibro:
                                        ShowVibroForm(id);
                                        break;
                                    case Inspection.InspectionType.FHA:
                                        ShowFHAForm(id);
                                        break;
                                    case Inspection.InspectionType.HARG:
                                        ShowHARGForm(id);
                                        break;
                                    case Inspection.InspectionType.Visual:
                                        ShowVisualForm(id);
                                        break;
                                    case Inspection.InspectionType.Warm:
                                        ShowWarmForm(id);
                                        break;
                                    case Inspection.InspectionType.Parameter:
                                        ShowParameterForm(id);
                                        break;
                                    case Inspection.InspectionType.Electrical:
                                        ShowElectricalForm(id);
                                        break;
                                }
                                return;
                            }
                        }
                    }
                }*/
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ShowTestForm(long id, long CondenserTestID)
        {
            try
            {
                bool bContinueNext = false;
                bool bContinuePrev = false;

                // если добавляем испытание
                // если нет коэффициентов А и В и нет никаких испытаний, кроме ресурсных, то добавляются только ресурсные испытания
                // если нет коэффициентов А и В и есть испытания, кроме ресурсных, то выводится сообщение, что необходимо вввести коэффициенты А и В
                // если есть коэффициенты А и В и нет никаких испытаний, то добавляется только приемо-сдаточное испытание
                // если есть коэффициенты А и В и есть приемо-сдаточное испытание, то добавляются только эксплуатационные испытания
                // если есть коэффициенты А и В и есть ресурсные испытания, то выводится сообщение о том, что данному конденсатору невозможно добавить испытание
                // если есть неоконченное эксплуатационное испытание, то вместо добавление вызывается окно с его изменением

                CondenserTest.CondenserTestType CondenserTestType = CondenserTest.CondenserTestType.None;

                DialogResult dr = System.Windows.Forms.DialogResult.Cancel;
                TestDataForm form = new TestDataForm(id, CondenserTestID, CondenserTestType);

                if (CondenserTestID <= 0)
                {
                    if (!form.VerifyOtherTest(id))
                        return;
                }

                form.m_bShowContinueMsg = true;

                dr = form.ShowDialog(this);
                bContinueNext = form.m_bContinueNext;
                bContinuePrev = form.m_bContinuePrev;

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshGridPos(id);

                    if (bContinueNext)
                    {
                        VerifyEnd(id);
                    }
                    else
                    {
                        if (bContinuePrev)
                        {
                            UpdatePassportData(id, form.m_CondenserTestID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UpdatePassportData(long id, long CondenserTestID)
        {
            try
            {
                PassportDataForm rf = new PassportDataForm(id, CondenserTestID);
                rf.m_bShowContinueMsg = true;
                DialogResult dr = rf.ShowDialog(this);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshGridPos(id);
                    if (rf.m_bContinueNext) ShowTestForm(id, rf.m_CondenserTestID);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateRecord()
        {
            try
            {
                if (MainGridView.FocusedRowHandle < 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать конденсатор.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = (DataRowView)(qMainEquipmentsBindingSource.Current);
                long id = Convert.ToInt64(drv.Row["CondenserID"]);

                UpdatePassportData(id, 0);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPassport_Click_1(object sender, EventArgs e)
        {
            UpdateRecord();
        }

        private void tFind_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == 13)
                {
                    bAcceptFind_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainGridView_DoubleClick(object sender, EventArgs e)
        {
            UpdateRecord();
        }

        private void PassportDataPopupMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            UpdateRecord();
        }

        private void btnPassportAdd_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = System.Windows.Forms.DialogResult.OK;

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    long id = -1;

                    PassportDataForm rf = new PassportDataForm(id, 0);
                    rf.m_bShowContinueMsg = true;
                    dr = rf.ShowDialog(this);
                    id = rf.m_id;
                    if (dr == System.Windows.Forms.DialogResult.OK)
                        RefreshGridPos(id);

                    if (rf.m_bContinueNext)
                    {
                        ShowTestForm(id, rf.m_CondenserTestID);
                    }
                }
                else
                {
                    tFind.Focus();
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainGridView_ShowFilterPopupListBox(object sender, DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventArgs e)
        {
            try
            {
                string custom = DevExpress.XtraGrid.Localization.GridLocalizer.Active.GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId.PopupFilterCustom);
                string blank = DevExpress.XtraGrid.Localization.GridLocalizer.Active.GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId.PopupFilterBlanks);
                string not_blank = DevExpress.XtraGrid.Localization.GridLocalizer.Active.GetLocalizedString(DevExpress.XtraGrid.Localization.GridStringId.PopupFilterNonBlanks);
                for (int i = e.ComboBox.Items.Count - 1; i >= 0; i--)
                {
                    if (e.ComboBox.Items[i].ToString() == custom)
                    {
                        e.ComboBox.Items.RemoveAt(i);
                        continue;
                    }
                    if (e.ComboBox.Items[i].ToString() == blank)
                    {
                        e.ComboBox.Items.RemoveAt(i);
                        continue;
                    }
                    if (e.ComboBox.Items[i].ToString() == not_blank)
                    {
                        e.ComboBox.Items.RemoveAt(i);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CondenserType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                CondenserTypeForm f = new CondenserTypeForm();
                f.ShowDialog(this);
                m_strFilterString = tFind.Text;
                FindEquipments(-1);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsulatingLiquidType_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                InsulatingLiquidTypeForm f = new InsulatingLiquidTypeForm();
                f.ShowDialog(this);
                m_strFilterString = tFind.Text;
                FindEquipments(-1);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAddTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainGridView.FocusedRowHandle < 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать конденсатор.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = (DataRowView)(qMainEquipmentsBindingSource.Current);
                long id = Convert.ToInt64(drv.Row["CondenserID"]);

                ShowTestForm(id, 0);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Size CalcCaptionTextSize(GraphicsCache cache, HeaderObjectInfoArgs ee, string caption)
        {
            try
            {
                Size captionSize = ee.Appearance.CalcTextSize(cache, caption, ee.CaptionRect.Width).ToSize();
                return captionSize;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return new Size(0, 0);
        }

        private int GetColumnBestHeight(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            try
            {
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = MainGridView.GetViewInfo() as GridViewInfo;

                GridColumnInfoArgs ex = null;
                viewInfo.GInfo.AddGraphics(null);
                ex = new GridColumnInfoArgs(viewInfo.GInfo.Cache, null);
                try
                {
                    ex.InnerElements.Add(new DrawElementInfo(new GlyphElementPainter(),
                                                            new GlyphElementInfoArgs(viewInfo.View.Images, 0, null),
                                                            StringAlignment.Near));
                    ex.SetAppearance(MainGridView.Appearance.HeaderPanel);
                    ex.Caption = column.Caption;
                    ex.CaptionRect = new Rectangle(0, 0, (int)(column.VisibleWidth - 10 * DpiXRel), 1000);
                }
                finally
                {
                    viewInfo.GInfo.ReleaseGraphics();
                }

                GraphicsInfo grInfo = new GraphicsInfo();
                grInfo.AddGraphics(null);
                ex.Cache = grInfo.Cache;
                Size captionSize = CalcCaptionTextSize(grInfo.Cache, ex as HeaderObjectInfoArgs, column.Caption);
                bool canDrawMore = true;
                Size res = ex.InnerElements.CalcMinSize(grInfo.Graphics, ref canDrawMore);

                res.Height = Math.Max(res.Height, captionSize.Height);
                res.Width += captionSize.Width;
                if (viewInfo.Painter != null)
                {
                    res = viewInfo.Painter.ElementsPainter.Column.CalcBoundsByClientRectangle(ex, new Rectangle(Point.Empty, res)).Size;
                    m_dictColumnHeight[column.Name] = res.Height;
                    return res.Height;
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return -1;
        }

        private void SetColumnHeight(DevExpress.XtraGrid.Columns.GridColumn column)
        {
            try
            {
                int col_height = GetColumnBestHeight(column);
                SetMaxColumnHeights();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainGridView_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            try
            {
                //SetColumnHeight(e.Column);
                for (int i = e.Column.VisibleIndex; i < MainGridView.Columns.Count; i++)
                {
                    SetColumnHeight(MainGridView.Columns[i]);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridColumn col in MainGridView.Columns)
                {
                    GetColumnBestHeight(col);
                }
                SetMaxColumnHeights();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAllTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (MainGridView.FocusedRowHandle < 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать конденсатор.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = (DataRowView)(qMainEquipmentsBindingSource.Current);
                long id = Convert.ToInt64(drv.Row["CondenserID"]);

                TestForm form = new TestForm(id);
                form.ShowDialog(this);
                RefreshGridPos(id);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AboutSubMenu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                AboutForm f = new AboutForm();
                f.ShowDialog();
                if (Program.m_bExpertMode)
                {
                    subSettingsMenu.Visibility = BarItemVisibility.Always;
                }

                if (Program.m_bExpertMode) panelExpertMode.Visible = true;
                else panelExpertMode.Visible = false;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolTip_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == MainGridControl)
            {
                ToolTipControlInfo info = null;
                //Get the view at the current mouse position
                GridView view = MainGridControl.GetViewAt(e.ControlMousePosition) as GridView;
                if (view == null) return;
                //Get the view's element information that resides at the current position
                GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
                //Display a hint for row indicator cells
                if (hi.HitTest == GridHitTest.RowIndicator || hi.HitTest == GridHitTest.RowCell && hi.Column.FieldName == "CondenserState")
                {
                    //An object that uniquely identifies a row indicator cell
                    object o = hi.HitTest.ToString() + hi.RowHandle.ToString();
                    string text = "";

                    DataRow row = MainGridView.GetDataRow(hi.RowHandle);
                    if (Convert.ToInt64(row["CondenserState"]) == 0)
                        text = "Работоспособен";
                    else
                        text = "Неработоспособен";

                    info = new ToolTipControlInfo(o, text);
                }
                //Supply tooltip information if applicable, otherwise preserve default tooltip (if any)
                if (info != null)
                    e.Info = info;
            }
        }

        private void MainGridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "CondenserState")
            {
                Rectangle rect = e.Bounds;
                int diametr = e.Bounds.Height - 4;
                rect.X = e.Bounds.X + e.Bounds.Width / 2 - diametr / 2;
                rect.Y = e.Bounds.Y + e.Bounds.Height / 2 - diametr / 2;
                rect.Width = rect.Height = diametr;

                DataRow row = MainGridView.GetDataRow(e.RowHandle);
                if (Convert.ToInt64(row["CondenserState"]) == 0)
                    e.Graphics.FillEllipse(new SolidBrush(Color.LightGreen), rect);
                else
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect);
                e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.Black)), rect);
                e.Handled = true;
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            if (MainGridView.FocusedRowHandle < 0)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать конденсатор.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView drv = (DataRowView)(qMainEquipmentsBindingSource.Current);
            long id = Convert.ToInt64(drv.Row["CondenserID"]);

            WaitingForm wf = new WaitingForm();
            wf.m_CondenserID = id;
            wf.ShowDialog(this);

            if (wf.m_Word != null)
            {
                wf.m_Word.SetVisible(true);
                wf.m_Word.DestroyWord();
            }
        }
    }
}

