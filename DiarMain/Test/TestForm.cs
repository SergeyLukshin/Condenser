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
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;

namespace Condenser
{
    public partial class TestForm : DevExpress.XtraEditors.XtraForm
    {
        long m_CondenserID = -1;
        //private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string, int>();
        Dictionary<int, int> dictBandHeigths = new Dictionary<int, int>();
        int m_singleLineHeight = 0;
        SQLiteConnection m_con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
        bool m_bDataLoadEnd = false;
        BindingList<DataSourceString> listTestType = new BindingList<DataSourceString>();
        float DpiXRel;
        float DpiYRel;

        public TestForm(long condenserID)
        {
            m_CondenserID = condenserID;
            InitializeComponent();
        }

        private void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;
                
            this.qCondenserTestTableAdapter.Fill(this.dataSetQuery.QCondenserTest, m_CondenserID);

            listTestType.Add(new DataSourceString(1, "ресурсные"));
            listTestType.Add(new DataSourceString(2, "приемо-сдаточные"));
            listTestType.Add(new DataSourceString(3, "эксплуатация"));
            repTestType.DataSource = listTestType;
            repTestType.DisplayMember = "VAL";
            repTestType.ValueMember = "KEY";
            repTestType.DropDownRows = listTestType.Count;

            Graphics formGraphics = this.CreateGraphics();

            DpiXRel = formGraphics.DpiX / 96.0f;
            DpiYRel = formGraphics.DpiY / 96.0f;

            SQLiteConnection connection_ = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
            connection_.Open();

            SQLiteCommand com_ = new SQLiteCommand(connection_);
            com_.CommandText = "SELECT c.CondenserNumber, ct.CondenserTypeName FROM Condensers AS c " +
                "INNER JOIN CondenserTypes AS ct ON c.CondenserTypeID = ct.CondenserTypeID WHERE c.CondenserID = @id";
            SQLiteParameter param1 = new SQLiteParameter("@id", DbType.Int64);
            param1.Value = m_CondenserID;
            com_.Parameters.Add(param1);
            com_.CommandType = CommandType.Text;
            SQLiteDataReader dr_ = com_.ExecuteReader();

            string strCondenserTypeName = "", strCondenserNumber = "";

            while (dr_.Read())
            {
                strCondenserTypeName = Convert.ToString(dr_["CondenserTypeName"]);
                strCondenserNumber = Convert.ToString(dr_["CondenserNumber"]);
            }
            dr_.Close();

            connection_.Close();

            teEquipmentNumber.EditValue = strCondenserNumber;
            teEquipmentType.EditValue = strCondenserTypeName;

            /*foreach (GridColumn col in GridView.Columns)
            {
                GetColumnBestHeight(col);
            }
            SetMaxColumnHeights();*/

            ControlNavigatorButtons cnb = controlNavigator1.Buttons;
            if (GridView.FocusedRowHandle < 0)
            {
                cnb.CustomButtons[0].Enabled = false;// cnb.Remove.Enabled;
                cnb.CustomButtons[1].Enabled = false;// cnb.Remove.Enabled;
            }
            else
            {
                cnb.CustomButtons[0].Enabled = true;
                cnb.CustomButtons[1].Enabled = true;
            }

            if (!Program.m_bExpertMode)
            {
                bandCH3OH.Visible = false;
                bandCO2_CO.Visible = false;
            }

            for (int i = 0; i < GridView.Bands.Count; i++)
            {
                GridView.Bands[i].Width = (int)(GridView.Bands[i].Width * formGraphics.DpiX / 96);
            }

            //BandedGridViewInfo viewInfo = GridView.GetViewInfo() as BandedGridViewInfo;
            Size captionSize = GridView.Appearance.BandPanel.CalcTextSize(formGraphics, "www", 1000).ToSize();
            m_singleLineHeight = captionSize.Height;// viewInfo.BandRowHeight;
            //DevExpress.Utils.AppearanceObject[] styles = {GridView.Appearance.BandPanel};
            //m_singleLineHeight = viewInfo.CalcMaxHeight(styles);
            //int ii = viewInfo.ColumnRowHeight;

            SetBandsHeight();

            colCondenserState.MaxWidth = (int)(30 * DpiXRel);
            colCondenserState.MinWidth = (int)(30 * DpiXRel);

            m_bDataLoadEnd = true;
        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ControlNavigatorButtons cnb = controlNavigator1.Buttons;
            if (e.FocusedRowHandle < 0)
            {
                cnb.CustomButtons[0].Enabled = false;// cnb.Remove.Enabled;
                cnb.CustomButtons[1].Enabled = false;
            }
            else
            {
                cnb.CustomButtons[0].Enabled = true;
                cnb.CustomButtons[1].Enabled = true;
            }
        }

        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                DeleteRecord();
            }

            if (e.KeyCode == Keys.Enter)
            {
                UpdateRecord();
            }

            if (!GridView.IsEditorFocused)
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
            }
        }

        private void GridView_DoubleClick(object sender, EventArgs e)
        {
            UpdateRecord();
        }

        private void RefreshGridPos(long id)
        {
            int f_row = GridView.FocusedRowHandle;
            if (id <= 0)
            {
                if (f_row > 0) f_row--;

                this.qCondenserTestTableAdapter.Fill(this.dataSetQuery.QCondenserTest, m_CondenserID);
                //RefreshData();
                if (GridView.RowCount > f_row)
                {
                    GridView.ClearSelection();
                    GridView.SelectRow(f_row);
                    GridView.FocusedRowHandle = f_row;
                }
            }
            else
            {
                this.qCondenserTestTableAdapter.Fill(this.dataSetQuery.QCondenserTest, m_CondenserID);
                //RefreshData();

                for (int i = 0; i < GridView.RowCount; i++)
                {
                    //DataRow r = this.dataSetQuery.QEquipments.Rows[i];
                    //int id_ = Convert.ToInt64(r["EquipmentID"]);
                    long id_ = Convert.ToInt64(GridView.GetRowCellValue(i, "CondenserTestID"));
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

        private void DeleteRecord()
        {
            try
            {
                if (GridView.FocusedRowHandle < 0) return;

                DataRowView drv = (DataRowView)(this.qCondenserTestBindingSource.Current);

                if (MyLocalizer.XtraMessageBoxShow("Удалить испытание?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    long id = Convert.ToInt64(drv.Row["CondenserTestID"]);
                    long condenser_id = Convert.ToInt64(drv.Row["CondenserID"]);
                    long condenser_test_type = Convert.ToInt64(drv.Row["CondenserTestType"]);

                    SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    connection.Open();
                    SQLiteCommand com = new SQLiteCommand(connection);
                    com.CommandType = CommandType.Text;

                    SQLiteParameter param1 = new SQLiteParameter("@id", DbType.Int64);
                    param1.Value = id;
                    com.Parameters.Add(param1);

                    SQLiteParameter param2 = new SQLiteParameter("@cid", DbType.Int64);
                    param2.Value = condenser_id;
                    com.Parameters.Add(param2);

                    if (condenser_test_type == (long)CondenserTest.CondenserTestType.Acceptance)
                    {
                        com.CommandText = "SELECT COUNT(*) AS Cnt FROM CondenserTest AS ct WHERE ct.CondenserID = @cid AND CondenserTestType <> 2 ";
                        SQLiteDataReader dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            if (Convert.ToInt64(dr["Cnt"]) > 0)
                            {
                                MyLocalizer.XtraMessageBoxShow("Данное испытание удалить невозможно, т.к. существуют эксплуатационные испытания этого конденсатора.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                dr.Close();
                                connection.Close();
                                return;
                            }
                        }
                        dr.Close();
                    }

                    com.CommandText = "DELETE FROM CondenserTestSheduler WHERE CondenserTestID = @id";
                    com.ExecuteNonQuery();

                    com.CommandText = "DELETE FROM CondenserTest WHERE CondenserTestID = @id";
                    com.ExecuteNonQuery();

                    /*SQLiteParameter param2 = new SQLiteParameter("@cid", DbType.Int64);
                    param2.Value = condenser_id;
                    com.Parameters.Add(param2);*/

                    com.CommandText = "UPDATE Condensers SET CondenserState = COALESCE((SELECT CondenserState FROM CondenserTest " +
                            "WHERE CondenserTest.CondenserID = @cid ORDER BY CondenserTestDate DESC LIMIT 1), 0), " + 
                        "CondenserTestType = (SELECT CondenserTestType FROM CondenserTest " +
                            "WHERE CondenserTest.CondenserID = @cid ORDER BY CondenserTestDate DESC LIMIT 1) " +
                        "WHERE CondenserID = @cid";
                    com.ExecuteNonQuery();

                    connection.Close();

                    RefreshGridPos(-1);
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

        public void InsertRecord()
        {
            TestDataForm form = new TestDataForm(m_CondenserID, 0);

            if (!form.VerifyOtherTest(m_CondenserID))
                return;

            form.m_bShowContinueMsg = false;
            DialogResult dr = form.ShowDialog(this);
            long id = form.m_CondenserTestID;
            if (dr == System.Windows.Forms.DialogResult.OK)
                RefreshGridPos(id);
        }

        public void UpdateRecord()
        {
            if (GridView.FocusedRowHandle < 0) return;

            DataRowView drv = (DataRowView)(this.qCondenserTestBindingSource.Current);
            long id = Convert.ToInt64(drv.Row["CondenserTestID"]);
            CondenserTest.CondenserTestType CondenserTestType = (CondenserTest.CondenserTestType)Convert.ToInt64(drv.Row["CondenserTestType"]);

            TestDataForm form = new TestDataForm(m_CondenserID, id, CondenserTestType);
            form.m_bShowContinueMsg = false;
            DialogResult dr = form.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
                RefreshGridPos(id);
        }

        private void controlNavigator1_ButtonClick(object sender, NavigatorButtonClickEventArgs e)
        {
            if (e.Button.ButtonType == NavigatorButtonType.Append)
            {
                e.Handled = true;
                InsertRecord();
                return;
            }
            if (e.Button.ButtonType == NavigatorButtonType.Custom)
            {
                if (e.Button.ImageIndex == 4)
                {
                    e.Handled = true;
                    UpdateRecord();
                }
                else
                {
                    if (e.Button.ImageIndex == 5)
                    {
                        if (GridView.FocusedRowHandle < 0)
                        {
                            MyLocalizer.XtraMessageBoxShow("Необходимо указать конденсатор.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        DataRowView drv = (DataRowView)(this.qCondenserTestBindingSource.Current);
                        long id = Convert.ToInt64(drv.Row["CondenserID"]);
                        long test_id = Convert.ToInt64(drv.Row["CondenserTestID"]);

                        WaitingForm wf = new WaitingForm();
                        wf.m_CondenserID = id;
                        wf.m_CondenserTestID = test_id;
                        wf.ShowDialog(this);

                        if (wf.m_Word != null)
                        {
                            wf.m_Word.SetVisible(true);
                            wf.m_Word.DestroyWord();
                        }
                    }
                }
                return;
            }
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                e.Handled = true;
                DeleteRecord();
                return;
            }
        }

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

            /*foreach (GridColumn col in GridView.Columns)
            {
                GetColumnBestHeight(col);
            }
            SetMaxColumnHeights();*/
        }

        private void GridView_BandWidthChanged(object sender, BandEventArgs e)
        {
            if (e.Band.ParentBand == null)
                SetBandsHeight(e.Band.Index);
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

        private void SetBandsHeight(int index = -1)
        {
            try
            {
                GridView.BeginUpdate();

                for (int i = 0; i < GridView.Bands.Count; i++)
                {
                    if (index >= 0 && i != index) continue;

                    int height = GetColumnBestHeight(GridView.Bands[i], GridView.Bands[i].Width);

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
                    if (GridView.Bands[i].Children.Count > 0)
                        GridView.Bands[i].RowCount = iRowCount - 1;
                    else
                        GridView.Bands[i].RowCount = iRowCount;
                }

                GridView.EndUpdate();
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridView_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "CondenserState")
            {
                Rectangle rect = e.Bounds;
                int diametr = e.Bounds.Height - 4;
                rect.X = e.Bounds.X + e.Bounds.Width / 2 - diametr / 2;
                rect.Y = e.Bounds.Y + e.Bounds.Height / 2 - diametr / 2;
                rect.Width = rect.Height = diametr;

                DataRow row = GridView.GetDataRow(e.RowHandle);
                if (Convert.ToInt64(row["CondenserState"]) == 0)
                    e.Graphics.FillEllipse(new SolidBrush(Color.LightGreen), rect);
                else
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect);
                e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.Black)), rect);
                e.Handled = true;
            }
        }

        private void toolTip_GetActiveObjectInfo(object sender, DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == GridGC)
            {
                ToolTipControlInfo info = null;
                //Get the view at the current mouse position
                BandedGridView view = GridGC.GetViewAt(e.ControlMousePosition) as BandedGridView;
                if (view == null) return;
                //Get the view's element information that resides at the current position
                GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
                //Display a hint for row indicator cells
                if (hi.HitTest == GridHitTest.RowIndicator || hi.HitTest == GridHitTest.RowCell && hi.Column.FieldName == "CondenserState")
                {
                    //An object that uniquely identifies a row indicator cell
                    object o = hi.HitTest.ToString() + hi.RowHandle.ToString();
                    string text = "";

                    DataRow row = GridView.GetDataRow(hi.RowHandle);
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
    }
}