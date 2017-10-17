namespace Condenser.DataSetQueryTableAdapters
{


    /// <summary>
    ///Represents the connection and commands used to retrieve and save data.
    ///</summary>
    public partial class QMainEquipmentsTableAdapter : global::System.ComponentModel.Component
    {

        public void SetCommandText(string strCommand)
        {
            CommandCollection[0].CommandText = strCommand;
        }
    }

}