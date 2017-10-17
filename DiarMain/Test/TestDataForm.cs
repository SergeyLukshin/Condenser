using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Drawing;
using DevExpress.Utils.Drawing;
using System.IO;
//using System.Linq;

namespace Condenser
{
    public partial class TestDataForm : DevExpress.XtraEditors.XtraForm
    {
        /*class ShedulerInfo
        {
            public ShedulerInfo(DateTime? date, Decimal? frequency, long? cycle_count)
            {
                m_Date = date;
                m_Frequency = frequency;
                m_CycleCount = cycle_count;
            }

            private DateTime? m_Date = null;
            private Decimal? m_Frequency = null;
            private long? m_CycleCount = null;

            public DateTime? DATE
            {
                get { return m_Date; }
                set { if (value == null) m_Date = DateTime.Now; else m_Date = value; }
            }

            public Decimal? FREQUENCY
            {
                get { return m_Frequency; }
                set { m_Frequency = value; }
            }

            public long? CYCLE_COUNT
            {
                get { return m_CycleCount; }
                set { m_CycleCount = value; }
            }
        }*/

        private long m_CondenserID = 0;
        public long m_CondenserTestID = 0;
        public CondenserTest.CondenserTestType m_CondenserTestType = CondenserTest.CondenserTestType.None;
        public long m_CondenserState = 0;

        public bool m_bContinueNext = false;
        public bool m_bContinuePrev = false;
        public bool m_bShowContinueMsg = false;
        bool m_bDataLoadEnd = false;
        Bitmap bmp_pb;
        List<float> m_listVoltagePoints = new List<float>();
        bool bPressDown = false;
        float fMargin = 20;
        float fTail = 5;
        float fRadius = 5;
        int m_iSelectedVoltagePoint = -1;
        int m_iMaxNodes = 64;
        //Point m_oldMousePoint = new Point(0, 0);
        float DpiXRel;
        float DpiYRel;
        BindingList<DataSourceString> listTestType = new BindingList<DataSourceString>();
        List<long> listDeletedSheduler = new List<long>();
        //BindingList<ShedulerInfo> listSheduler = new BindingList<ShedulerInfo>();
        DataTable tableSheduler = new DataTable();
        /*int iPosShedulerID = 0;
        int iPosShedulerDate = 1;
        int iPosShedulerFrequency = 2;
        int iPosShedulerCycleCount = 3;
        int iPosShedulerWasChange = 4;*/

        bool m_bChangeData = false;
        bool m_bCondenserTestIsLast = false;
        string m_strRecommendation = "";
        string m_strConclusion = "";
        //bool m_bNeedShowRecommendation = false;
        //bool m_bLoadDataEnd = false;

        Brush m_BrushGray = new SolidBrush(Color.Gray);
        Brush m_BrushBlack = new SolidBrush(Color.Black);
        Brush m_BrushRed = new SolidBrush(Color.Red);

        public Dictionary<string, double?> dictLastAnalysisData = new Dictionary<string, double?>();
        //DateTime? m_dtLastAnalysisDateProbe = null;
        //DateTime? m_dtOldAnalysisDateProbe = null;
        private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string, int>();

        SQLiteConnection m_con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);

        public TestDataForm(long id, long test_id, CondenserTest.CondenserTestType condenser_test_type = CondenserTest.CondenserTestType.None)
        {
            m_CondenserID = id;
            m_CondenserTestID = test_id;
            m_CondenserTestType = condenser_test_type;
            InitializeComponent();
        }

        private void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        public bool VerifyOtherTest(long CondenserID)
        {
            try
            {
                m_CondenserTestType = CondenserTest.CondenserTestType.None;

                m_con.Open();
                SQLiteCommand com = new SQLiteCommand(m_con);
                com.CommandText = "Select COUNT(*) AS Cnt, CondenserTestType, MAX(CondenserState) AS MaxState from CondenserTest WHERE CondenserID = @id GROUP BY CondenserTestType";
                com.CommandType = CommandType.Text;
                SQLiteParameter param1 = new SQLiteParameter("@id", DbType.Int64);
                param1.Value = CondenserID;
                com.Parameters.Add(param1);
                SQLiteDataReader drType = com.ExecuteReader();

                Dictionary<CondenserTest.CondenserTestType, KeyValuePair<long, long>> dictTest = new Dictionary<CondenserTest.CondenserTestType, KeyValuePair<long, long>>();

                /*foreach (Condenser.CondenserType type_ in Enum.GetValues(typeof(Condenser.CondenserType)))
                {
                    dictTest[type_] = 0;
                }*/

                while (drType.Read())
                {
                    dictTest[(CondenserTest.CondenserTestType)Convert.ToInt64(drType["CondenserTestType"])] = new KeyValuePair<long, long>(Convert.ToInt64(drType["Cnt"]), Convert.ToInt64(drType["MaxState"]));
                }
                drType.Close();

                Decimal? koefA = null;
                Decimal? koefB = null;
                com.CommandText = "Select KoefA, KoefB, CondenserTypeID from CondenserTypeParameters WHERE CondenserTypeID IN " +
                    "(SELECT CondenserTypeID FROM Condensers WHERE CondenserID = @id) OR CondenserTypeID = NULL " +
                    "ORDER BY COALESCE(CondenserTypeID, 0) DESC LIMIT 1";
                SQLiteDataReader drKoef = com.ExecuteReader();
                while (drKoef.Read())
                {
                    if (drKoef["KoefA"] != DBNull.Value) koefA = Convert.ToDecimal(drKoef["KoefA"]);
                    if (drKoef["KoefB"] != DBNull.Value) koefB = Convert.ToDecimal(drKoef["KoefB"]);
                    break;
                }
                drKoef.Close();

                if (koefA == null || koefB == null)
                {
                    if (dictTest.Count == 0 || dictTest.Count == 1 && dictTest.ContainsKey(CondenserTest.CondenserTestType.Resource))
                    {
                        if (dictTest.ContainsKey(CondenserTest.CondenserTestType.Resource) &&
                            dictTest[CondenserTest.CondenserTestType.Resource].Value != 0)
                        {
                            m_con.Close();
                            MyLocalizer.XtraMessageBoxShow("Невозможно добавить испытание данному конденсатору, т.к. он неработоспособен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        m_CondenserTestType = CondenserTest.CondenserTestType.Resource;
                    }
                    else
                    {
                        m_con.Close();
                        MyLocalizer.XtraMessageBoxShow("Для работы с испытаниями необходимо указать коэффициенты А и В степенной функции аппроксимации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    if (!dictTest.ContainsKey(CondenserTest.CondenserTestType.Resource))
                    {
                        if (dictTest.Count == 0) m_CondenserTestType = CondenserTest.CondenserTestType.Acceptance;
                        else m_CondenserTestType = CondenserTest.CondenserTestType.Operational;

                        if (dictTest.ContainsKey(CondenserTest.CondenserTestType.Operational))
                        {
                            if (dictTest[CondenserTest.CondenserTestType.Operational].Value != 0)
                            {
                                m_con.Close();
                                MyLocalizer.XtraMessageBoxShow("Невозможно добавить испытание данному конденсатору, т.к. он неработоспособен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }

                            com.CommandText = "Select CondenserTestID FROM CondenserTest WHERE CondenserID = @id AND CondenserTestType = @type AND AnalysisDateProbe IS NULL";
                            SQLiteParameter param2 = new SQLiteParameter("@type", DbType.Int64);
                            param2.Value = (long)CondenserTest.CondenserTestType.Operational;
                            com.Parameters.Add(param2);

                            SQLiteDataReader drTest = com.ExecuteReader();
                            while (drTest.Read())
                            {
                                m_CondenserTestID = Convert.ToInt64(drTest["CondenserTestID"]);
                                m_CondenserTestType = CondenserTest.CondenserTestType.Operational;
                                break;
                            }
                            drTest.Close();
                        }
                    }
                    else
                    {
                        m_con.Close();
                        MyLocalizer.XtraMessageBoxShow("Невозможно добавить ресурсное испытание данному конденсатору, т.к. тип конденсатора уже зарегистрирован в БД.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                m_con.Close();
            }
            catch (SQLiteException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (DBConcurrencyException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void GetPrevAnalysisData()
        {
            try
            {
                //if (m_dtOldAnalysisDateProbe != null && (DateTime)m_dtOldAnalysisDateProbe == dtAnalysisDateProbe.DateTime)
                //    return;

                dictLastAnalysisData.Clear();
                //m_dtLastAnalysisDateProbe = null;
                //m_dtOldAnalysisDateProbe = dtAnalysisDateProbe.DateTime;

                m_con.Open();

                SQLiteCommand com = new SQLiteCommand(m_con);
                com.CommandType = CommandType.Text;

                /*com.CommandText = "SELECT * FROM CondenserTest " +
                    " WHERE CondenserTest.CondenserTestID = (SELECT CondenserTestID FROM CondenserTest WHERE " +
                    " CondenserID = @condenser_id AND AnalysisDateProbe < @date AND CondenserTestID <> @condenser_test_id ORDER BY AnalysisDateProbe DESC LIMIT 1)";*/

                com.CommandText = "SELECT * FROM CondenserTest " +
                    " WHERE CondenserTest.CondenserTestID = (SELECT CondenserTestID FROM CondenserTest WHERE " +
                    " CondenserID = @condenser_id AND (CondenserTestID < @condenser_test_id OR @condenser_test_id <= 0) ORDER BY CondenserTestID DESC LIMIT 1)";

                //DateTime dateAnalysisDateProbe = dtAnalysisDateProbe.DateTime;
                //dateAnalysisDateProbe = dateAnalysisDateProbe.Date;

                AddParam(com, "@condenser_id", DbType.Int64, m_CondenserID);
                //AddParam(com, "@date", DbType.DateTime, dateAnalysisDateProbe);
                AddParam(com, "@condenser_test_id", DbType.Int64, m_CondenserTestID);

                SQLiteDataReader drLastData = com.ExecuteReader();
                if (drLastData.HasRows)
                {
                    while (drLastData.Read())
                    {
                        //m_dtLastAnalysisDateProbe = Convert.ToDateTime(drLastData["AnalysisDateProbe"]);

                        if (drLastData["H2"] != DBNull.Value) dictLastAnalysisData["H2"] = Convert.ToDouble(drLastData["H2"]);
                        else dictLastAnalysisData["H2"] = null;

                        if (drLastData["O2"] != DBNull.Value) dictLastAnalysisData["O2"] = Convert.ToDouble(drLastData["O2"]);
                        else dictLastAnalysisData["O2"] = null;

                        if (drLastData["N2"] != DBNull.Value) dictLastAnalysisData["N2"] = Convert.ToDouble(drLastData["N2"]);
                        else dictLastAnalysisData["N2"] = null;

                        if (drLastData["CH4"] != DBNull.Value) dictLastAnalysisData["CH4"] = Convert.ToDouble(drLastData["CH4"]);
                        else dictLastAnalysisData["CH4"] = null;

                        if (drLastData["CO"] != DBNull.Value) dictLastAnalysisData["CO"] = Convert.ToDouble(drLastData["CO"]);
                        else dictLastAnalysisData["CO"] = null;

                        if (drLastData["CO2"] != DBNull.Value) dictLastAnalysisData["CO2"] = Convert.ToDouble(drLastData["CO2"]);
                        else dictLastAnalysisData["CO2"] = null;

                        if (drLastData["C2H4"] != DBNull.Value) dictLastAnalysisData["C2H4"] = Convert.ToDouble(drLastData["C2H4"]);
                        else dictLastAnalysisData["C2H4"] = null;

                        if (drLastData["C2H6"] != DBNull.Value) dictLastAnalysisData["C2H6"] = Convert.ToDouble(drLastData["C2H6"]);
                        else dictLastAnalysisData["C2H6"] = null;

                        if (drLastData["C2H2"] != DBNull.Value) dictLastAnalysisData["C2H2"] = Convert.ToDouble(drLastData["C2H2"]);
                        else dictLastAnalysisData["C2H2"] = null;

                        if (drLastData["CH3OH"] != DBNull.Value) dictLastAnalysisData["CH3OH"] = Convert.ToDouble(drLastData["CH3OH"]);
                        else dictLastAnalysisData["CH3OH"] = null;

                        if (drLastData["CO2_CO"] != DBNull.Value) dictLastAnalysisData["CO2_CO"] = Convert.ToDouble(drLastData["CO2_CO"]);
                        else dictLastAnalysisData["CO2_CO"] = null;

                        if (m_CondenserTestID == 0 && Convert.ToInt64(drLastData["CondenserTestType"]) == (long)m_CondenserTestType)
                        {
                            DataRowView dgv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);
                            dgv["TestVoltageFrequency"] = drLastData["TestVoltageFrequency"];
                            dgv["TestVoltageAmplitude"] = drLastData["TestVoltageAmplitude"];
                            dgv["CondenserTestEquipment"] = drLastData["CondenserTestEquipment"];
                            dgv["TestVoltageImagePoints"] = drLastData["TestVoltageImagePoints"];

                            teVoltageFrequency.EditValue = dgv["TestVoltageFrequency"];
                            teVoltageAmplitude.EditValue = dgv["TestVoltageAmplitude"];
                            teTestEquipment.EditValue = dgv["CondenserTestEquipment"];
                            m_bChangeData = true;
                        }

                        break;
                    }
                }
                drLastData.Close();

                m_con.Close();

                CalcAllV();
            }
            catch (SQLiteException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (DBConcurrencyException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void GetSheduler()
        {
            try
            {
                if (m_CondenserTestID > 0 && m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                {
                    m_con.Open();

                    SQLiteCommand com_ = new SQLiteCommand(m_con);
                    com_.CommandText = "SELECT ShedulerID, ShedulerDate, ShedulerCycleFrequency, ShedulerCycleCount FROM CondenserTestSheduler WHERE CondenserTestID = @id ORDER BY ShedulerID";
                    SQLiteParameter param1 = new SQLiteParameter("@id", DbType.Int64);
                    param1.Value = m_CondenserTestID;
                    com_.Parameters.Add(param1);
                    com_.CommandType = CommandType.Text;
                    SQLiteDataReader dr_ = com_.ExecuteReader();

                    while (dr_.Read())
                    {
                        /*object[] ItemArray = new object[5];
                        ItemArray[iPosShedulerID] = Convert.ToInt64(dr_["ShedulerID"]);
                        ItemArray[iPosShedulerDate] = Convert.ToDateTime(dr_["ShedulerDate"]);
                        ItemArray[iPosShedulerFrequency] = Convert.ToDecimal(dr_["ShedulerCycleFrequency"]);
                        ItemArray[iPosShedulerCycleCount] = Convert.ToInt64(dr_["ShedulerCycleCount"]);
                        ItemArray[iPosShedulerWasChange] = false;*/
                        DataRow row = tableSheduler.NewRow();
                        row["ID"] = Convert.ToInt64(dr_["ShedulerID"]);
                        row["DATE"] = Convert.ToDateTime(dr_["ShedulerDate"]);
                        row["FREQUENCY"] = Convert.ToDecimal(dr_["ShedulerCycleFrequency"]);
                        row["CYCLE_COUNT"] = Convert.ToInt64(dr_["ShedulerCycleCount"]);
                        row["WAS_CHANGE"] = false;
                        tableSheduler.Rows.Add(row);
                    }
                    dr_.Close();

                    m_con.Close();
                }
            }
            catch (SQLiteException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (DBConcurrencyException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void TestDataForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            try
            {
                this.WindowState = FormWindowState.Maximized;

                //m_bExpertMode = ((MainForm)Owner).m_bExpertMode;


                string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (strSeparator == ".") strSeparator = "\\.";
                this.repDecimal4.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,4})";
                this.repDecimal2.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,2})";
                this.teVoltageAmplitude.Properties.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,2})";
                this.teVoltageFrequency.Properties.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,2})";

                try
                {
                    this.qCondenserTestRecordTableAdapter.Fill(this.dataSetQuery.QCondenserTestRecord, m_CondenserTestID, m_CondenserID);
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                listTestType.Add(new DataSourceString(1, "ресурсные"));
                listTestType.Add(new DataSourceString(2, "приемо-сдаточные"));
                listTestType.Add(new DataSourceString(3, "эксплуатация"));
                cbTestType.Properties.DataSource = listTestType;
                cbTestType.Properties.DisplayMember = "VAL";
                cbTestType.Properties.ValueMember = "KEY";
                cbTestType.Properties.DropDownRows = listTestType.Count;

                //listSheduler.Add(new ShedulerInfo(DateTime.Now, 1, 2));
                //listSheduler.AllowNew = true;
                //listSheduler.AddingNew += new AddingNewEventHandler(listSheduler_AddingNew);
                //listSheduler.RaiseListChangedEvents = true;
                //listSheduler.ListChanged += new ListChangedEventHandler(listSheduler_ListChanged);

                tableSheduler.Columns.Add("ID", typeof(long));
                tableSheduler.Columns.Add("DATE", typeof(DateTime));
                tableSheduler.Columns.Add("FREQUENCY", typeof(Decimal));
                tableSheduler.Columns.Add("CYCLE_COUNT", typeof(long));
                tableSheduler.Columns.Add("WAS_CHANGE", typeof(bool));
                tableSheduler.TableNewRow += new DataTableNewRowEventHandler(tableSheduler_TableNewRow);
                tableSheduler.RowChanging += new DataRowChangeEventHandler(tableSheduler_RowChanging);
                tableSheduler.RowDeleting += new DataRowChangeEventHandler(tableSheduler_RowDeleting);
                tableSheduler.RowChanged += new DataRowChangeEventHandler(tableSheduler_RowChanged);
                tableSheduler.RowDeleted += new DataRowChangeEventHandler(tableSheduler_RowDeleted);

                GridGC.DataSource = tableSheduler;

                bmp_pb = new Bitmap(pb.Width, pb.Height);
                pb.Image = bmp_pb;

                if (m_bShowContinueMsg)
                {
                    bPrev.Visible = true;
                    //bNext.Visible = true;
                }

                teCntNodes.Text = "0";
                //for (int i = 0; i < 0; i++) m_listVoltagePoints.Add(0);

                Graphics gr = pb.CreateGraphics();

                DpiXRel = gr.DpiX / 96.0f;
                DpiYRel = gr.DpiY / 96.0f;

                fMargin *= DpiXRel;
                fTail *= DpiXRel;
                fRadius *= DpiXRel;

                DataRowView dgv = null;
                if (m_CondenserTestID > 0)
                {
                    dgv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);

                    this.Text = "Изменение испытаний";

                    if (dgv["CycleCount"] != DBNull.Value)
                        teCycleCount.EditValue = Convert.ToInt64(dgv["CycleCount"]);
                    else
                        teCycleCount.EditValue = null;
                    m_CondenserTestType = (CondenserTest.CondenserTestType)Convert.ToInt64(dgv["CondenserTestType"]);
                    m_CondenserState = Convert.ToInt64(dgv["CondenserState"]);
                    cbDoNotWork.Checked = m_CondenserState == 0 ? false : true;

                    GetSheduler();
                }
                else
                {
                    this.Text = "Добавление новых испытаний";

                    cbCanChangeVoltagePoints.Checked = true;
                    cbCanEdit.Checked = true;
                    teCycleCount.EditValue = DBNull.Value;

                    this.qCondenserTestRecordBindingSource.AddNew();
                    dgv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);
                    dgv["CondenserID"] = m_CondenserID;
                    dgv["CondenserTestType"] = (long)m_CondenserTestType;
                    cbTestType.EditValue = (long)m_CondenserTestType;
                }

                if (this.qCondenserTestRecordBindingSource.Count == 0)
                {
                    this.Visible = false;
                    MyLocalizer.XtraMessageBoxShow("Не удалось найти испытание.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                    return;
                }

                try
                {
                    String strCondenserTypeName = "", strCondenserNumber = "";
                    m_con.Open();
                    SQLiteCommand com_ = new SQLiteCommand(m_con);

                    // определяем, последний ли тест
                    if (m_CondenserTestID <= 0)
                        m_bCondenserTestIsLast = true;
                    else
                    {
                        com_.CommandText = "SELECT COUNT(*) AS Cnt FROM CondenserTest WHERE CondenserID = @id AND CondenserTestID > @test_id";
                        com_.Parameters.Clear();
                        SQLiteParameter id = new SQLiteParameter("@id", DbType.Int64);
                        id.Value = m_CondenserID;
                        com_.Parameters.Add(id);
                        SQLiteParameter test_id = new SQLiteParameter("@test_id", DbType.Int64);
                        test_id.Value = m_CondenserTestID;
                        com_.Parameters.Add(test_id);
                        com_.CommandType = CommandType.Text;
                        SQLiteDataReader drCnt = com_.ExecuteReader();
                        while (drCnt.Read())
                        {
                            if (Convert.ToInt64(drCnt["Cnt"]) == 0) m_bCondenserTestIsLast = true;
                            break;
                        }
                        drCnt.Close();
                    }


                    com_.CommandText = "SELECT c.CondenserNumber, ct.CondenserTypeName, c.NominalVoltage FROM Condensers AS c " +
                        "INNER JOIN CondenserTypes AS ct ON c.CondenserTypeID = ct.CondenserTypeID WHERE c.CondenserID = @id";
                    SQLiteParameter param1 = new SQLiteParameter("@id", DbType.Int64);
                    param1.Value = m_CondenserID;
                    com_.Parameters.Clear();
                    com_.Parameters.Add(param1);
                    com_.CommandType = CommandType.Text;
                    SQLiteDataReader dr_ = com_.ExecuteReader();

                    Decimal fNominalVoltage = 0;
                    while (dr_.Read())
                    {
                        strCondenserTypeName = Convert.ToString(dr_["CondenserTypeName"]);
                        strCondenserNumber = Convert.ToString(dr_["CondenserNumber"]);
                        fNominalVoltage = Convert.ToDecimal(dr_["NominalVoltage"]);
                    }
                    dr_.Close();

                    m_con.Close();

                    teEquipmentNumber.EditValue = strCondenserNumber;
                    teEquipmentType.EditValue = strCondenserTypeName;

                    if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                    {
                        teVoltageAmplitude.Properties.ReadOnly = true;
                        teVoltageAmplitude.EditValue = fNominalVoltage;
                        dgv["TestVoltageAmplitude"] = fNominalVoltage;
                        teCycleCount.Properties.ReadOnly = true;
                    }
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (m_CondenserTestType != CondenserTest.CondenserTestType.Resource) panelCycles.Visible = false;
                if (m_CondenserTestType != CondenserTest.CondenserTestType.Operational) panelSheduler.Visible = false;
                //if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational) panelCycles.Visible = false;

                if (!Program.m_bExpertMode)
                {
                    rCH3OH.Visible = false;
                    rCO2_CO.Visible = false;
                }

                if (!m_bCondenserTestIsLast || m_CondenserTestType == CondenserTest.CondenserTestType.Acceptance) cbDoNotWork.Visible = false;

                UpdateCycleCount();

                String strVoltagePoints = dgv["TestVoltageImagePoints"].ToString();
                if (strVoltagePoints != "")
                {
                    m_listVoltagePoints.Clear();
                    string[] vec = strVoltagePoints.Split(';');
                    if (vec.GetLength(0) > 0)
                    {
                        for (int i = 0; i < vec.GetLength(0); i++)
                        {
                            try
                            {
                                m_listVoltagePoints.Add((float)Convert.ToDouble(vec[i]));
                            }
                            catch (Exception)
                            {
                            }
                        }
                        // приводим к масштабу окна изображения
                        float height = pb.Height - fMargin * 2;
                        for (int i = 0; i < m_listVoltagePoints.Count; i++)
                        {
                            m_listVoltagePoints[i] *= height / 10000.0f;
                        }
                    }
                }

                teCntNodes.Text = m_listVoltagePoints.Count.ToString();

                pb_Paint();

                /*foreach (GridColumn col in GridShedulerView.Columns)
                {
                    GetColumnBestHeight(col);
                }
                SetMaxColumnHeights();*/

                if (!Program.m_bExpertMode)
                {
                    for (int i = 0; i < GridVertical.Rows.Count; i++)
                    {
                        //((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]).PropertiesCollection[0].CellWidth = 10000;
                        //((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]).PropertiesCollection[1].CellWidth = 10;
                        //((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]).PropertiesCollection[2].CellWidth = 10;

                        ((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]).PropertiesCollection.RemoveAt(2);
                        ((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]).PropertiesCollection.RemoveAt(1);
                    }
                }

                GetPrevAnalysisData();

                m_bDataLoadEnd = true;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void tableSheduler_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            UpdateCycleCount();
        }

        void tableSheduler_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            /*if (e.Action == DataRowAction.Change)
            {
                int index = tableSheduler.Rows.IndexOf(e.Row);
                tableSheduler.Rows[index].ItemArray[iPosShedulerWasChange] = true;
                //e.Row["WAS_CHANGE"] = true;
                //tableSheduler.AcceptChanges();
            }*/

            UpdateCycleCount();
        }

        void UpdateCycleCount()
        {
            if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
            {
                long iCycleCount = 0;

                for (int i = 0; i < tableSheduler.Rows.Count; i++)
                {
                    if (tableSheduler.Rows[i]["CYCLE_COUNT"] != DBNull.Value)
                        iCycleCount += Convert.ToInt64(tableSheduler.Rows[i]["CYCLE_COUNT"]);
                }

                teCycleCount.EditValue = iCycleCount;
                
                DataRowView dgv = null;
                dgv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);
                dgv["CycleCount"] = teCycleCount.EditValue;
            }
        }

        void tableSheduler_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (e.Action == DataRowAction.Delete)
            {
                if (Convert.ToInt64(e.Row["ID"]) > 0)
                {
                    listDeletedSheduler.Add(Convert.ToInt64(e.Row["ID"]));
                }
            }
            //throw new NotImplementedException();
        }

        void tableSheduler_TableNewRow(object sender, DataTableNewRowEventArgs e)
        {
            e.Row["ID"] = 0;
            DateTime date = DateTime.Now;
            if (tableSheduler.Rows.Count > 0)
            {
                date = Convert.ToDateTime(tableSheduler.Rows[tableSheduler.Rows.Count - 1]["DATE"]);
            }
            e.Row["DATE"] = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
            e.Row["WAS_CHANGE"] = true;
            //throw new NotImplementedException();
        }

        void tableSheduler_RowChanging(object sender, DataRowChangeEventArgs e)
        {
            /*if (e.Action == DataRowAction.Change)
            {
                int index = tableSheduler.Rows.IndexOf(e.Row);
                ((DataRow)(tableSheduler.Rows[index])).ItemArray[iPosShedulerWasChange] = true;
            }*/
        }

        /*void listSheduler_ListChanged(object sender, ListChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void listSheduler_AddingNew(object sender, AddingNewEventArgs e)
        {
            //throw new NotImplementedException();
            e.NewObject = new ShedulerInfo(DateTime.Now, null, null);
        }*/

        private void TestForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Form f = this.Owner;
                while (f.Owner != null)
                {
                    f.Hide();
                    f = f.Owner;
                }
                f.Hide();// = FormWindowState.Minimized;
                //this.ShowInTaskbar = true;

            }
            if (this.WindowState != FormWindowState.Minimized && m_bDataLoadEnd /*&& this.ShowInTaskbar*/)
            {
                Form f = this.Owner;
                while (f.Owner != null)
                {
                    if (!f.Visible) f.Show();
                    f = f.Owner;
                }

                if (!f.Visible) f.Show();
                //this.ShowInTaskbar = false;
            }            
        }

        private void bCancel_Click(object sender, EventArgs e)
        {

        }

        private void DrawModel(Graphics gr)
        {
            Pen pen = new Pen(Color.DarkGray);
            Pen pen_g = new Pen(Color.LightGray);
            pen_g.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            // рисуем оси
            gr.DrawLine(pen, new PointF(fMargin - fTail, pb.Height - fMargin), new PointF(pb.Width - fMargin + fTail, pb.Height - fMargin));
            gr.DrawLine(pen, new PointF(fMargin, pb.Height - fMargin + fTail), new PointF(fMargin, fMargin - fTail));

            // рисуем сетку
            float step = (pb.Height - fMargin * 2) / 5.0f;
            for (int i = 1; i <= 5; i++)
            {
                gr.DrawLine(pen_g, new PointF(fMargin, pb.Height - fMargin - i * step), new PointF(pb.Width - fMargin, pb.Height - fMargin - i * step));
            }

            step = 0;
            if (m_listVoltagePoints.Count >= 2)
            {
                step = (pb.Width - fMargin * 2) / (m_listVoltagePoints.Count - 1);
                //int dif = 0;
                for (int i = 1; i < m_listVoltagePoints.Count; i++)
                {
                    gr.DrawLine(pen_g, new PointF(fMargin + i * step, pb.Height - fMargin), new PointF(fMargin + i * step, fMargin));
                }

                for (int i = 0; i < m_listVoltagePoints.Count; i++)
                {
                    if (i > 0)
                    {
                        gr.DrawLine(new Pen(Color.Black), new PointF(fMargin + (i - 1) * step, pb.Height - fMargin - m_listVoltagePoints[i - 1]), new PointF(fMargin + i * step, pb.Height - fMargin - m_listVoltagePoints[i]));
                    }
                }
            }

            for (int i = 0; i < m_listVoltagePoints.Count; i++)
            {
                if (cbCanChangeVoltagePoints.Checked)
                    gr.FillEllipse(new SolidBrush(Color.Green), fMargin + i * step - fRadius, pb.Height - fMargin - m_listVoltagePoints[i] - fRadius, fRadius * 2, fRadius * 2);
                else
                    gr.FillEllipse(new SolidBrush(Color.Black), fMargin + i * step - fRadius, pb.Height - fMargin - m_listVoltagePoints[i] - fRadius, fRadius * 2, fRadius * 2);
            }
        }

        private void pb_Paint()
        {
            Graphics gr = Graphics.FromImage(bmp_pb);// e.Graphics;
            gr.FillRectangle(new SolidBrush(Color.White), pb.ClientRectangle);

            DrawModel(gr);

            pb.Invalidate();
        }

        private void teCntNodes_Leave(object sender, EventArgs e)
        {
            if (teCntNodes.Text == "") teCntNodes.Text = "0";
            int iCntNodes = Convert.ToInt32(teCntNodes.Text);
            if (iCntNodes > m_iMaxNodes)
            {
                teCntNodes.Text = m_iMaxNodes.ToString();
                iCntNodes = m_iMaxNodes;
            }

            if (m_listVoltagePoints.Count > iCntNodes)
            {
                m_listVoltagePoints.RemoveRange(iCntNodes, m_listVoltagePoints.Count - iCntNodes);
            }
            if (m_listVoltagePoints.Count < iCntNodes)
            {
                for (int i = m_listVoltagePoints.Count; i < iCntNodes; i++) m_listVoltagePoints.Add(0);
            }
            pb_Paint();
        }

        private void pb_MouseMove(object sender, MouseEventArgs e)
        {
            PointF ptMouse = new PointF(e.X, e.Y);
            if (cbCanChangeVoltagePoints.Checked)
            {
                if (!bPressDown)
                {
                    float step = 0;
                    if (m_listVoltagePoints.Count >= 2)
                        step = (pb.Width - fMargin * 2) / (m_listVoltagePoints.Count - 1);
                    m_iSelectedVoltagePoint = -1;
                    for (int i = 0; i < m_listVoltagePoints.Count; i++)
                    {
                        PointF pt = new PointF(fMargin + (i) * step, pb.Height - fMargin - m_listVoltagePoints[i]);

                        if (ptMouse.X >= pt.X - fRadius && ptMouse.X <= pt.X + fRadius &&
                            ptMouse.Y >= pt.Y - fRadius && ptMouse.Y <= pt.Y + fRadius)
                        {
                            Cursor.Current = Cursors.Hand;
                            m_iSelectedVoltagePoint = i;
                            return;
                        }
                    }
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    if (m_iSelectedVoltagePoint >= 0)
                    {
                        Cursor.Current = Cursors.Hand;
                        m_listVoltagePoints[m_iSelectedVoltagePoint] = pb.Height - fMargin - e.Y;
                        if (m_listVoltagePoints[m_iSelectedVoltagePoint] < 0) m_listVoltagePoints[m_iSelectedVoltagePoint] = 0;
                        if (m_listVoltagePoints[m_iSelectedVoltagePoint] > pb.Height - fMargin * 2) m_listVoltagePoints[m_iSelectedVoltagePoint] = pb.Height - fMargin * 2;
                        pb_Paint();
                        //m_oldMousePoint = new Point(e.X, e.Y);
                    }
                }
            }
        }

        private void pb_MouseDown(object sender, MouseEventArgs e)
        {
            if (cbCanChangeVoltagePoints.Checked)
            {
                if (m_iSelectedVoltagePoint >= 0)
                {
                    bPressDown = true;
                }
            }

            //pb_Paint();
        }

        private void pb_MouseUp(object sender, MouseEventArgs e)
        {
            bPressDown = false;
        }

        private void cbCanChangeVoltagePoints_CheckedChanged(object sender, EventArgs e)
        {
            /*if (m_bDataLoadEnd)*/ m_bChangeData = true;

            if (cbCanChangeVoltagePoints.Checked)
            {
                teCntNodes.Properties.ReadOnly = false;
                if (teCntNodes.Text == "0") teCntNodes.Text = "5";
            }
            else
            {
                teCntNodes.Properties.ReadOnly = true;
                //peImage.EditValue = bmp_pb;
            }
            pb_Paint();
        }

        private void teCntNodes_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            /*if (!m_bDataLoadEnd) return;
            timer1.Enabled = false;
            timer1.Enabled = true;*/
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (teCntNodes.Text == "") teCntNodes.EditValue = 0;
            int iCntNodes = Convert.ToInt32(teCntNodes.EditValue);
            if (iCntNodes > m_iMaxNodes)
            {
                teCntNodes.Text = m_iMaxNodes.ToString();
                iCntNodes = m_iMaxNodes;
            }

            if (m_listVoltagePoints.Count > iCntNodes)
            {
                m_listVoltagePoints.RemoveRange(iCntNodes, m_listVoltagePoints.Count - iCntNodes);
            }
            if (m_listVoltagePoints.Count < iCntNodes)
            {
                for (int i = m_listVoltagePoints.Count; i < iCntNodes; i++) m_listVoltagePoints.Add(0);
            }

            pb_Paint();
        }

        private long GetCondenserState(ref string strRecommendation, ref string strConclusion, ref long? CCNext, ref double? VR, ref double? KCF, bool bShowMsg = false)
        {
            strRecommendation = "";
            strConclusion = "";
            CCNext = null;
            VR = null;
            KCF = null;

            // получаем данные для типа конденсатора
            Decimal HLS = 0;
            Decimal? KI1 = null;
            /*/Decimal? KIn80 = null;
            Decimal? KIn90 = null;
            Decimal? KIn100 = null;*/
            Dictionary<long, ParameterRecordForm.RecommendationInfo> dictKIn = new Dictionary<long, ParameterRecordForm.RecommendationInfo>();

            Decimal? KoefA = null;
            Decimal? KoefB = null;
            CondenserTest.FunctionType functionType = CondenserTest.FunctionType.Degree;

            try
            {
                //m_con.Open();

                SQLiteCommand com = new SQLiteCommand(m_con);
                com.CommandText = "select ParameterValue from CommonParameters where ParameterID = 1";
                com.CommandType = CommandType.Text;
                SQLiteDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    HLS = Convert.ToDecimal(dr["ParameterValue"]);
                }
                dr.Close();

                com.CommandText = "select * from CondenserTypeParameters WHERE CondenserTypeID IN " +
                    "(SELECT CondenserTypeID FROM Condensers WHERE CondenserID = @id)";
                com.CommandType = CommandType.Text;
                AddParam(com, "@id", DbType.Int64, (long)m_CondenserID);

                dr = com.ExecuteReader();
                long iKoefID = 0;
                while (dr.Read())
                {
                    iKoefID = Convert.ToInt64(dr["KoefID"]);
                    if (dr["KI1"] != DBNull.Value) KI1 = Convert.ToDecimal(dr["KI1"]);
                    //if (dr["KIn80"] != DBNull.Value) KIn80 = Convert.ToDecimal(dr["KIn80"]);
                    //if (dr["KIn90"] != DBNull.Value) KIn90 = Convert.ToDecimal(dr["KIn90"]);
                    //if (dr["KIn100"] != DBNull.Value) KIn100 = Convert.ToDecimal(dr["KIn100"]);
                    if (dr["KoefA"] != DBNull.Value) KoefA = Convert.ToDecimal(dr["KoefA"]);
                    if (dr["KoefB"] != DBNull.Value) KoefB = Convert.ToDecimal(dr["KoefB"]);
                    if (dr["KoefB"] != DBNull.Value) functionType = (CondenserTest.FunctionType)Convert.ToInt64(dr["FunctionType"]);

                    break;
                }
                dr.Close();

                if (iKoefID > 0)
                {
                    com.CommandText = "select * from ParameterRecommendations WHERE KoefID = @KoefID";
                    com.Parameters.Clear();
                    AddParam(com, "@koefID", DbType.Int64, iKoefID);
                    dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        long iPosition = Convert.ToInt64(dr["Position"]);
                        Decimal? fValue = null;
                        if (dr["Value"] != DBNull.Value) fValue = Convert.ToDecimal(dr["Value"]);
                        string strConclusion_ = Convert.ToString(dr["Conclusion"]);
                        string strRecommendation_ = Convert.ToString(dr["Recommendation"]);
                        dictKIn[iPosition] = new ParameterRecordForm.RecommendationInfo(null, strRecommendation_, strConclusion_, fValue);
                    }
                    dr.Close();
                }

                long iNormalizedResource = 0;
                long iAllCycleCount = 0;
                if (teCycleCount.EditValue != null && teCycleCount.EditValue != DBNull.Value) iAllCycleCount = Convert.ToInt64(teCycleCount.EditValue);
                com.CommandText = "select NormalizedResource, (SELECT SUM(COALESCE(CycleCount, 0)) FROM CondenserTest WHERE CondenserID = @id AND CondenserTestID <> @test_id) AS AllCycleCount FROM Condensers WHERE CondenserID = @id";
                com.CommandType = CommandType.Text;

                com.Parameters.Clear();
                AddParam(com, "@id", DbType.Int64, (long)m_CondenserID);
                AddParam(com, "@test_id", DbType.Int64, (long)m_CondenserTestID);

                dr = com.ExecuteReader();
                while (dr.Read())
                {
                    if (dr["NormalizedResource"] != DBNull.Value) iNormalizedResource = Convert.ToInt64(dr["NormalizedResource"]);
                    if (dr["AllCycleCount"] != DBNull.Value) iAllCycleCount += Convert.ToInt64(dr["AllCycleCount"]);

                    break;
                }
                dr.Close();

                DataRowView drv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);

                if (m_CondenserTestType == CondenserTest.CondenserTestType.Acceptance)
                {
                    if (KI1 == null)
                    {
                        m_con.Close();
                        if (bShowMsg)
                        {
                            if (!Program.m_bExpertMode)
                                MyLocalizer.XtraMessageBoxShow("Для получения заключения и рекомендаций необходимо чтобы текущий тип конденсатора был зарегистрирован в БД.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else 
                                MyLocalizer.XtraMessageBoxShow("Для получения заключения и рекомендаций необходимо указать значение КЦ1.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return -1;
                        }
                        return 0;
                    }
                    // Концентрация С2Н2 ≥ ПЧX - Заключение: Конденсатор не допущен в эксплуатацию
                    // иначе - Заключение: Конденсатор допущен в эксплуатацию, рекомендация: следующий отбор пробы масла для анализа провести после КЦ1** циклов
                    if (Convert.ToDecimal(drv.Row["C2H2"]) >= HLS)
                    {
                        strConclusion = "Конденсатор не допущен в эксплуатацию";
                        return 1;
                    }
                    else
                    {
                        strConclusion = "Конденсатор допущен в эксплуатацию";
                        CCNext = (int)(iNormalizedResource * (Decimal)KI1 / 100);
                        strRecommendation = "Следующий отбор пробы масла для анализа провести после " + CCNext.ToString() + " циклов";
                        return 0;
                    }
                }
                if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                {
                    if (dictKIn.Count == 0 || KoefA == null || KoefB == null)
                    {
                        if (bShowMsg)
                        {
                            if (!Program.m_bExpertMode)
                                MyLocalizer.XtraMessageBoxShow("Для получения заключения и рекомендаций необходимо чтобы текущий тип конденсатора был зарегистрирован в БД.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MyLocalizer.XtraMessageBoxShow("Для получения заключения и рекомендаций необходимо указать значения КЦn, коэффициентов A и B.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return -1;
                        }
                        return 0;
                    }

                    if (functionType == CondenserTest.FunctionType.Degree)
                        VR = Convert.ToDouble((Decimal)KoefA) * Math.Pow(Convert.ToDouble((Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"]))), Convert.ToDouble(KoefB));
                    if (functionType == CondenserTest.FunctionType.Exp)
                        VR = Convert.ToDouble((Decimal)KoefA) * Math.Exp(Convert.ToDouble((Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"]))) * Convert.ToDouble(KoefB));
                    if (functionType == CondenserTest.FunctionType.Log)
                        VR = Convert.ToDouble((Decimal)KoefA) * Math.Log(Convert.ToDouble((Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"])))) + Convert.ToDouble(KoefB);

                    KCF = (double)iAllCycleCount / iNormalizedResource * 100;

                    int iCalcPosition = 0;
                    if (VR <= 80)
                    {
                        if (KCF <= 80) iCalcPosition = 11;
                        else if (KCF <= 90) iCalcPosition = 12;
                        else if (KCF < 100) iCalcPosition = 13;
                        else iCalcPosition = 14;
                    }
                    else
                    {
                        if (VR <= 90)
                        {
                            if (KCF <= 80) iCalcPosition = 21;
                            else if (KCF <= 90) iCalcPosition = 22;
                            else if (KCF < 100) iCalcPosition = 23;
                            else iCalcPosition = 24;
                        }
                        else
                        {
                            if (VR < 100)
                            {
                                if (KCF <= 80) iCalcPosition = 31;
                                else if (KCF <= 90) iCalcPosition = 32;
                                else if (KCF < 100) iCalcPosition = 33;
                                else iCalcPosition = 34;
                            }
                            else
                            {
                                if (KCF <= 80) iCalcPosition = 41;
                                else if (KCF <= 90) iCalcPosition = 42;
                                else if (KCF < 100) iCalcPosition = 43;
                                else iCalcPosition = 44;
                            }
                        }
                    }

                    if (dictKIn[iCalcPosition].m_fValue != null)
                    {
                        CCNext = (int)(iNormalizedResource * (Decimal)dictKIn[iCalcPosition].m_fValue / 100);
                        strConclusion = dictKIn[iCalcPosition].m_strConclusion;
                        strRecommendation = dictKIn[iCalcPosition].m_strRecommendation;
                        strRecommendation = strRecommendation.Replace("$", CCNext.ToString());

                        return 0;
                    }
                    else
                    {
                        strConclusion = dictKIn[iCalcPosition].m_strConclusion;
                        strRecommendation = dictKIn[iCalcPosition].m_strRecommendation;
                        
                        return 1;
                    }

                    /*if (VR <= 80 && KCF <= 80)
                    {
                        CCNext = (int)(iNormalizedResource * (Decimal)KIn80 / 100);
                        strConclusion = "Конденсатор работоспособен";
                        strRecommendation = "Следующий отбор пробы масла для анализа провести после " + CCNext.ToString() + " циклов";

                        return 0;
                    }
                    else
                    {
                        if (VR <= 90 && KCF <= 90)
                        {
                            CCNext = (int)(iNormalizedResource * (Decimal)KIn90 / 100);
                            strConclusion = "Конденсатор работоспособен";
                            strRecommendation = "Следующий отбор пробы масла для анализа провести после " + CCNext.ToString() + " циклов";

                            return 0;
                        }
                        else
                        {
                            if (VR <= 90)
                            {
                                CCNext = (int)(iNormalizedResource * (Decimal)KIn100 / 100);
                                strConclusion = "Конденсатор допущен в эксплуатацию";
                                strRecommendation = "Следующий отбор пробы масла для анализа провести после " + CCNext.ToString() + " циклов";

                                return 0;
                            }
                            else
                            {
                                if (VR >= 100)
                                {
                                    strConclusion = "По показаниям газосодержания масла ресурс конденсатора исчерпан";
                                    strRecommendation = "Вывести конденсатор из эксплуатации";

                                    return 1;
                                }
                                else
                                {
                                    if (KCF < 100)
                                    {
                                        CCNext = (int)(iNormalizedResource * (Decimal)KIn100 / 100);
                                        strConclusion = "По показаниям газосодержания масла выработано более 90% ресурса конденсатора";
                                        strRecommendation = "Следующий отбор пробы масла для анализа провести после " + CCNext.ToString() + " циклов";

                                        return 0;
                                    }
                                    else
                                    {
                                        strConclusion = "По показаниям газосодержания масла выработано более 90% ресурса конденсатора";
                                        strRecommendation = "Целесообразно вывести конденсатор из эксплуатации";

                                        return 1;
                                    }
                                }
                            }
                        }
                    }*/
                }
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DBConcurrencyException ex)
            {
                //m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            return 0;
        }

        private bool SaveData()
        {
            SQLiteTransaction tran = null;
            try
            {
                DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                if (drv == null) return false;

                object val = drv.Row["CondenserTestDate"];
                if (val == null || val == DBNull.Value)
                {
                    dtTestDate.Focus();
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать дату проведения испытаний.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                val = drv.Row["CondenserTestType"];
                if (val == null || val == DBNull.Value)
                {
                    cbTestType.Focus();
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать вид испытаний.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (teTemperature.Text == "")
                {
                    teTemperature.Focus();
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать температуру конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (m_CondenserTestType == CondenserTest.CondenserTestType.Resource)
                {
                    val = drv.Row["CycleCount"];
                    if (val == null || val == DBNull.Value || Convert.ToInt64(val) == 0)
                    {
                        teCycleCount.Focus();
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать количество циклов после предыдущего анализа масла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                {
                    if (tableSheduler.Rows.Count == 0)
                    {
                        MyLocalizer.XtraMessageBoxShow("Необходимо заполнить график эксплуатации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    for (int i = 0; i < tableSheduler.Rows.Count; i++)
                    {
                        /*if (tableSheduler.Rows[i]["FREQUENCY"] == DBNull.Value || Convert.ToInt64(tableSheduler.Rows[i]["FREQUENCY"]) <= 0)
                        {
                            MyLocalizer.XtraMessageBoxShow("В графике эксплуатации в одной из записей необходимо указать частоту циклов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }*/
                        if (tableSheduler.Rows[i]["CYCLE_COUNT"] == DBNull.Value || Convert.ToInt64(tableSheduler.Rows[i]["CYCLE_COUNT"]) <= 0)
                        {
                            MyLocalizer.XtraMessageBoxShow("В графике эксплуатации в одной из записей необходимо указать количество циклов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }

                val = drv.Row["TestVoltageFrequency"];
                if (val == null || val == DBNull.Value)
                {
                    teVoltageFrequency.Focus();
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать частоту циклов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                val = drv.Row["TestVoltageAmplitude"];
                if (val == null || val == DBNull.Value)
                {
                    teVoltageAmplitude.Focus();
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать амплитуду импульсов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                bool bNeedVerify = false;
                if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                {
                    if (!m_bCondenserTestIsLast)
                        bNeedVerify = true;

                    val = drv.Row["AnalysisDateProbe"];
                    if (val != null && val != DBNull.Value) bNeedVerify = true;

                    val = drv.Row["AnalysisDateExecute"];
                    if (val != null && val != DBNull.Value) bNeedVerify = true;

                    if (teAnalysisEmployeeFIO.Text != "") bNeedVerify = true;

                    if (drv.Row["H2"] != null && drv.Row["H2"] != DBNull.Value
                        || drv.Row["O2"] != null && drv.Row["O2"] != DBNull.Value
                        || drv.Row["N2"] != null && drv.Row["N2"] != DBNull.Value
                        || drv.Row["CH4"] != null && drv.Row["CH4"] != DBNull.Value
                        || drv.Row["CO"] != null && drv.Row["CO"] != DBNull.Value
                        || drv.Row["CO2"] != null && drv.Row["CO2"] != DBNull.Value
                        || drv.Row["C2H4"] != null && drv.Row["C2H4"] != DBNull.Value
                        || drv.Row["C2H6"] != null && drv.Row["C2H6"] != DBNull.Value
                        || drv.Row["C2H2"] != null && drv.Row["C2H2"] != DBNull.Value
                        /*|| (drv.Row["CH3OH"] != null && drv.Row["CH3OH"] != DBNull.Value) && Program.m_bExpertMode*/)
                    {
                        bNeedVerify = true;
                    }
                }

                if (m_CondenserTestType == CondenserTest.CondenserTestType.Resource ||
                    m_CondenserTestType == CondenserTest.CondenserTestType.Acceptance ||
                    bNeedVerify)
                {
                    val = drv.Row["AnalysisDateProbe"];
                    if (val == null || val == DBNull.Value)
                    {
                        dtAnalysisDateProbe.Focus();
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать дату отбора пробы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    val = drv.Row["AnalysisDateExecute"];
                    if (val == null || val == DBNull.Value)
                    {
                        dtAnalysisDateExecute.Focus();
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать дату выполнения анализа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (teAnalysisEmployeeFIO.Text == "")
                    {
                        teAnalysisEmployeeFIO.Focus();
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать ФИО выполняющего анализ работника.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (drv.Row["H2"] == null || drv.Row["H2"] == DBNull.Value
                        || drv.Row["O2"] == null || drv.Row["O2"] == DBNull.Value
                        || drv.Row["N2"] == null || drv.Row["N2"] == DBNull.Value
                        || drv.Row["CH4"] == null || drv.Row["CH4"] == DBNull.Value
                        || drv.Row["CO"] == null || drv.Row["CO"] == DBNull.Value
                        || drv.Row["CO2"] == null || drv.Row["CO2"] == DBNull.Value
                        || drv.Row["C2H4"] == null || drv.Row["C2H4"] == DBNull.Value
                        || drv.Row["C2H6"] == null || drv.Row["C2H6"] == DBNull.Value
                        || drv.Row["C2H2"] == null || drv.Row["C2H2"] == DBNull.Value
                        /*|| (drv.Row["CH3OH"] == null || drv.Row["CH3OH"] == DBNull.Value) && Program.m_bExpertMode*/)
                    {
                        GridVertical.FocusedRow = rC2H2;
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать значения концентраций всех перечисленных газов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Convert.ToDecimal(drv.Row["CO"]) == 0 || Convert.ToDecimal(drv.Row["CO2"]) == 0)
                    {
                        GridVertical.FocusedRow = rC2H2;
                        MyLocalizer.XtraMessageBoxShow("Необходимо указать ненулевые значения концентраций газов СО и CO₂.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Convert.ToDateTime(dtTestDate.EditValue).Date > Convert.ToDateTime(dtAnalysisDateProbe.EditValue).Date)
                    {
                        dtAnalysisDateProbe.Focus();
                        MyLocalizer.XtraMessageBoxShow("Дата проведения испытаний не должна превышать дату отбора пробы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (Convert.ToDateTime(dtAnalysisDateProbe.EditValue).Date > Convert.ToDateTime(dtAnalysisDateExecute.EditValue).Date)
                    {
                        dtAnalysisDateProbe.Focus();
                        MyLocalizer.XtraMessageBoxShow("Дата отбора пробы не должна превышать дату выполнения анализа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                    {
                        DateTime dtDateProbe = Convert.ToDateTime(dtAnalysisDateProbe.EditValue);
                        if (tableSheduler.Rows.Count > 0)
                        {
                            //DateTime dtFirstShedulerDate = Convert.ToDateTime(tableSheduler.Rows[0]["DATE"]);
                            DateTime dtLastShedulerDate = Convert.ToDateTime(tableSheduler.Rows[tableSheduler.Rows.Count - 1]["DATE"]);
                            if (dtDateProbe.Date < dtLastShedulerDate.Date)
                            {
                                dtAnalysisDateProbe.Focus();
                                MyLocalizer.XtraMessageBoxShow("Дата отбора пробы не должна быть меньше даты из последней записи графика эксплуатации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return false;
                            }
                        }
                    }
                }

                string strVoltagePoints = "";
                float height = pb.Height - fMargin * 2;
                for (int i = 0; i < m_listVoltagePoints.Count; i++)
                {
                    long h = Convert.ToInt64(Math.Round(m_listVoltagePoints[i] * 10000.0f / height, 0));
                    if (strVoltagePoints == "") strVoltagePoints = h.ToString();
                    else strVoltagePoints += ";" + h.ToString();
                }
                //((DataRowView)qCondenserTestRecordBindingSource.Current).BeginEdit();
                drv.Row["TestVoltageImagePoints"] = strVoltagePoints;
                if (strVoltagePoints != "")
                {
                    System.IO.MemoryStream stream = new System.IO.MemoryStream();
                    bmp_pb.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    drv.Row["TestVoltageImageForm"] = stream.ToArray();
                }
                else
                    drv.Row["TestVoltageImageForm"] = DBNull.Value;

                m_con.Open();

                // проверка на уникальность даты проведения испытания, даты отбора пробы, на корректность нахождения дат
                /*SQLiteCommand com = new SQLiteCommand(m_con);
                {
                    com.CommandText = "SELECT CASE WHEN CondenserTestDate = @test_date THEN 1 ELSE 2 END AS ErrorType from CondenserTest WHERE (AnalysisDateProbe = @probe_date OR CondenserTestDate = @test_date) AND CondenserID = @cond_id AND CondenserTestID <> @condenser_test_id";
                    com.CommandType = CommandType.Text;
                    SQLiteParameter param1_ = new SQLiteParameter("@test_date", DbType.DateTime);
                    param1_.Value = dtTestDate.EditValue;
                    SQLiteParameter param2_ = new SQLiteParameter("@cond_id", DbType.Int64);
                    param2_.Value = m_CondenserID;
                    SQLiteParameter param3_ = new SQLiteParameter("@condenser_test_id", DbType.Int64);
                    param3_.Value = m_CondenserTestID;
                    SQLiteParameter param4_ = new SQLiteParameter("@probe_date", DbType.DateTime);
                    param4_.Value = dtAnalysisDateProbe.EditValue;
                    //SQLiteParameter param5 = new SQLiteParameter("@execute_date", DbType.DateTime);
                    //param5.Value = dtAnalysisDateExecute.EditValue;
                    com.Parameters.Add(param1_);
                    com.Parameters.Add(param2_);
                    com.Parameters.Add(param3_);
                    com.Parameters.Add(param4_);
                    //com.Parameters.Add(param5);
                    SQLiteDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            if (Convert.ToInt64(dr["ErrorType"]) == 1)
                                MyLocalizer.XtraMessageBoxShow("Испытание данного конденсатора в эту дату уже проводилось.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MyLocalizer.XtraMessageBoxShow("Отбор пробы из данного конденсатора в эту дату уже проводился.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            dr.Close();
                            m_con.Close();
                            return false;
                        }
                    }
                    dr.Close();
                }*/

                // сравнение с предыдущими и последующими записями
                // PrevAnalysisExecuteDate <= TestDate
                // AnalysisExecuteDate <= NextTextDate

                SQLiteCommand com = new SQLiteCommand(m_con);
                //if (dtAnalysisDateExecute.EditValue != null && dtAnalysisDateExecute.EditValue != DBNull.Value)
                {
                    com.CommandText = "SELECT CASE WHEN AnalysisDateExecute > @test_date AND CondenserTestID < @condenser_test_id THEN 1 " +
                        "WHEN NOT @execute_date IS NULL AND CondenserTestDate < @execute_date AND CondenserTestID > @condenser_test_id THEN 2 ELSE 3 END AS ErrorType " +
                        "FROM CondenserTest WHERE CondenserID = @cond_id " +
                        "AND (AnalysisDateExecute > @test_date AND CondenserTestID < @condenser_test_id " +
                        "OR NOT @execute_date IS NULL AND CondenserTestDate < @execute_date AND CondenserTestID > @condenser_test_id " +
                        "OR NOT @sheduler_date IS NULL AND AnalysisDateProbe > @sheduler_date AND CondenserTestID < @condenser_test_id)";
                    com.CommandType = CommandType.Text;
                    com.Parameters.Clear();
                    SQLiteParameter param1_ = new SQLiteParameter("@test_date", DbType.DateTime);
                    param1_.Value = dtTestDate.EditValue;
                    SQLiteParameter param2_ = new SQLiteParameter("@cond_id", DbType.Int64);
                    param2_.Value = m_CondenserID;
                    SQLiteParameter param3_ = new SQLiteParameter("@condenser_test_id", DbType.Int64);
                    if (m_CondenserTestID > 0)
                        param3_.Value = m_CondenserTestID;
                    else
                        param3_.Value = long.MaxValue;
                    SQLiteParameter param4_ = new SQLiteParameter("@execute_date", DbType.DateTime);
                    if (dtAnalysisDateExecute.EditValue != null && dtAnalysisDateExecute.EditValue != DBNull.Value)
                        param4_.Value = dtAnalysisDateExecute.EditValue;
                    else
                        param4_.Value = DBNull.Value;

                    SQLiteParameter param5_ = new SQLiteParameter("@sheduler_date", DbType.DateTime);
                    param5_.Value = DBNull.Value;

                    com.Parameters.Add(param1_);
                    com.Parameters.Add(param2_);
                    com.Parameters.Add(param3_);
                    com.Parameters.Add(param4_);

                    if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                    {
                        DateTime dtFirstShedulerDate = Convert.ToDateTime(tableSheduler.Rows[0]["DATE"]);
                        param5_.Value = dtFirstShedulerDate;
                    }
                    com.Parameters.Add(param5_);
                    
                    //com.Parameters.Add(param5);
                    SQLiteDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            long iErrorType = Convert.ToInt64(dr["ErrorType"]);
                            if (iErrorType == 1)
                                MyLocalizer.XtraMessageBoxShow("Дата проведения испытания не должна быть меньше даты выполнения анализа предыдущих испытаний.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (iErrorType == 2)
                                MyLocalizer.XtraMessageBoxShow("Дата выполнения анализа не должна быть больше даты проведения следующих испытаний.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (iErrorType == 3)
                                MyLocalizer.XtraMessageBoxShow("Дата начала эксплуатации не должна быть меньше, чем дата предыдущего отбора пробы масла.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            dr.Close();
                            m_con.Close();
                            return false;
                        }
                    }
                    dr.Close();
                }

                string strRecommendation = drv.Row["Recommendation"].ToString();
                string strConclusion = drv.Row["Conclusion"].ToString();
                long? CCNext = null;
                if (drv.Row["CCNext"] != DBNull.Value) CCNext = Convert.ToInt64(drv.Row["CCNext"]);
                double? VR = null;
                if (drv.Row["VR"] != DBNull.Value) VR = Convert.ToDouble(drv.Row["VR"]);
                double? KCF = null;
                if (drv.Row["KCF"] != DBNull.Value) KCF = Convert.ToDouble(drv.Row["KCF"]);

                // определяем работоспособность конденсатора - либо вручную, либо по алгоритму
                if (m_CondenserTestType == CondenserTest.CondenserTestType.Acceptance)
                {
                    // расчет по алгоритму
                    m_CondenserState = GetCondenserState(ref strRecommendation, ref strConclusion, ref CCNext, ref VR, ref KCF, true);
                    if (m_CondenserState < 0)
                    {
                        m_con.Close();
                        return false;
                    }
                    m_strRecommendation = strRecommendation;
                    m_strConclusion = strConclusion;
                }
                else
                {
                    if (m_bCondenserTestIsLast)
                    {
                        if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                        {
                            if (dtAnalysisDateProbe.EditValue != null && dtAnalysisDateProbe.EditValue != DBNull.Value)
                            {
                                m_CondenserState = GetCondenserState(ref strRecommendation, ref strConclusion, ref CCNext, ref VR, ref KCF, true);
                                if (m_CondenserState < 0)
                                {
                                    m_con.Close();
                                    return false;
                                }
                                m_strRecommendation = strRecommendation;
                                m_strConclusion = strConclusion;
                            }
                            else
                            {
                                strRecommendation = "";
                                strConclusion = "";
                                CCNext = null;
                                VR = null;
                                KCF = null;
                            }
                        }

                        m_CondenserState = cbDoNotWork.Checked ? 1 : 0;
                    }
                }

                tran = m_con.BeginTransaction();

                if (m_CondenserTestID > 0)
                {
                    //((DataRowView)qEquipmentRecordBindingSource.Current).BeginEdit();
                    ((DataRowView)this.qCondenserTestRecordBindingSource.Current).EndEdit();

                    SQLiteCommand upd_com = new SQLiteCommand(/*this.qCondenserTestRecordTableAdapter.Connection*/m_con);
                    upd_com.CommandText = "UPDATE CondenserTest SET " +
                        "CondenserTestType = @param1, " +
                        "CondenserTestDate = @param2, " +
                        "CondenserTestEquipment = @param3, " +
                        "TestVoltageImageForm = @param4, " +
                        "TestVoltageImagePoints = @param5, " +
                        "TestVoltageFrequency = @param6, " +
                        "TestVoltageAmplitude = @param7, " +
                        "AnalysisDateProbe = @param8, " +
                        "AnalysisDateExecute = @param9, " +
                        "H2 = @param10, " +
                        "CH4 = @param11, " +
                        "CO = @param12, " +
                        "CO2 = @param13, " +
                        "C2H2 = @param14, " +
                        "C2H4 = @param15, " +
                        "C2H6 = @param16, " +
                        "O2 = @param17, " +
                        "N2 = @param18, " +
                        "H2_Vabs = @param19, " +
                        "H2_Votn = @param20, " +
                        "CH4_Vabs = @param21, " +
                        "CH4_Votn = @param22, " +
                        "CO_Vabs = @param23, " +
                        "CO_Votn = @param24, " +
                        "CO2_Vabs = @param25, " +
                        "CO2_Votn = @param26, " +
                        "C2H2_Vabs = @param27, " +
                        "C2H2_Votn = @param28, " +
                        "C2H4_Vabs = @param29, " +
                        "C2H4_Votn = @param30, " +
                        "C2H6_Vabs = @param31, " +
                        "C2H6_Votn = @param32, " +
                        "O2_Vabs = @param33, " +
                        "O2_Votn = @param34, " +
                        "N2_Vabs = @param35, " +
                        "N2_Votn = @param36, ";
                    if (Program.m_bExpertMode)
                    {
                        upd_com.CommandText +=
                            "CH3OH = @param37, " +
                            "CH3OH_Votn = @param38, " +
                            "CH3OH_Vabs = @param39, ";
                    }
                    upd_com.CommandText +=
                        "AnalysisEmployeeFIO = @param40, " +
                        "CycleCount = @param41, " +
                        "CondenserState = @param42, " +
                        "Temperature = @param43, " +
                        "Recommendation = @param44, " +
                        "Conclusion = @param45, " +
                        "CCNext = @param46, " +
                        "VR = @param47, " +
                        "KCF = @param48, " +
                        "CO2_CO = @param49, " +
                        "CO2_CO_Vabs = @param50, " +
                        "CO2_CO_Votn = @param51 " +
                        "WHERE CondenserTestID = @param_id";

                    upd_com.Parameters.Add("@param1", DbType.Int64).SourceColumn = "CondenserTestType";
                    upd_com.Parameters.Add("@param2", DbType.DateTime).SourceColumn = "CondenserTestDate";
                    upd_com.Parameters.Add("@param3", DbType.String).SourceColumn = "CondenserTestEquipment";
                    upd_com.Parameters.Add("@param4", DbType.Object).SourceColumn = "TestVoltageImageForm";
                    upd_com.Parameters.Add("@param5", DbType.String).SourceColumn = "TestVoltageImagePoints";
                    upd_com.Parameters.Add("@param6", DbType.Decimal).SourceColumn = "TestVoltageFrequency";
                    upd_com.Parameters.Add("@param7", DbType.Decimal).SourceColumn = "TestVoltageAmplitude";
                    upd_com.Parameters.Add("@param8", DbType.DateTime).SourceColumn = "AnalysisDateProbe";
                    upd_com.Parameters.Add("@param9", DbType.DateTime).SourceColumn = "AnalysisDateExecute";
                    upd_com.Parameters.Add("@param10", DbType.Decimal).SourceColumn = "H2";
                    upd_com.Parameters.Add("@param11", DbType.Decimal).SourceColumn = "CH4";
                    upd_com.Parameters.Add("@param12", DbType.Decimal).SourceColumn = "CO";
                    upd_com.Parameters.Add("@param13", DbType.Decimal).SourceColumn = "CO2";
                    upd_com.Parameters.Add("@param14", DbType.Decimal).SourceColumn = "C2H2";
                    upd_com.Parameters.Add("@param15", DbType.Decimal).SourceColumn = "C2H4";
                    upd_com.Parameters.Add("@param16", DbType.Decimal).SourceColumn = "C2H6";
                    upd_com.Parameters.Add("@param17", DbType.Decimal).SourceColumn = "O2";
                    upd_com.Parameters.Add("@param18", DbType.Decimal).SourceColumn = "N2";
                    upd_com.Parameters.Add("@param19", DbType.Decimal).SourceColumn = "H2_Vabs";
                    upd_com.Parameters.Add("@param20", DbType.Decimal).SourceColumn = "H2_Votn";
                    upd_com.Parameters.Add("@param21", DbType.Decimal).SourceColumn = "CH4_Vabs";
                    upd_com.Parameters.Add("@param22", DbType.Decimal).SourceColumn = "CH4_Votn";
                    upd_com.Parameters.Add("@param23", DbType.Decimal).SourceColumn = "CO_Vabs";
                    upd_com.Parameters.Add("@param24", DbType.Decimal).SourceColumn = "CO_Votn";
                    upd_com.Parameters.Add("@param25", DbType.Decimal).SourceColumn = "CO2_Vabs";
                    upd_com.Parameters.Add("@param26", DbType.Decimal).SourceColumn = "CO2_Votn";
                    upd_com.Parameters.Add("@param27", DbType.Decimal).SourceColumn = "C2H2_Vabs";
                    upd_com.Parameters.Add("@param28", DbType.Decimal).SourceColumn = "C2H2_Votn";
                    upd_com.Parameters.Add("@param29", DbType.Decimal).SourceColumn = "C2H4_Vabs";
                    upd_com.Parameters.Add("@param30", DbType.Decimal).SourceColumn = "C2H4_Votn";
                    upd_com.Parameters.Add("@param31", DbType.Decimal).SourceColumn = "C2H6_Vabs";
                    upd_com.Parameters.Add("@param32", DbType.Decimal).SourceColumn = "C2H6_Votn";
                    upd_com.Parameters.Add("@param33", DbType.Decimal).SourceColumn = "O2_Vabs";
                    upd_com.Parameters.Add("@param34", DbType.Decimal).SourceColumn = "O2_Votn";
                    upd_com.Parameters.Add("@param35", DbType.Decimal).SourceColumn = "N2_Vabs";
                    upd_com.Parameters.Add("@param36", DbType.Decimal).SourceColumn = "N2_Votn";
                    if (Program.m_bExpertMode)
                    {
                        upd_com.Parameters.Add("@param37", DbType.Decimal).SourceColumn = "CH3OH";
                        upd_com.Parameters.Add("@param38", DbType.Decimal).SourceColumn = "CH3OH_Votn";
                        upd_com.Parameters.Add("@param39", DbType.Decimal).SourceColumn = "CH3OH_Vabs";
                    }
                    upd_com.Parameters.Add("@param40", DbType.String).SourceColumn = "AnalysisEmployeeFIO";
                    upd_com.Parameters.Add("@param41", DbType.Int64).SourceColumn = "CycleCount";

                    SQLiteParameter paramState = new SQLiteParameter("@param42", DbType.Int64);
                    paramState.Value = m_CondenserState;
                    upd_com.Parameters.Add(paramState);

                    upd_com.Parameters.Add("@param43", DbType.String).SourceColumn = "Temperature";

                    SQLiteParameter paramRecommendation = new SQLiteParameter("@param44", DbType.String);
                    paramRecommendation.Value = strRecommendation;
                    upd_com.Parameters.Add(paramRecommendation);
                    SQLiteParameter paramConclusion = new SQLiteParameter("@param45", DbType.String);
                    paramConclusion.Value = strConclusion;
                    upd_com.Parameters.Add(paramConclusion);
                    SQLiteParameter paramCCNext = new SQLiteParameter("@param46", DbType.Int64);
                    if (CCNext == null) paramCCNext.Value = DBNull.Value;
                    else paramCCNext.Value = CCNext;
                    upd_com.Parameters.Add(paramCCNext);
                    SQLiteParameter paramVR = new SQLiteParameter("@param47", DbType.Double);
                    if (VR == null) paramVR.Value = DBNull.Value;
                    else paramVR.Value = VR;
                    upd_com.Parameters.Add(paramVR);
                    SQLiteParameter paramKCF = new SQLiteParameter("@param48", DbType.Double);
                    if (KCF == null) paramKCF.Value = DBNull.Value;
                    else paramKCF.Value = KCF;
                    upd_com.Parameters.Add(paramKCF);

                    upd_com.Parameters.Add("@param49", DbType.Decimal).SourceColumn = "CO2_CO";
                    upd_com.Parameters.Add("@param50", DbType.Decimal).SourceColumn = "CO2_CO_Vabs";
                    upd_com.Parameters.Add("@param51", DbType.Decimal).SourceColumn = "CO2_CO_Votn";

                    upd_com.Parameters.Add("@param_id", DbType.Int64).SourceColumn = "CondenserTestID";
                    this.qCondenserTestRecordTableAdapter.Adapter.UpdateCommand = upd_com;
                }
                else
                {
                    if (((DataRowView)qCondenserTestRecordBindingSource.Current).IsEdit)
                        ((DataRowView)qCondenserTestRecordBindingSource.Current).EndEdit();

                    SQLiteCommand ins_com = new SQLiteCommand(/*this.qCondenserTestRecordTableAdapter.Connection*/m_con);
                    ins_com.CommandText = "INSERT INTO CondenserTest (" +
                        "CondenserID, " +
                        "CondenserTestType, " +
                        "CondenserTestDate, " +
                        "CondenserTestEquipment, " +
                        "TestVoltageImageForm, " +
                        "TestVoltageImagePoints, " +
                        "TestVoltageFrequency, " +
                        "TestVoltageAmplitude, " +
                        "AnalysisDateProbe, " +
                        "AnalysisDateExecute, " +
                        "H2, " +
                        "CH4, " +
                        "CO, " +
                        "CO2, " +
                        "C2H2, " +
                        "C2H4, " +
                        "C2H6, " +
                        "O2, " +
                        "N2, " +
                        "H2_Vabs, " +
                        "H2_Votn, " +
                        "CH4_Vabs, " +
                        "CH4_Votn, " +
                        "CO_Vabs, " +
                        "CO_Votn, " +
                        "CO2_Vabs, " +
                        "CO2_Votn, " +
                        "C2H2_Vabs, " +
                        "C2H2_Votn, " +
                        "C2H4_Vabs, " +
                        "C2H4_Votn, " +
                        "C2H6_Vabs, " +
                        "C2H6_Votn, " +
                        "O2_Vabs, " +
                        "O2_Votn, " +
                        "N2_Vabs, " +
                        "N2_Votn, ";
                    if (Program.m_bExpertMode)
                    {
                        ins_com.CommandText +=
                            "CH3OH, " +
                            "CH3OH_Votn, " +
                            "CH3OH_Vabs, ";
                    }
                    ins_com.CommandText +=
                        "AnalysisEmployeeFIO, " +
                        "CycleCount, " +
                        "CondenserState, " +
                        "Temperature, " +
                        "Recommendation, " +
                        "Conclusion, " +
                        "CCNext, " +
                        "VR, " +
                        "KCF, " +
                        "CO2_CO, " + 
                        "CO2_CO_Vabs, " +
                        "CO2_CO_Votn " +
                        ") VALUES (" +
                        "@param_id, " +
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
                        "@param11, " +
                        "@param12, " +
                        "@param13, " +
                        "@param14, " +
                        "@param15, " +
                        "@param16, " +
                        "@param17, " +
                        "@param18, " +
                        "@param19, " +
                        "@param20, " +
                        "@param21, " +
                        "@param22, " +
                        "@param23, " +
                        "@param24, " +
                        "@param25, " +
                        "@param26, " +
                        "@param27, " +
                        "@param28, " +
                        "@param29, " +
                        "@param30, " +
                        "@param31, " +
                        "@param32, " +
                        "@param33, " +
                        "@param34, " +
                        "@param35, " +
                        "@param36, ";
                    if (Program.m_bExpertMode)
                    {
                        ins_com.CommandText +=
                            "@param37, " +
                            "@param38, " +
                            "@param39, ";
                    }
                    ins_com.CommandText +=
                        "@param40, " +
                        "@param41, " +
                        "@param42, " +
                        "@param43, " +
                        "@param44, " +
                        "@param45, " +
                        "@param46, " +
                        "@param47, " +
                        "@param48, " +
                        "@param49, " +
                        "@param50, " +
                        "@param51);";

                    SQLiteParameter paramCondenserID = new SQLiteParameter("@param_id", DbType.Int64);
                    paramCondenserID.Value = m_CondenserID;
                    ins_com.Parameters.Add(paramCondenserID);

                    ins_com.Parameters.Add("@param1", DbType.Int64).SourceColumn = "CondenserTestType";
                    ins_com.Parameters.Add("@param2", DbType.DateTime).SourceColumn = "CondenserTestDate";
                    ins_com.Parameters.Add("@param3", DbType.String).SourceColumn = "CondenserTestEquipment";
                    ins_com.Parameters.Add("@param4", DbType.Object).SourceColumn = "TestVoltageImageForm";
                    ins_com.Parameters.Add("@param5", DbType.String).SourceColumn = "TestVoltageImagePoints";
                    ins_com.Parameters.Add("@param6", DbType.Decimal).SourceColumn = "TestVoltageFrequency";
                    ins_com.Parameters.Add("@param7", DbType.Decimal).SourceColumn = "TestVoltageAmplitude";
                    ins_com.Parameters.Add("@param8", DbType.DateTime).SourceColumn = "AnalysisDateProbe";
                    ins_com.Parameters.Add("@param9", DbType.DateTime).SourceColumn = "AnalysisDateExecute";
                    ins_com.Parameters.Add("@param10", DbType.Decimal).SourceColumn = "H2";
                    ins_com.Parameters.Add("@param11", DbType.Decimal).SourceColumn = "CH4";
                    ins_com.Parameters.Add("@param12", DbType.Decimal).SourceColumn = "CO";
                    ins_com.Parameters.Add("@param13", DbType.Decimal).SourceColumn = "CO2";
                    ins_com.Parameters.Add("@param14", DbType.Decimal).SourceColumn = "C2H2";
                    ins_com.Parameters.Add("@param15", DbType.Decimal).SourceColumn = "C2H4";
                    ins_com.Parameters.Add("@param16", DbType.Decimal).SourceColumn = "C2H6";
                    ins_com.Parameters.Add("@param17", DbType.Decimal).SourceColumn = "O2";
                    ins_com.Parameters.Add("@param18", DbType.Decimal).SourceColumn = "N2";
                    ins_com.Parameters.Add("@param19", DbType.Decimal).SourceColumn = "H2_Vabs";
                    ins_com.Parameters.Add("@param20", DbType.Decimal).SourceColumn = "H2_Votn";
                    ins_com.Parameters.Add("@param21", DbType.Decimal).SourceColumn = "CH4_Vabs";
                    ins_com.Parameters.Add("@param22", DbType.Decimal).SourceColumn = "CH4_Votn";
                    ins_com.Parameters.Add("@param23", DbType.Decimal).SourceColumn = "CO_Vabs";
                    ins_com.Parameters.Add("@param24", DbType.Decimal).SourceColumn = "CO_Votn";
                    ins_com.Parameters.Add("@param25", DbType.Decimal).SourceColumn = "CO2_Vabs";
                    ins_com.Parameters.Add("@param26", DbType.Decimal).SourceColumn = "CO2_Votn";
                    ins_com.Parameters.Add("@param27", DbType.Decimal).SourceColumn = "C2H2_Vabs";
                    ins_com.Parameters.Add("@param28", DbType.Decimal).SourceColumn = "C2H2_Votn";
                    ins_com.Parameters.Add("@param29", DbType.Decimal).SourceColumn = "C2H4_Vabs";
                    ins_com.Parameters.Add("@param30", DbType.Decimal).SourceColumn = "C2H4_Votn";
                    ins_com.Parameters.Add("@param31", DbType.Decimal).SourceColumn = "C2H6_Vabs";
                    ins_com.Parameters.Add("@param32", DbType.Decimal).SourceColumn = "C2H6_Votn";
                    ins_com.Parameters.Add("@param33", DbType.Decimal).SourceColumn = "O2_Vabs";
                    ins_com.Parameters.Add("@param34", DbType.Decimal).SourceColumn = "O2_Votn";
                    ins_com.Parameters.Add("@param35", DbType.Decimal).SourceColumn = "N2_Vabs";
                    ins_com.Parameters.Add("@param36", DbType.Decimal).SourceColumn = "N2_Votn";
                    if (Program.m_bExpertMode)
                    {
                        ins_com.Parameters.Add("@param37", DbType.Decimal).SourceColumn = "CH3OH";
                        ins_com.Parameters.Add("@param38", DbType.Decimal).SourceColumn = "CH3OH_Vabs";
                        ins_com.Parameters.Add("@param39", DbType.Decimal).SourceColumn = "CH3OH_Votn";
                    }
                    ins_com.Parameters.Add("@param40", DbType.String).SourceColumn = "AnalysisEmployeeFIO";
                    ins_com.Parameters.Add("@param41", DbType.Int64).SourceColumn = "CycleCount";

                    SQLiteParameter paramState = new SQLiteParameter("@param42", DbType.Int64);
                    paramState.Value = m_CondenserState;
                    ins_com.Parameters.Add(paramState);

                    ins_com.Parameters.Add("@param43", DbType.String).SourceColumn = "Temperature";

                    SQLiteParameter paramRecommendation = new SQLiteParameter("@param44", DbType.String);
                    paramRecommendation.Value = strRecommendation;
                    ins_com.Parameters.Add(paramRecommendation);
                    SQLiteParameter paramConclusion = new SQLiteParameter("@param45", DbType.String);
                    paramConclusion.Value = strConclusion;
                    ins_com.Parameters.Add(paramConclusion);
                    SQLiteParameter paramCCNext = new SQLiteParameter("@param46", DbType.Int64);
                    if (CCNext == null) paramCCNext.Value = DBNull.Value;
                    else paramCCNext.Value = CCNext;
                    ins_com.Parameters.Add(paramCCNext);
                    SQLiteParameter paramVR = new SQLiteParameter("@param47", DbType.Double);
                    if (VR == null) paramVR.Value = DBNull.Value;
                    else paramVR.Value = VR;
                    ins_com.Parameters.Add(paramVR);
                    SQLiteParameter paramKCF = new SQLiteParameter("@param48", DbType.Double);
                    if (KCF == null) paramKCF.Value = DBNull.Value;
                    else paramKCF.Value = KCF;
                    ins_com.Parameters.Add(paramKCF);

                    ins_com.Parameters.Add("@param49", DbType.Decimal).SourceColumn = "CO2_CO";
                    ins_com.Parameters.Add("@param50", DbType.Decimal).SourceColumn = "CO2_CO_Vabs";
                    ins_com.Parameters.Add("@param51", DbType.Decimal).SourceColumn = "CO2_CO_Votn";

                    this.qCondenserTestRecordTableAdapter.Adapter.InsertCommand = ins_com;
                }

                this.qCondenserTestRecordTableAdapter.Adapter.Update(dataSetQuery.QCondenserTestRecord);

                //SQLiteConnection connection_ = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                //connection_.Open();

                if (m_CondenserTestID <= 0)
                {
                    SQLiteCommand com_ = new SQLiteCommand(m_con);
                    com_.CommandText = "select seq from sqlite_sequence where name = 'CondenserTest'";
                    com_.CommandType = CommandType.Text;
                    SQLiteDataReader dr_ = com_.ExecuteReader();

                    while (dr_.Read())
                    {
                        m_CondenserTestID = Convert.ToInt64(dr_["seq"]);
                    }
                    dr_.Close();
                }

                // заносим состояние в конденсатор
                if (m_bCondenserTestIsLast /*|| m_CondenserTestType == CondenserTest.CondenserTestType.Acceptance*/)
                {
                    SQLiteCommand com_ = new SQLiteCommand(m_con);
                    com_.CommandType = CommandType.Text;
                    com_.CommandText = "UPDATE Condensers SET CondenserState = @state, CondenserTestType = @test_type WHERE CondenserID = @condenser_id";
                    com_.Parameters.Clear();

                    SQLiteParameter param1 = new SQLiteParameter("@state", DbType.Int64);
                    param1.Value = m_CondenserState;
                    com_.Parameters.Add(param1);

                    SQLiteParameter param2 = new SQLiteParameter("@condenser_id", DbType.Int64);
                    param2.Value = m_CondenserID;
                    com_.Parameters.Add(param2);

                    SQLiteParameter param3 = new SQLiteParameter("@test_type", DbType.Int64);
                    param3.Value = (long)m_CondenserTestType;
                    com_.Parameters.Add(param3);

                    com_.ExecuteNonQuery();
                }

                // добавляем график для эксплуатационных испытаний
                if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational)
                {
                    SQLiteCommand com_ = new SQLiteCommand(m_con);
                    com_.CommandType = CommandType.Text;

                    for (int i = 0; i < listDeletedSheduler.Count; i++)
                    {
                        com_.CommandText = "DELETE FROM CondenserTestSheduler WHERE ShedulerID = @sheduler_id AND CondenserTestID = @id";
                        com_.Parameters.Clear();

                        SQLiteParameter param1 = new SQLiteParameter("@sheduler_id", DbType.Int64);
                        param1.Value = listDeletedSheduler[i];
                        com_.Parameters.Add(param1);

                        SQLiteParameter param2 = new SQLiteParameter("@id", DbType.Int64);
                        param2.Value = m_CondenserTestID;
                        com_.Parameters.Add(param2);

                        com_.ExecuteNonQuery();
                    }

                    for (int i = 0; i < tableSheduler.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(tableSheduler.Rows[i]["WAS_CHANGE"]))
                        {
                            com_.Parameters.Clear();

                            SQLiteParameter param2 = new SQLiteParameter("@id", DbType.Int64);
                            param2.Value = m_CondenserTestID;
                            com_.Parameters.Add(param2);

                            SQLiteParameter param3 = new SQLiteParameter("@date", DbType.DateTime);
                            param3.Value = tableSheduler.Rows[i]["DATE"];//.ItemArray[iPosShedulerDate];
                            com_.Parameters.Add(param3);

                            SQLiteParameter param4 = new SQLiteParameter("@frequency", DbType.Decimal);
                            param4.Value = teVoltageFrequency.EditValue;// tableSheduler.Rows[i]["FREQUENCY"];//.ItemArray[iPosShedulerFrequency];
                            com_.Parameters.Add(param4);

                            SQLiteParameter param5 = new SQLiteParameter("@cycle_count", DbType.Int64);
                            param5.Value = tableSheduler.Rows[i]["CYCLE_COUNT"];//.ItemArray[iPosShedulerCycleCount];
                            com_.Parameters.Add(param5);

                            if (Convert.ToInt64(tableSheduler.Rows[i]["ID"]) > 0)
                            {
                                com_.CommandText = "UPDATE CondenserTestSheduler SET ShedulerDate = @date, ShedulerCycleFrequency = @frequency, ShedulerCycleCount = @cycle_count " +
                                    "WHERE ShedulerID = @sheduler_id AND CondenserTestID = @id";

                                SQLiteParameter param1 = new SQLiteParameter("@sheduler_id", DbType.Int64);
                                param1.Value = tableSheduler.Rows[i]["ID"];
                                com_.Parameters.Add(param1);
                            }
                            else
                            {
                                com_.CommandText = "INSERT INTO CondenserTestSheduler (CondenserTestID, ShedulerDate, ShedulerCycleFrequency, ShedulerCycleCount) " +
                                    "VALUES (@id, @date, @frequency, @cycle_count)";
                            }

                            com_.ExecuteNonQuery();
                        }
                    }
                }

                //connection_.Close();
                if (tran != null) tran.Commit();
                m_con.Close();

                m_bChangeData = false;

                return true;
            }
            catch (SQLiteException ex)
            {
                if (tran != null) tran.Rollback();
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (DBConcurrencyException ex)
            {
                if (tran != null) tran.Rollback();
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            bool bChecked = cbCanChangeVoltagePoints.Checked;
            cbCanChangeVoltagePoints.Checked = false;
            if (!SaveData())
            {
                cbCanChangeVoltagePoints.Checked = bChecked;
                return;
            }

            if (m_strRecommendation != "" || m_strConclusion != "")
            {
                RecommendationForm form = new RecommendationForm(m_strRecommendation, m_strConclusion);
                DialogResult res = form.ShowDialog();
                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    WaitingForm wf = new WaitingForm();
                    wf.m_CondenserID = m_CondenserID;
                    wf.m_CondenserTestID = m_CondenserTestID;
                    wf.ShowDialog(this);

                    if (wf.m_Word != null)
                    {
                        wf.m_Word.SetVisible(true);
                        wf.m_Word.DestroyWord();
                    }
                }
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        static public double Round(double value, int digits)
        {
            double scale = Math.Pow(10.0, digits);
            double round = Math.Floor(Math.Abs(value) * scale + 0.5);
            return (Math.Sign(value) * round / scale);
        }

        private void CalcV(DevExpress.XtraVerticalGrid.Rows.MultiEditorRow row)
        {
            if (teCycleCount.EditValue == null 
                || teCycleCount.EditValue == DBNull.Value
                || Convert.ToInt64(teCycleCount.EditValue) == 0)
            {
                if (Program.m_bExpertMode)
                {
                    row.PropertiesCollection[1].Value = null;
                    row.PropertiesCollection[2].Value = null;
                }
                else
                {
                    DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                    drv[row.Properties.FieldName + "_Vabs"] = DBNull.Value;
                    drv[row.Properties.FieldName + "_Votn"] = DBNull.Value;
                }
                return;
            }
            
            /*DateTime dt = dtAnalysisDateProbe.DateTime;
            dt = dt.Date;

            if ((DateTime)m_dtLastAnalysisDateProbe >= dt)
            {
                if (Program.m_bExpertMode)
                {
                    row.PropertiesCollection[1].Value = null;
                    row.PropertiesCollection[2].Value = null;
                }
                else
                {
                    DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                    drv[row.Properties.FieldName + "_Vabs"] = DBNull.Value;
                    drv[row.Properties.FieldName + "_Votn"] = DBNull.Value;
                }
                return;
            }*/

            //TimeSpan span = dt - (DateTime)m_dtLastAnalysisDateProbe;
            //double months = span.TotalDays / 30;

            double? v_abs = null;
            double? v_otn = null;

            double? val = null;
            double? val_last = null;

            if (row.Properties.Value != null && row.Properties.Value != DBNull.Value)
                val = Convert.ToDouble(row.Properties.Value);

            long iCycleCount = Convert.ToInt64(teCycleCount.EditValue);

            if (dictLastAnalysisData.ContainsKey(row.Properties.FieldName))
            {
                val_last = dictLastAnalysisData[row.Properties.FieldName];
            }

            if (val != null && val_last != null)
            {
                v_abs = (double)((double)val - val_last) * 1000 / ((double)iCycleCount);
                if (Math.Abs((double)val_last) > 0.009)
                    v_otn = Round((double)v_abs * 100 / ((double)val_last), 2);
                v_abs = Round((double)v_abs, 2);
            }

            if (v_otn != null && double.IsInfinity((double)v_otn))
            {
                v_otn = null;
            }

            if (Program.m_bExpertMode)
            {
                row.PropertiesCollection[1].Value = v_abs;
                row.PropertiesCollection[2].Value = v_otn;
            }
            else
            {
                DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                if (v_abs == null)
                    drv[row.Properties.FieldName + "_Vabs"] = DBNull.Value;
                else
                    drv[row.Properties.FieldName + "_Vabs"] = v_abs;
                if (v_otn == null)
                    drv[row.Properties.FieldName + "_Votn"] = DBNull.Value;
                else
                    drv[row.Properties.FieldName + "_Votn"] = v_otn;
            }
        }

        private void CalcAllV()
        {
            GridVertical.BeginUpdate();
            for (int i = 1; i < GridVertical.Rows.Count; i++)
            {
                CalcV((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.Rows[i]);
            }
            GridVertical.EndUpdate();
        }

        private void GridVertical_CellValueChanged(object sender, DevExpress.XtraVerticalGrid.Events.CellValueChangedEventArgs e)
        {
            if (m_bDataLoadEnd)
            {
                m_bChangeData = true;
                GridVertical.BeginUpdate();

                if (e.Row.Properties.FieldName == "CO" || e.Row.Properties.FieldName == "CO2")
                {
                    DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                    if (drv.Row["CO2"] != null && drv.Row["CO2"] != DBNull.Value &&
                        drv.Row["CO"] != null && drv.Row["CO"] != DBNull.Value &&
                        Convert.ToDecimal(drv.Row["CO"]) != 0)
                        drv.Row["CO2_CO"] = Round((double)(Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"])), 4);
                    else
                        drv.Row["CO2_CO"] = DBNull.Value;

                    CalcV((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)GridVertical.GetRowByFieldName("CO2_CO"));
                }

                CalcV((DevExpress.XtraVerticalGrid.Rows.MultiEditorRow)e.Row);
                GridVertical.EndUpdate();
            }
        }

        private void teVoltageFrequency_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
            if (teVoltageFrequency.Text == "") teVoltageFrequency.EditValue = DBNull.Value;
        }

        private void teVoltageAmplitude_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
            if (teVoltageAmplitude.Text == "") teVoltageAmplitude.EditValue = DBNull.Value;
        }

        private void dtTestDate_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd)
            {
                m_bChangeData = true;

                if (m_CondenserTestType == CondenserTest.CondenserTestType.Operational && tableSheduler.Rows.Count == 0)
                {
                    DataRow row = tableSheduler.NewRow();
                    row["ID"] = 0;
                    row["DATE"] = dtTestDate.EditValue;
                    row["FREQUENCY"] = DBNull.Value;
                    row["CYCLE_COUNT"] = DBNull.Value;
                    row["WAS_CHANGE"] = true;
                    tableSheduler.Rows.Add(row);
                }
            }
        }

        private void cbTestType_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void teTestEquipment_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void dtAnalysisDateProbe_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd)
            {
                m_bChangeData = true;

                /*DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                if (drv == null) return;

                if (dtAnalysisDateProbe.EditValue == null)
                    drv["AnalysisDateProbe"] = DBNull.Value;
                else
                    drv["AnalysisDateProbe"] = dtAnalysisDateProbe.EditValue;*/

                //CalcAllV();
                //GetLastAnalysisData();
            }
        }

        private void dtAnalysisDateExecute_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void TestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            dtTestDate.Focus();
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

        private void bPrev_Click(object sender, EventArgs e)
        {
            if (m_bShowContinueMsg)
            {
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

                m_bContinuePrev = true;

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            if (m_bShowContinueMsg)
            {
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

                m_bContinueNext = true;

                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void GridVertical_ShowingEditor(object sender, CancelEventArgs e)
        {
            if (GridVertical.FocusedRecordCellIndex > 0)
            {
                GridVertical.FocusedRecordCellIndex = 0;
                e.Cancel = true;
                return;
            }

            string row_name = GridVertical.FocusedRow.Properties.FieldName;
            if (row_name == "#" || GridVertical.FocusedRow.Properties.FieldName == "CO2_CO")
            {
                GridVertical.FocusedRecordCellIndex = 0;
                e.Cancel = true;
                return;
            }
        }

        private void GridVertical_CustomDrawRowValueCell(object sender, DevExpress.XtraVerticalGrid.Events.CustomDrawRowValueCellEventArgs e)
        {
            string str = e.CellText;
            Rectangle rect = e.Bounds;
            rect.X += 3;
            rect.Width -= 6;

            if (e.Row.Appearance.Options.UseBackColor && !e.Row.HasChildren || e.CellIndex > 0 || e.Row.Properties.FieldName == "CO2_CO")
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(240, 240, 240)), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.White), e.Bounds);
            }

            if (e.Row.Properties.FieldName == "#")
            {
                if (e.CellIndex == 0) str = "Концентрация, ppm";
                if (e.CellIndex == 1) str = "Vабс, ppm на 1000 циклов";//(цикл*10ˉ³)";
                if (e.CellIndex == 2) str = "Vотн, % на 1000 циклов";//(цикл*10ˉ³)";
                e.Graphics.DrawString(str, e.Appearance.Font, m_BrushBlack, rect, e.Appearance.GetStringFormat());
            }
            else
            {
                if (str == "")
                {
                    if (e.CellIndex == 0 && e.Row.Properties.FieldName != "CO2_CO")
                        e.Graphics.DrawString("данные отсутствуют", e.Appearance.Font, m_BrushGray, rect, e.Appearance.GetStringFormat());
                    else
                        e.Graphics.DrawString("данные для расчета отсутствуют", e.Appearance.Font, m_BrushGray, rect, e.Appearance.GetStringFormat());
                }
                else
                {
                    if (e.CellIndex > 0)
                    {
                        e.Graphics.DrawString(str, e.Appearance.Font, m_BrushBlack, rect, e.Appearance.GetStringFormat());
                    }
                    else
                    {
                        e.Graphics.DrawString(str, e.Appearance.Font, m_BrushBlack, rect, e.Appearance.GetStringFormat());
                    }
                }
            }

            e.Handled = true;
        }

        private void teAnalysisEmployeeFIO_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void GridVertical_CustomDrawRowHeaderCell(object sender, DevExpress.XtraVerticalGrid.Events.CustomDrawRowHeaderCellEventArgs e)
        {
            if (e.Row.Properties.FieldName == "#")
            {
                Rectangle rect = e.Bounds;
                rect.X += 3;
                rect.Width -= 6;

                string str = "";
                if (e.CellIndex == 0) str = "Газ";
                if (e.CellIndex == 1) str = "";
                if (e.CellIndex == 2) str = "";
                Font font = new System.Drawing.Font(e.Appearance.Font, FontStyle.Bold);
                StringFormat sf = new StringFormat(e.Appearance.GetStringFormat());
                sf.Alignment = StringAlignment.Center;
                e.Graphics.DrawString(str, font, m_BrushBlack, rect, sf);
                e.Handled = true;
            }
        }

        private void repDecimal4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                if (e.KeyChar == '.') e.KeyChar = ',';

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void cbCanEdit_CheckedChanged(object sender, EventArgs e)
        {
            /*if (m_bDataLoadEnd)*/ m_bChangeData = true;

            if (cbCanEdit.Checked) GridShedulerView.OptionsBehavior.Editable = true;
            else GridShedulerView.OptionsBehavior.Editable = false;
        }

        private void GridShedulerView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            if (MyLocalizer.XtraMessageBoxShow("Сохранить запись в графике эксплуатации?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
            {
                e.ErrorText = "";
                e.Valid = false;
                return;
            }

            DataRowView row = (DataRowView)e.Row;
            if (row["DATE"] == DBNull.Value)
            {
                e.ErrorText = "Необходимо указать дату и время начала эксплуатации.";
                e.Valid = false;
                return;
            }
            /*if (row["FREQUENCY"] == DBNull.Value || Convert.ToInt64(row["FREQUENCY"]) <= 0)
            {
                e.ErrorText = "Необходимо указать частоту циклов.";
                e.Valid = false;
                return;
            }*/
            if (row["CYCLE_COUNT"] == DBNull.Value || Convert.ToInt64(row["CYCLE_COUNT"]) <= 0)
            {
                e.ErrorText = "Необходимо указать количество циклов.";
                e.Valid = false;
                return;
            }

            DataRow rowPrev = null;
            DataRow rowNext = null;
            int rowIndex = tableSheduler.Rows.IndexOf(row.Row);
            if (rowIndex >= 0)
            {
                if (rowIndex > 0)
                {
                    rowPrev = tableSheduler.Rows[e.RowHandle - 1];
                }
                if (rowIndex < tableSheduler.Rows.Count - 1)
                {
                    rowNext = tableSheduler.Rows[e.RowHandle + 1];
                }
            }
            else
            {
                if (tableSheduler.Rows.Count > 0)
                    rowPrev = tableSheduler.Rows[tableSheduler.Rows.Count - 1];
            }

            if (rowPrev != null)
            {
                DateTime dtPrev = Convert.ToDateTime(rowPrev["DATE"]);
                if (dtPrev >= Convert.ToDateTime(row["DATE"]))
                {
                    e.ErrorText = "Дата и время начала эксплуатации должна быть больше предыдущего значения.";
                    e.Valid = false;
                    return;
                }
            }

            if (rowNext != null)
            {
                DateTime dtNext = Convert.ToDateTime(rowNext["DATE"]);
                if (dtNext <= Convert.ToDateTime(row["DATE"]))
                {
                    e.ErrorText = "Дата и время начала эксплуатации должна быть меньше следующего значения.";
                    e.Valid = false;
                    return;
                }
            }

            row["WAS_CHANGE"] = true;
        }

        private void GridShedulerView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            if (e.ErrorText != "")
                MyLocalizer.XtraMessageBoxShow(e.ErrorText, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                GridShedulerView.ColumnPanelRowHeight = maxHeight;
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
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = GridShedulerView.GetViewInfo() as GridViewInfo;

                GridColumnInfoArgs ex = null;
                viewInfo.GInfo.AddGraphics(null);
                ex = new GridColumnInfoArgs(viewInfo.GInfo.Cache, null);
                try
                {
                    ex.InnerElements.Add(new DrawElementInfo(new GlyphElementPainter(),
                                                            new GlyphElementInfoArgs(viewInfo.View.Images, 0, null),
                                                            StringAlignment.Near));
                    ex.SetAppearance(GridShedulerView.Appearance.HeaderPanel);
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

        private void GridShedulerView_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            try
            {
                for (int i = e.Column.VisibleIndex; i < GridShedulerView.Columns.Count; i++)
                {
                    SetColumnHeight(GridShedulerView.Columns[i]);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            try
            {
                timer2.Enabled = false;
                foreach (GridColumn col in GridShedulerView.Columns)
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

        private void dtAnalysisDateProbe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtAnalysisDateProbe.EditValue = DBNull.Value;
            }
        }

        private void dtAnalysisDateExecute_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
            {
                dtAnalysisDateExecute.EditValue = DBNull.Value;
            }
        }

        private void GridVertical_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() == "") e.Value = DBNull.Value;
        }

        private void teCycleCount_EditValueChanged(object sender, EventArgs e)
        {
            teCycleCount2.EditValue = teCycleCount.EditValue;

            if (m_bDataLoadEnd)
            {
                m_bChangeData = true;

                if (teCycleCount.Text == "") teCycleCount.EditValue = DBNull.Value;

                DataRowView drv = (DataRowView)this.qCondenserTestRecordBindingSource.Current;
                if (drv == null) return;

                if (teCycleCount.EditValue == null)
                    drv["CycleCount"] = DBNull.Value;
                else
                    drv["CycleCount"] = teCycleCount.EditValue;

                CalcAllV();
            }
        }

        private void teCntNodes_EditValueChanged(object sender, EventArgs e)
        {
            int iCntNodes = Convert.ToInt32(teCntNodes.EditValue);
            if (iCntNodes > m_iMaxNodes)
            {
                teCntNodes.EditValue = m_iMaxNodes;
                iCntNodes = m_iMaxNodes;
            }
            if (iCntNodes < 0)
            {
                teCntNodes.EditValue = 0;
                iCntNodes = 0;
            }

            if (m_listVoltagePoints.Count > iCntNodes)
            {
                m_listVoltagePoints.RemoveRange(iCntNodes, m_listVoltagePoints.Count - iCntNodes);
            }
            if (m_listVoltagePoints.Count < iCntNodes)
            {
                for (int i = m_listVoltagePoints.Count; i < iCntNodes; i++) m_listVoltagePoints.Add(0);
            }

            pb_Paint();
        }

        private void GridShedulerView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!GridShedulerView.IsEditorFocused)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }
        }

        private void GridVertical_KeyDown(object sender, KeyEventArgs e)
        {
            /*if (GridVertical.Is.IsEditorRow)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }*/
        }

        private void cbDoNotWork_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;

            if (cbDoNotWork.Checked)
            {
                cbDoNotWork.Text = "Конденсатор неработоспособен.";
                cbDoNotWork.Properties.Appearance.ForeColor = Color.Red;
            }
            else
            {
                cbDoNotWork.Text = "Вывести из эксплуатации.";
                cbDoNotWork.Properties.Appearance.ForeColor = Color.Red;//.FromArgb(32, 31, 53);
            }
        }

        private void teTemperature_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bDataLoadEnd) m_bChangeData = true;
        }

        private void teCycleCount_Validated(object sender, EventArgs e)
        {            
        }

        private void ImportData(string strFileName)
        {
            StreamReader sr = StreamReader.Null;
            bool bNeedFindGases = false;
            bool bFindGases = false;

            DataRowView drv = (DataRowView)(this.qCondenserTestRecordBindingSource.Current);
            
            try
            {
                sr = new StreamReader(strFileName, System.Text.Encoding.Default);

                if (sr != StreamReader.Null)
                {
                    while (!sr.EndOfStream)
                    {
                        string str = sr.ReadLine();

                        if (bNeedFindGases)
                        {
                            string[] res = str.Split(new string[] { "  " }, StringSplitOptions.RemoveEmptyEntries);
                            if (res.Length == 6)
                            {
                                string strGasName = res[1].Trim().ToLower();
                                string strValue = res[5].Trim();

                                bool bFind = false;
                                if (strGasName == "угарный газ")
                                {
                                    drv.Row["CO"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "углекислый газ")
                                {
                                    drv.Row["CO2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "водород")
                                {
                                    drv.Row["H2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "кислород")
                                {
                                    drv.Row["O2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "азот")
                                {
                                    drv.Row["N2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "метан")
                                {
                                    drv.Row["CH4"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "ацетилен")
                                {
                                    drv.Row["C2H2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "этилен")
                                {
                                    drv.Row["C2H4"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "этан")
                                {
                                    drv.Row["C2H6"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }
                                if (strGasName == "метанол")
                                {
                                    drv.Row["CH3OH"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                    bFind = true;
                                }

                                if (!bFind)
                                {
                                    strGasName = strGasName.Replace("о", "o").Replace("н", "h").Replace("с", "c");

                                    if (strGasName == "co")
                                    {
                                        drv.Row["CO"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "co2")
                                    {
                                        drv.Row["CO2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "h2")
                                    {
                                        drv.Row["H2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "o2")
                                    {
                                        drv.Row["O2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "n2")
                                    {
                                        drv.Row["N2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "ch4")
                                    {
                                        drv.Row["CH4"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "c2h2")
                                    {
                                        drv.Row["C2H2"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "c2h4")
                                    {
                                        drv.Row["C2H4"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "c2h6")
                                    {
                                        drv.Row["C2H6"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                    if (strGasName == "ch3oh")
                                    {
                                        drv.Row["CH3OH"] = Convert.ToDecimal(Convert.ToDouble(strValue));
                                        bFind = true;
                                    }
                                }

                                if (bFind)
                                {
                                    bFindGases = true;
                                }
                            }
                        }

                        if (str == "Идентификация") // начинаются значения газов
                        {
                            bNeedFindGases = true;

                            drv.Row["CO"] = 0;
                            drv.Row["CO2"] = 0;
                            drv.Row["H2"] = 0;
                            drv.Row["O2"] = 0;
                            drv.Row["N2"] = 0;
                            drv.Row["CH4"] = 0;
                            drv.Row["C2H2"] = 0;
                            drv.Row["C2H4"] = 0;
                            drv.Row["C2H6"] = 0;
                            drv.Row["CH3OH"] = 0;
                            drv.Row["CO2_CO"] = DBNull.Value;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (sr != StreamReader.Null) sr.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при загрузке результатов ХАРГ: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (sr != StreamReader.Null) sr.Close();

            if (!bFindGases)
            {
                MyLocalizer.XtraMessageBoxShow("Процесс выполнения ХАРГ не завершён или файл с результатами ХАРГ повреждён.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (Convert.ToDecimal(drv.Row["CO"]) != 0)
                {
                    drv.Row["CO2_CO"] = Round((double)(Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"])), 4); //Convert.ToDecimal(drv.Row["CO2"]) / Convert.ToDecimal(drv.Row["CO"]);
                }

                CalcAllV();
                GridVertical.Invalidate();
                m_bChangeData = true;
                MyLocalizer.XtraMessageBoxShow("Результаты ХАРГ успешно импортированы.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            // ищем файл в папке Exports, последний по дате 
            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Exports"))
            {
                /*List<string> allFoundFiles = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Exports", "*.txt", SearchOption.TopDirectoryOnly)
                    .Where(s => s.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                    .ToList();*/

                string[] allFound = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Exports", "*.txt", SearchOption.TopDirectoryOnly);
                List<string> allFoundFiles = new List<string>();

                for (int i = 0; i < allFound.Length; i++)
                {
                    string strname = allFound[i] + "###";
                    if (strname.IndexOf(".txt###") >= 0)
                        allFoundFiles.Add(allFound[i]);
                }

                // ищем файл с самым поздним временем создания
                DateTime maxDateLastWrite = DateTime.MinValue;
                string strFileName = "";
                for (int i = 0; i < allFoundFiles.Count; i++)
                {
                    try
                    {
                        FileInfo fi = new FileInfo(allFoundFiles[i]);
                        if (fi != null)
                        {
                            DateTime dt = fi.CreationTimeUtc;
                            if (maxDateLastWrite < dt)
                            {
                                maxDateLastWrite = dt;
                                strFileName = allFoundFiles[i];
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // ошибка при получении информации о файле файла
                    }
                }
                if (strFileName != "")
                {
                    ImportData(strFileName);
                }
                else
                {
                    MyLocalizer.XtraMessageBoxShow("Не удалось найти файл с результатами ХАРГ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MyLocalizer.XtraMessageBoxShow("Не удалось найти папку Exports.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (openDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string strFileName = openDlg.FileName;
                ImportData(strFileName);
            }
        }
    }
}