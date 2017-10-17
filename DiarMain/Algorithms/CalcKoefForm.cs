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
    public partial class CalcKoefForm : DevExpress.XtraEditors.XtraForm
    {
        class CondenserTestInfo
        {
            public CondenserTestInfo(Decimal CO, Decimal CO2, long CycleCount)
            {
                m_CO = CO;
                m_CO2 = CO2;
                m_CO2_CO = CO2 / CO;
                m_CycleCount = CycleCount;
            }

            public Decimal m_CO;
            public Decimal m_CO2;
            public Decimal m_CO2_CO;
            public long m_CycleCount;
            public PointF m_PointGraph1 = new PointF();
            public PointF m_PointGraph2 = new PointF();
        }

        class CondenserInfo
        {
            public CondenserInfo(long CondenserID, long CondenserTestType,
                    long CondenserState, string strCondenserNumber, long NormalizedResource)
            {
                m_CondenserID = CondenserID;
                m_CondenserTestType = CondenserTestType;
                m_CycleCount = 0;
                m_Use = false;
                m_maxCO2_CO = 0;
                m_CondenserState = CondenserState;
                m_strCondenserNumber = strCondenserNumber;
                m_NormalizedResource = NormalizedResource;
            }

            public void AddTest(Decimal CO, Decimal CO2, long CycleCount)
            {
                m_CycleCount += CycleCount;
                listTest.Add(new CondenserTestInfo(CO, CO2, m_CycleCount));

                if (m_maxCO2_CO < CO2 / CO) m_maxCO2_CO = CO2 / CO;
            }

            private long m_CondenserID;
            private long m_CondenserTestType;
            private long m_CycleCount = 0;
            private bool m_Use = false;
            private long m_CondenserState;
            private string m_strCondenserNumber;
            private long m_NormalizedResource;
            public List<CondenserTestInfo> listTest = new List<CondenserTestInfo>();
            private Decimal m_maxCO2_CO = 0;
            private Color m_Color = Color.Empty;

            public long CondenserID
            {
                get { return m_CondenserID; }
                set { m_CondenserID = value; }
            }

            public long CondenserTestType
            {
                get { return m_CondenserTestType; }
                set { m_CondenserTestType = value; }
            }

            public long CycleCount
            {
                get { return m_CycleCount; }
                set { m_CycleCount = value; }
            }

            public bool Use
            {
                get { return m_Use; }
                set { m_Use = value; }
            }

            public long CondenserState
            {
                get { return m_CondenserState; }
                set { m_CondenserState = value; }
            }

            public string CondenserNumber
            {
                get { return m_strCondenserNumber; }
                set { m_strCondenserNumber = value; }
            }

            public Decimal MaxCO2_CO
            {
                get { return m_maxCO2_CO; }
                set { m_maxCO2_CO = value; }
            }

            public Color UseColor
            {
                get { return m_Color; }
                set { m_Color = value; }
            }
        };

        public object m_iCondenserTypeID = null;
        public CondenserTest.FunctionType m_FunctionType = CondenserTest.FunctionType.Degree;
        private Dictionary<string, int> m_dictColumnHeight = new Dictionary<string, int>();
        float DpiXRel;
        float DpiYRel;
        BindingList<DataSourceString> listTestType = new BindingList<DataSourceString>();
        BindingList<CondenserInfo> listCondenser = new BindingList<CondenserInfo>();
        Bitmap bmp_pb1;
        Bitmap bmp_pb2;

        float fMarginBig = 50;
        float fMarginSmall = 30;
        float fTail = 5;
        float fRadius = 3;
        float fSelectRadius = 4;

        public double m_koefA = 0;
        public double m_koefB = 0;
        public double m_koefR2 = 0;

        public double m_koefA_line = 0;
        public double m_koefB_line = 0;

        long maxCycleValue = 0;
        long maxCO2COValue = 0;

        int m_iSelectIndex = -1;

        List<double> vecX = new List<double>();
        List<double> vecY = new List<double>();

        BindingList<DataSourceString> listFunctionType = new BindingList<DataSourceString>();

        //private List<KnownColor> colors;
        /*int[] colors = new int[] 
        { 
        0xff, 0xff00, 0xffff00, 0xff0000, 0xff00ff, 0,
        0xc0c0ff, 0xc0e0ff, 0xc0ffff, 0xc0ffc0, 0xffffc0, 0xffc0c0, 0xffc0ff, 0xe0e0e0, 0x8080ff, 0x80c0ff, 0x80ffff, 0x80ff80,
        0xffff80, 0xff8080, 0xff80ff, 0xc0c0c0, 0x80ff, 0x808080, 0xc0, 0x40c0, 0xc0c0,
        0xc000, 0xc0c000, 0xc00000, 0xc000c0, 0x404040, 0x80, 0x4080, 0x8080, 0x8000, 0x808000, 0x800000, 0x800080,
        0x40, 0x404080, 0x4040, 0x4000, 0x404000, 0x400000, 0x400040
        };*/
        List<Color> listColors = new List<Color>();
        Dictionary<Color, int> m_dictColors = new Dictionary<Color, int>();
        //Dictionary<KeyValuePair<float, float>, string> m_dictPoints2 = new Dictionary<KeyValuePair<float, float>, string>();

        bool m_bEndLoadForm = false;

        public CalcKoefForm()
        {
            InitializeComponent();
        }

        private void bActivation_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
        }

        private void CalcKoefForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypes' table. You can move, or remove it, as needed.
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (strSeparator == ".") strSeparator = "\\.";
            teKoefA.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";
            teKoefB.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";
            teR2.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";

            // TODO: This line of code loads data into the 'dataSetQuery.QCondenserTypesForTable' table. You can move, or remove it, as needed.
            this.qCondenserTypesTableAdapter.Fill(this.dataSetQuery.QCondenserTypes);
            cbCondenserType.Properties.DropDownRows = this.dataSetQuery.QCondenserTypes.Count;
            cbCondenserType.EditValue = m_iCondenserTypeID;

            panelGraph1.BackColor = this.BackColor;
            panelGraph2.BackColor = this.BackColor;

            listFunctionType.Add(new DataSourceString(0, "степенная"));
            listFunctionType.Add(new DataSourceString(1, "экспоненциальная"));
            listFunctionType.Add(new DataSourceString(2, "логарифмическая"));
            cbFunctionType.Properties.DataSource = listFunctionType;
            cbFunctionType.Properties.DisplayMember = "VAL";
            cbFunctionType.Properties.ValueMember = "KEY";
            cbFunctionType.Properties.DropDownRows = listFunctionType.Count;
            cbFunctionType.EditValue = (long)0;

            listTestType.Add(new DataSourceString(1, "ресурсные"));
            listTestType.Add(new DataSourceString(2, "приемо-сдаточные"));
            listTestType.Add(new DataSourceString(3, "эксплуатация"));
            repTestType.DataSource = listTestType;
            repTestType.DisplayMember = "VAL";
            repTestType.ValueMember = "KEY";
            repTestType.DropDownRows = listTestType.Count;

            listColors.Add(Color.FromArgb(255, 0, 0));
            listColors.Add(Color.FromArgb(0, 0, 255));
            listColors.Add(Color.FromArgb(0, 255, 0));
            listColors.Add(Color.FromArgb(0, 255, 255));
            listColors.Add(Color.FromArgb(255, 0, 255));
            listColors.Add(Color.FromArgb(153, 0, 204));
            listColors.Add(Color.FromArgb(255, 102, 0));
            listColors.Add(Color.FromArgb(255, 153, 204));
            listColors.Add(Color.FromArgb(102, 51, 51));
            listColors.Add(Color.FromArgb(102, 255, 204));
            listColors.Add(Color.FromArgb(102, 51, 153));
            listColors.Add(Color.FromArgb(153, 204, 153));
            listColors.Add(Color.FromArgb(51, 0, 153));
            listColors.Add(Color.FromArgb(175, 175, 175));
            listColors.Add(Color.FromArgb(51, 102, 102));
            listColors.Add(Color.FromArgb(216, 169, 114));
            listColors.Add(Color.FromArgb(0, 171, 137));
            listColors.Add(Color.FromArgb(51, 153, 102));
            listColors.Add(Color.FromArgb(204, 51, 0));
            listColors.Add(Color.FromArgb(153, 102, 51));
            listColors.Add(Color.FromArgb(204, 102, 51));
            listColors.Add(Color.FromArgb(0, 204, 255));
            listColors.Add(Color.FromArgb(153, 51, 102));
            listColors.Add(Color.FromArgb(51, 51, 51));
            listColors.Add(Color.FromArgb(204, 51, 255));
            listColors.Add(Color.FromArgb(255, 0, 102));
            listColors.Add(Color.FromArgb(102, 153, 153));
            listColors.Add(Color.FromArgb(102, 102, 51));
            listColors.Add(Color.FromArgb(128, 128, 0));
            listColors.Add(Color.FromArgb(153, 153, 255));
            listColors.Add(Color.FromArgb(204, 153, 255));
            listColors.Add(Color.FromArgb(255, 153, 102));
            listColors.Add(Color.FromArgb(51, 102, 153));
            listColors.Add(Color.FromArgb(153, 102, 153));
            listColors.Add(Color.FromArgb(153, 0, 0));
            listColors.Add(Color.FromArgb(255, 102, 51));
            listColors.Add(Color.FromArgb(102, 153, 255));
            listColors.Add(Color.FromArgb(36, 185, 107));
            listColors.Add(Color.FromArgb(247, 170, 94));
            listColors.Add(Color.FromArgb(154, 154, 154));
            listColors.Add(Color.FromArgb(102, 0, 102));
            listColors.Add(Color.FromArgb(0, 204, 199));
            listColors.Add(Color.FromArgb(235, 182, 0));
            listColors.Add(Color.FromArgb(0, 134, 0));
            listColors.Add(Color.FromArgb(102, 51, 255));
            listColors.Add(Color.FromArgb(234, 98, 162));
            listColors.Add(Color.FromArgb(102, 102, 102));
            listColors.Add(Color.FromArgb(153, 204, 204));
            listColors.Add(Color.FromArgb(0, 0, 102));
            listColors.Add(Color.FromArgb(153, 0, 153));
            listColors.Add(Color.FromArgb(0, 102, 51));
            listColors.Add(Color.FromArgb(239, 107, 107));
            listColors.Add(Color.FromArgb(153, 204, 102));
            listColors.Add(Color.FromArgb(102, 102, 204));
            listColors.Add(Color.FromArgb(204, 102, 204));
            listColors.Add(Color.FromArgb(153, 153, 102));
            listColors.Add(Color.FromArgb(102, 153, 51));
            listColors.Add(Color.FromArgb(204, 204, 51));
            listColors.Add(Color.FromArgb(0, 0, 0));
            listColors.Add(Color.FromArgb(102, 102, 255));
            listColors.Add(Color.FromArgb(153, 51, 204));
            for (int i = 0; i < listColors.Count; i++)
            {
                m_dictColors[listColors[i]] = 0;
            }


            bmp_pb1 = new Bitmap(pb1.Width, pb1.Height);
            pb1.Image = bmp_pb1;

            bmp_pb2 = new Bitmap(pb2.Width, pb2.Height);
            pb2.Image = bmp_pb2;

            try
            {
                long CondenserTypeID = 0;
                if (cbCondenserType.EditValue != null && cbCondenserType.EditValue != DBNull.Value)
                    CondenserTypeID = (long)cbCondenserType.EditValue;

                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "select c.CondenserID, c.CondenserTestType, c.CondenserState, c.CondenserNumber, " + 
                    "ct.CycleCount, ct.CO, ct.CO2, c.NormalizedResource " +
                    "FROM CondenserTest AS ct INNER JOIN Condensers AS c ON c.CondenserID = ct.CondenserID " +
                    "WHERE (@ctid = 0 OR c.CondenserTypeID = @ctid) AND ct.CondenserTestType IN (1, 3) AND NOT ct.CO IS NULL AND NOT ct.CO2 IS NULL " +
                    "ORDER BY c.CondenserID, ct.CondenserTestDate";
                com.CommandType = CommandType.Text;

                AddParam(com, "@ctid", DbType.Int64, CondenserTypeID);

                SQLiteDataReader dr = com.ExecuteReader();

                Dictionary<long, int> dictCondenserIndex = new Dictionary<long, int>();
                while (dr.Read())
                {
                    long CondenserID = Convert.ToInt64(dr["CondenserID"]);
                    long CondenserTestType = Convert.ToInt64(dr["CondenserTestType"]);
                    long CondenserState = Convert.ToInt64(dr["CondenserState"]);
                    string CondenserNumber = Convert.ToString(dr["CondenserNumber"]);
                    long CycleCount = Convert.ToInt64(dr["CycleCount"]);
                    Decimal CO = Convert.ToDecimal(dr["CO"]);
                    Decimal CO2 = Convert.ToDecimal(dr["CO2"]);
                    long NormalizedResource = Convert.ToInt64(dr["NormalizedResource"]);

                    // сначала ищем элемент по CondenserID
                    if (dictCondenserIndex.ContainsKey(CondenserID))
                    {
                        int index = dictCondenserIndex[CondenserID];
                        listCondenser[index].AddTest(CO, CO2, CycleCount);
                    }
                    else
                    {
                        listCondenser.Add(new CondenserInfo(CondenserID, CondenserTestType, CondenserState, CondenserNumber, NormalizedResource));
                        int index = listCondenser.Count - 1;
                        listCondenser[index].AddTest(CO, CO2, CycleCount);
                        dictCondenserIndex[CondenserID] = listCondenser.Count - 1;
                    }
                }
                dr.Close();

                GridGC.DataSource = listCondenser;
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                Graphics gr = this.CreateGraphics();

                DpiXRel = gr.DpiX / 96.0f;
                DpiYRel = gr.DpiY / 96.0f;

                colCondenserState.MaxWidth = (int)(30 * DpiXRel);
                colCondenserState.MinWidth = (int)(30 * DpiXRel);

                colUse.MaxWidth = (int)(80 * DpiXRel);
                colUse.MinWidth = (int)(80 * DpiXRel);

                colColor.MaxWidth = (int)(55 * DpiXRel);
                colColor.MinWidth = (int)(55 * DpiXRel);

                fMarginBig *= DpiXRel;
                fMarginSmall *= DpiXRel;
                fTail *= DpiXRel;
                fRadius *= DpiXRel;
                fSelectRadius *= DpiXRel;

                /*foreach (GridColumn col in GridView.Columns)
                {
                    GetColumnBestHeight(col);
                }
                SetMaxColumnHeights();*/
            }
            catch (Exception ex)
            {
                MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //colors = GetColors();

            m_bEndLoadForm = true;
            timer1.Enabled = true;
            RefreshGraphics();
        }

        private void cbCondenserTestType_EditValueChanged(object sender, EventArgs e)
        {

        }
        
        private void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        private void bCalc_Click(object sender, EventArgs e)
        {
            
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            foreach (GridColumn col in GridView.Columns)
            {
                GetColumnBestHeight(col);
            }
            SetMaxColumnHeights();
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

                int index = GridView.GetDataSourceRowIndex(e.RowHandle);

                if (listCondenser[index].CondenserState == 0)
                    e.Graphics.FillEllipse(new SolidBrush(Color.LightGreen), rect);
                else
                    e.Graphics.FillEllipse(new SolidBrush(Color.Red), rect);
                e.Graphics.DrawEllipse(new Pen(new SolidBrush(Color.Black)), rect);
                e.Handled = true;
            }

            if (e.Column.Name == "colColor")
            {
                int index = GridView.GetDataSourceRowIndex(e.RowHandle);

                if (listCondenser[index].UseColor != Color.Empty)
                {
                    e.Graphics.FillRectangle(new SolidBrush(listCondenser[index].UseColor), e.Bounds);
                    e.Handled = true;
                }
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

                    int index = GridView.GetDataSourceRowIndex(hi.RowHandle);

                    if (listCondenser[index].CondenserState == 0)
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

        private void GridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {            
        }

        private void GetMaxData(ref long maxCycleCount, ref Decimal maxCO2_CO)
        {
            maxCycleCount = 0;
            maxCO2_CO = 0;

            for (int i = 0; i < listCondenser.Count; i++)
            {
                if (listCondenser[i].Use)
                {
                    if (maxCycleCount < listCondenser[i].CycleCount)
                        maxCycleCount = listCondenser[i].CycleCount;

                    if (maxCO2_CO < listCondenser[i].MaxCO2_CO)
                        maxCO2_CO = listCondenser[i].MaxCO2_CO;
                }
            }
        }

        private long GetMaxAxisValue(Decimal val)
        {
            long maxAxisValue = 0;
            string strVal = val.ToString("0");
            maxAxisValue = Convert.ToInt64(strVal);
            if (maxAxisValue < val) maxAxisValue++;
            return GetMaxAxisValue(maxAxisValue);
        }

        private long GetMaxAxisValue(long val)
        {
            long maxAxisValue = 0;
            string strVal = val.ToString();
            int iCntDigits = strVal.Length;
            if (iCntDigits >= 3)
            {
                int FixCntDigits = 2;
                if (iCntDigits == 3) FixCntDigits = 1;
                for (int i = 0; i < iCntDigits; i++)
                {
                    int digit = Convert.ToInt32(strVal.ToCharArray(i, 1)[0].ToString());
                    if (i < FixCntDigits) maxAxisValue = 10 * maxAxisValue + digit;
                    else
                    {
                        if (i == FixCntDigits)
                        {
                            if (digit == 0) maxAxisValue = 10 * maxAxisValue;
                            else if (digit < 5) maxAxisValue = 10 * maxAxisValue + 5;
                            else maxAxisValue = (maxAxisValue + 1) * 10;
                        }
                        else maxAxisValue = 10 * maxAxisValue;
                    }
                }
            }
            else
            {
                maxAxisValue = (val / 5) * 5;
                if (maxAxisValue < val) maxAxisValue += 5;
            }

            return maxAxisValue;
        }

        private void DrawXYAxis(Graphics gr, int Height, int Width, long maxXAxis, long maxYAxis, bool bInverse, string strXAxisStr, string strYAxisStr)
        {
            Pen pen = new Pen(Color.DarkGray);
            Pen pen_g = new Pen(Color.LightGray);
            pen_g.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            // рисуем оси
            if (!bInverse)
            {
                gr.DrawLine(pen, new PointF(fMarginBig - fTail, Height - fMarginBig), new PointF(Width - fMarginSmall + fTail, Height - fMarginBig));
                gr.DrawLine(pen, new PointF(fMarginBig, Height - fMarginBig + fTail), new PointF(fMarginBig, fMarginSmall - fTail));
            }
            else
            {
                gr.DrawLine(pen, new PointF(fMarginSmall - fTail, Height - fMarginBig), new PointF(Width - fMarginBig + fTail, Height - fMarginBig));
                gr.DrawLine(pen, new PointF(Width - fMarginBig, Height - fMarginBig + fTail), new PointF(Width - fMarginBig, fMarginSmall - fTail));
            }

            // рисуем сетку
            float step = (Height - (fMarginBig + fMarginSmall)) / 10.0f;
            Decimal stepVal = 0;
            for (int i = 1; i <= 10; i++)
            {
                if (!bInverse)
                    gr.DrawLine(pen_g, new PointF(fMarginBig, Height - fMarginBig - i * step), new PointF(Width - fMarginSmall, Height - fMarginBig - i * step));
                else
                    gr.DrawLine(pen_g, new PointF(fMarginSmall, Height - fMarginBig - i * step), new PointF(Width - fMarginBig, Height - fMarginBig - i * step));

                if (maxXAxis != 0 && maxYAxis != 0)
                {
                    stepVal += maxYAxis / new Decimal(10f);
                    if (!bInverse)
                    {
                        SizeF size = gr.MeasureString(stepVal.ToString("0.0"), new Font("Segoe UI", 8 * DpiYRel));
                        gr.DrawString(stepVal.ToString("0.0"), new Font("Segoe UI", 8 * DpiYRel), new SolidBrush(Color.Black), new PointF(fMarginBig - size.Width, Height - fMarginBig - i * step - size.Height / 2f));
                    }
                    else
                    {
                        SizeF size = gr.MeasureString(stepVal.ToString("0"), new Font("Segoe UI", 8 * DpiYRel));
                        gr.DrawString(stepVal.ToString("0"), new Font("Segoe UI", 8 * DpiYRel), new SolidBrush(Color.Black), new PointF(Width - fMarginBig /*+ size.Width*/, Height - fMarginBig - i * step - size.Height / 2f));
                    }
                }
            }

            step = (Width - (fMarginBig + fMarginSmall)) / 10.0f;
            stepVal = 0;
            int i_beg = 1;
            int i_end = 10;
            /*if (bInverse)
            {
                i_beg = 0;
                i_end = 9;
            }*/

            for (int i = i_beg; i <= i_end; i++)
            {
                if (!bInverse)
                    gr.DrawLine(pen_g, new PointF(fMarginBig + i * step, fMarginSmall), new PointF(fMarginBig + i * step, Height - fMarginBig));
                else
                    gr.DrawLine(pen_g, new PointF(Width - fMarginBig - i * step, fMarginSmall), new PointF(Width - fMarginBig - i * step, Height - fMarginBig));

                if (maxXAxis != 0 && maxYAxis != 0)
                {
                    stepVal += maxXAxis / new Decimal(10f);
                    if (!bInverse)
                    {
                        SizeF size = gr.MeasureString(stepVal.ToString("0"), new Font("Segoe UI", 8 * DpiYRel));
                        gr.DrawString(stepVal.ToString("0"), new Font("Segoe UI", 8 * DpiYRel), new SolidBrush(Color.Black), new PointF(fMarginBig + i * step - size.Width / 2f, Height - fMarginBig));
                    }
                    else
                    {
                        SizeF size = gr.MeasureString(stepVal.ToString("0.0"), new Font("Segoe UI", 8 * DpiYRel));
                        gr.DrawString(stepVal.ToString("0.0"), new Font("Segoe UI", 8 * DpiYRel), new SolidBrush(Color.Black), new PointF(Width - fMarginBig - i * step - size.Width / 2f, Height - fMarginBig));
                    }
                }
            }

            // выводим подписи
            if (strXAxisStr != "")
            {
                SizeF size = gr.MeasureString(strXAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold));
                float wGr = Width - (fMarginBig + fMarginSmall);

                if (!bInverse)
                    gr.DrawString(strXAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(fMarginBig + (wGr - size.Width) / 2f, Height - (fMarginBig) / 2f));
                else
                    gr.DrawString(strXAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(fMarginSmall + (wGr - size.Width) / 2f, Height - (fMarginBig) / 2f));
            }

            if (strYAxisStr != "")
            {
                //StringFormat formatver = new StringFormat(StringFormatFlags.DirectionVertical);

                SizeF size = gr.MeasureString(strYAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold));
                float hGr = Height - (fMarginBig + fMarginSmall);

                float dx = 0;
                float dy = 0;
                if (!bInverse)
                {
                    dx = fMarginBig / 2f - size.Height / 2;
                    dy = fMarginSmall + hGr / 2f;
                }
                else
                {
                    dx = Width - (fMarginBig / 2f - size.Height / 2);
                    dy = fMarginSmall + hGr / 2f;
                }

                gr.TranslateTransform(dx, dy);
                gr.RotateTransform(270);

                if (!bInverse)
                {
                    gr.DrawString(strYAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(-size.Width / 2, -size.Height / 2));
                }
                else
                {
                    gr.DrawString(strYAxisStr, new Font("Segoe UI", 8 * DpiYRel, FontStyle.Bold), new SolidBrush(Color.Black), new PointF(-size.Width / 2, -size.Height / 2));
                }
                
                gr.RotateTransform(-270);
                gr.TranslateTransform(-dx, -dy);
            }
        }

        private void DrawCondenser1(int index, Graphics gr, int Height, int Width, long maxXAxis, long maxYAxis, bool bLine)
        {
            float wGr = Width - (fMarginBig + fMarginSmall);
            float hGr = Height - (fMarginBig + fMarginSmall);

            float wGrProp = wGr / (float)maxXAxis;
            float hGrProp = hGr / (float)maxYAxis;

            float oldXPos = 0;
            float oldYPos = 0;

            Pen pen = null;
            if (m_iSelectIndex == index)
                pen = new Pen(listCondenser[index].UseColor, 2 * DpiXRel);
            else
                pen = new Pen(listCondenser[index].UseColor);
            SolidBrush br = new SolidBrush(listCondenser[index].UseColor);

            for (int i = 0; i < listCondenser[index].listTest.Count; i++)
            {
                float fProcent = listCondenser[index].listTest[i].m_CycleCount * 100 / (float)listCondenser[index].CycleCount;
                if (fProcent > maxYAxis) continue;
                float Xpos = wGr - (float)Convert.ToDouble(listCondenser[index].listTest[i].m_CO2_CO) * wGrProp + fMarginSmall;
                float Ypos = hGr - fProcent * hGrProp + fMarginSmall;

                listCondenser[index].listTest[i].m_PointGraph1 = new PointF(Xpos, Ypos);

                if (index == m_iSelectIndex)
                    gr.FillRectangle(br, Xpos - fRadius, Ypos - fRadius, fSelectRadius * 2, fSelectRadius * 2);
                else
                    gr.FillRectangle(br, Xpos - fRadius, Ypos - fRadius, fRadius * 2, fRadius * 2);

                if (i > 0 && bLine)
                {
                    gr.DrawLine(pen, oldXPos, oldYPos, Xpos, Ypos);
                }

                oldXPos = Xpos;
                oldYPos = Ypos;
            }
        }

        private void DrawCondenser2(int index, Graphics gr, int Height, int Width, long maxXAxis, long maxYAxis, bool bLine)
        {
            float wGr = Width - (fMarginBig + fMarginSmall);
            float hGr = Height - (fMarginBig + fMarginSmall);

            float wGrProp = wGr / (float)maxXAxis;
            float hGrProp = hGr / (float)maxYAxis;

            float oldXPos = 0;
            float oldYPos = 0;

            Pen pen = null;
            if (m_iSelectIndex == index)
                pen = new Pen(listCondenser[index].UseColor, 2 * DpiXRel);
            else
                pen = new Pen(listCondenser[index].UseColor);
            SolidBrush br = new SolidBrush(listCondenser[index].UseColor);

            for (int i = 0; i < listCondenser[index].listTest.Count; i++)
            {
                float Xpos = listCondenser[index].listTest[i].m_CycleCount * wGrProp + fMarginBig;
                float Ypos = hGr - (float)Convert.ToDouble(listCondenser[index].listTest[i].m_CO2_CO) * hGrProp + fMarginSmall;

                listCondenser[index].listTest[i].m_PointGraph2 = new PointF(Xpos, Ypos);

                if (index == m_iSelectIndex)
                    gr.FillRectangle(br, Xpos - fRadius, Ypos - fRadius, fSelectRadius * 2, fSelectRadius * 2);
                else
                    gr.FillRectangle(br, Xpos - fRadius, Ypos - fRadius, fRadius * 2, fRadius * 2);
                //m_dictPoints2[new KeyValuePair<float, float>(Xpos, Ypos)] = listCondenser[index].CondenserNumber + " - " + listCondenser[index].listTest[i].m_CO2_CO.ToString("0.0000");

                if (i > 0 && bLine)
                {
                    gr.DrawLine(pen, oldXPos, oldYPos, Xpos, Ypos);
                }

                oldXPos = Xpos;
                oldYPos = Ypos;
            }
        }

        private void DrawGraphic1(long maxXAxis, long maxYAxis)
        {
            Graphics gr = Graphics.FromImage(bmp_pb1);// e.Graphics;
            gr.FillRectangle(new SolidBrush(Color.White), pb1.ClientRectangle);

            DrawXYAxis(gr, pb1.Height, pb1.Width, maxXAxis, maxYAxis, true, "CO2/CO", "Выработанный ресурс , %");

            Random color = new Random();
            //m_dictPoints2.Clear();
            for (int i = 0; i < listCondenser.Count; i++)
            {
                if (listCondenser[i].Use)
                {
                    if (m_iSelectIndex == i) continue;
                    DrawCondenser1(i, gr, pb1.Height, pb1.Width, maxXAxis, maxYAxis, cbShowLines.Checked);
                }
            }
            if (m_iSelectIndex >= 0)
                DrawCondenser1(m_iSelectIndex, gr, pb1.Height, pb1.Width, maxXAxis, maxYAxis, cbShowLines.Checked);

            pb1.Invalidate();
        }

        private List<KnownColor> GetColors()
        {
            //create a generic list of strings
            List<KnownColor> colors = new List<KnownColor>();
            //get the color names from the Known color enum
            string[] colorNames = Enum.GetNames(typeof(KnownColor));
            //iterate thru each string in the colorNames array
            foreach (string colorName in colorNames)
            {
                //cast the colorName into a KnownColor
                KnownColor knownColor = (KnownColor)Enum.Parse(typeof(KnownColor), colorName);
                //check if the knownColor variable is a System color
                if (knownColor > KnownColor.Transparent && knownColor != KnownColor.White)
                {
                    //add it to our list
                    colors.Add(knownColor);
                }
            }
            //return the color list
            return colors;
        }

        private void DrawGraphic2(long maxXAxis, long maxYAxis)
        {
            Graphics gr = Graphics.FromImage(bmp_pb2);// e.Graphics;
            gr.FillRectangle(new SolidBrush(Color.White), pb2.ClientRectangle);

            DrawXYAxis(gr, pb2.Height, pb2.Width, maxXAxis, maxYAxis, false, "Кол-во циклов", "CO2/CO");

            Random color = new Random();
            //m_dictPoints2.Clear();
            for (int i = 0; i < listCondenser.Count; i++)
            {
                if (listCondenser[i].Use)
                {
                    if (m_iSelectIndex == i) continue;
                    DrawCondenser2(i, gr, pb2.Height, pb2.Width, maxXAxis, maxYAxis, cbShowLines.Checked);
                }
            }
            if (m_iSelectIndex >= 0)
                DrawCondenser2(m_iSelectIndex, gr, pb2.Height, pb2.Width, maxXAxis, maxYAxis, cbShowLines.Checked);

            pb2.Invalidate();
        }

        private void DrawResult(double koefA, double koefB, long maxXAxis, long maxYAxis)
        {
            Graphics gr = Graphics.FromImage(bmp_pb1);// e.Graphics;

            float wGr = pb1.Width - (fMarginBig + fMarginSmall);
            float hGr = pb1.Height - (fMarginBig + fMarginSmall);

            float wGrProp = wGr / (float)maxXAxis;
            float hGrProp = hGr / (float)maxYAxis;

            float oldXPos = 0;
            float oldYPos = 0;

            Pen pen = new Pen(new SolidBrush(Color.Black), 1);

            for (int i = 1; i < (int)wGr; i++)
            {
                double fVal = i / wGrProp;
                double y = 0;
                if (m_FunctionType == CondenserTest.FunctionType.Degree)
                    y = koefA * Math.Pow(fVal, koefB);
                if (m_FunctionType == CondenserTest.FunctionType.Exp)
                    y = koefA * Math.Exp(koefB * fVal);
                if (m_FunctionType == CondenserTest.FunctionType.Log)
                    y = koefA * Math.Log(fVal) + koefB;

                if (y > maxYAxis)
                {
                    oldYPos = hGr - (float)maxYAxis * hGrProp + fMarginSmall;
                    oldXPos = wGr - i + fMarginSmall;
                    continue;
                }
                float Ypos = hGr - (float)y * hGrProp + fMarginSmall;
                float Xpos = wGr - i + fMarginSmall;

                if (oldXPos > 0 && oldYPos > 0)
                    gr.DrawLine(pen, new PointF(oldXPos, oldYPos), new PointF(Xpos, Ypos));

                oldXPos = Xpos;
                oldYPos = Ypos;
            }
        }

        private void CalcR2(List<double> vecX, List<double> vecY, double koefA, double koefB)
        {
            List<double> vecYc = new List<double>();

            double fYAvg = 0;
            double fSumY = 0;

            for (int i = 0; i < vecY.Count; i++)
            {
                fSumY += vecY[i];
            }

            fYAvg = fSumY / vecY.Count;

            for (int i = 0; i < vecX.Count; i++)
            {
                //vecYc.Add(koefA * Math.Pow(vecX[i], koefB));
                vecYc.Add(koefA + koefB * vecX[i]);
            }

            double fSum1 = 0;
            double fSum2 = 0;
            for (int i = 0; i < vecY.Count; i++)
            {
                fSum1 += (vecY[i] - vecYc[i]) * (vecY[i] - vecYc[i]);
                fSum2 += (vecY[i] - fYAvg) * (vecY[i] - fYAvg);
            }

            if (Math.Abs(fSum2 - fSumY * fYAvg) > 0.00009)
            {
                teR2.EditValue = 1 - fSum1 / fSum2;
            }
            else
            {
                teR2.EditValue = null;
            }
        }

        private void CalcKoef(long maxXAxis, long maxYAxis)
        {
            //List<double> vecX2 = new List<double>();
            //List<double> vecXY = new List<double>();
            double fSumX2 = 0;
            double fSumX = 0;
            double fSumY = 0;
            double fSumXY = 0;

            //double fSumX2_Line = 0;
            //double fSumX_Line = 0;
            //double fSumY_Line = 0;
            //double fSumXY_Line = 0;
            
            int iCnt = 0;

            m_koefA = 0;
            m_koefB = 0;

            vecX.Clear();
            vecY.Clear();

            for (int i = 0; i < listCondenser.Count; i++)
            {
                if (listCondenser[i].Use)
                {
                    for (int j = 0; j < listCondenser[i].listTest.Count; j++)
                    {
                        double fProcent = listCondenser[i].listTest[j].m_CycleCount * 100 / (double)listCondenser[i].CycleCount;
                        if (fProcent <= 100)
                        {
                            double Xval = (double)listCondenser[i].listTest[j].m_CO2_CO;
                            double Yval = fProcent;

                            double X = 0;
                            double Y = 0;
                            if (m_FunctionType == CondenserTest.FunctionType.Degree)
                            {
                                X = Math.Log(Xval);
                                Y = Math.Log(Yval);
                            }
                            if (m_FunctionType == CondenserTest.FunctionType.Exp)
                            {
                                X = Xval;
                                Y = Math.Log(Yval);
                            }
                            if (m_FunctionType == CondenserTest.FunctionType.Log)
                            {
                                X = Math.Log(Xval);
                                Y = Yval;
                            }

                            vecX.Add(X);//(double)listCondenser[i].listTest[j].m_CO2_CO);
                            vecY.Add(Y);//fProcent);

                            fSumX += X;
                            fSumY += Y;
                            fSumX2 += X * X;
                            fSumXY += X * Y;

                            //fSumX_Line += X_Line;
                            //fSumY_Line += Y_Line;
                            //fSumX2_Line += X_Line * X_Line;
                            //fSumXY_Line += X_Line * Y_Line;
                            
                            iCnt ++;
                        }
                    }
                }
            }

            if (Math.Abs(iCnt * fSumX2 - fSumX * fSumX) > 0.00009 && iCnt > 0)
            {
                double a1 = (iCnt * fSumXY - fSumX * fSumY) / (iCnt * fSumX2 - fSumX * fSumX);
                double a0 = (fSumY - a1 * fSumX) / iCnt;

                //m_koefB_line = (iCnt * fSumXY_Line - fSumX_Line * fSumY_Line) / (iCnt * fSumX2_Line - fSumX_Line * fSumX_Line);
                //m_koefA_line = (fSumY_Line - m_koefB_line * fSumX_Line) / iCnt;
                
                //a0 = Math.Exp(a0);

                m_koefA_line = a0;
                m_koefB_line = a1;

                if (m_FunctionType == CondenserTest.FunctionType.Degree)
                {
                    m_koefA = Math.Exp(a0);
                    m_koefB = a1;
                }
                if (m_FunctionType == CondenserTest.FunctionType.Exp)
                {
                    m_koefA = Math.Exp(a0);
                    m_koefB = a1;
                }
                if (m_FunctionType == CondenserTest.FunctionType.Log)
                {
                    m_koefA = a1;
                    m_koefB = a0;
                }

                teKoefA.EditValue = m_koefA;
                teKoefB.EditValue = m_koefB;

                timer2.Enabled = false;

                CalcR2(vecX, vecY, m_koefA_line, m_koefB_line);
            }
            else
            {
                teKoefB.EditValue = null;
                teKoefA.EditValue = null;
                teR2.EditValue = null;

                timer2.Enabled = false;
            }
        }

        private void RefreshGraphics()
        {
            // ищем максимальное значение среды выбранных конденсаторов по полям m_CycleCount, CO2/CO
            long maxCycleCount = 0;
            Decimal maxCO2_CO = 0;
            GetMaxData(ref maxCycleCount, ref maxCO2_CO);
            maxCycleValue = GetMaxAxisValue(maxCycleCount);
            maxCO2COValue = GetMaxAxisValue(maxCO2_CO);

            DrawGraphic1(maxCO2COValue, 100);
            DrawGraphic2(maxCycleValue, maxCO2COValue);

            CalcKoef(maxCO2COValue, 100);
            if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
                DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);
        }

        private Color GetNextColor()
        {
            Color cl = Color.Red;
            int minCnt = Int32.MaxValue;
            for (int i = 0; i < listColors.Count; i++)
            {
                if (minCnt > m_dictColors[listColors[i]])
                {
                    minCnt = m_dictColors[listColors[i]];
                    cl = listColors[i];
                }
            }

            return cl;
        }

        private void GridView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "Use")
            {
                int index = GridView.GetDataSourceRowIndex(e.RowHandle);

                listCondenser[index].Use = Convert.ToBoolean(e.Value);
                if (Convert.ToBoolean(e.Value) == false)
                {
                    m_dictColors[listCondenser[index].UseColor]--;
                    listCondenser[index].UseColor = Color.Empty;
                }
                else
                {
                    listCondenser[index].UseColor = GetNextColor();
                    m_dictColors[listCondenser[index].UseColor]++;

                }
                GridView.Invalidate();
                RefreshGraphics();
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            if (teKoefA.EditValue == null || teKoefB.EditValue == null
                || teKoefA.EditValue.ToString() == "" || teKoefB.EditValue.ToString() == "")
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значения коэффициентов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            m_koefA = Convert.ToDouble(teKoefA.EditValue);
            m_koefB = Convert.ToDouble(teKoefB.EditValue);
            m_koefR2 = Convert.ToDouble(teR2.EditValue);

            /*try
            {
                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "UPDATE CondenserTypeParameters SET KoefA = @koefA, KoefB = @koefB, FunctionType = @ftype WHERE COALESCE(CondenserTypeID, 0) = @ctid";
                com.CommandType = CommandType.Text;
                SQLiteParameter param = new SQLiteParameter("@ctid", DbType.Int64);
                param.Value = m_iCondenserTypeID == DBNull.Value ? (long)0 : m_iCondenserTypeID;
                com.Parameters.Add(param);

                SQLiteParameter param2 = new SQLiteParameter("@koefA", DbType.Decimal);
                param2.Value = koefA;
                com.Parameters.Add(param2);

                SQLiteParameter param3 = new SQLiteParameter("@koefB", DbType.Decimal);
                param3.Value = koefB;
                com.Parameters.Add(param3);

                SQLiteParameter param4 = new SQLiteParameter("@ftype", DbType.Int64);
                param4.Value = Convert.ToInt64(cbFunctionType.EditValue);
                com.Parameters.Add(param4);

                com.ExecuteNonQuery();

                connection.Close();
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void CalcKoefForm_Resize(object sender, EventArgs e)
        {
        }

        private void xtraTabControl1_Resize(object sender, EventArgs e)
        {
        }

        private void xtraTabControl1_SizeChanged(object sender, EventArgs e)
        {
        }

        private void pb1_SizeChanged(object sender, EventArgs e)
        {
            if (m_bEndLoadForm)
            {
                if (pb1.Width > 0 && pb1.Height > 0)
                {
                    bmp_pb1 = new Bitmap(pb1.Width, pb1.Height);
                    pb1.Image = bmp_pb1;
                    DrawGraphic1(maxCO2COValue, 100);
                    if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
                        DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);
                }
            }
        }

        private void pb2_SizeChanged(object sender, EventArgs e)
        {
            if (m_bEndLoadForm)
            {
                if (pb2.Width > 0 && pb2.Height > 0)
                {
                    bmp_pb2 = new Bitmap(pb2.Width, pb2.Height);
                    pb2.Image = bmp_pb2;
                    DrawGraphic2(maxCycleValue, maxCO2COValue);
                }
            }
        }

        private void pb2_Move(object sender, EventArgs e)
        {

        }

        private void pb2_MouseMove(object sender, MouseEventArgs e)
        {
            //toolTip.ShowHint(DateTime.Now.Millisecond.ToString(), Cursor.Position);
        }

        private void pb2_MouseDown(object sender, MouseEventArgs e)
        {
            int iOldSelectIndex = m_iSelectIndex;
            m_iSelectIndex = -1;
            for (int i = listCondenser.Count - 1; i >= 0; i--)
            {
                if (listCondenser[i].Use)
                {
                    float fRad = fRadius;
                    if (iOldSelectIndex == i) fRad = fSelectRadius;

                    for (int j = 0; j < listCondenser[i].listTest.Count; j++)
                    {
                        if (e.X >= listCondenser[i].listTest[j].m_PointGraph2.X - fRad &&
                            e.X <= listCondenser[i].listTest[j].m_PointGraph2.X + fRad &&
                            e.Y >= listCondenser[i].listTest[j].m_PointGraph2.Y - fRad &&
                            e.Y <= listCondenser[i].listTest[j].m_PointGraph2.Y + fRad)
                        {
                            toolTip.ShowHint(listCondenser[i].CondenserNumber + "\nCO₂/CO = " + listCondenser[i].listTest[j].m_CO2_CO.ToString("0.000") + "\nКол-во циклов = " + listCondenser[i].listTest[j].m_CycleCount.ToString(), Cursor.Position);
                            m_iSelectIndex = i;
                            DrawGraphic2(maxCycleValue, maxCO2COValue);

                            int row = GridView.GetRowHandle(m_iSelectIndex);
                            GridView.ClearSelection();
                            GridView.SelectRow(row);
                            GridView.FocusedRowHandle = GridView.GetRowHandle(row);
                            return;
                        }
                    }
                }
            }
            toolTip.HideHint();
            DrawGraphic2(maxCycleValue, maxCO2COValue);
        }

        private void pb2_MouseLeave(object sender, EventArgs e)
        {
            toolTip.HideHint();
        }

        private void pb1_MouseLeave(object sender, EventArgs e)
        {
            toolTip.HideHint();
        }

        private void pb1_MouseDown(object sender, MouseEventArgs e)
        {
            int iOldSelectIndex = m_iSelectIndex;
            m_iSelectIndex = -1;
            for (int i = listCondenser.Count - 1; i >= 0; i--)
            {
                if (listCondenser[i].Use)
                {
                    float fRad = fRadius;
                    if (iOldSelectIndex == i) fRad = fSelectRadius;

                    for (int j = 0; j < listCondenser[i].listTest.Count; j++)
                    {
                        if (e.X >= listCondenser[i].listTest[j].m_PointGraph1.X - fRad &&
                            e.X <= listCondenser[i].listTest[j].m_PointGraph1.X + fRad &&
                            e.Y >= listCondenser[i].listTest[j].m_PointGraph1.Y - fRad &&
                            e.Y <= listCondenser[i].listTest[j].m_PointGraph1.Y + fRad)
                        {
                            toolTip.ShowHint(listCondenser[i].CondenserNumber + "\nCO₂/CO = " + listCondenser[i].listTest[j].m_CO2_CO.ToString("0.000") + "\nКол-во циклов = " + listCondenser[i].listTest[j].m_CycleCount.ToString(), Cursor.Position);
                            m_iSelectIndex = i;
                            DrawGraphic1(maxCO2COValue, 100);
                            if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
                                DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);

                            int row = GridView.GetRowHandle(m_iSelectIndex);
                            GridView.ClearSelection();
                            GridView.SelectRow(row);
                            GridView.FocusedRowHandle = GridView.GetRowHandle(row);
                            return;
                        }
                    }
                }
            }
            toolTip.HideHint();
            DrawGraphic1(maxCO2COValue, 100);
            if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
                DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);
        }

        private void teKoefA_EditValueChanged(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer2.Enabled = true;
        }

        private void teKoefB_EditValueChanged(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;

            if (teKoefA.EditValue == null || teKoefB.EditValue == null
                || teKoefA.EditValue.ToString() == "" || teKoefB.EditValue.ToString() == "") return;

            m_koefA = Convert.ToDouble(teKoefA.EditValue);
            m_koefB = Convert.ToDouble(teKoefB.EditValue);

            DrawGraphic1(maxCO2COValue, 100);
            if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
            {
                double a0 = 0;
                double a1 = 0;

                if (m_FunctionType == CondenserTest.FunctionType.Degree)
                {
                    a0 = Math.Log(m_koefA);
                    a1 = m_koefB;
                }
                if (m_FunctionType == CondenserTest.FunctionType.Exp)
                {
                    a0 = Math.Log(m_koefA);
                    a1 = m_koefB;
                }
                if (m_FunctionType == CondenserTest.FunctionType.Log)
                {
                    a1 = m_koefA;
                    a0 = m_koefB;
                }

                m_koefA_line = a0;
                m_koefB_line = a1;

                DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);

                if (m_koefA_line != double.NaN && m_koefB_line != double.NaN)
                    CalcR2(vecX, vecY, m_koefA_line, m_koefB_line);
                else
                    teR2.EditValue = null;
            }
            else
                teR2.EditValue = null;
        }

        private void cbFunctionType_EditValueChanged(object sender, EventArgs e)
        {
            if (m_bEndLoadForm)
            {
                m_FunctionType = (CondenserTest.FunctionType)Convert.ToInt64(cbFunctionType.EditValue);

                if (m_FunctionType == CondenserTest.FunctionType.Degree)
                    lFunctionType.Text = "y = Axᴮ";
                if (m_FunctionType == CondenserTest.FunctionType.Exp)
                    lFunctionType.Text = "y = Aeᴮˣ";
                if (m_FunctionType == CondenserTest.FunctionType.Log)
                    lFunctionType.Text = "y = Aln(x) + B";

                RefreshGraphics();
            }
        }

        private void cbShowLines_CheckedChanged(object sender, EventArgs e)
        {
            if (m_bEndLoadForm)
            {
                DrawGraphic1(maxCO2COValue, 100);
                DrawGraphic2(maxCycleValue, maxCO2COValue);

                if (Math.Abs(m_koefA) > 0.000009 && Math.Abs(m_koefB) > 0.000009)
                    DrawResult(m_koefA, m_koefB, maxCO2COValue, 100);
            }
        }

        private void xtraTabControl1_Click(object sender, EventArgs e)
        {

        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                cbFunctionType.Enabled = false;
                teKoefA.Enabled = false;
                teKoefB.Enabled = false;
                teR2.Enabled = false;
                lFunctionType.Enabled = false;
                lFunctionTypeLabel.Enabled = false;
                lKoefALabel.Enabled = false;
                lKoefBLabel.Enabled = false;
                lR2Label.Enabled = false;
            }
            else
            {
                cbFunctionType.Enabled = true;
                teKoefA.Enabled = true;
                teKoefB.Enabled = true;
                teR2.Enabled = true;
                lFunctionType.Enabled = true;
                lFunctionTypeLabel.Enabled = true;
                lKoefALabel.Enabled = true;
                lKoefBLabel.Enabled = true;
                lR2Label.Enabled = true;
            }
        }
    }
}