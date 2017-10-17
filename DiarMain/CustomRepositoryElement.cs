using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.Registrator;
using System.ComponentModel;
using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors.Popup;
using System.Data;
using System.Reflection;


namespace Condenser
{

    [UserRepositoryItem("Register")]

    public class RepositoryItemMyLookUpEdit : RepositoryItemLookUpEdit
    {

        static RepositoryItemMyLookUpEdit()
        {

            Register();

        }

        public RepositoryItemMyLookUpEdit() { }



        internal const string EditorName = "MyLookUpEdit";



        public static void Register()
        {

            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo(EditorName, typeof(MyLookUpEdit),

                typeof(RepositoryItemMyLookUpEdit), typeof(DevExpress.XtraEditors.ViewInfo.LookUpEditViewInfo),

                new DevExpress.XtraEditors.Drawing.ButtonEditPainter(), true, null, typeof(DevExpress.Accessibility.ButtonEditAccessible)));
        }

        public override string EditorTypeName
        {

            get { return EditorName; }

        }

    }



    public class MyLookUpEdit : LookUpEdit
    {
        int m_oldSelectedIndex = -1;

        static MyLookUpEdit()
        {

            RepositoryItemMyLookUpEdit.Register();

        }

        public MyLookUpEdit() {
            Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Properties.Appearance.Options.UseTextOptions = true;
            //this.Properties.Appearance.Font = Font.Clone();
            this.Properties.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
            this.Properties.Appearance.TextOptions.Trimming = DevExpress.Utils.Trimming.Word;
            this.Properties.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        }



        public override string EditorTypeName
        {

            get { return RepositoryItemMyLookUpEdit.EditorName; }

        }



        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]

        public new RepositoryItemMyLookUpEdit Properties
        {

            get { return base.Properties as RepositoryItemMyLookUpEdit; }

        }

        protected override DevExpress.XtraEditors.Drawing.BaseControlPainter Painter
        {
            get
            {
                //DevExpress.XtraEditors.Drawing.BaseControlPainter p = new DevExpress.XtraEditors.Drawing.BaseControlPainter();
                //p.Draw()
                return base.Painter;
            }
        }

        protected override void  OnPaint(PaintEventArgs pe)
        {
            // get the graphics object to use to draw
            Graphics g = pe.Graphics;
            string str = this.Text;
            Rectangle rect = pe.ClipRectangle;
            rect.X += 2;
            rect.Width -= 4;
            g.FillRectangle(new SolidBrush(Color.White /*FromArgb(180, 180, 180)*/), pe.ClipRectangle);

            if (str == "")
            {
                g.DrawString("данные отсутствуют", Font, new SolidBrush(Color.Gray), rect, this.Properties.Appearance.GetStringFormat());
            }
            else
            {
                g.DrawString(str, Font, new SolidBrush(Color.Black), rect, this.Properties.Appearance.GetStringFormat());
            }
        }

        protected override PopupBaseForm CreatePopupForm()
        {

            PopupBaseForm frm = new MyPopupLookUpEditForm(this);
            frm.MouseMove += new MouseEventHandler(frm_MouseMove);
            return frm;
        }

        void frm_MouseMove(object sender, MouseEventArgs e)
        {
            PopupLookUpEditForm editform = sender as PopupLookUpEditForm;
            PropertyInfo pi = typeof(PopupBaseForm).GetProperty("ViewInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            PopupLookUpEditFormViewInfo vInfo = pi.GetValue(editform, null) as PopupLookUpEditFormViewInfo;
            LookUpPopupHitTest hitTest = vInfo.GetHitTest(new Point(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y));

            int SelectedIndex = -1;
            if (hitTest.HitType == LookUpPopupHitType.Row)
            {
                SelectedIndex = hitTest.Index;
            }

            //if (SelectedIndex == -1 || m_oldSelectedIndex != SelectedIndex)
            //    InspectionDataForm.ttc.HideHint();
            
            if (m_oldSelectedIndex != SelectedIndex)
            {
                object row = this.Properties.GetDataSourceValue("VAL", SelectedIndex);//.GetDataSourceRowIndex(SelectedIndex);//.GetDataSourceRowByKeyValue(SelectedIndex);
                if (row != null)
                {
                    string item = row.ToString();
                    //if (item.Length > 100) InspectionDataForm.ttc.ShowHint(item);
                }
            }

            m_oldSelectedIndex = SelectedIndex;
        }
    }

    class MyPopupLookUpEditForm : PopupLookUpEditForm
    {
        public MyPopupLookUpEditForm(LookUpEdit ownerEdit)
            : base(ownerEdit)
        {
            ownerEdit.Tag = this.ViewInfo;
        }
    }

}
