using DevExpress.XtraEditors.Controls;
using System;
using System.Collections.Generic;
using DevExpress.XtraEditors;
using DevExpress.Utils;

namespace Condenser
{
    sealed public class MyLocalizer : Localizer
    {
        static string m_strYes = "Да";
        static string m_strNo = "Нет";

        public override string GetLocalizedString(StringId id)
        {
            switch (id)
            {
                case StringId.XtraMessageBoxYesButtonText:
                    return m_strYes;

                case StringId.XtraMessageBoxOkButtonText:
                    return "Да";

                case StringId.XtraMessageBoxNoButtonText:
                    return m_strNo;

                case StringId.XtraMessageBoxCancelButtonText:
                    return "Отменить";

                default:
                    return base.GetLocalizedString(id);

            }         
        }

        public static System.Windows.Forms.DialogResult XtraMessageBoxShow(string strMsg, string strCaption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon)
        {
            m_strYes = "Да";
            m_strNo = "Нет";

            System.Windows.Forms.DialogResult dr = XtraMessageBox.Show(strMsg, strCaption, buttons, icon);
            return dr;
        }

        public static System.Windows.Forms.DialogResult XtraMessageBoxShow(string strMsg, string strCaption, System.Windows.Forms.MessageBoxButtons buttons, System.Windows.Forms.MessageBoxIcon icon, string strYes, string strNo)
        {
            m_strYes = strYes;
            m_strNo = strNo;

            System.Windows.Forms.DialogResult dr = XtraMessageBox.Show(strMsg, strCaption, buttons, icon);
            //AppearanceObject.DefaultFont = font;
            return dr;
        }
    }
}
