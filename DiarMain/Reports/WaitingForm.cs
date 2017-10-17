using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data;
using System.Drawing;
using System.IO;

namespace Condenser
{
    public partial class WaitingForm : DevExpress.XtraEditors.XtraForm
    {
        class CondenserInfo
        {
            public class CondenserTestInfo
            {
                public class ShedulerInfo
                {
                    public DateTime dtShedulerDate;
                    public long iShedulerCycleCount;
                }

                public long iCondenserTestID;
                public CondenserTest.CondenserTestType CondenserTestType;
                public DateTime dtCondenserTestDate;
                public string strCondenserTestEquipment;
                public long? iCycleCount = null;
                public string strTemperature;
                public Image TestVoltageImageForm = null;
                public Decimal fTestVoltageFrequency;
                public Decimal fTestVoltageAmplitude;
                public DateTime? dtAnalysisDateProbe = null;
                public DateTime? dtAnalysisDateExecute = null;
                public string strAnalysisEmployeeFIO;
                public CondenserTest.CondenserTestState CondenserState;
                public Decimal? fH2;
                public Decimal? fO2;
                public Decimal? fN2;
                public Decimal? fCH4;
                public Decimal? fCO;
                public Decimal? fCO2;
                public Decimal? fC2H2;
                public Decimal? fC2H4;
                public Decimal? fC2H6;
                public string strRecommendation;
                public string strConclusion;

                public List<ShedulerInfo> listSheduler = new List<ShedulerInfo>();
            }

            public string strCondenserNumber;
            public string strCondenserTypeName;
            public string strInsulatingLiquidTypeName;
            public Decimal fNominalVoltage;
            public Decimal fNominalCapacitance;
            public Decimal fEnergyCharge;
            public Decimal fInductivity;
            public Decimal fMaxDischargeCurrent;
            public Decimal fCondenserWeight;
            public Decimal fCondenserVolume;
            public long iNormalizedResource;

            public int iCondenserTestIndex = -1;

            public List<CondenserTestInfo> listCondenserTest = new List<CondenserTestInfo>();
        }

        public long m_CondenserID = -1;
        public long m_CondenserTestID = -1;
        private bool m_bEnd = false;

        public Word m_Word = null;
        SQLiteConnection m_con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
        //Dictionary<string, string> m_dictMainFields = new Dictionary<string, string>();
        CondenserInfo ci = new CondenserInfo();

        public WaitingForm()
        {
            InitializeComponent();
        }

        private void GetCondenserData()
        {
            // получаем данные по конденсатору
            try
            {
                m_con.Open();
                SQLiteCommand com = new SQLiteCommand(m_con);
                com.CommandText = "SELECT c.CondenserNumber, ct.CondenserTypeName, ilt.InsulatingLiquidTypeName, c.NominalVoltage, c.NominalCapacitance, c.EnergyCharge, c.Inductivity, " +
                    "c.MaxDischargeCurrent, c.CondenserWeight, c.CondenserVolume, c.NormalizedResource " + 
                    "FROM Condensers AS c " + 
                    "INNER JOIN CondenserTypes AS ct ON c.CondenserTypeID = ct.CondenserTypeID " +
                    "INNER JOIN InsulatingLiquidTypes AS ilt ON ilt.InsulatingLiquidTypeID = ct.InsulatingLiquidTypeID " +
                    "WHERE c.CondenserID = @id";
                com.CommandType = CommandType.Text;

                SQLiteParameter paramID = new SQLiteParameter("@id", DbType.Int64);
                paramID.Value = m_CondenserID;
                com.Parameters.Add(paramID);

                SQLiteDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    ci.strCondenserNumber = Convert.ToString(dr["CondenserNumber"]);
                    ci.strCondenserTypeName = Convert.ToString(dr["CondenserTypeName"]);
                    ci.strInsulatingLiquidTypeName = Convert.ToString(dr["InsulatingLiquidTypeName"]);
                    ci.fNominalVoltage = Convert.ToDecimal(dr["NominalVoltage"]);
                    ci.fNominalCapacitance = Convert.ToDecimal(dr["NominalCapacitance"]);
                    ci.fEnergyCharge = Convert.ToDecimal(dr["EnergyCharge"]);
                    ci.fInductivity = Convert.ToDecimal(dr["Inductivity"]);
                    ci.fMaxDischargeCurrent = Convert.ToDecimal(dr["MaxDischargeCurrent"]);
                    ci.fCondenserWeight = Convert.ToDecimal(dr["CondenserWeight"]);
                    ci.fCondenserVolume = Convert.ToDecimal(dr["CondenserVolume"]);
                    ci.iNormalizedResource = Convert.ToInt64(dr["NormalizedResource"]);
                }
                dr.Close();

                worker.ReportProgress(20);

                // получаем данные по тестам
                com.CommandText = "SELECT ct.CondenserTestID, ct.CondenserTestType, ct.CondenserTestDate, ct.CondenserTestEquipment, ct.CycleCount, ct.AnalysisDateProbe, ct.Temperature, ct.TestVoltageImageForm, " +
                    "ct.TestVoltageFrequency, ct.TestVoltageAmplitude, ct.AnalysisDateExecute, ct.AnalysisEmployeeFIO, ct.CondenserState, " + 
                    "ct.H2, ct.O2, ct.N2, ct.CH4, ct.CO, ct.CO2, ct.C2H2, ct.C2H4, ct.C2H6, " +
                    "ct.Recommendation, ct.Conclusion " +
                    "FROM CondenserTest AS ct " +
                    "WHERE ct.CondenserID = @id " +
                    "ORDER BY CondenserTestDate";
                com.CommandType = CommandType.Text;

                dr = com.ExecuteReader();

                while (dr.Read())
                {
                    CondenserInfo.CondenserTestInfo cti = new CondenserInfo.CondenserTestInfo();

                    cti.iCondenserTestID = Convert.ToInt64(dr["CondenserTestID"]);

                    if (cti.iCondenserTestID == m_CondenserTestID)
                        ci.iCondenserTestIndex = ci.listCondenserTest.Count;

                    cti.CondenserTestType = (CondenserTest.CondenserTestType)Convert.ToInt64(dr["CondenserTestType"]);
                    cti.dtCondenserTestDate = Convert.ToDateTime(dr["CondenserTestDate"]);
                    cti.strCondenserTestEquipment = Convert.ToString(dr["CondenserTestEquipment"]);
                    if (dr["CycleCount"] != DBNull.Value)
                        cti.iCycleCount = Convert.ToInt64(dr["CycleCount"]);
                    cti.strTemperature = Convert.ToString(dr["Temperature"]);
                    if (dr["TestVoltageImageForm"] != DBNull.Value)
                    {
                        cti.TestVoltageImageForm = new Bitmap(new MemoryStream((byte[])dr["TestVoltageImageForm"]));
                        //cti.TestVoltageImageForm = resizeImage(cti.TestVoltageImageForm, new System.Drawing.Size(550, cti.TestVoltageImageForm.Height * 550 / cti.TestVoltageImageForm.m_image.Width));
                    }
                    cti.fTestVoltageFrequency = Convert.ToDecimal(dr["TestVoltageFrequency"]);
                    cti.fTestVoltageAmplitude = Convert.ToDecimal(dr["TestVoltageAmplitude"]);
                    if (dr["AnalysisDateProbe"] != DBNull.Value)
                        cti.dtAnalysisDateProbe = Convert.ToDateTime(dr["AnalysisDateProbe"]);
                    if (dr["AnalysisDateExecute"] != DBNull.Value)
                        cti.dtAnalysisDateExecute = Convert.ToDateTime(dr["AnalysisDateExecute"]);
                    cti.strAnalysisEmployeeFIO = Convert.ToString(dr["AnalysisEmployeeFIO"]);
                    cti.CondenserState = (CondenserTest.CondenserTestState)Convert.ToInt64(dr["CondenserState"]);
                    if (dr["H2"] != DBNull.Value) cti.fH2 = Convert.ToDecimal(dr["H2"]);
                    else cti.fH2 = null;
                    if (dr["O2"] != DBNull.Value) cti.fO2 = Convert.ToDecimal(dr["O2"]);
                    else cti.fO2 = null;
                    if (dr["N2"] != DBNull.Value) cti.fN2 = Convert.ToDecimal(dr["N2"]);
                    else cti.fN2 = null;
                    if (dr["CH4"] != DBNull.Value) cti.fCH4 = Convert.ToDecimal(dr["CH4"]);
                    else cti.fCH4 = null;
                    if (dr["CO"] != DBNull.Value) cti.fCO = Convert.ToDecimal(dr["CO"]);
                    else cti.fCO = null;
                    if (dr["CO2"] != DBNull.Value) cti.fCO2 = Convert.ToDecimal(dr["CO2"]);
                    else cti.fCO2 = null;
                    if (dr["C2H2"] != DBNull.Value) cti.fC2H2 = Convert.ToDecimal(dr["C2H2"]);
                    else cti.fC2H2 = null;
                    if (dr["C2H4"] != DBNull.Value) cti.fC2H4 = Convert.ToDecimal(dr["C2H4"]);
                    else cti.fC2H4 = null;
                    if (dr["C2H6"] != DBNull.Value) cti.fC2H6 = Convert.ToDecimal(dr["C2H6"]);
                    else cti.fC2H6 = null;
                    cti.strRecommendation = Convert.ToString(dr["Recommendation"]);
                    cti.strConclusion = Convert.ToString(dr["Conclusion"]);

                    ci.listCondenserTest.Add(cti);
                }
                dr.Close();

                worker.ReportProgress(40);

                if (ci.iCondenserTestIndex < 0)
                    ci.iCondenserTestIndex = ci.listCondenserTest.Count - 1;

                // для эксплуатации требуется график
                if (m_CondenserTestID > 0)
                {
                    com.CommandText = "SELECT ShedulerDate, ShedulerCycleCount " +
                    "FROM CondenserTestSheduler WHERE CondenserTestID = @testID ORDER BY ShedulerDate";
                    com.CommandType = CommandType.Text;

                    SQLiteParameter paramTestID = new SQLiteParameter("@testID", DbType.Int64);
                    paramTestID.Value = m_CondenserTestID;
                    com.Parameters.Add(paramTestID);

                    dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        CondenserInfo.CondenserTestInfo.ShedulerInfo si = new CondenserInfo.CondenserTestInfo.ShedulerInfo();
                        si.dtShedulerDate = Convert.ToDateTime(dr["ShedulerDate"]);
                        si.iShedulerCycleCount = Convert.ToInt64(dr["ShedulerCycleCount"]);

                        ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler.Add(si);
                    }
                    dr.Close();
                }

                m_con.Close();
            }
            catch (SQLiteException ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                m_con.Close();
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Word PrintReport()
        {
            worker.ReportProgress(5);

            GetCondenserData();

            CondenserTest.ReportType type = CondenserTest.ReportType.Common;
            if (ci.listCondenserTest.Count == 0)
            {
                MyLocalizer.XtraMessageBoxShow("Для данного конденсатора не проводились испытания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (ci.listCondenserTest[ci.iCondenserTestIndex].CondenserTestType == CondenserTest.CondenserTestType.Resource)
                type = CondenserTest.ReportType.Resource;

            if (ci.listCondenserTest[ci.iCondenserTestIndex].CondenserTestType == CondenserTest.CondenserTestType.Acceptance)
                type = CondenserTest.ReportType.Acceptance;

            if (ci.listCondenserTest[ci.iCondenserTestIndex].CondenserTestType == CondenserTest.CondenserTestType.Operational && m_CondenserTestID > 0)
                type = CondenserTest.ReportType.Operational;

            Word word = new Word();
            List<KeyValuePair<string, string>> bookmarks = new List<KeyValuePair<string, string>>();

            worker.ReportProgress(50);

            //try
            {
                switch (type)
                {
                    case CondenserTest.ReportType.Resource:
                        if (!word.Start(AppDomain.CurrentDomain.BaseDirectory + "/Reports/Ресурсные_испытания.dot")) return null;
                        break;
                    case CondenserTest.ReportType.Acceptance:
                        if (!word.Start(AppDomain.CurrentDomain.BaseDirectory + "/Reports/Приемо_сдаточные_испытания.dot")) return null;
                        break;
                    case CondenserTest.ReportType.Operational:
                        if (!word.Start(AppDomain.CurrentDomain.BaseDirectory + "/Reports/Эксплуатационные_испытания.dot")) return null;
                        break;
                    case CondenserTest.ReportType.Common:
                        if (!word.Start(AppDomain.CurrentDomain.BaseDirectory + "/Reports/Сводный_протокол.dot")) return null;
                        break;
                }

                bookmarks.Add(new KeyValuePair<string, string>("CondenserNumber", ci.strCondenserNumber));
                bookmarks.Add(new KeyValuePair<string, string>("CondenserNumber2", ci.strCondenserNumber));
                bookmarks.Add(new KeyValuePair<string, string>("CondenserNumber3", ci.strCondenserNumber));
                bookmarks.Add(new KeyValuePair<string, string>("CondenserTypeName", ci.strCondenserTypeName));
                bookmarks.Add(new KeyValuePair<string, string>("CondenserVolume", ci.fCondenserVolume.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("CondenserWeight", ci.fCondenserWeight.ToString()));
                if (type == CondenserTest.ReportType.Acceptance || type == CondenserTest.ReportType.Operational || type == CondenserTest.ReportType.Common)
                    bookmarks.Add(new KeyValuePair<string, string>("NormalizedResource", ci.iNormalizedResource.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("EnergyCharge", ci.fEnergyCharge.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("Inductivity", ci.fInductivity.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("InsulatingLiquidTypeName", ci.strInsulatingLiquidTypeName));
                bookmarks.Add(new KeyValuePair<string, string>("MaxDischargeCurrent", ci.fMaxDischargeCurrent.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("NominalCapacitance", ci.fNominalCapacitance.ToString()));
                bookmarks.Add(new KeyValuePair<string, string>("NominalVoltage", ci.fNominalVoltage.ToString()));
                if (type == CondenserTest.ReportType.Common)
                {
                    if (ci.listCondenserTest[0].CondenserTestType == CondenserTest.CondenserTestType.Acceptance)
                    {
                        bookmarks.Add(new KeyValuePair<string, string>("PSIDate", ci.listCondenserTest[0].dtCondenserTestDate.ToShortDateString()));
                        if (ci.listCondenserTest[0].fH2 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("H2", ci.listCondenserTest[0].fH2.ToString()));
                        if (ci.listCondenserTest[0].fN2 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("N2", ci.listCondenserTest[0].fN2.ToString()));
                        if (ci.listCondenserTest[0].fO2 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("O2", ci.listCondenserTest[0].fO2.ToString()));
                        if (ci.listCondenserTest[0].fC2H2 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("C2H2", ci.listCondenserTest[0].fC2H2.ToString()));
                        if (ci.listCondenserTest[0].fC2H4 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("C2H4", ci.listCondenserTest[0].fC2H4.ToString()));
                        if (ci.listCondenserTest[0].fC2H6 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("C2H6", ci.listCondenserTest[0].fC2H6.ToString()));
                        if (ci.listCondenserTest[0].fCH4 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("CH4", ci.listCondenserTest[0].fCH4.ToString()));
                        if (ci.listCondenserTest[0].fCO != null)
                            bookmarks.Add(new KeyValuePair<string, string>("CO", ci.listCondenserTest[0].fCO.ToString()));
                        if (ci.listCondenserTest[0].fCO2 != null)
                            bookmarks.Add(new KeyValuePair<string, string>("CO2", ci.listCondenserTest[0].fCO2.ToString()));
                    }
                }

                if (type != CondenserTest.ReportType.Common)
                {
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].CondenserTestType == CondenserTest.CondenserTestType.Operational)
                        bookmarks.Add(new KeyValuePair<string, string>("Index", (ci.iCondenserTestIndex).ToString("00"))); // т.к. первое всегда идет приемо-сдаточное
                    else
                        bookmarks.Add(new KeyValuePair<string, string>("Index", (ci.iCondenserTestIndex + 1).ToString("00")));

                    bookmarks.Add(new KeyValuePair<string, string>("CondenserTestDate", ci.listCondenserTest[ci.iCondenserTestIndex].dtCondenserTestDate.ToShortDateString()));

                    if (type == CondenserTest.ReportType.Resource || type == CondenserTest.ReportType.Operational)
                        bookmarks.Add(new KeyValuePair<string, string>("CycleCount", ci.listCondenserTest[ci.iCondenserTestIndex].iCycleCount.ToString()));

                    if (type == CondenserTest.ReportType.Resource)
                        bookmarks.Add(new KeyValuePair<string, string>("Conclusion", ci.listCondenserTest[ci.iCondenserTestIndex].CondenserState == CondenserTest.CondenserTestState.Work ? "работоспособен" : "неработоспособен"));
                    if (type == CondenserTest.ReportType.Acceptance || type == CondenserTest.ReportType.Operational)
                    {
                        bookmarks.Add(new KeyValuePair<string, string>("Conclusion", ci.listCondenserTest[ci.iCondenserTestIndex].strConclusion));
                        bookmarks.Add(new KeyValuePair<string, string>("Recommendation", ci.listCondenserTest[ci.iCondenserTestIndex].strRecommendation));
                    }
                    bookmarks.Add(new KeyValuePair<string, string>("CondenserTestEquipment", ci.listCondenserTest[ci.iCondenserTestIndex].strCondenserTestEquipment));

                    bookmarks.Add(new KeyValuePair<string, string>("Temperature", ci.listCondenserTest[ci.iCondenserTestIndex].strTemperature));
                    bookmarks.Add(new KeyValuePair<string, string>("TestVoltageAmplitude", ci.listCondenserTest[ci.iCondenserTestIndex].fTestVoltageAmplitude.ToString()));
                    bookmarks.Add(new KeyValuePair<string, string>("TestVoltageFrequency", ci.listCondenserTest[ci.iCondenserTestIndex].fTestVoltageFrequency.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].dtAnalysisDateExecute != null)
                        bookmarks.Add(new KeyValuePair<string, string>("AnalysisDateExecute", ((DateTime)ci.listCondenserTest[ci.iCondenserTestIndex].dtAnalysisDateExecute).ToShortDateString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].dtAnalysisDateProbe != null)
                        bookmarks.Add(new KeyValuePair<string, string>("AnalysisDateProbe", ((DateTime)ci.listCondenserTest[ci.iCondenserTestIndex].dtAnalysisDateProbe).ToShortDateString()));
                    bookmarks.Add(new KeyValuePair<string, string>("AnalysisEmployeeFIO", ci.listCondenserTest[ci.iCondenserTestIndex].strAnalysisEmployeeFIO));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fH2 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("H2", ci.listCondenserTest[ci.iCondenserTestIndex].fH2.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fN2 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("N2", ci.listCondenserTest[ci.iCondenserTestIndex].fN2.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fO2 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("O2", ci.listCondenserTest[ci.iCondenserTestIndex].fO2.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fC2H2 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("C2H2", ci.listCondenserTest[ci.iCondenserTestIndex].fC2H2.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fC2H4 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("C2H4", ci.listCondenserTest[ci.iCondenserTestIndex].fC2H4.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fC2H6 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("C2H6", ci.listCondenserTest[ci.iCondenserTestIndex].fC2H6.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fCH4 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("CH4", ci.listCondenserTest[ci.iCondenserTestIndex].fCH4.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fCO != null)
                        bookmarks.Add(new KeyValuePair<string, string>("CO", ci.listCondenserTest[ci.iCondenserTestIndex].fCO.ToString()));
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].fCO2 != null)
                        bookmarks.Add(new KeyValuePair<string, string>("CO2", ci.listCondenserTest[ci.iCondenserTestIndex].fCO2.ToString()));
                }

                word.SetBookmarkText(bookmarks);
                
                worker.ReportProgress(80);

                if (type == CondenserTest.ReportType.Operational)
                {
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler.Count > 1)
                    {
                        word.InsertRowsInTable(3, ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler.Count - 1);
                    }

                    for (int i = 0; i < ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler.Count; i++)
                    {
                        word.SetTextInCell(1, i + 1 + 1, ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler[i].dtShedulerDate.ToShortDateString(), 3);
                        word.SetTextInCell(2, i + 1 + 1, ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler[i].dtShedulerDate.ToString("HH:mm"), 3);
                        word.SetTextInCell(3, i + 1 + 1, ci.listCondenserTest[ci.iCondenserTestIndex].listSheduler[i].iShedulerCycleCount.ToString(), 3);
                    }
                }

                if (type == CondenserTest.ReportType.Common)
                {
                    word.InsertRowsInTable(2, ci.listCondenserTest.Count - 2);

                    int iRowIndex = 4;
                    long iCycleCount = 0;
                    for (int i = 0; i < ci.listCondenserTest.Count; i++)
                    {
                        if (ci.listCondenserTest[i].CondenserTestType == CondenserTest.CondenserTestType.Operational)
                        {
                            word.SetTextInCell(1, iRowIndex, ci.listCondenserTest[i].dtCondenserTestDate.ToShortDateString(), 2);

                            if (ci.listCondenserTest[i].iCycleCount != null)
                                iCycleCount += (long)ci.listCondenserTest[i].iCycleCount;
                            word.SetTextInCell(2, iRowIndex, iCycleCount.ToString(), 2);

                            if (ci.listCondenserTest[i].fH2 != null)
                                word.SetTextInCell(3, iRowIndex, ci.listCondenserTest[i].fH2.ToString(), 2);
                            if (ci.listCondenserTest[i].fO2 != null)
                                word.SetTextInCell(4, iRowIndex, ci.listCondenserTest[i].fO2.ToString(), 2);
                            if (ci.listCondenserTest[i].fN2 != null)
                                word.SetTextInCell(5, iRowIndex, ci.listCondenserTest[i].fN2.ToString(), 2);
                            if (ci.listCondenserTest[i].fCH4 != null) 
                                word.SetTextInCell(6, iRowIndex, ci.listCondenserTest[i].fCH4.ToString(), 2);
                            if (ci.listCondenserTest[i].fCO != null)
                                word.SetTextInCell(7, iRowIndex, ci.listCondenserTest[i].fCO.ToString(), 2);
                            if (ci.listCondenserTest[i].fCO2 != null)
                                word.SetTextInCell(8, iRowIndex, ci.listCondenserTest[i].fCO2.ToString(), 2);
                            if (ci.listCondenserTest[i].fC2H4 != null)
                                word.SetTextInCell(9, iRowIndex, ci.listCondenserTest[i].fC2H4.ToString(), 2);
                            if (ci.listCondenserTest[i].fC2H6 != null)
                                word.SetTextInCell(10, iRowIndex, ci.listCondenserTest[i].fC2H6.ToString(), 2);
                            if (ci.listCondenserTest[i].fC2H2 != null)
                                word.SetTextInCell(11, iRowIndex, ci.listCondenserTest[i].fC2H2.ToString(), 2);
                            iRowIndex++;
                        }
                    }

                    if (ci.listCondenserTest.Count > 2)
                    {
                        List<Word.BorderInfo> listBorders = new List<Word.BorderInfo>();
                        listBorders.Add(new Word.BorderInfo(Microsoft.Office.Interop.Word.WdBorderType.wdBorderHorizontal, Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth075pt, Microsoft.Office.Interop.Word.WdColor.wdColorBlack));
                        listBorders.Add(new Word.BorderInfo(Microsoft.Office.Interop.Word.WdBorderType.wdBorderVertical, Microsoft.Office.Interop.Word.WdLineStyle.wdLineStyleSingle, Microsoft.Office.Interop.Word.WdLineWidth.wdLineWidth075pt, Microsoft.Office.Interop.Word.WdColor.wdColorBlack));
                        word.SetCellsBorders(1, 11, 4, 4 + ci.listCondenserTest.Count - 1, listBorders, 2);
                    }
                }
                else
                {
                    if (ci.listCondenserTest[ci.iCondenserTestIndex].TestVoltageImageForm != null)
                    {
                        ci.listCondenserTest[ci.iCondenserTestIndex].TestVoltageImageForm.Save(AppDomain.CurrentDomain.BaseDirectory + "/Reports/tmp.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                        word.InsertPicture(AppDomain.CurrentDomain.BaseDirectory + "/Reports/tmp.jpg", Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter, "TestVoltageImageForm");
                    }
                }

                worker.ReportProgress(100);

                return word;
            }
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_Word = PrintReport();
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progress.Position = e.ProgressPercentage;
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progress.Position = 100;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            m_bEnd = true;
            Close();
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            worker.RunWorkerAsync();
        }

        private void WaitingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_bEnd) e.Cancel = true;
        }
    }
}