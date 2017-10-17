using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Drawing;
using System.Data.SQLite;

namespace Condenser
{
    public partial class ParameterRecordForm : DevExpress.XtraEditors.XtraForm
    {
        class DataSourceInfo
        {
            public DataSourceInfo(string strVal1, string strVal2, string strVal3, string strVal4, string strVal5)
            {
                m_strVal1 = strVal1;
                m_strVal2 = strVal2;
                m_strVal3 = strVal3;
                m_strVal4 = strVal4;
                m_strVal5 = strVal5;
            }

            private string m_strVal1;
            private string m_strVal2;
            private string m_strVal3;
            private string m_strVal4;
            private string m_strVal5;

            public string VAL1
            {
                get { return m_strVal1; }
                set { m_strVal2 = value; }
            }

            public string VAL2
            {
                get { return m_strVal2; }
                set { m_strVal2 = value; }
            }

            public string VAL3
            {
                get { return m_strVal3; }
                set { m_strVal3 = value; }
            }

            public string VAL4
            {
                get { return m_strVal4; }
                set { m_strVal4 = value; }
            }

            public string VAL5
            {
                get { return m_strVal5; }
                set { m_strVal5 = value; }
            }

        };

        public class RecommendationInfo
        {
            public RecommendationInfo()
            {
                m_ID = null;
                m_strRecommendation = "";
                m_strConclusion = "";
                m_fValue = null;
                m_strResult = "";
            }

            public RecommendationInfo(long? ID, string strRecommendation, string strConclusion, Decimal? fValue)
            {
                m_ID = ID;
                m_strRecommendation = strRecommendation;
                m_strConclusion = strConclusion;
                m_fValue = fValue;

                m_strResult = "";
                if (strConclusion != "")
                    m_strResult = "<b>Заключение:</b> " + strConclusion;
                if (strRecommendation != "")
                {
                    if (strConclusion != "") m_strResult += "<br>";
                    if (fValue != null)
                        m_strResult += "<b>Рекомендация:</b> " + strRecommendation.Replace("$", "<b>" + fValue.ToString() + "%</b>");
                    else
                        m_strResult += "<b>Рекомендация:</b> " + strRecommendation.Replace("$", "");
                }
            }

            public long? m_ID;
            public string m_strRecommendation;
            public string m_strConclusion;
            public Decimal? m_fValue;
            public string m_strResult;
        };

        Dictionary<int, int> dictBandHeigths = new Dictionary<int,int>();
        int m_singleLineHeight = 0;
        public object m_iCondenserTypeID = null;
        public object m_iKoefID = null;
        BindingList<DataSourceString> listFunctionType = new BindingList<DataSourceString>();
        Dictionary<long, RecommendationInfo> dictRecords = new Dictionary<long, RecommendationInfo>();
        BindingList<DataSourceInfo> listData = new BindingList<DataSourceInfo>();

        float DpiXRel;
        float DpiYRel;

        bool m_bLoadEnd = false;

        public ParameterRecordForm()
        {
            InitializeComponent();
        }

        private void TableForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypes' table. You can move, or remove it, as needed.
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (strSeparator == ".") strSeparator = "\\.";
            teKoefA.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";
            teKoefB.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";
            teR2.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";
            teKI1.Properties.Mask.EditMask = "(\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";

            listFunctionType.Add(new DataSourceString(0, "степенная"));
            listFunctionType.Add(new DataSourceString(1, "экспоненциальная"));
            listFunctionType.Add(new DataSourceString(2, "логарифмическая"));
            cbFunctionType.Properties.DataSource = listFunctionType;
            cbFunctionType.Properties.DisplayMember = "VAL";
            cbFunctionType.Properties.ValueMember = "KEY";
            cbFunctionType.Properties.DropDownRows = listFunctionType.Count;
            cbFunctionType.EditValue = (long)0;

            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypesForTable' table. You can move, or remove it, as needed.
            this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);
            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypes' table. You can move, or remove it, as needed.
            if (this.dataSetQuery.QCondenserTypes.Count < 7)
                cbCondenserType.Properties.DropDownRows = this.dataSetQuery.QCondenserTypes.Count;
            else
                cbCondenserType.Properties.DropDownRows = 7;

            Graphics formGraphics = this.CreateGraphics();
            DpiXRel = formGraphics.DpiX / 96.0f;
            DpiYRel = formGraphics.DpiY / 96.0f;


            GridDC.DataSource = listData;

            //BandedGridViewInfo viewInfo = GridView.GetViewInfo() as BandedGridViewInfo;
            Size captionSize = GridView.Appearance.BandPanel.CalcTextSize(formGraphics, "www", 1000).ToSize();
            m_singleLineHeight = captionSize.Height;// viewInfo.BandRowHeight;
            //DevExpress.Utils.AppearanceObject[] styles = {GridView.Appearance.BandPanel};
            //m_singleLineHeight = viewInfo.CalcMaxHeight(styles);
            //int ii = viewInfo.ColumnRowHeight;

            cbCondenserType.EditValue = m_iCondenserTypeID;

            for (int i = 1; i <= 4; i++)
            {
                for (int j = 1; j <= 4; j++)
                    dictRecords[i * 10 + j] = new RecommendationInfo();
            }

            if (m_iKoefID != DBNull.Value)
            {
                try
                {
                    SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    connection.Open();
                    SQLiteCommand com = new SQLiteCommand(connection);
                    com.CommandText = "SELECT KI1, KoefA, KoefB, KoefR2, FunctionType FROM CondenserTypeParameters WHERE KoefID = @koefID";
                    com.CommandType = CommandType.Text;
                    SQLiteParameter param1 = new SQLiteParameter("@koefID", DbType.Int64);
                    param1.Value = (long)m_iKoefID;
                    com.Parameters.Add(param1);

                    SQLiteDataReader dr = com.ExecuteReader();
                    while (dr.Read())
                    {
                        if (dr["KI1"] != DBNull.Value) teKI1.EditValue = dr["KI1"];
                        if (dr["KoefA"] != DBNull.Value) teKoefA.EditValue = dr["KoefA"];
                        if (dr["KoefB"] != DBNull.Value) teKoefB.EditValue = dr["KoefB"];
                        if (dr["KoefR2"] != DBNull.Value) teR2.EditValue = dr["KoefR2"];
                        if (dr["FunctionType"] != DBNull.Value) cbFunctionType.EditValue = dr["FunctionType"];

                        break;
                    }
                    dr.Close();

                    com.CommandText = "select ID, Position, Value, Conclusion, Recommendation FROM ParameterRecommendations WHERE KoefID = @KoefID";
                    com.CommandType = CommandType.Text;

                    SQLiteDataReader drRecomm = com.ExecuteReader();

                    while (drRecomm.Read())
                    {
                        long ID = Convert.ToInt64(drRecomm["ID"]);
                        long iPosition = Convert.ToInt64(drRecomm["Position"]);
                        Decimal? fValue = null;
                        if (drRecomm["Value"] != DBNull.Value) fValue = Convert.ToDecimal(drRecomm["Value"]);
                        string strConclusion = Convert.ToString(drRecomm["Conclusion"]);
                        string strRecommendation = Convert.ToString(drRecomm["Recommendation"]);
                        dictRecords[iPosition] = new RecommendationInfo(ID, strRecommendation, strConclusion, fValue);

                        /*if (fValue != null)
                            strRecommendation = strRecommendation.Replace("$", "<b>" + fValue.ToString() + "%</b>");
                        else
                            strRecommendation = strRecommendation.Replace("$", "");

                        if (strConclusion != "")
                            dictRecords[iPosition] = "<b>Заключение:</b> " + strConclusion;
                        if (strRecommendation != "")
                        {
                            if (strConclusion != "") dictRecords[iPosition] += "<br>";
                            dictRecords[iPosition] += "<b>Рекомендация:</b> " + strRecommendation;
                        }*/
                    }
                    drRecomm.Close();

                    connection.Close();
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                teKI1.EditValue = null;// 20;
                teKoefA.EditValue = null;//11;
                teKoefB.EditValue = null;//-1.1;
                teR2.EditValue = null;

                /*dictRecords[11] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 20);
                dictRecords[12] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 10);
                dictRecords[13] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 5);
                dictRecords[14] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 5);

                dictRecords[21] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 10);
                dictRecords[22] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 10);
                dictRecords[23] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 5);
                dictRecords[24] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "конденсатор работоспособен.", 5);

                dictRecords[31] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
                dictRecords[32] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
                dictRecords[33] = new RecommendationInfo(null, "следующий отбор пробы провести после $ от РЕС циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
                dictRecords[34] = new RecommendationInfo(null, "целесообразно вывести конденсатор из эксплуатации", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", null);

                dictRecords[41] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
                dictRecords[42] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
                dictRecords[43] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
                dictRecords[44] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);*/
            }

            FillData();
            GridView.FocusedColumn = GridView.Columns[1];
            GridView.FocusedRowHandle = 0;


            this.WindowState = FormWindowState.Maximized;

            m_bLoadEnd = true;

            timer1.Enabled = true;

            //SetBandsHeight();
        }

        private int GetColumnBestHeight(GridBand column, int width/*, GridBand parent_column*/)
        {
            GridBandInfoArgs ex = null;
            BandedGridViewInfo viewInfo = GridView.GetViewInfo() as BandedGridViewInfo;
            viewInfo.GInfo.AddGraphics(null);
            ex = new GridBandInfoArgs(null, viewInfo.GInfo.Cache);
            try
            {
                ex.InnerElements.Add(new DrawElementInfo(new GlyphElementPainter(),
                                                        new GlyphElementInfoArgs(viewInfo.View.Images, 0, null),
                                                        StringAlignment.Near));
                ex.SetAppearance(GridView.Appearance.BandPanel);
                ex.Caption = column.Caption;
                ex.CaptionRect = new Rectangle(0, 0, (int)(width - 10 * DpiXRel), 1000);
            }
            finally
            {
                viewInfo.GInfo.ReleaseGraphics();
            }

            GraphicsInfo grInfo = new GraphicsInfo();
            grInfo.AddGraphics(null);
            ex.Cache = grInfo.Cache;
            int Height = CalcCaptionTextSize(grInfo.Cache, ex as HeaderObjectInfoArgs, column.Caption);
            return Height;
        }

        int CalcCaptionTextSize(GraphicsCache cache, HeaderObjectInfoArgs ee, string caption)
        {
            string[] arr = caption.Split('\n');
            int height = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                Size captionSize = ee.Appearance.CalcTextSize(cache, arr[i], ee.CaptionRect.Width).ToSize();
                height += captionSize.Height;
            }
            //captionSize.Height++; captionSize.Width++;
            return height;
        }

        private void GridView_BandWidthChanged(object sender, DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs e)
        {
            //if (e.Band.Children.Count > 0)
                //SetBandHeight(e.Band);
            //SetBandsHeight(e.Band.Index);
            for (int i = e.Band.Index; i < GridView.Bands.Count; i++)
            {
                SetBandsHeight(i);
            }
        }

        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }

        private void SetBandsHeight(int index = -1)
        {
            try
            {
                if (!m_bLoadEnd) return;

                GridView.BeginUpdate();

                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    if (index >= 0 && i != index) continue;

                    int height = GetColumnBestHeight(GridView.Bands[i], GridView.Bands[i].VisibleWidth);

                    dictBandHeigths[i] = height;
                }

                int max_height = 0;
                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    if (max_height < dictBandHeigths[i]) max_height = dictBandHeigths[i];
                }
                int iRowCount = max_height / m_singleLineHeight;
                if (iRowCount * m_singleLineHeight < max_height) iRowCount++;

                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    GridView.Bands[i].RowCount = iRowCount;
                }

                GridView.EndUpdate();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            SetBandsHeight();
            //FillData();
        }

        private void TableForm_SizeChanged(object sender, EventArgs e)
        {
            SetBandsHeight();
        }

        private void cbCondenserType_KeyUp(object sender, KeyEventArgs e)
        {
            /*if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back) )
            {
                ((LookUpEdit)sender).ClosePopup();
                ((LookUpEdit)sender).EditValue = null;
            }*/
        }

        private void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        private void FillData()
        {
            DevExpress.XtraGrid.Columns.GridColumn col = GridView.FocusedColumn;
            int row = GridView.FocusedRowHandle;
            GridView.BeginUpdate();
            listData.Clear();
            listData.Add(new DataSourceInfo("ВР ≤ 80% РЕС", dictRecords[11].m_strResult, dictRecords[12].m_strResult, dictRecords[13].m_strResult, dictRecords[14].m_strResult));
            listData.Add(new DataSourceInfo("80% РЕС < ВР ≤ 90% РЕС", dictRecords[21].m_strResult, dictRecords[22].m_strResult, dictRecords[23].m_strResult, dictRecords[24].m_strResult));
            listData.Add(new DataSourceInfo("90% РЕС < ВР < 100% РЕС", dictRecords[31].m_strResult, dictRecords[32].m_strResult, dictRecords[33].m_strResult, dictRecords[34].m_strResult));
            listData.Add(new DataSourceInfo("ВР ≥ 100% РЕС", dictRecords[41].m_strResult, dictRecords[42].m_strResult, dictRecords[43].m_strResult, dictRecords[44].m_strResult));
            GridView.EndUpdate();

            GridView.FocusedColumn = col;
            GridView.FocusedRowHandle = row;

            if (m_bLoadEnd)
                SetBandsHeight();
        }

        private void cbCondenserType_EditValueChanged(object sender, EventArgs e)
        {
        }

        private void GridGC_Click(object sender, EventArgs e)
        {

        }

        private void bCalcKoef_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridView.FocusedRowHandle < 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо выбрать запись.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                CalcKoefForm f = new CalcKoefForm();
                f.m_iCondenserTypeID = m_iCondenserTypeID;
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    teKoefA.EditValue = new Decimal(Convert.ToInt32(Math.Abs(f.m_koefA) * 10000), 0, 0, f.m_koefA < 0, 4);
                    teKoefB.EditValue = new Decimal(Convert.ToInt32(Math.Abs(f.m_koefB) * 10000), 0, 0, f.m_koefB < 0, 4);
                    teR2.EditValue = new Decimal(Convert.ToInt32(Math.Abs(f.m_koefR2) * 10000), 0, 0, f.m_koefR2 < 0, 4);
                    cbFunctionType.EditValue = (long)f.m_FunctionType;
                };
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
             if (teKI1.EditValue == null)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значение поля \"КЦ1\".", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                teKI1.Focus();
                return;
            }

             if (teKoefA.EditValue == null)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значение коэффициента А.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                teKoefA.Focus();
                return;
            }

            if (teKoefB.EditValue == null)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значение коэффициента B.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                teKoefB.Focus();
                return;
            }

            foreach (KeyValuePair<long, RecommendationInfo> pair in dictRecords)
            {
                if (pair.Value.m_strConclusion == "" && pair.Value.m_strRecommendation == "")
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо указать заключение и рекомендации во всех ячейках таблицы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
            SQLiteTransaction tran = null;
            try
            {
                connection.Open();
                tran = connection.BeginTransaction();

                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandType = CommandType.Text;
                if (m_iKoefID != null && m_iKoefID != DBNull.Value)
                {
                    com.CommandText = "UPDATE CondenserTypeParameters SET KI1 = @ki1, KoefA = @koefA, KoefB = @koefB, KoefR2 = @koefR2, FunctionType = @fntype WHERE KoefID = @koefID";
                    AddParam(com, "@koefID", DbType.Int64, (long)m_iKoefID);
                }
                else
                {
                    com.CommandText = "INSERT INTO CondenserTypeParameters (KI1, KoefA, KoefB, KoefR2, FunctionType, CondenserTypeID) VALUES (@ki1, @koefA, @koefB, @koefR2, @fntype, @ctid)";
                    AddParam(com, "@ctid", DbType.Int64, (long)m_iCondenserTypeID);
                }

                AddParam(com, "@ki1", DbType.Decimal, Convert.ToDecimal(teKI1.EditValue));
                AddParam(com, "@koefA", DbType.Decimal, Convert.ToDecimal(teKoefA.EditValue));
                AddParam(com, "@koefB", DbType.Decimal, Convert.ToDecimal(teKoefB.EditValue));
                if (teR2.EditValue == DBNull.Value || teR2.EditValue == null)
                    AddParam(com, "@koefR2", DbType.Decimal, DBNull.Value);
                else
                    AddParam(com, "@koefR2", DbType.Decimal, Convert.ToDecimal(teR2.EditValue));
                AddParam(com, "@fntype", DbType.Int64, Convert.ToInt64(((Condenser.DataSourceString)cbFunctionType.GetSelectedDataRow()).KEY));

                com.ExecuteNonQuery();

                long iKoefID = 0;
                if (m_iKoefID == null || m_iKoefID == DBNull.Value)
                {
                    com.CommandText = "select seq from sqlite_sequence where name = 'CondenserTypeParameters'";
                    com.Parameters.Clear();
                    SQLiteDataReader dr = com.ExecuteReader();

                    while (dr.Read())
                    {
                        iKoefID = Convert.ToInt64(dr["seq"]);
                    }
                    dr.Close();
                }
                else
                {
                    iKoefID = (long)m_iKoefID;
                }

                // сохраняем данные из таблицы
                com.CommandText = "DELETE FROM ParameterRecommendations where KoefID = @koefID";
                com.Parameters.Clear();
                AddParam(com, "@koefID", DbType.Int64, (long)iKoefID);
                com.ExecuteNonQuery();

                com.CommandText = "INSERT INTO ParameterRecommendations (KoefID, Position, Value, Recommendation, Conclusion) VALUES (@koefID, @pos, @val, @recom, @concl)";
                foreach (KeyValuePair<long, RecommendationInfo> pair in dictRecords)
                {
                    com.Parameters.Clear();
                    AddParam(com, "@koefID", DbType.Int64, iKoefID);
                    AddParam(com, "@pos", DbType.Int64, pair.Key);
                    AddParam(com, "@val", DbType.Decimal, pair.Value.m_fValue);
                    AddParam(com, "@recom", DbType.String, pair.Value.m_strRecommendation);
                    AddParam(com, "@concl", DbType.String, pair.Value.m_strConclusion);

                    com.ExecuteNonQuery();
                }

                if (tran != null) tran.Commit();
                connection.Close();
            }
            catch (SQLiteException ex)
            {
                if (tran != null) tran.Rollback();
                connection.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void UpdateCell()
        {
            DevExpress.XtraGrid.Columns.GridColumn col = GridView.FocusedColumn;
            int row = GridView.FocusedRowHandle;

            if (col.VisibleIndex <= 0)
            {
                return;
            }

            int iPosition = (row + 1) * 10 + col.VisibleIndex;
            string strCaption = "при " + col.Caption + " и " + listData[row].VAL1;
            RecommendationRecordForm f = new RecommendationRecordForm(dictRecords[iPosition], strCaption);

            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                dictRecords[iPosition] = f.m_RecommendationInfo;
                FillData();
            }
        }

        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            UpdateCell();
        }

        private void bEdit_Click(object sender, EventArgs e)
        {
            UpdateCell();
        }

        private void GridView_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (e.FocusedColumn.VisibleIndex <= 0)
                bEdit.Enabled = false;
            else
                bEdit.Enabled = true;
        }

        private void teKI1_EditValueChanged(object sender, EventArgs e)
        {
            if (teKI1.Text == "") teKI1.EditValue = null;
        }

        private void teKoefA_EditValueChanged(object sender, EventArgs e)
        {
            if (teKoefA.Text == "") teKoefA.EditValue = null;
        }

        private void teKoefB_EditValueChanged(object sender, EventArgs e)
        {
            if (teKoefB.Text == "") teKoefB.EditValue = null;
        }

        private void bDefault_Click(object sender, EventArgs e)
        {
            teKI1.EditValue = 20;
            teKoefA.EditValue = 11;
            teKoefB.EditValue = -1.1;
            teR2.EditValue = null;

            dictRecords[11] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 20);
            dictRecords[12] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 10);
            dictRecords[13] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 5);
            dictRecords[14] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 5);

            dictRecords[21] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 10);
            dictRecords[22] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 10);
            dictRecords[23] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 5);
            dictRecords[24] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "конденсатор работоспособен.", 5);

            dictRecords[31] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
            dictRecords[32] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
            dictRecords[33] = new RecommendationInfo(null, "следующий отбор пробы провести после $ циклов", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", 5);
            dictRecords[34] = new RecommendationInfo(null, "целесообразно вывести конденсатор из эксплуатации", "по показаниям газосодержания масла выработано более 90% ресурса конденсатора.", null);

            dictRecords[41] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
            dictRecords[42] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
            dictRecords[43] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);
            dictRecords[44] = new RecommendationInfo(null, "вывести конденсатор из эксплуатации", "по показаниям газосодержания масла ресурс конденсатора исчерпан.", null);

            FillData();
        }

        private void cbFunctionType_EditValueChanged(object sender, EventArgs e)
        {
            CondenserTest.FunctionType ft = (CondenserTest.FunctionType)Convert.ToInt64(cbFunctionType.EditValue);
            if (ft == CondenserTest.FunctionType.Degree)
                lFunctionType.Text = "y = Axᴮ";
            if (ft == CondenserTest.FunctionType.Exp)
                lFunctionType.Text = "y = Aeᴮˣ";
            if (ft == CondenserTest.FunctionType.Log)
                lFunctionType.Text = "y = Aln(x) + B";
        }
    }
}