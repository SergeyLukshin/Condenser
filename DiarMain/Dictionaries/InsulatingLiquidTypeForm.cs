using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;

namespace Condenser
{
    public partial class InsulatingLiquidTypeForm : DevExpress.XtraEditors.XtraForm
    {
        bool m_bAcceptChanges = true;
        bool m_bUpdateID = false;
        public bool m_bCanSelect = false;
        public long m_SelectID = 0;
        bool m_bNeedCancel = false;

        public InsulatingLiquidTypeForm()
        {
            InitializeComponent();
        }

        private void InsulatingLiquidTypeForm_Load(object sender, EventArgs e)
        {
            if (Program.m_bExpertMode) panelExpertMode.Visible = true;
            else panelExpertMode.Visible = false;

            // TODO: This line of code loads data into the 'dataSetQuery.QInsulatingLiquidTypes' table. You can move, or remove it, as needed.
            this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);
            // TODO: This line of code loads data into the 'dataSetQuery.QInsulatingLiquidTypes' table. You can move, or remove it, as needed.
            this.qInsulatingLiquidTypesTableAdapter.Fill(this.dataSetQuery.QInsulatingLiquidTypes);
            // TODO: This line of code loads data into the 'dataSetQuery1.QInsulatingLiquidTypes' table. You can move, or remove it, as needed.

            this.dataSetQuery.QInsulatingLiquidTypes.QInsulatingLiquidTypesRowDeleting += new DataSetQuery.QInsulatingLiquidTypesRowChangeEventHandler(QInsulatingLiquidTypes_QInsulatingLiquidTypesRowDeleting);
            this.dataSetQuery.QInsulatingLiquidTypes.QInsulatingLiquidTypesRowDeleted += new DataSetQuery.QInsulatingLiquidTypesRowChangeEventHandler(QInsulatingLiquidTypes_QInsulatingLiquidTypesRowDeleted);
            this.dataSetQuery.QInsulatingLiquidTypes.QInsulatingLiquidTypesRowChanged += new DataSetQuery.QInsulatingLiquidTypesRowChangeEventHandler(QInsulatingLiquidTypes_QInsulatingLiquidTypesRowChanged);
            GridView.OptionsBehavior.Editable = false;

            if (m_bCanSelect)
            {
                cbCanEdit.Checked = true;
                panelSelect.Visible = true;
            }
            else
            {
                panelSelect.Visible = false;
            }
        }

        void QInsulatingLiquidTypes_QInsulatingLiquidTypesRowChanged(object sender, DataSetQuery.QInsulatingLiquidTypesRowChangeEvent e)
        {
            if (e.Action == DataRowAction.Add || e.Action == DataRowAction.Change)
            {
                if (m_bNeedCancel)
                {
                    m_bNeedCancel = false;
                    e.Row.RejectChanges();
                    return;
                }

                /*if (!m_bAcceptChanges)
                    e.Row.RejectChanges();
                else
                {*/
                    try
                    {
                        if (e.Action == DataRowAction.Change && m_bUpdateID)
                        {
                            this.dataSetQuery.QInsulatingLiquidTypes.AcceptChanges();
                            m_bUpdateID = false;
                            return;
                        }
                        else
                            using (var cmdBuilder = new SQLiteCommandBuilder(this.qInsulatingLiquidTypesTableAdapter.Adapter)) this.qInsulatingLiquidTypesTableAdapter.Adapter.Update(this.dataSetQuery.QInsulatingLiquidTypes);

                        if (e.Action == DataRowAction.Add)
                        {
                            SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                            connection.Open();
                            SQLiteCommand com = new SQLiteCommand(connection);
                            com.CommandText = "select seq from sqlite_sequence where name = 'InsulatingLiquidTypes'";
                            com.CommandType = CommandType.Text;
                            SQLiteDataReader dr = com.ExecuteReader();

                            long id = 0;
                            while (dr.Read())
                            {
                                id = Convert.ToInt64(dr["seq"]);
                            }
                            dr.Close();

                            m_bUpdateID = true;
                            ((DataRowView)(qInsulatingLiquidTypesBindingSource.Current)).Row["InsulatingLiquidTypeID"] = id;

                            connection.Close();
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
                //}
            }
        }

        void QInsulatingLiquidTypes_QInsulatingLiquidTypesRowDeleted(object sender, DataSetQuery.QInsulatingLiquidTypesRowChangeEvent e)
        {
            if (e.Action == DataRowAction.Delete)
            {
                if (!m_bAcceptChanges)
                {
                    e.Row.RejectChanges();
                }
                else
                {
                    try
                    {
                        using (var cmdBuilder = new SQLiteCommandBuilder(this.qInsulatingLiquidTypesTableAdapter.Adapter)) this.qInsulatingLiquidTypesTableAdapter.Adapter.Update(this.dataSetQuery.QInsulatingLiquidTypes);
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
            }
        }

        void QInsulatingLiquidTypes_QInsulatingLiquidTypesRowDeleting(object sender, DataSetQuery.QInsulatingLiquidTypesRowChangeEvent e)
        {
            try
            {
                if (e.Action == DataRowAction.Delete)
                {
                    if (MyLocalizer.XtraMessageBoxShow("Удалить запись?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        m_bAcceptChanges = false;
                        return;
                    }
                    else
                    {
                        long id = Convert.ToInt64(e.Row["InsulatingLiquidTypeID"]);

                        SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                        connection.Open();
                        SQLiteCommand com = new SQLiteCommand(connection);
                        com.CommandText = "Select COUNT(*) AS Cnt from CondenserTypes AS c WHERE c.InsulatingLiquidTypeID = ?";
                        com.CommandType = CommandType.Text;
                        SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.Int64);
                        param1.Value = id;
                        com.Parameters.Add(param1);
                        SQLiteDataReader dr = com.ExecuteReader();
                        while (dr.Read())
                        {
                            if (Convert.ToInt64(dr["Cnt"]) > 0)
                            {
                                MyLocalizer.XtraMessageBoxShow("Существуют типы конденсаторов, имеющие данную марку изоляционной жидкости.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                m_bAcceptChanges = false;
                                dr.Close();
                                connection.Close();
                                return;
                            }
                        }
                        dr.Close();

                        m_bAcceptChanges = true;

                        connection.Close();
                    }
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

        private void cbCanEdit_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCanEdit.Checked) GridView.OptionsBehavior.Editable = true;
            else GridView.OptionsBehavior.Editable = false;
        }

        private void SubjectView_KeyDown(object sender, KeyEventArgs e)
        {
            if (cbCanEdit.Checked)
            {
                if (e.KeyCode == Keys.Delete && qInsulatingLiquidTypesBindingSource.Current != null)
                {
                    ((DataRowView)(qInsulatingLiquidTypesBindingSource.Current)).Row.Delete();
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                //GridView.State
                if (!GridView.IsEditorFocused)
                {
                    Close();
                }                
            }
        }

        private void GridView_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
        {
            try
            {
                long id = 0;

                if (qInsulatingLiquidTypesBindingSource.Current == null) return;

                DataRowView row = (DataRowView)(qInsulatingLiquidTypesBindingSource.Current);

                /*if (MyLocalizer.XtraMessageBoxShow("Сохранить данные?", "Сообщение", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.Yes)
                {
                    e.ErrorText = "";
                    e.Valid = false;
                    return;
                }*/

                System.Windows.Forms.DialogResult res = MyLocalizer.XtraMessageBoxShow("Сохранить данные?", "Сообщение", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
                if (res == System.Windows.Forms.DialogResult.Cancel)
                {
                    e.ErrorText = "";
                    e.Valid = false;
                    return;
                }
                if (res == System.Windows.Forms.DialogResult.No)
                {
                    m_bNeedCancel = true;
                    return;
                }

                if (!row.IsNew)
                {
                    id = Convert.ToInt64(row["InsulatingLiquidTypeID"]);
                }

                string strName = row["InsulatingLiquidTypeName"].ToString();
                strName = strName.Trim();
                if (strName == "")
                {
                    e.ErrorText = "Необходимо указать наименование типа изоляционной жидкости.";
                    e.Valid = false;
                    return;
                }

                SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                connection.Open();
                SQLiteCommand com = new SQLiteCommand(connection);
                com.CommandText = "Select * from InsulatingLiquidTypes WHERE EQUAL_STR(InsulatingLiquidTypeName, ?) = 0 AND InsulatingLiquidTypeID <> ?";
                com.CommandType = CommandType.Text;
                SQLiteParameter param1 = new SQLiteParameter("@Param1", DbType.String);
                param1.Value = strName;
                SQLiteParameter param2 = new SQLiteParameter("@Param2", DbType.Int64);
                param2.Value = id;
                com.Parameters.Add(param1);
                com.Parameters.Add(param2);
                SQLiteDataReader dr = com.ExecuteReader();
                if (dr.HasRows)
                {
                    e.ErrorText = "Марка изоляционной жидкости с таким наименованием уже существует.";
                    e.Valid = false;
                    dr.Close();
                    connection.Close();
                    return;
                }
                dr.Close();
                connection.Close();
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

        private void GridView_InvalidRowException(object sender, DevExpress.XtraGrid.Views.Base.InvalidRowExceptionEventArgs e)
        {
            e.ExceptionMode = DevExpress.XtraEditors.Controls.ExceptionMode.NoAction;
            if (e.ErrorText != "")
                MyLocalizer.XtraMessageBoxShow(e.ErrorText, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void bSelect_Click(object sender, EventArgs e)
        {
            if (qInsulatingLiquidTypesBindingSource.Current != null)
            {
                m_SelectID = Convert.ToInt64(((DataRowView)(qInsulatingLiquidTypesBindingSource.Current)).Row["InsulatingLiquidTypeID"]);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                MyLocalizer.XtraMessageBoxShow("Необходимо выбрать запись", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
       
    }
}