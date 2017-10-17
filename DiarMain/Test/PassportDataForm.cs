using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SQLite;

namespace Condenser
{
    public partial class PassportDataForm : DevExpress.XtraEditors.XtraForm
    {
        public long m_id;
        public long m_CondenserTestID;
        bool m_bDataLoad = true;
        bool m_bDataLoadEnd = false;
        bool m_bChangeData = false;
        public bool m_bContinueNext = false;
        public bool m_bContinuePrev = false;
        public bool m_bShowContinueMsg = false;

        BindingList<DataSourceString> listDielectricType = new BindingList<DataSourceString>();
        BindingList<DataSourceString> listCasingType = new BindingList<DataSourceString>();

        public PassportDataForm(long id, long CondenserTestID)
        {
            InitializeComponent();
            m_id = id;
            m_CondenserTestID = CondenserTestID;
        }

        private void EquipmentRecordForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (strSeparator == ".") strSeparator = "\\.";
            this.repDecimal4.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,4})";
            this.repDecimal2.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,2})";
            this.repDecimal4Neg.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";

            this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);
            this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);
            this.qCondenserRecordTableAdapter.Fill(this.dataSetQuery.QCondenserRecord, m_id);

            this.WindowState = FormWindowState.Maximized;

            listDielectricType.Add(new DataSourceString(1, "Бумажный"));
            listDielectricType.Add(new DataSourceString(2, "Комбинированный (бумажно-пленочный)"));
            listDielectricType.Add(new DataSourceString(3, "Пленочный"));
            repDielectricType.DataSource = listDielectricType;
            repDielectricType.DisplayMember = "VAL";
            repDielectricType.ValueMember = "KEY";
            repDielectricType.DropDownRows = listDielectricType.Count;

            listCasingType.Add(new DataSourceString(1, "Фольга"));
            listCasingType.Add(new DataSourceString(2, "Металлизированная бумага односторонняя"));
            listCasingType.Add(new DataSourceString(3, "Металлизированная бумага двухсторонняя"));
            listCasingType.Add(new DataSourceString(4, "Металлизированная пленка односторонняя"));
            listCasingType.Add(new DataSourceString(5, "Металлизированная пленка двухсторонняя"));
            repCasingType.DataSource = listCasingType;
            repCasingType.DisplayMember = "VAL";
            repCasingType.ValueMember = "KEY";
            repCasingType.DropDownRows = listCasingType.Count;

            if (this.dataSetQuery.QCondenserTypes.Count > 7)
                cbCondenserType.Properties.DropDownRows = 7;
            else
                cbCondenserType.Properties.DropDownRows = this.dataSetQuery.QCondenserTypes.Count;

            if (this.dataSetQuery.QInsulatingLiquidTypes.Count > 7)
                repInsulatingLiquidType.DropDownRows = 7;
            else
                repInsulatingLiquidType.DropDownRows = this.dataSetQuery.QInsulatingLiquidTypes.Count;

            DataRowView dgv = null;
            if (m_id > 0)
            {
                dgv = (DataRowView)(this.qCondenserRecordBindingSource.Current);
                cbCondenserType.EditValue = dgv.Row["CondenserTypeID"];

                this.Text = "Изменение паспортных данных конденсатора";

                /*this.repSubject.ReadOnly = true;
                this.repSubject.Buttons[1].Enabled = false;
                GridVertical.GetRowByFieldName("SubjectID").Appearance.BackColor = Color.FromArgb(240, 240, 240);
                GridVertical.GetRowByFieldName("SubjectID").Appearance.Options.UseBackColor = true;*/
            }
            else
            {
                this.Text = "Добавление нового конденсатора";

                this.qCondenserRecordBindingSource.AddNew();
                dgv = (DataRowView)(this.qCondenserRecordBindingSource.Current);

                //dgv.Row["ConstructionType"] = 1;
            }

            if (this.qCondenserRecordBindingSource.Count == 0)
            {
                this.Visible = false;
                MyLocalizer.XtraMessageBoxShow("Не удалось найти конденсатор.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
                return;
            }

            if (m_CondenserTestID <= 0)
                bNext.Text = "Добавить испытание";
            else
                bNext.Text = "Изменить испытание";

            GridVertical.GetRowByFieldName("EnergyCharge").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("EnergyCharge").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("SpecificCapacityByWeight").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("SpecificCapacityByWeight").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("SpecificEnergyByWeight").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("SpecificEnergyByWeight").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("SpecificEnergyByVolume").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("SpecificEnergyByVolume").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("InsulatingLiquidTypeID").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("InsulatingLiquidTypeID").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("DielectricType").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("DielectricType").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("DielectricThickness").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("DielectricThickness").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("TangentAngle").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("TangentAngle").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("DielectricInductiveCapacity").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("DielectricInductiveCapacity").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("CasingType").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("CasingType").Appearance.Options.UseBackColor = true;

            GridVertical.GetRowByFieldName("CasingThickness").Appearance.BackColor = Color.FromArgb(240, 240, 240);
            GridVertical.GetRowByFieldName("CasingThickness").Appearance.Options.UseBackColor = true;

            m_bDataLoad = false;

            //repInsulatingLiquidType.KeyDown += new KeyEventHandler(rep_KeyDown);
            //repCasingType.KeyDown += new KeyEventHandler(rep_KeyDown);
            //repDielectricType.KeyDown += new KeyEventHandler(rep_KeyDown);

            m_bDataLoadEnd = true;

            if (m_bShowContinueMsg && m_id > 0)
            {
                bNext.Visible = true;
            }
        }

        void rep_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (!((LookUpEdit)sender).Properties.ReadOnly)
                {
                    DevExpress.XtraVerticalGrid.Rows.BaseRow row = GridVertical.FocusedRow;
                    GridVertical.BeginUpdate();
                    GridVertical.SetCellValue(row, 0, DBNull.Value);
                    GridVertical.EndUpdate();
                }
                //GridVertical.Refresh();
            }
        }

        private void GridVertical_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (GridVertical.FocusedRow.Name == "rEnergyCharge"
                || GridVertical.FocusedRow.Name == "rSpecificCapacityByWeight"
                || GridVertical.FocusedRow.Name == "rSpecificEnergyByWeight"
                || GridVertical.FocusedRow.Name == "rSpecificEnergyByVolume"
                || GridVertical.FocusedRow.Name == "rInsulatingLiquidType"
                || GridVertical.FocusedRow.Name == "rDielectricType"
                || GridVertical.FocusedRow.Name == "rDielectricThickness"
                || GridVertical.FocusedRow.Name == "rTangentAngle"
                || GridVertical.FocusedRow.Name == "rDielectricInductiveCapacity"
                || GridVertical.FocusedRow.Name == "rCasingType"
                || GridVertical.FocusedRow.Name == "rCasingThickness")
            {
                e.Cancel = true;
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!SaveData()) return;

            if (m_bShowContinueMsg)
            {
                if (MyLocalizer.XtraMessageBoxShow("Перейти к испытаниям?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    m_bContinueNext = true;
                }
                else
                {
                    m_bDataLoad = true;
                    m_bDataLoadEnd = false;
                    GridVertical.BeginUpdate();
                    this.qCondenserRecordTableAdapter.Fill(this.dataSetQuery.QCondenserRecord, m_id);
                    GridVertical.EndUpdate();
                    m_bDataLoad = false;
                    m_bDataLoadEnd = true;

                    return;
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private bool SaveData()
        {
            try
            {
                long EquipmentTypeID = -1;
                if (cbCondenserType.EditValue != null && cbCondenserType.EditValue != DBNull.Value)
                {
                    DataRowView drv_ = (DataRowView)cbCondenserType.GetSelectedDataRow();
                    EquipmentTypeID = Convert.ToInt64(drv_.Row["CondenserTypeID"]);
                }
                string strEquipmentNumber = teEquipmentNumber.Text.Trim();

                if (strEquipmentNumber == "")
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать идентификационный номер конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    teEquipmentNumber.Focus();
                    return false;
                }

                if (EquipmentTypeID <= 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать тип конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    cbCondenserType.Focus();
                    return false;
                }

                DataRowView drv = (DataRowView)this.qCondenserRecordBindingSource.Current;
                if (drv == null) return false;

                object val = drv.Row["NominalVoltage"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rNominalVoltage;
                    GridVertical.MakeRowVisible(rNominalVoltage);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать номинальное напряжение конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["NominalCapacitance"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rNominalCapacitance;
                    GridVertical.MakeRowVisible(rNominalCapacitance);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать номинальную емкость конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["CondenserWeight"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rCondenserWeight;
                    GridVertical.MakeRowVisible(rCondenserWeight);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать массу конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["CondenserVolume"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rCondenserVolume;
                    GridVertical.MakeRowVisible(rCondenserVolume);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать объем конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                /*val = drv.Row["InsulatingLiquidTypeID"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rInsulatingLiquidType;
                    GridVertical.MakeRowVisible(rInsulatingLiquidType);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать марку изоляционной жидкости конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["SolidInsulationType"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rDielectricType;
                    GridVertical.MakeRowVisible(rDielectricType);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать тип твердой изоляции конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["InsulationThickness"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rDielectricThickness;
                    GridVertical.MakeRowVisible(rDielectricThickness);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать толщину изоляции конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["CasingType"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rCasingType;
                    GridVertical.MakeRowVisible(rCasingType);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать тип обкладок конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }

                val = drv.Row["CasingThickness"];
                if (val == null || val == DBNull.Value)
                {
                    GridVertical.FocusedRow = rCasingThickness;
                    GridVertical.MakeRowVisible(rCasingThickness);
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать толщину обкладок.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GridVertical.ShowEditor();
                    return false;
                }*/

                // проверка на уникальность наименования и номера
                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "Select * from Condensers WHERE EQUAL_STR(CondenserNumber, ?) = 0 AND CondenserID <> ?";
                com.CommandType = CommandType.Text;
                SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.String);
                param1.Value = strEquipmentNumber;
                SQLiteParameter param2 = new SQLiteParameter("@Param2", DbType.Int64);
                param2.Value = m_id;
                com.Parameters.Add(param1);
                com.Parameters.Add(param2);
                SQLiteDataReader dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Close();
                    connection.Close();
                    MyLocalizer.XtraMessageBoxShow("Конденсатор с таким идентификационным номером уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                dr.Close();

                connection.Close();

                if (m_id > 0)
                {
                    //((DataRowView)qEquipmentRecordBindingSource.Current).BeginEdit();
                    ((DataRowView)this.qCondenserRecordBindingSource.Current).EndEdit();

                    SQLiteCommand upd_com = new SQLiteCommand(this.qCondenserRecordTableAdapter.Connection);
                    upd_com.CommandText = "UPDATE Condensers SET " +
                        "CondenserNumber = @param1, " +
                        "CondenserTypeID = @param2, " +
                        "NominalVoltage = @param3, " +
                        "NominalCapacitance = @param4, " +
                        "EnergyCharge = @param5, " +
                        "Inductivity = @param6, " +
                        "NormalizedResource = @param7, " +
                        "MaxDischargeCurrent = @param8, " +
                        "CondenserWeight = @param9, " +
                        "CondenserVolume = @param10, " +
                        //"InsulatingLiquidTypeID = @param11, " +
                        //"SolidInsulationType = @param12, " +
                        //"InsulationThickness = @param13, " +
                        //"CasingType = @param14, " +
                        //"CasingThickness = @param15, " +
                        "DesignFeaturesSection = @param16, " +
                        "InsulationResistance = @param17, " +
                        //"TangentAngle = @param18, " +
                        "TemperatureKoeff = @param19, " +
                        "TechnologicalFeatures = @param20, " +
                        "CondenserImage = @param21, " +
                        "SpecificCapacityByWeight = @param22, " +
                        "SpecificEnergyByWeight = @param23, " +
                        "SpecificEnergyByVolume = @param24 " +
                        "WHERE CondenserID = @param_id";
                    upd_com.Parameters.Add("@param1", DbType.String).SourceColumn = "CondenserNumber";
                    upd_com.Parameters.Add("@param2", DbType.Int64).SourceColumn = "CondenserTypeID";
                    upd_com.Parameters.Add("@param3", DbType.Decimal).SourceColumn = "NominalVoltage";
                    upd_com.Parameters.Add("@param4", DbType.Decimal).SourceColumn = "NominalCapacitance";
                    upd_com.Parameters.Add("@param5", DbType.Decimal).SourceColumn = "EnergyCharge";
                    upd_com.Parameters.Add("@param6", DbType.Decimal).SourceColumn = "Inductivity";
                    upd_com.Parameters.Add("@param7", DbType.Int64).SourceColumn = "NormalizedResource";
                    upd_com.Parameters.Add("@param8", DbType.Decimal).SourceColumn = "MaxDischargeCurrent";
                    upd_com.Parameters.Add("@param9", DbType.Decimal).SourceColumn = "CondenserWeight";
                    upd_com.Parameters.Add("@param10", DbType.Decimal).SourceColumn = "CondenserVolume";
                    /*upd_com.Parameters.Add("@param11", DbType.Int64).SourceColumn = "InsulatingLiquidTypeID";
                    upd_com.Parameters.Add("@param12", DbType.Int64).SourceColumn = "SolidInsulationType";
                    upd_com.Parameters.Add("@param13", DbType.Int64).SourceColumn = "InsulationThickness";
                    upd_com.Parameters.Add("@param14", DbType.Int64).SourceColumn = "CasingType";
                    upd_com.Parameters.Add("@param15", DbType.Int64).SourceColumn = "CasingThickness";*/
                    upd_com.Parameters.Add("@param16", DbType.String).SourceColumn = "DesignFeaturesSection";
                    upd_com.Parameters.Add("@param17", DbType.Decimal).SourceColumn = "InsulationResistance";
                    //upd_com.Parameters.Add("@param18", DbType.Decimal).SourceColumn = "TangentAngle";
                    upd_com.Parameters.Add("@param19", DbType.Decimal).SourceColumn = "TemperatureKoeff";
                    upd_com.Parameters.Add("@param20", DbType.String).SourceColumn = "TechnologicalFeatures";
                    SQLiteParameter paramImage = new SQLiteParameter("@param21", DbType.Object);
                    paramImage.Value = peImage.EditValue;
                    upd_com.Parameters.Add(paramImage);
                    upd_com.Parameters.Add("@param22", DbType.Decimal).SourceColumn = "SpecificCapacityByWeight";
                    upd_com.Parameters.Add("@param23", DbType.Decimal).SourceColumn = "SpecificEnergyByWeight";
                    upd_com.Parameters.Add("@param24", DbType.Decimal).SourceColumn = "SpecificEnergyByVolume";
                    upd_com.Parameters.Add("@param_id", DbType.Int64).SourceColumn = "CondenserID";
                    this.qCondenserRecordTableAdapter.Adapter.UpdateCommand = upd_com;
                }
                else
                {
                    if (((DataRowView)qCondenserRecordBindingSource.Current).IsEdit)
                        ((DataRowView)qCondenserRecordBindingSource.Current).EndEdit();

                    SQLiteCommand ins_com = new SQLiteCommand(this.qCondenserRecordTableAdapter.Connection);
                    ins_com.CommandText = "INSERT INTO Condensers (" +
                        "CondenserNumber, " +
                        "CondenserTypeID, " +
                        "NominalVoltage, " +
                        "NominalCapacitance, " +
                        "EnergyCharge, " +
                        "Inductivity, " +
                        "NormalizedResource, " +
                        "MaxDischargeCurrent, " +
                        "CondenserWeight, " +
                        "CondenserVolume, " +
                        /*"InsulatingLiquidTypeID, " +
                        "SolidInsulationType, " +
                        "InsulationThickness, " +
                        "CasingType, " +
                        "CasingThickness, " +*/
                        "DesignFeaturesSection, " +
                        "InsulationResistance, " +
                        //"TangentAngle, " +
                        "TemperatureKoeff, " +
                        "TechnologicalFeatures, " +
                        "CondenserImage, " +
                        "SpecificCapacityByWeight, " +
                        "SpecificEnergyByWeight, " +
                        "SpecificEnergyByVolume " +
                        ") VALUES (" +
                        "@param1, " +
                        "@param2, " +
                        "@param3, " +
                        "@param4, " +
                        "@param5, " +
                        "@param6, " +
                        "@param7, " +
                        "@param8, " +
                        "@param9, " +
                        "@param10, " +
                        /*"@param11, " +
                        "@param12, " +
                        "@param13, " +
                        "@param14, " +
                        "@param15, " +*/
                        "@param16, " +
                        "@param17, " +
                        //"@param18, " +
                        "@param19, " +
                        "@param20, " +
                        "@param21, " +
                        "@param22, " +
                        "@param23, " + 
                        "@param24);";
                    ins_com.Parameters.Add("@param1", DbType.String).SourceColumn = "CondenserNumber";
                    ins_com.Parameters.Add("@param2", DbType.Int64).SourceColumn = "CondenserTypeID";
                    ins_com.Parameters.Add("@param3", DbType.Decimal).SourceColumn = "NominalVoltage";
                    ins_com.Parameters.Add("@param4", DbType.Decimal).SourceColumn = "NominalCapacitance";
                    ins_com.Parameters.Add("@param5", DbType.Decimal).SourceColumn = "EnergyCharge";
                    ins_com.Parameters.Add("@param6", DbType.Decimal).SourceColumn = "Inductivity";
                    ins_com.Parameters.Add("@param7", DbType.Int64).SourceColumn = "NormalizedResource";
                    ins_com.Parameters.Add("@param8", DbType.Decimal).SourceColumn = "MaxDischargeCurrent";
                    ins_com.Parameters.Add("@param9", DbType.Decimal).SourceColumn = "CondenserWeight";
                    ins_com.Parameters.Add("@param10", DbType.Decimal).SourceColumn = "CondenserVolume";
                    /*ins_com.Parameters.Add("@param11", DbType.Int64).SourceColumn = "InsulatingLiquidTypeID";
                    ins_com.Parameters.Add("@param12", DbType.Int64).SourceColumn = "SolidInsulationType";
                    ins_com.Parameters.Add("@param13", DbType.Int64).SourceColumn = "InsulationThickness";
                    ins_com.Parameters.Add("@param14", DbType.Int64).SourceColumn = "CasingType";
                    ins_com.Parameters.Add("@param15", DbType.Int64).SourceColumn = "CasingThickness";*/
                    ins_com.Parameters.Add("@param16", DbType.String).SourceColumn = "DesignFeaturesSection";
                    ins_com.Parameters.Add("@param17", DbType.Decimal).SourceColumn = "InsulationResistance";
                    //ins_com.Parameters.Add("@param18", DbType.Decimal).SourceColumn = "TangentAngle";
                    ins_com.Parameters.Add("@param19", DbType.Decimal).SourceColumn = "TemperatureKoeff";
                    ins_com.Parameters.Add("@param20", DbType.String).SourceColumn = "TechnologicalFeatures";
                    SQLiteParameter paramImage = new SQLiteParameter("@param21", DbType.Object);
                    paramImage.Value = peImage.EditValue;
                    ins_com.Parameters.Add(paramImage);
                    ins_com.Parameters.Add("@param22", DbType.Decimal).SourceColumn = "SpecificCapacityByWeight";
                    ins_com.Parameters.Add("@param23", DbType.Decimal).SourceColumn = "SpecificEnergyByWeight";
                    ins_com.Parameters.Add("@param24", DbType.Decimal).SourceColumn = "SpecificEnergyByVolume";

                    this.qCondenserRecordTableAdapter.Adapter.InsertCommand = ins_com;
                }

                this.qCondenserRecordTableAdapter.Adapter.Update(dataSetQuery.QCondenserRecord);

                SQLiteConnection connection_ = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection_.Open();

                if (m_id <= 0)
                {
                    SQLiteCommand com_ = new SQLiteCommand(connection_);
                    com_.CommandText = "select seq from sqlite_sequence where name = 'Condensers'";
                    com_.CommandType = CommandType.Text;
                    SQLiteDataReader dr_ = com_.ExecuteReader();

                    while (dr_.Read())
                    {
                        m_id = Convert.ToInt64(dr_["seq"]);
                    }
                    dr_.Close();
                }

                connection_.Close();

                m_bChangeData = false;

                return true;
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DBConcurrencyException ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void GridVertical_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            DevExpress.XtraVerticalGrid.Rows.BaseRow  row = GridVertical.FocusedRow;

            if (row.Name == "rMaxDischargeCurrent" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Convert.ToDouble(e.Value) < 0.001 || Convert.ToDouble(e.Value) > 1000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение максимального разрядного тока должно находиться между значениями 0,001 и 1000 кА.";
                    return;
                }
            }

            if (row.Name == "rNominalVoltage" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Math.Abs(Convert.ToDouble(e.Value)) < 0.0009 || Convert.ToDouble(e.Value) >= 1000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение номинального напряжения должно находиться между значениями 0 и 1000 кВ.";
                    return;
                }
            }
            /*if (row.Name == "rInsulationThickness" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Convert.ToInt32(e.Value) <= 0 || Convert.ToInt32(e.Value) >= 1000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение толщины изоляции должно находиться между значениями 0 и 1000 мкм";
                    return;
                }
            }
            if (row.Name == "rCasingThickness" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Convert.ToInt32(e.Value) <= 0 || Convert.ToInt32(e.Value) >= 1000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение толщины обкладки должно находиться между значениями 0 и 1000 мкм";
                    return;
                }
            }*/
            if (row.Name == "rInsulationResistance" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Math.Abs(Convert.ToDouble(e.Value)) < 0.0009 || Convert.ToDouble(e.Value) >= 10000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение сопротивления изоляции должно находиться между значениями 0 и 10000 МОм.";
                    return;
                }
            }

            if (row.Name == "rNormalizedResource" && e.Value != null && e.Value != DBNull.Value && e.Value.ToString() != "")
            {
                if (Math.Abs(Convert.ToInt64(e.Value)) < 10 || Convert.ToInt64(e.Value) > 1000000000)
                {
                    e.Valid = false;
                    e.ErrorText = "Значение ресурса должно находиться между значениями 10 и 1000000000 циклов.";
                    return;
                }
            }

            if (e.Value != null && e.Value.ToString() == "") e.Value = DBNull.Value;
        }

        static public double Round(double value, int digits)
        {
            double scale = Math.Pow(10.0, digits);
            double round = Math.Floor(Math.Abs(value) * scale + 0.5);
            return (Math.Sign(value) * round / scale);
        }

        private void UpdateEnergyCharge()
        {
            DataRowView drv = (DataRowView)(this.qCondenserRecordBindingSource.Current);

            double fVoltage = 0;
            double fCapacitance = 0;

            object val = drv.Row["NominalVoltage"];
            if (val != null && val != DBNull.Value)
            {
                fVoltage = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["EnergyCharge"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            val = drv.Row["NominalCapacitance"];
            if (val != null && val != DBNull.Value)
            {
                fCapacitance = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["EnergyCharge"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            GridVertical.BeginUpdate();
            drv.Row["EnergyCharge"] = Round(fCapacitance * fVoltage * fVoltage / (2 * 1000), 4);
            GridVertical.EndUpdate();
        }

        private void UpdateCapacityByWeight()
        {
            DataRowView drv = (DataRowView)(this.qCondenserRecordBindingSource.Current);

            double fWeight = 0;
            double fCapacitance = 0;

            object val = drv.Row["CondenserWeight"];
            if (val != null && val != DBNull.Value)
            {
                fWeight = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificCapacityByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            if (Math.Abs(fWeight) < 0.00009)
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificCapacityByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            val = drv.Row["NominalCapacitance"];
            if (val != null && val != DBNull.Value)
            {
                fCapacitance = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificCapacityByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            GridVertical.BeginUpdate();
            drv.Row["SpecificCapacityByWeight"] = Round(fCapacitance / fWeight, 4);
            GridVertical.EndUpdate();
        }

        private void UpdateEnergyByWeight()
        {
            DataRowView drv = (DataRowView)(this.qCondenserRecordBindingSource.Current);

            double fWeight = 0;
            double fEnergy = 0;

            object val = drv.Row["CondenserWeight"];
            if (val != null && val != DBNull.Value)
            {
                fWeight = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            if (Math.Abs(fWeight) < 0.00009)
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            val = drv.Row["EnergyCharge"];
            if (val != null && val != DBNull.Value)
            {
                fEnergy = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByWeight"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            GridVertical.BeginUpdate();
            drv.Row["SpecificEnergyByWeight"] = Round(fEnergy * 1000 / fWeight, 4);
            GridVertical.EndUpdate();
        }

        private void UpdateEnergyByVolume()
        {
            DataRowView drv = (DataRowView)(this.qCondenserRecordBindingSource.Current);

            double fVolume = 0;
            double fEnergy = 0;

            object val = drv.Row["CondenserVolume"];
            if (val != null && val != DBNull.Value)
            {
                fVolume = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByVolume"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            if (Math.Abs(fVolume) < 0.00009)
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByVolume"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            val = drv.Row["EnergyCharge"];
            if (val != null && val != DBNull.Value)
            {
                fEnergy = Convert.ToDouble(val);
            }
            else
            {
                GridVertical.BeginUpdate();
                drv.Row["SpecificEnergyByVolume"] = DBNull.Value;
                GridVertical.EndUpdate();
                return;
            }

            GridVertical.BeginUpdate();
            drv.Row["SpecificEnergyByVolume"] = Round(fEnergy * 1000 / fVolume, 4);
            GridVertical.EndUpdate();
        }
        private void vGridControl1_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            try
            {
                if (m_bDataLoadEnd) m_bChangeData = true;

                if (e.Row.Name == "rNominalVoltage")
                {
                    UpdateEnergyCharge();
                    UpdateEnergyByWeight();
                    UpdateEnergyByVolume();
                }
                if (e.Row.Name == "rNominalCapacitance")
                {
                    UpdateEnergyCharge();
                    UpdateCapacityByWeight();
                    UpdateEnergyByWeight();
                    UpdateEnergyByVolume();
                }
                if (e.Row.Name == "rCondenserWeight")
                {
                    UpdateCapacityByWeight();
                    UpdateEnergyByWeight();
                }
                if (e.Row.Name == "rCondenserVolume")
                {
                    UpdateEnergyByVolume();
                }
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DBConcurrencyException ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridVertical_FocusedRowChanged(object sender, DevExpress.XtraVerticalGrid.Events.FocusedRowChangedEventArgs e)
        {
            if (!e.Row.OptionsRow.AllowFocus)
            {
                ((DevExpress.XtraVerticalGrid.Rows.BaseRow)e.Row).Visible = false;
                GridVertical.FocusedRow = e.OldRow;
            }
        }

        private void btnClearPicture_Click(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
            peImage.EditValue = null;
        }

        private void peImage_DoubleClick(object sender, EventArgs e)
        {
            ChangePicture();
        }

        private void btnChangePicture_Click(object sender, EventArgs e)
        {
            ChangePicture();
        }

        private void ChangePicture()
        {
            ImageForm f = new ImageForm();
            f.m_img = peImage.EditValue;
            DialogResult res = f.ShowDialog(this);
            if (res == System.Windows.Forms.DialogResult.OK)
            {
                m_bChangeData = true;
                peImage.EditValue = f.m_img;
            }
        }

        private void PassportDataForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            teEquipmentNumber.Focus();
            if (DialogResult != System.Windows.Forms.DialogResult.OK && m_bChangeData)
            {
                if (MyLocalizer.XtraMessageBoxShow("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!SaveData())
                    {
                        e.Cancel = true;
                        return;
                    }
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                }
            }
        }

        private void teEquipmentName_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void teEquipmentNumber_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void repLookUp_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void bNext_Click(object sender, EventArgs e)
        {
            if (m_bShowContinueMsg)
            {
                m_bContinueNext = true;

                if (m_bChangeData)
                {
                    if (MyLocalizer.XtraMessageBoxShow("Сохранить изменения?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!SaveData())
                        {
                            return;
                        }
                    }
                }

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }            
        }

        private void PassportDataForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Form f = this.Owner;
                while (f.Owner != null)
                {
                    f.Hide();
                    f = f.Owner;
                }
                f.Hide();
            }

            if (this.WindowState != FormWindowState.Minimized && m_bDataLoadEnd /*&& this.ShowInTaskbar*/)
            {
                DevExpress.XtraEditors.XtraForm f = (DevExpress.XtraEditors.XtraForm)this.Owner;
                while (f.Owner != null)
                {
                    if (!f.Visible) f.Show();
                    f = (DevExpress.XtraEditors.XtraForm)f.Owner;
                }

                if (!f.Visible) f.Show();
                //this.ShowInTaskbar = false;
            }
        }

        private void GridVertical_CustomDrawRowValueCell(object sender, DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventArgs e)
        {
            string str = e.CellText;
            Rectangle rect = e.Bounds;
            rect.X += 3;
            rect.Width -= 6;

            if (e.Row.Appearance.Options.UseBackColor && !e.Row.HasChildren)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.White) /*.FromArgb(180, 180, 180))*/, e.Bounds);
            }

            if (str == "")
            {
                e.Graphics.DrawString("данные отсутствуют", e.Appearance.Font, new SolidBrush(Color.Gray), rect, e.Appearance.GetStringFormat());
            }
            else
                e.Graphics.DrawString(str, e.Appearance.Font, new SolidBrush(Color.Black), rect, e.Appearance.GetStringFormat());

            e.Handled = true;
        }

        private void peImage_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void repInsulatingLiquidType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

        }

        private void cbCondenserType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
            {
                CondenserTypeForm f = new CondenserTypeForm();
                f.m_bCanSelect = true;
                DialogResult res = f.ShowDialog(this);

                this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);

                this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);
                if (this.dataSetQuery.QCondenserTypes.Count > 7)
                    cbCondenserType.Properties.DropDownRows = 7;
                else
                    cbCondenserType.Properties.DropDownRows = this.dataSetQuery.QCondenserTypes.Count;

                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    cbCondenserType.EditValue = f.m_SelectID;
                }
            }
        }

        private void repDecimal4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                if (e.KeyChar == '.') e.KeyChar = ',';

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void cbCondenserType_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                GridVertical.BeginUpdate();

                DataRowView drv = (DataRowView)(qCondenserRecordBindingSource.Current);
                if (cbCondenserType.EditValue == null || cbCondenserType.EditValue == DBNull.Value)
                {
                    drv.Row["CondenserTypeID"] = DBNull.Value;
                    drv.Row["InsulatingLiquidTypeID"] = DBNull.Value;
                    drv.Row["DielectricType"] = DBNull.Value;
                    drv.Row["DielectricThickness"] = DBNull.Value;
                    drv.Row["TangentAngle"] = DBNull.Value;
                    drv.Row["DielectricInductiveCapacity"] = DBNull.Value;
                    drv.Row["CasingType"] = DBNull.Value;
                    drv.Row["CasingThickness"] = DBNull.Value;
                }
                else
                {
                    DataRowView drvType = (DataRowView)cbCondenserType.GetSelectedDataRow();
                    drv.Row["CondenserTypeID"] = drvType.Row["CondenserTypeID"];
                    drv.Row["InsulatingLiquidTypeID"] = drvType.Row["InsulatingLiquidTypeID"];
                    drv.Row["DielectricType"] = drvType.Row["DielectricType"];
                    drv.Row["DielectricThickness"] = drvType.Row["DielectricThickness"];
                    drv.Row["TangentAngle"] = drvType.Row["TangentAngle"];
                    drv.Row["DielectricInductiveCapacity"] = drvType.Row["DielectricInductiveCapacity"];
                    drv.Row["CasingType"] = drvType.Row["CasingType"];
                    drv.Row["CasingThickness"] = drvType.Row["CasingThickness"];
                }

                GridVertical.EndUpdate();
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DBConcurrencyException ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
    }
}