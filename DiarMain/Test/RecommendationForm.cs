using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Condenser
{
    public partial class RecommendationForm : DevExpress.XtraEditors.XtraForm
    {
        private string m_strRecommendation;
        private string m_strConclusion;

        public RecommendationForm(string strRecommendation, string strConclusion)
        {
            InitializeComponent();

            m_strRecommendation = strRecommendation;
            m_strConclusion = strConclusion;
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            teRecommendation.Text = m_strRecommendation;
            teConclusion.Text = m_strConclusion;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bProtocol_Click(object sender, EventArgs e)
        {

        }
    }
}