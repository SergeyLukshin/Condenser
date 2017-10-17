using System;
using System.Collections.Generic;

namespace Condenser
{
    public partial class NoFindEquipmentMessageForm : DevExpress.XtraEditors.XtraForm
    {
        public string m_strMessage;

        public NoFindEquipmentMessageForm()
        {
            InitializeComponent();
        }

        private void NoFindEquipmentMessageForm_Load(object sender, EventArgs e)
        {
            lMessage.Text = m_strMessage;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bAdd_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}