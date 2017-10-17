using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
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
    public partial class CondenserForm : DevExpress.XtraEditors.XtraForm
    {
        private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string, int>();

        float DpiXRel;
        float DpiYRel;
        BindingList<DataSourceString> listTestType = new BindingList<DataSourceString>();

        public CondenserForm()
        {
            InitializeComponent();
        }

        private void CondenserForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetQuery.QCondensers' table. You can move, or remove it, as needed.
            this.qCondensersTableAdapter.Fill(this.dataSetQuery.QCondensers);
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            // TODO: данная строка кода позволяет загрузить данные в таблицу "dataSetMain.EquipmentKinds". При необходимости она может быть перемещена или удалена.

            listTestType.Add(new DataSourceString(1, "ресурсные"));
            listTestType.Add(new DataSourceString(2, "приемо-сдаточные"));
            listTestType.Add(new DataSourceString(3, "эксплуатация"));
            repTestType.DataSource = listTestType;
            repTestType.DisplayMember = "VAL";
            repTestType.ValueMember = "KEY";
            repTestType.DropDownRows = listTestType.Count;

            ControlNavigatorButtons cnb = controlNavigator1.Buttons;
            if (GridView.FocusedRowHandle < 0)
            {
                cnb.CustomButtons[0].Enabled = false;// cnb.Remove.Enabled;
            }
            else
            {
                cnb.CustomButtons[0].Enabled = true;
            }

            try
            {
                Graphics gr = this.CreateGraphics();

                DpiXRel = gr.DpiX / 96.0f;
                DpiYRel = gr.DpiY / 96.0f;

                colCondenserState.MaxWidth = (int)(30 * DpiXRel);
                colCondenserState.MinWidth = (int)(30 * DpiXRel);

                /*foreach (GridColumn col in GridView.Columns)
                {
                    GetColumnBestHeight(col);
                }
                SetMaxColumnHeights();*/

                timer1.Enabled = true;
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
                GridView.ColumnPanelRowHeight = maxHeight;
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GridViewView_KeyDown(object sender, KeyEventArgs e)
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
                this.qCondensersTableAdapter.Fill(this.dataSetQuery.QCondensers);
                if (GridView.RowCount > f_row)
                {
                    GridView.ClearSelection();
                    GridView.SelectRow(f_row);
                    GridView.FocusedRowHandle = f_row;
                }
            }
            else
            {
                this.qCondensersTableAdapter.Fill(this.dataSetQuery.QCondensers);

                for (int i = 0; i < GridView.RowCount/*this.dataSetQuery.QCondensers.Rows.Count*/; i++)
                {
                    //DataRow r = this.dataSetQuery.QCondensers.Rows[i];
                    //int id_ = Convert.ToInt64(r["CondenserID"]);
                    long id_ = Convert.ToInt64(GridView.GetRowCellValue(i, "CondenserID"));
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

                DataRowView drv = (DataRowView)(qCondensersBindingSource.Current);

                if (MyLocalizer.XtraMessageBoxShow("Удалить конденсатор? Все испытания, связанные с данным конденсатором, будут удалены.", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                {
                    return;
                }
                else
                {
                    long id = Convert.ToInt64(drv.Row["CondenserID"]);

                    SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    connection.Open();
                    SQLiteCommand com = new SQLiteCommand(connection);
                    com.CommandType = CommandType.Text;

                    SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.Int64);
                    param1.Value = id;
                    com.Parameters.Add(param1);

                    com.CommandText = "DELETE FROM CondenserTestSheduler WHERE CondenserTestID IN (SELECT CondenserTestID FROM CondenserTest WHERE CondenserID = ?)";
                    com.ExecuteNonQuery();

                    com.CommandText = "DELETE FROM CondenserTest WHERE CondenserID = ?";
                    com.ExecuteNonQuery();

                    com.CommandText = "DELETE FROM Condensers WHERE CondenserID = ?";
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
            try
            {
                DialogResult dr = System.Windows.Forms.DialogResult.OK;

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    long id = -1;

                    PassportDataForm rf = new PassportDataForm(id, 0);
                    rf.m_bShowContinueMsg = false;
                    dr = rf.ShowDialog(this);
                    id = rf.m_id;
                    if (dr == System.Windows.Forms.DialogResult.OK)
                        RefreshGridPos(id);

                    /*if (rf.m_bContinueNext)
                    {
                        ShowTestForm(id, rf.m_CondenserTestID);
                    }*/
                }
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

        public void UpdateRecord()
        {
            if (GridView.FocusedRowHandle < 0) return;

            DataRowView drv = (DataRowView)(qCondensersBindingSource.Current);
            long id = Convert.ToInt64(drv.Row["CondenserID"]);

            try
            {
                PassportDataForm rf = new PassportDataForm(id, 0);
                rf.m_bShowContinueMsg = false;
                DialogResult dr = rf.ShowDialog(this);
                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    RefreshGridPos(id);
                    //if (rf.m_bContinueNext) ShowTestForm(id, rf.m_CondenserTestID);
                }
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            /*PassportDataForm rf = new PassportDataForm(id);
            DialogResult dr = rf.ShowDialog(this);
            if (dr == System.Windows.Forms.DialogResult.OK)
                RefreshGridPos(id);*/
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
                e.Handled = true;
                UpdateRecord();
                return;
            }
            if (e.Button.ButtonType == NavigatorButtonType.Remove)
            {
                e.Handled = true;
                DeleteRecord();
                return;
            }
        }

        private void GridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ControlNavigatorButtons cnb = controlNavigator1.Buttons;
            if (e.FocusedRowHandle < 0)
            {
                cnb.CustomButtons[0].Enabled = false;// cnb.Remove.Enabled;
            }
            else
            {
                cnb.CustomButtons[0].Enabled = true;
            }
        }

        private void GridView_ShowFilterPopupListBox(object sender, DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventArgs e)
        {
            //GridView gv = sender as GridView;
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
                //SetColumnHeight(e.Column);
                for (int i = e.Column.VisibleIndex; i < GridView.Columns.Count; i++)
                {
                    SetColumnHeight(GridView.Columns[i]);
                }
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolTip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl == GridGC)
            {
                ToolTipControlInfo info = null;
                //Get the view at the current mouse position
                GridView view = GridGC.GetViewAt(e.ControlMousePosition) as GridView;
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            foreach (GridColumn col in GridView.Columns)
            {
                GetColumnBestHeight(col);
            }
            SetMaxColumnHeights();
        }
    }
}