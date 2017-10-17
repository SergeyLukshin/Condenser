using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Condenser
{
    public partial class LicenseForm : DevExpress.XtraEditors.XtraForm
    {
        public string m_strCode;

        public LicenseForm()
        {
            InitializeComponent();
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            teCode.Text = m_strCode;
        }

        private void bActivation_Click(object sender, EventArgs e)
        {
            byte[] key = ASCIIEncoding.ASCII.GetBytes("DIAR");

            /*RC4 encoder = new RC4(key);
            byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(m_strCode);
            byte[] result2 = encoder.Encode(testBytes, testBytes.Length);
            string encryptedString = encoder.GetByteString(result2);// ASCIIEncoding.ASCII.GetString(result);*/

            RC4 decoder = new RC4(key);
            byte[] result = decoder.SetByteString(teActivationKey.Text);
            byte[] decryptedBytes = decoder.Decode(result, result.Length);
            string decryptedString = ASCIIEncoding.ASCII.GetString(decryptedBytes);

            if (decryptedString == m_strCode)
            {
                SQLiteConnection con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                try
                {
                    con.Open();
                    SQLiteCommand com = new SQLiteCommand(con);
                    com.CommandType = CommandType.Text;
                    com.CommandText = "INSERT INTO Licenses (Code) VALUES ('" + teActivationKey.Text + "')";
                    com.ExecuteNonQuery();
                    con.Close();
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MyLocalizer.XtraMessageBoxShow("Активация успешно произведена.\nНеобходимо перезапустить программу.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            else
            {
                MyLocalizer.XtraMessageBoxShow("Неверный ключ активации.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}