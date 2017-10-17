using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Condenser
{
    public partial class RecommendationRecordForm : DevExpress.XtraEditors.XtraForm
    {
        public ParameterRecordForm.RecommendationInfo m_RecommendationInfo = null;
        string m_strCaption;

        public RecommendationRecordForm(ParameterRecordForm.RecommendationInfo RecommendationInfo, string strCaption)
        {
            m_RecommendationInfo = RecommendationInfo;
            m_strCaption = strCaption;
            InitializeComponent();
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (strSeparator == ".") strSeparator = "\\.";
            teKIn.Properties.Mask.EditMask = "(-?\\d{1,6}|-?\\d{1,6}" + strSeparator + "\\d{1,4})";

            panel.Text = m_strCaption;

            teRecommendation.Text = m_RecommendationInfo.m_strRecommendation;
            teConclusion.Text = m_RecommendationInfo.m_strConclusion;

            if (m_RecommendationInfo.m_fValue != null)
                teKIn.EditValue = m_RecommendationInfo.m_fValue;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bProtocol_Click(object sender, EventArgs e)
        {
            if (teRecommendation.Text.IndexOf("$") >= 0 && teKIn.EditValue == null)
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо указать значение коэффициента КЦn.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Decimal? val = null;
            if (teKIn.EditValue != null) val = Convert.ToDecimal(teKIn.EditValue);
            m_RecommendationInfo = new ParameterRecordForm.RecommendationInfo(m_RecommendationInfo.m_ID, teRecommendation.Text, teConclusion.Text, val);
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void teKIn_EditValueChanged(object sender, EventArgs e)
        {
            if (teKIn.Text == "") teKIn.EditValue = null;
        }
    }
}