using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Condenser
{
    public partial class AboutForm : DevExpress.XtraEditors.XtraForm
    {
        int m_iCntClicks = 0;
        public AboutForm()
        {
            InitializeComponent();
        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) return;
            m_iCntClicks++;
            if (m_iCntClicks >= 7)
            {
                m_iCntClicks = 0;
                try
                {
                    PswForm f = new PswForm();
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Program.m_bExpertMode = true;
                        MyLocalizer.XtraMessageBoxShow("Режим Эксперта активирован.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void pictureEdit1_DoubleClick(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) return;
            m_iCntClicks+=2;
            if (m_iCntClicks >= 7)
            {
                m_iCntClicks = 0;
                try
                {
                    PswForm f = new PswForm();
                    if (f.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Program.m_bExpertMode = true;
                        MyLocalizer.XtraMessageBoxShow("Режим Эксперта активирован.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("В программе произошла ошибка. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            lVersion.Text = Program.m_strVersion;
            lVersionDB.Text = Program.m_CurVersionDB.ToString("0.##").Replace(",", ".");
            lVersionDate.Text = Program.m_strDateVersion;
            lBuildNumber.Text = Program.m_strBuildNumber;
        }
    }
}