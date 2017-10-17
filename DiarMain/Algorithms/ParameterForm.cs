using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SQLite;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Drawing;
using System.IO;
using System.Text;

namespace Condenser
{
    public partial class ParameterForm : DevExpress.XtraEditors.XtraForm
    {
        bool m_bAcceptChanges = true;
        bool m_bUpdateID = false;
        public long m_SelectID = 0;
        private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string, int>();
        BindingList<DataSourceString> listFunctionType = new BindingList<DataSourceString>();
        bool m_bLoadData = true;
        float DpiXRel;
        float DpiYRel;

        bool bAdding = false;

        public ParameterForm()
        {
            InitializeComponent();
        }

        private void ParameterForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;
            
            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypes' table. You can move, or remove it, as needed.
            try
            {
                string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                if (strSeparator == ".") strSeparator = "\\.";
                this.teHLS.Properties.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,2})";

                listFunctionType.Add(new DataSourceString(0, "степенная"));
                listFunctionType.Add(new DataSourceString(1, "экспоненциальная"));
                listFunctionType.Add(new DataSourceString(2, "логарифмическая"));
                repFunctionType.DataSource = listFunctionType;
                repFunctionType.DisplayMember = "VAL";
                repFunctionType.ValueMember = "KEY";
                repFunctionType.DropDownRows = listFunctionType.Count;

                
                // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypeParameters' table. You can move, or remove it, as needed.
                this.qCondenserTypeParametersTableAdapter.Fill(this.dataSetQuery.QCondenserTypeParameters);
                this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);

                if (this.dataSetQuery.QCondenserTypes.Count < 7)
                    repCondenserType.DropDownRows = this.dataSetQuery.QCondenserTypes.Count;
                else
                    repCondenserType.DropDownRows = 7;

                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "select ParameterValue from CommonParameters where ParameterID = 1";
                com.CommandType = CommandType.Text;
                SQLiteDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    teHLS.EditValue = dr["ParameterValue"];
                }
                dr.Close();

                connection.Close();

                Graphics formGraphics = this.CreateGraphics();
                DpiXRel = formGraphics.DpiX / 96.0f;
                DpiYRel = formGraphics.DpiY / 96.0f;

            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            m_bLoadData = false;
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
                GridView.ColumnPanelRowHeight = maxHeight;
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
                DevExpress.XtraGrid.Views.Grid.ViewInfo.GridViewInfo viewInfo = GridView.GetViewInfo() as GridViewInfo;

                GridColumnInfoArgs ex = null;
                viewInfo.GInfo.AddGraphics(null);
                ex = new GridColumnInfoArgs(viewInfo.GInfo.Cache, null);
                try
                {
                    ex.InnerElements.Add(new DrawElementInfo(new GlyphElementPainter(),
                                                            new GlyphElementInfoArgs(viewInfo.View.Images, 0, null),
                                                            StringAlignment.Near));
                    ex.SetAppearance(GridView.Appearance.HeaderPanel);
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

        private void GridView_ColumnWidthChanged(object sender, DevExpress.XtraGrid.Views.Base.ColumnEventArgs e)
        {
            try
            {
                SetColumnHeight(e.Column);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ParameterForm_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridColumn col in GridView.Columns)
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            foreach (GridColumn col in GridView.Columns)
            {
                GetColumnBestHeight(col);
            }
            SetMaxColumnHeights();
        }

        private void teHLS_EditValueChanged(object sender, EventArgs e)
        {
            if (!m_bLoadData)
                bSave.Enabled = true;
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (teHLS.EditValue == null || teHLS.EditValue.ToString() == "")
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значение порога чувствительности хроматографа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (Convert.ToDecimal(teHLS.EditValue) < new Decimal(0.1) || Convert.ToDecimal(teHLS.EditValue) > new Decimal(100))
            {
                MyLocalizer.XtraMessageBoxShow("Значение порога чувствительности хроматографа должно быть в пределах от 0.1 до 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "UPDATE CommonParameters SET ParameterValue = @val WHERE ParameterID = 1";
                com.CommandType = CommandType.Text;
                SQLiteParameter param = new SQLiteParameter("@val", DbType.String);
                param.Value = teHLS.EditValue;
                com.Parameters.Add(param);
                com.ExecuteNonQuery();

                connection.Close();
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bSave.Enabled = false;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            if (dlgSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();

                string strPath = dlgSave.FileName;
                //StreamWriter sw = new StreamWriter(strPath, false);
                char chTab = (char)9;
                List<string> vecData = new List<string>();
                string strData = "ПЧХ" + chTab + teHLS.EditValue.ToString();
                //sw.WriteLine(strData);
                vecData.Add(strData);
                for (int i = 0; i < qCondenserTypeParametersBindingSource.Count; i++)
                {
                    DataRowView drv = (DataRowView)(qCondenserTypeParametersBindingSource[i]);

                    long CondenserTypeID = -1;
                    string strCondenserTypeName = "";
                    if (drv["CondenserTypeID"] != DBNull.Value)
                    {
                        CondenserTypeID = Convert.ToInt64(drv["CondenserTypeID"]);
                        int itemFound = qCondenserTypesBindingSource.Find("CondenserTypeID", CondenserTypeID);
                        strCondenserTypeName = ((DataRowView)(qCondenserTypesBindingSource[itemFound]))["CondenserTypeName"].ToString();
                    }

                    if (drv["KoefID"] != DBNull.Value)
                    {
                        strData = "Data";
                        //sw.WriteLine(strData);
                        vecData.Add(strData);

                        strData = "CondenserType" + chTab + strCondenserTypeName + chTab
                            + "KI1" + chTab + drv["KI1"].ToString() + chTab
                            //+ "КИn80" + chTab + drv["KIn80"].ToString() + chTab
                            //+ "КИn90" + chTab + drv["KIn90"].ToString() + chTab
                            //+ "КИn100" + chTab + drv["KIn100"].ToString() + chTab
                            + "KoefA" + chTab + drv["KoefA"].ToString() + chTab
                            + "KoefB" + chTab + drv["KoefB"].ToString() + chTab
                            + "KoefR2" + chTab + drv["KoefR2"].ToString() + chTab
                            + "FunctionType" + chTab + drv["FunctionType"].ToString();
                        //sw.WriteLine(strData);
                        vecData.Add(strData);

                        strData = "KIn";
                        //sw.WriteLine(strData);
                        vecData.Add(strData);

                        long iKoefID = Convert.ToInt64(drv["KoefID"]);

                        try
                        {
                            SQLiteCommand com = new SQLiteCommand(connection);
                            com.CommandText = "select ID, Position, Value, Conclusion, Recommendation FROM ParameterRecommendations WHERE KoefID = @KoefID ORDER BY Position";
                            com.CommandType = CommandType.Text;

                            SQLiteParameter param1 = new SQLiteParameter("@koefID", DbType.Int64);
                            param1.Value = (long)iKoefID;
                            com.Parameters.Add(param1);

                            SQLiteDataReader drRecomm = com.ExecuteReader();

                            // заносим данные по рекомендациям и КЦn
                            while (drRecomm.Read())
                            {
                                Decimal? fValue = null;
                                if (drRecomm["Value"] != DBNull.Value) fValue = Convert.ToDecimal(drRecomm["Value"]);

                                strData = "Pos" + chTab + Convert.ToInt64(drRecomm["Position"]) + chTab
                                    + "Val" + chTab + fValue.ToString() + chTab
                                    + "Recommendation" + chTab + Convert.ToString(drRecomm["Recommendation"]) + chTab
                                    + "Conclusion" + chTab + Convert.ToString(drRecomm["Conclusion"]);
                                //sw.WriteLine(strData);
                                vecData.Add(strData);
                            }
                            drRecomm.Close();
                        }
                        catch (SQLiteException ex)
                        {
                            MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                //sw.Close();
                connection.Close();

                try
                {
                    byte[] key = Encoding.Unicode.GetBytes("ELCHROM");
                    RC4 encoder = new RC4(key);

                    System.IO.StreamWriter fsWrite = new System.IO.StreamWriter(strPath);
                    for (int i = 0; i < vecData.Count; i++)
                    {
                        byte[] testBytes = Encoding.Unicode.GetBytes(vecData[i]);
                        byte[] result = encoder.Encode(testBytes, testBytes.Length);
                        string encryptedString = encoder.GetByteString(result);

                        fsWrite.WriteLine(encryptedString);
                    }
                    fsWrite.Close();
                }
                catch(Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MyLocalizer.XtraMessageBoxShow("Экспорт настроек успешно выполнен.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void RefreshGridPos(long id)
        {
            int f_row = GridView.FocusedRowHandle;
            if (id <= 0)
            {
                if (f_row > 0) f_row--;
                this.qCondenserTypeParametersTableAdapter.Fill(this.dataSetQuery.QCondenserTypeParameters);
                if (GridView.RowCount > f_row)
                {
                    GridView.ClearSelection();
                    GridView.SelectRow(f_row);
                    GridView.FocusedRowHandle = f_row;
                }
            }
            else
            {
                this.qCondenserTypeParametersTableAdapter.Fill(this.dataSetQuery.QCondenserTypeParameters);

                for (int i = 0; i < GridView.RowCount/*this.dataSetQuery.QCondensers.Rows.Count*/; i++)
                {
                    //DataRow r = this.dataSetQuery.QCondensers.Rows[i];
                    //int id_ = Convert.ToInt64(r["CondenserID"]);
                    long id_ = Convert.ToInt64(GridView.GetRowCellValue(i, "CondenserTypeID"));
                    if (id_ == id)
                    {
                        GridView.ClearSelection();
                        GridView.SelectRow(i);
                        GridView.FocusedRowHandle = i;
                        return;
                    }
                }
            }
        }

        private void UpdateRecord()
        {
            try
            {
                if (GridView.FocusedRowHandle < 0)
                {
                    MyLocalizer.XtraMessageBoxShow("Необходимо выбрать запись.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DataRowView drv = (DataRowView)(qCondenserTypeParametersBindingSource.Current);

                ParameterRecordForm f = new ParameterRecordForm();
                f.m_iCondenserTypeID = drv.Row["CondenserTypeID"];
                f.m_iKoefID = drv.Row["KoefID"];
                if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    RefreshGridPos((long)f.m_iCondenserTypeID);
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

        private void bEdit_Click(object sender, EventArgs e)
        {
            UpdateRecord();
        }

        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            UpdateRecord();
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            if (GridView.FocusedRowHandle < 0)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо выбрать запись.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DataRowView drv = (DataRowView)(qCondenserTypeParametersBindingSource.Current);

            if (MyLocalizer.XtraMessageBoxShow("Очистить коэффициенты у выбранного типа конденсатора?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                try
                {
                    SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    connection.Open();
                    SQLiteCommand com = new SQLiteCommand(connection);
                    com.CommandText = "DELETE FROM CondenserTypeParameters WHERE CondenserTypeID = @ctypeid";
                    com.CommandType = CommandType.Text;
                    SQLiteParameter param1 = new SQLiteParameter("@ctypeid", DbType.Int64);
                    param1.Value = (long)drv.Row["CondenserTypeID"];
                    com.Parameters.Add(param1);

                    com.ExecuteNonQuery();
                    connection.Close();

                    RefreshGridPos((long)drv.Row["CondenserTypeID"]);

                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
    }
}