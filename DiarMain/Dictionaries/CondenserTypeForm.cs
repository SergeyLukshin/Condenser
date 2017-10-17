using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.SQLite;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraGrid.Drawing;

namespace Condenser
{
    public partial class CondenserTypeForm : DevExpress.XtraEditors.XtraForm
    {
        bool m_bAcceptChanges = true;
        bool m_bUpdateID = false;
        public bool m_bCanSelect = false;
        public long m_SelectID = 0;
        Dictionary<int, int> dictBandHeigths = new Dictionary<int, int>();
        Dictionary<int, int> dictChildBandHeigths = new Dictionary<int, int>();
        int m_singleBandLineHeight = 0;
        int m_singleChildBandLineHeight = 0;
        float DpiXRel;
        float DpiYRel;
        bool m_bNeedCancel = false;

        BindingList<DataSourceString> listDielectricType = new BindingList<DataSourceString>();
        BindingList<DataSourceString> listCasingType = new BindingList<DataSourceString>();
        BindingList<DataSourceString> listStatus = new BindingList<DataSourceString>();

        public CondenserTypeForm()
        {
            InitializeComponent();
        }

        private void CondenserTypeForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;


            string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (strSeparator == ".") strSeparator = "\\.";
            this.repDecimal3.Mask.EditMask = "(\\d{1,6}|\\d{1,6}" + strSeparator + "\\d{1,3})";

            // TODO: This line of code loads data into the 'dataSetQuery.QInsulatingLiquidTypes' table. You can move, or remove it, as needed.
            this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);
            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypes' table. You can move, or remove it, as needed.
            this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);
            // TODO: This line of code loads data into the 'dataSetQuery1.QCondenserTypes' table. You can move, or remove it, as needed.

            this.dataSetQuery.QCondenserTypes.QCondenserTypesRowDeleting += new DataSetQuery.QCondenserTypesRowChangeEventHandler(QCondenserTypes_QCondenserTypesRowDeleting);
            this.dataSetQuery.QCondenserTypes.QCondenserTypesRowDeleted += new DataSetQuery.QCondenserTypesRowChangeEventHandler(QCondenserTypes_QCondenserTypesRowDeleted);
            this.dataSetQuery.QCondenserTypes.QCondenserTypesRowChanged += new DataSetQuery.QCondenserTypesRowChangeEventHandler(QCondenserTypes_QCondenserTypesRowChanged);
            GridView.OptionsBehavior.Editable = false;

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

            listStatus.Add(new DataSourceString(0, "Не зарегистрирован"));
            listStatus.Add(new DataSourceString(1, "Зарегистрирован"));
            repStatus.DataSource = listStatus;
            repStatus.DisplayMember = "VAL";
            repStatus.ValueMember = "KEY";
            repStatus.DropDownRows = listStatus.Count;

            repInsulatingLiquidType.DropDownRows = this.dataSetQuery.QInsulatingLiquidTypes.Count;

            try
            {
                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "select CondenserTypeID, KoefA, KoefB from CondenserTypeParameters";
                com.CommandType = CommandType.Text;
                SQLiteDataReader dr = com.ExecuteReader();

                while (dr.Read())
                {
                    long CondenserTypeID = Convert.ToInt64(dr["CondenserTypeID"]);
                    if (dr["KoefA"] != DBNull.Value && dr["KoefB"] != DBNull.Value)
                    {
                        for (int i = 0; i < this.dataSetQuery.QCondenserTypes.Count; i++)
                        {
                            DataRow r = this.dataSetQuery.QCondenserTypes.Rows[i];
                            long CondenserTypeID_ = Convert.ToInt64(r["CondenserTypeID"]);
                            if (CondenserTypeID == CondenserTypeID_)
                            {
                                r["Status"] = (long)1;
                                break;
                            }
                        }
                    }
                }
                dr.Close();

                connection.Close();
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (m_bCanSelect)
            {
                cbCanEdit.Checked = true;
                panelSelect.Visible = true;
            }
            else
            {
                panelSelect.Visible = false;
            }

            Graphics formGraphics = this.CreateGraphics();
            DpiXRel = formGraphics.DpiX / 96.0f;
            DpiYRel = formGraphics.DpiY / 96.0f;

            for (int i = 0; i < GridView.Bands.Count; i++)
            {
                GridView.Bands[i].Width = (int)(GridView.Bands[i].Width * formGraphics.DpiX / 96);
            }

            Size captionSize = gridBand1.AppearanceHeader.CalcTextSize(formGraphics, "www", 1000).ToSize();
            m_singleBandLineHeight = captionSize.Height;// viewInfo.BandRowHeight;
            captionSize = GridView.Appearance.BandPanel.CalcTextSize(formGraphics, "www", 1000).ToSize();
            m_singleChildBandLineHeight = captionSize.Height;// viewInfo.BandRowHeight;

            SetBandsHeight();
        }

        void QCondenserTypes_QCondenserTypesRowChanged(object sender, DataSetQuery.QCondenserTypesRowChangeEvent e)
        {
            if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
            {
                if (m_bNeedCancel)
                {
                    m_bNeedCancel = false;
                    e.Row.RejectChanges();
                    return;
                }

                /*if (!m_bAcceptChanges)
                    e.Row.RejectChanges();
                else
                {*/
                    try
                    {
                        if (e.Action == DataRowAction.Change && m_bUpdateID)
                        {
                            this.dataSetQuery.QCondenserTypes.AcceptChanges();
                            m_bUpdateID = false;
                            return;
                        }
                        else
                            using (var cmdBuilder = new SQLiteCommandBuilder(this.qCondenserTypesTableAdapter.Adapter)) this.qCondenserTypesTableAdapter.Adapter.Update(this.dataSetQuery.QCondenserTypes);

                        if (e.Action == DataRowAction.Add)
                        {
                            SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                            connection.Open();
                            SQLiteCommand com = new SQLiteCommand(connection);
                            com.CommandText = "select seq from sqlite_sequence where name = 'CondenserTypes'";
                            com.CommandType = CommandType.Text;
                            SQLiteDataReader dr = com.ExecuteReader();
                            
                            long id = 0;
                            while (dr.Read())
                            {
                                id = Convert.ToInt64(dr["seq"]);
                            }
                            dr.Close();

                            m_bUpdateID = true;
                            ((DataRowView)(qCondenserTypesBindingSource.Current)).Row["CondenserTypeID"] = id;

                            connection.Close();
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
                //}
            }
        }

        void QCondenserTypes_QCondenserTypesRowDeleted(object sender, DataSetQuery.QCondenserTypesRowChangeEvent e)
        {
            if (e.Action == DataRowAction.Delete)
            {
                if (!m_bAcceptChanges)
                {
                    e.Row.RejectChanges();
                }
                else
                {
                    try
                    {
                        using (var cmdBuilder = new SQLiteCommandBuilder(this.qCondenserTypesTableAdapter.Adapter)) this.qCondenserTypesTableAdapter.Adapter.Update(this.dataSetQuery.QCondenserTypes);
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

        void QCondenserTypes_QCondenserTypesRowDeleting(object sender, DataSetQuery.QCondenserTypesRowChangeEvent e)
        {
            try
            {
                if (e.Action == DataRowAction.Delete)
                {
                    if (MyLocalizer.XtraMessageBoxShow("Удалить запись?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        m_bAcceptChanges = false;
                        return;
                    }
                    else
                    {
                        long id = Convert.ToInt64(e.Row["CondenserTypeID"]);

                        SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                        connection.Open();
                        SQLiteCommand com = new SQLiteCommand(connection);
                        com.CommandText = "Select COUNT(*) AS Cnt from Condensers AS c WHERE c.CondenserTypeID = ?";
                        com.CommandType = CommandType.Text;
                        SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.Int64);
                        param1.Value = id;
                        com.Parameters.Add(param1);
                        SQLiteDataReader dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            if (Convert.ToInt64(dr["Cnt"]) > 0)
                            {
                                MyLocalizer.XtraMessageBoxShow("Существуют конденсаторы данного типа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                m_bAcceptChanges = false;
                                dr.Close();
                                connection.Close();
                                return;
                            }
                        }
                        dr.Close();

                        com.CommandText = "Delete from ParameterRecommendations WHERE KoefID IN (SELECT KoefID FROM CondenserTypeParameters WHERE CondenserTypeID = ?)";
                        com.ExecuteNonQuery();
                        com.CommandText = "Delete from CondenserTypeParameters WHERE CondenserTypeID = ?";
                        com.ExecuteNonQuery();

                        m_bAcceptChanges = true;

                        connection.Close();
                    }
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

        private void cbCanEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCanEdit.Checked) GridView.OptionsBehavior.Editable = true;
            else GridView.OptionsBehavior.Editable = false;
        }

        private void SubjectView_KeyDown(object sender, KeyEventArgs e)
        {
            if (cbCanEdit.Checked)
            {
                if (e.KeyCode == Keys.Delete && qCondenserTypesBindingSource.Current != null)
                {
                    ((DataRowView)(qCondenserTypesBindingSource.Current)).Row.Delete();
                }
            }

            if (!GridView.IsEditorFocused)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }
        }

        private void SubjectView_ShowingEditor(object sender, CancelEventArgs e)
        {
        }

        private void SubjectView_DoubleClick(object sender, EventArgs e)
        {
        }

        private void GridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                long id = 0;

                if (qCondenserTypesBindingSource.Current == null) return;

                DataRowView row = (DataRowView)(qCondenserTypesBindingSource.Current);

                System.Windows.Forms.DialogResult res = MyLocalizer.XtraMessageBoxShow("Сохранить данные?", "Сообщение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.ErrorText = "";
                    e.Valid = false;
                    return;
                }
                if (res == System.Windows.Forms.DialogResult.No)
                {
                    m_bNeedCancel = true;
                    return;
                }

                /*if (MyLocalizer.XtraMessageBoxShow("Сохранить данные?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.ErrorText = "";
                    e.Valid = false;
                    return;
                }*/

                if (!row.IsNew)
                {
                    id = Convert.ToInt64(row["CondenserTypeID"]);
                }

                string strName = row["CondenserTypeName"].ToString();
                strName = strName.Trim();
                if (strName == "")
                {
                    e.ErrorText = "Необходимо указать наименование типа конденсатора.";
                    e.Valid = false;
                    return;
                }

                if (row["InsulatingLiquidTypeID"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать марку изоляционной жидкости.";
                    e.Valid = false;
                    return;
                }

                if (row["DielectricType"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать тип диэлектрика.";
                    e.Valid = false;
                    return;
                }

                if (row["DielectricThickness"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать толщину диэлектрика.";
                    e.Valid = false;
                    return;
                }
                else
                {
                    if (Convert.ToDecimal(row["DielectricThickness"]) <= 0 || Convert.ToDecimal(row["DielectricThickness"]) > 1000)
                    {
                        e.Valid = false;
                        e.ErrorText = "Значение толщины диэлектрика должно находиться между значениями 0 и 1000 мкм";
                        return;
                    }
                }

                if (row["TangentAngle"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать тангенс угла диэлектрических потерь.";
                    e.Valid = false;
                    return;
                }
                else
                {
                    Decimal val = Convert.ToDecimal(row["TangentAngle"]);
                    if (val < new Decimal(0.001) || val > 10)
                    {
                        e.ErrorText = "Значение тангенса диэлектрических потерь должно находиться между значениями 0.001 и 10.";
                        e.Valid = false;
                        return;
                    }
                }

                if (row["DielectricInductiveCapacity"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать относительную диэлектрическую проницаемость.";
                    e.Valid = false;
                    return;
                }
                else
                {
                    Decimal val = Convert.ToDecimal(row["DielectricInductiveCapacity"]);
                    if (val < new Decimal(0.1) || val > 100000)
                    {
                        e.ErrorText = "Значение относительной диэлектрической проницаемости должно находиться между значениями 0.1 и 100000.";
                        e.Valid = false;
                        return;
                    }
                }
                if (row["CasingType"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать тип обкладок.";
                    e.Valid = false;
                    return;
                }

                if (row["CasingThickness"] == DBNull.Value)
                {
                    e.ErrorText = "Необходимо указать толщину обкладки.";
                    e.Valid = false;
                    return;
                }
                else
                {
                    if (Convert.ToDecimal(row["CasingThickness"]) <= 0 || Convert.ToDecimal(row["CasingThickness"]) > 1000)
                    {
                        e.Valid = false;
                        e.ErrorText = "Значение толщины обкладки должно находиться между значениями 0 и 1000 мкм";
                        return;
                    }
                }

                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "Select * from CondenserTypes WHERE EQUAL_STR(CondenserTypeName, ?) = 0 AND CondenserTypeID <> ?";
                com.CommandType = CommandType.Text;
                SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.String);
                param1.Value = strName;
                SQLiteParameter param2 = new SQLiteParameter("@Param2", DbType.Int64);
                param2.Value = id;
                com.Parameters.Add(param1);
                com.Parameters.Add(param2);
                SQLiteDataReader dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    e.ErrorText = "Тип конденсатора с таким наименованием уже существует.";
                    e.Valid = false;
                    dr.Close();
                    connection.Close();
                    return;
                }
                dr.Close();
                connection.Close();
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

        private void bSelect_Click(object sender, EventArgs e)
        {
            if (qCondenserTypesBindingSource.Current != null)
            {
                m_SelectID = Convert.ToInt64(((DataRowView)(qCondenserTypesBindingSource.Current)).Row["CondenserTypeID"]);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо выбрать запись", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private int GetColumnBestHeight(GridBand column, int width, DevExpress.Utils.AppearanceObject app)
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
                ex.SetAppearance(app);
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

        private void SetBandsHeight(int index = -1, int child_index = -1)
        {
            try
            {
                GridView.BeginUpdate();

                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    if (index >= 0 && i != index) continue;

                    int height = GetColumnBestHeight(GridView.Bands[i], GridView.Bands[i].Width, GridView.Bands[i].AppearanceHeader);
                    int iRowCount = height / m_singleBandLineHeight;
                    if (iRowCount * m_singleBandLineHeight < height) iRowCount++;

                    dictBandHeigths[i] = iRowCount;

                    for (int j = 0; j < GridView.Bands[i].Children.Count; j++)
                    {
                        if (child_index >= 0 && j != child_index) continue;

                        height = GetColumnBestHeight(GridView.Bands[i].Children[j], GridView.Bands[i].Children[j].Width, GridView.Appearance.BandPanel);
                        iRowCount = height / m_singleChildBandLineHeight;
                        if (iRowCount * m_singleChildBandLineHeight < height) iRowCount++;

                        dictChildBandHeigths[j] = iRowCount;
                    }
                }

                int max_child_row = 0;
                foreach (KeyValuePair<int, int> pair in dictChildBandHeigths)
                {
                    if (pair.Value > max_child_row) max_child_row = pair.Value;
                }

                int max_row = 0;
                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    if (GridView.Bands[i].Children.Count == 0)
                    {
                        if (dictBandHeigths[i] > max_row) max_row = dictBandHeigths[i];
                    }
                    else
                    {
                        if (dictBandHeigths[i] + max_child_row > max_row) max_row = dictBandHeigths[i] + max_child_row;
                    }
                }

                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    for (int j = 0; j < GridView.Bands[i].Children.Count; j++)
                    {
                        GridView.Bands[i].Children[j].RowCount = max_child_row;
                    }

                    if (GridView.Bands[i].Children.Count == 0) GridView.Bands[i].RowCount = max_row;
                    else GridView.Bands[i].RowCount = max_row - max_child_row;

                    if (GridView.Bands[i].RowCount <= 1) GridView.Bands[i].RowCount = 2;
                }

                GridView.EndUpdate();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridView_BandWidthChanged(object sender, DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs e)
        {
            if (e.Band.ParentBand == null)
                SetBandsHeight(e.Band.Index);
            else
                SetBandsHeight(e.Band.ParentBand.Index, e.Band.Index);
        }

        private void GridGC_Click(object sender, EventArgs e)
        {

        }

        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (cbCanEdit.Checked)
            {
                if (e.KeyCode == Keys.Delete && qCondenserTypesBindingSource.Current != null)
                {
                    ((DataRowView)(qCondenserTypesBindingSource.Current)).Row.Delete();
                }
            }

            if (!GridView.IsEditorFocused)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }
        }

        private void GridView_InvalidRowException_1(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            if (e.ErrorText != "")
                MyLocalizer.XtraMessageBoxShow(e.ErrorText, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void repInsulatingLiquidType_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (e.Button.Kind == DevExpress.XtraEditors.Controls.ButtonPredefines.Ellipsis)
            {
                string FieldName = "InsulatingLiquidTypeID";

                InsulatingLiquidTypeForm f = new InsulatingLiquidTypeForm();
                f.m_bCanSelect = true;
                DialogResult res = f.ShowDialog(this);

                this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);
                if (this.dataSetQuery.QInsulatingLiquidTypes.Count > 7)
                    repInsulatingLiquidType.DropDownRows = 7;
                else
                    repInsulatingLiquidType.DropDownRows = this.dataSetQuery.QInsulatingLiquidTypes.Count;

                if (res == System.Windows.Forms.DialogResult.OK)
                {
                    GridView.BeginUpdate();
                    DataRowView drv = (DataRowView)(this.qCondenserTypesBindingSource.Current);
                    drv.Row[FieldName] = f.m_SelectID;
                    GridView.EndUpdate();
                }
            }
        }

        private void rep_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back))
            {
                if (!((LookUpEdit)sender).Properties.ReadOnly)
                {
                    ((LookUpEdit)sender).ClosePopup();
                    ((LookUpEdit)sender).EditValue = null;
                }
            }
        }

        private void repDecimal3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ",")
                if (e.KeyChar == '.') e.KeyChar = ',';

            if (System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator == ".")
                if (e.KeyChar == ',') e.KeyChar = '.';
        }

        private void CondenserTypeForm_SizeChanged(object sender, EventArgs e)
        {

        }

        private void GridView_ValidatingEditor(object sender, DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() == "") e.Value = DBNull.Value;
        }

        private void GridView_ShowingEditor(object sender, CancelEventArgs e)
        {

        }

        private void GridView_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            ((DataRowView)(qCondenserTypesBindingSource.Current)).Row["Status"] = 0;
        }

        private void qCondenserTypesBindingSource_AddingNew(object sender, AddingNewEventArgs e)
        {
        }
       
    }
}