using System;
using System.Collections.Generic;


namespace Condenser
{
    public partial class ImageForm : DevExpress.XtraEditors.XtraForm
    {
        public object m_img = null;

        public ImageForm()
        {
            InitializeComponent();
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            m_img = peImage.EditValue;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            peImage.LoadImage();
        }

        private void ImageForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            peImage.EditValue = m_img;
        }
    }
}