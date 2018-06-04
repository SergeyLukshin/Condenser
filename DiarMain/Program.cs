using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress;
using DevExpress.Utils;
using System.Text;
using System.Management;
using System.Drawing;
using System.Data.SQLite;
using System.Data;
using System.IO;
using System.Reflection;
//using System.Linq;

namespace Condenser
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static Decimal m_fVersion = new Decimal(0);
        public static bool m_bExpertMode = false;
        public static List<string> listSkin = new List<string>();
        public static List<string> listDLL = new List<string>();

        public static string m_strVersion = "1.1";
        public static Decimal m_CurVersionDB = new Decimal(1.04);
        public static string m_strDateVersion = "23.05.2017";
        public static string m_strBuildNumber = "23";

        static void RegisterAssembly()
        {
            for (int i = 0; i < listSkin.Count; i++)
            {
                if (File.Exists(Environment.CurrentDirectory + "/" + listSkin[i]))
                {
                    Assembly skin = Assembly.LoadFile(Environment.CurrentDirectory + "/" + listSkin[i]);
                    DevExpress.Skins.SkinManager.Default.RegisterAssembly(skin);
                }
                else
                {
                    if (!File.Exists(Environment.CurrentDirectory + "/DLL/" + listSkin[i])) throw new Exception("Не удалось найти файл библиотеки \"" + listSkin[i] + "\"");
                    Assembly skin = Assembly.LoadFile(Environment.CurrentDirectory + "/DLL/" + listSkin[i]);
                    DevExpress.Skins.SkinManager.Default.RegisterAssembly(skin);
                }
            }

            for (int i = 0; i < listDLL.Count; i++)
            {
                if (!File.Exists(Environment.CurrentDirectory + "/" + listDLL[i])
                    && !File.Exists(Environment.CurrentDirectory + "/DLL/" + listDLL[i])) throw new Exception("Не удалось найти файл библиотеки \"" + listDLL[i] + "\"");
            }
        }

        static void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        static bool LoadParamData(string strFileName)
        {
            StreamReader sr = StreamReader.Null;
            try
            {
                sr = new StreamReader(strFileName);
                bool bReadKoef = false;
                bool bReadKIn = false;
                string strVal = "";
                string strSeparator = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
                string strCondenserType = "";
                long KoefID = 0;
                Decimal? fVal = null;
                if (sr != StreamReader.Null)
                {
                    SQLiteConnection connection = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    connection.Open();

                    byte[] key = Encoding.Unicode.GetBytes("ELCHROM");
                    RC4 decoder = new RC4(key);

                    while (!sr.EndOfStream)
                    {
                        string strData = sr.ReadLine();

                        // расшифровка
                        byte[] result = decoder.SetByteString(strData);
                        byte[] decryptedBytes = decoder.Decode(result, result.Length);
                        strData = Encoding.Unicode.GetString(decryptedBytes);


                        if (strData == "Data")
                        {
                            bReadKoef = true;
                            bReadKIn = false;
                            continue;
                        }
                        else
                        {
                            if (bReadKoef && strData == "KIn")
                            {
                                bReadKIn = true;
                                continue;
                            }

                            string[] vecData = strData.Split('\t');
                            if (bReadKoef)
                            {
                                try
                                {
                                    SQLiteCommand com = new SQLiteCommand(connection);
                                    com.CommandType = CommandType.Text;

                                    if (!bReadKIn)
                                    {
                                        Decimal? fKI1 = null, fkoefA = null, fkoefB = null, fkoefR2 = null;
                                        long iFunctionType = 0;

                                        fKI1 = null;
                                        fkoefA = null;
                                        fkoefB = null;
                                        fkoefR2 = null;
                                        iFunctionType = 0;

                                        //if (vecData.GetLength(0) != 16) throw new Exception("некорректный формат файла.");
                                        for (int j = 0; j < vecData.GetLength(0); j += 2)
                                        {
                                            strVal = vecData[j + 1];

                                            if (vecData[j] == "CondenserType")
                                                strCondenserType = vecData[j + 1];
                                            if (vecData[j] == "KI1" && strVal != "")
                                            {
                                                strVal = strVal.Replace(".", strSeparator);
                                                strVal = strVal.Replace(",", strSeparator);
                                                fKI1 = Convert.ToDecimal(strVal);
                                            }
                                            if (vecData[j] == "KoefA" && strVal != "")
                                            {
                                                strVal = strVal.Replace(".", strSeparator);
                                                strVal = strVal.Replace(",", strSeparator);
                                                fkoefA = Convert.ToDecimal(strVal);
                                            }
                                            if (vecData[j] == "KoefB" && strVal != "")
                                            {
                                                strVal = strVal.Replace(".", strSeparator);
                                                strVal = strVal.Replace(",", strSeparator);
                                                fkoefB = Convert.ToDecimal(strVal);
                                            }
                                            if (vecData[j] == "KoefR2" && strVal != "")
                                            {
                                                strVal = strVal.Replace(".", strSeparator);
                                                strVal = strVal.Replace(",", strSeparator);
                                                fkoefR2 = Convert.ToDecimal(strVal);
                                            }
                                            if (vecData[j] == "FunctionType" && strVal != "")
                                                iFunctionType = Convert.ToInt64(strVal);
                                        }

                                        if (fKI1 == null) throw new Exception("отсутствует значение в поле КЦ1.");

                                        com.CommandText = "SELECT (SELECT CondenserTypeID FROM CondenserTypes WHERE CondenserTypeName = @tname) AS CondenserTypeID, " +
                                            "(SELECT KoefID FROM CondenserTypeParameters WHERE CondenserTypeID = (SELECT CondenserTypeID FROM CondenserTypes WHERE CondenserTypeName = @tname)) AS KoefID";

                                        AddParam(com, "@tname", DbType.String, strCondenserType);

                                        SQLiteDataReader dr = com.ExecuteReader();

                                        long CondenserTypeID = 0;
                                        KoefID = 0;
                                        while (dr.Read())
                                        {
                                            if (dr["CondenserTypeID"] != DBNull.Value)
                                                CondenserTypeID = Convert.ToInt64(dr["CondenserTypeID"]);
                                            if (dr["KoefID"] != DBNull.Value)
                                                KoefID = Convert.ToInt64(dr["KoefID"]);
                                        }
                                        dr.Close();

                                        if (CondenserTypeID == 0) continue;

                                        com.Parameters.Clear();

                                        if (KoefID > 0)
                                            com.CommandText = "UPDATE CondenserTypeParameters SET KI1 = @ki1, KoefA = @koefA, KoefB = @koefB, KoefR2 = @koefR2, FunctionType = @ftype WHERE CondenserTypeID = @ctypeid";
                                        else
                                            com.CommandText = "INSERT INTO CondenserTypeParameters (CondenserTypeID, KI1, KoefA, KoefB, KoefR2, FunctionType) VALUES (@ctypeid, @ki1, @koefA, @koefB, @koefR2, @ftype)";

                                        AddParam(com, "@ctypeid", DbType.Int64, CondenserTypeID);

                                        AddParam(com, "@ki1", DbType.Decimal, fKI1);
                                        AddParam(com, "@koefA", DbType.Decimal, fkoefA);
                                        AddParam(com, "@koefB", DbType.Decimal, fkoefB);
                                        AddParam(com, "@koefR2", DbType.Decimal, fkoefR2);
                                        AddParam(com, "@ftype", DbType.Int64, iFunctionType);

                                        com.ExecuteNonQuery();

                                        if (KoefID == 0)
                                        {
                                            com.CommandText = "select seq from sqlite_sequence where name = 'CondenserTypeParameters'";
                                            com.Parameters.Clear();
                                            SQLiteDataReader dr_ = com.ExecuteReader();

                                            while (dr_.Read())
                                            {
                                                KoefID = Convert.ToInt64(dr_["seq"]);
                                            }
                                            dr_.Close();
                                        }
                                        else
                                        {
                                            com.CommandText = "DELETE FROM ParameterRecommendations where KoefID = @koefID";
                                            com.Parameters.Clear();
                                            AddParam(com, "@koefID", DbType.Int64, KoefID);
                                            com.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        long iPosition = 0;
                                        Decimal? fValue = null;
                                        string strRecommendation = "";
                                        string strConclusion = "";

                                        com.CommandText = "INSERT INTO ParameterRecommendations (KoefID, Position, Value, Recommendation, Conclusion) VALUES (@koefID, @pos, @val, @recom, @concl)";

                                        for (int j = 0; j < vecData.GetLength(0); j += 2)
                                        {
                                            strVal = vecData[j + 1];

                                            if (vecData[j] == "Pos")
                                                iPosition = Convert.ToInt64(vecData[j + 1]);
                                            if (vecData[j] == "Val" && strVal != "")
                                            {
                                                strVal = strVal.Replace(".", strSeparator);
                                                strVal = strVal.Replace(",", strSeparator);
                                                fValue = Convert.ToDecimal(strVal);
                                            }
                                            if (vecData[j] == "Recommendation")
                                                strRecommendation = strVal;
                                            if (vecData[j] == "Conclusion")
                                                strConclusion = strVal;
                                        }

                                        com.Parameters.Clear();
                                        AddParam(com, "@koefID", DbType.Int64, KoefID);
                                        AddParam(com, "@pos", DbType.Int64, iPosition);
                                        AddParam(com, "@val", DbType.Decimal, fValue);
                                        AddParam(com, "@recom", DbType.String, strRecommendation);
                                        AddParam(com, "@concl", DbType.String, strConclusion);

                                        com.ExecuteNonQuery();
                                    }
                                }
                                catch (SQLiteException ex)
                                {
                                    throw ex;
                                }
                            }
                            else
                            {
                                fVal = null;

                                if (vecData.GetLength(0) != 2) throw new Exception("некорректный формат файла.");
                                if (vecData[0] == "ПЧХ" && vecData[1] != "")
                                {
                                    strVal = vecData[1];
                                    strVal = strVal.Replace(".", strSeparator);
                                    strVal = strVal.Replace(",", strSeparator);

                                    fVal = Convert.ToDecimal(strVal);
                                    if (fVal == null) throw new Exception("отсутствует значение в поле ПЧХ.");
                                }

                                try
                                {
                                    SQLiteCommand com = new SQLiteCommand(connection);
                                    com.CommandText = "UPDATE CommonParameters SET ParameterValue = @pval WHERE ParameterName = @pname";
                                    com.CommandType = CommandType.Text;

                                    AddParam(com, "@pval", DbType.String, fVal.ToString());
                                    AddParam(com, "@pname", DbType.String, vecData[0]);

                                    com.ExecuteNonQuery();
                                }
                                catch (SQLiteException ex)
                                {
                                    throw ex;
                                }
                            }
                        }
                    }
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                if (sr != StreamReader.Null) sr.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при загрузке коэффициентов аппроксимации: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (sr != StreamReader.Null) sr.Close();
            return true;
        }

        static void LoadParameters()
        {
            // автоматическая загрузка файлов настроек
            string[] allFound = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dat", SearchOption.TopDirectoryOnly);
            List<string> allFoundFiles = new List<string>();

            for (int i = 0; i < allFound.Length; i++)
            {
                string strname = allFound[i] + "###";
            	if (strname.IndexOf(".dat###") >= 0)
                    allFoundFiles.Add(allFound[i]);
            }

            // ищем файл с самым поздним временем создания
            DateTime maxDateLastWrite = DateTime.MinValue;
            string strFileName = "";
            for (int i = 0; i < allFoundFiles.Count; i++)
            {
                try
                {
                    FileInfo fi = new FileInfo(allFoundFiles[i]);
                    if (fi != null)
                    {
                        DateTime dt = fi.LastWriteTimeUtc;
                        if (maxDateLastWrite < dt)
                        {
                            maxDateLastWrite = dt;
                            strFileName = allFoundFiles[i];
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ошибка при получении информации о файле файла
                }
            }
            if (strFileName != "")
            {
                // загружаем настройки
                bool bRes = LoadParamData(strFileName);
                if (bRes) MyLocalizer.XtraMessageBoxShow("Новые коэффициенты аппроксимации успешно загружены.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // удаляем файлы
            for (int i = 0; i < allFoundFiles.Count; i++)
            {
                try
                {
                    System.IO.File.Delete(allFoundFiles[i]);
                }
                catch (Exception ex)
                {
                    // ошибка при удалении файла
                }
            }
        }

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

                //AppDomain.CurrentDomain.SetData("DataDirectory", Environment.CurrentDirectory);

                /*AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
                {
                    // В вашем случае в args.Name будет лежать строка
                    // Oracle.ManagedDataAccess, Version=4.121.2.0, Culture=neutral, PublicKeyToken=89b483f429c47342
                    // Также сюда вам необходимо каким-то образом передать каталог, где лежит COM-компонент
                    // и зависимая сборка (например, считать через реестр: HKEY_CLASSES_ROOT\CLSID\{guid_вашего_компонента}\InprocServer32)
                    // Пусть comPath - каталог, где лежит ваша зависимая сборка

                    string depAssemblyFileName = new AssemblyName(args.Name).Name + ".dll";
                    string depAssemblyPath = Path.Combine("", depAssemblyFileName);
                    return Assembly.Load(depAssemblyPath);
                };*/

                //DevExpress.UserSkins.SkinFHA fha = new DevExpress.UserSkins.SkinFHA();
                //string str = fha.SkinInfo1[0];

                listSkin.Add("SkinMyButton.dll");
                /*listSkin.Add("SkinPassport.dll");
                listSkin.Add("SkinReport.dll");
                listSkin.Add("SkinAdd2.dll");*/

                listDLL.Add("DevExpress.BonusSkins.v11.1.dll");
                listDLL.Add("DevExpress.Data.v11.1.dll");
                listDLL.Add("DevExpress.OfficeSkins.v11.1.dll");
                listDLL.Add("DevExpress.Printing.v11.1.Core.dll");
                listDLL.Add("DevExpress.Utils.v11.1.dll");
                listDLL.Add("DevExpress.XtraBars.v11.1.dll");
                listDLL.Add("DevExpress.XtraEditors.v11.1.dll");
                listDLL.Add("DevExpress.XtraGrid.v11.1.dll");
                listDLL.Add("DevExpress.XtraLayout.v11.1.dll");
                listDLL.Add("DevExpress.XtraVerticalGrid.v11.1.dll");
                listDLL.Add("DevExpress.RichEdit.v11.1.Core.dll");
                listDLL.Add("DevExpress.XtraRichEdit.v11.1.dll");

                //string str = typeof(DevExpress.UserSkins.SkinFHA).ToString();
                RegisterAssembly();

                /*DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinHARG).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinVibro).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinVisual).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinParameter).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinWarm).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinPassport).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinReport).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinAdd).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinAdd2).Assembly); //Register!
                DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.SkinElectrical).Assembly); //Register!*/

                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.UserSkins.OfficeSkins.Register();

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "DB\\Condenser.sqlite"))
                {
                    Properties.Settings.Default["condenserConnectionString"] = "data source=" + AppDomain.CurrentDomain.BaseDirectory + "DB\\Condenser.sqlite";
                }
                else
                {
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Condenser.sqlite"))
                    {
                        Properties.Settings.Default["condenserConnectionString"] = "data source=" + AppDomain.CurrentDomain.BaseDirectory + "Condenser.sqlite";
                    }
                    else
                    {
                        MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: отсутствует файл Condenser.sqlite.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                AppearanceObject.DefaultFont = new Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));

                // -----------------
                // проверка лицензии
                // -----------------

                string testString = GetUniqueHardwaeId();

                byte[] key = ASCIIEncoding.ASCII.GetBytes("DIAR");

                RC4 encoder = new RC4(key);
                byte[] testBytes = ASCIIEncoding.ASCII.GetBytes(testString);
                byte[] result = encoder.Encode(testBytes, testBytes.Length);
                string encryptedString = encoder.GetByteString(result);// ASCIIEncoding.ASCII.GetString(result);

                SQLiteConnection con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                bool bNoLicense = true;
                string strLicenseCode = "";

                try
                {
                    con.Open();

                    SQLiteCommand com = new SQLiteCommand(con);
                    com.CommandType = CommandType.Text;

                    com.CommandText = "Select Code FROM Licenses WHERE Code = '" + encryptedString + "'";

                    SQLiteDataReader dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        bNoLicense = false;
                        strLicenseCode = dr["Code"].ToString();
                    }
                    dr.Close();

                    con.Close();
                }
                catch (SQLiteException ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (bNoLicense)
                {
                    LicenseForm f = new LicenseForm();
                    f.m_strCode = testString;

                    Application.Run(f);
                }
                else
                {
                    try
                    {
                        con.Open();
                        SQLiteCommand com = new SQLiteCommand(con);
                        com.CommandText = "Select CurVersion from Version";
                        com.CommandType = CommandType.Text;
                        SQLiteDataReader dr = com.ExecuteReader();

                        //Decimal curVersion = new decimal(0);
                        while (dr.Read())
                        {
                            m_fVersion = Convert.ToDecimal(dr.GetValue(0));
                        }
                        dr.Close();
                        con.Close();

                        ServicePackForm sp = new ServicePackForm();
                        if (m_fVersion < Program.m_CurVersionDB)
                        {
                            sp.m_DBVersion = m_fVersion;
                            if (sp.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                return;
                            };
                        }

                        con.Close();
                    }
                    catch (SQLiteException ex)
                    {
                        MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    LoadParameters();

                    Application.Run(new MainForm());
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "(" + ex.StackTrace + ")", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show("Неизвестная ошибка от объекта " + e.ExceptionObject.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            //throw new NotImplementedException();
            MessageBox.Show(e.Exception.Message + "(" + e.Exception.StackTrace + ")", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
        }

        private static string GetUniqueHardwaeId()
        {
            /*Dictionary<string, string> ids =
            new Dictionary<string, string>();

            ManagementObjectSearcher searcher;

            //процессор
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("ProcessorId", queryObj["ProcessorId"].ToString());

            //мать
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM CIM_Card");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("CardID", queryObj["SerialNumber"].ToString());

            //клавиатура
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM CIM_KeyBoard");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("KeyBoardID", queryObj["DeviceId"].ToString());

            //ОС
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM CIM_OperatingSystem");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("OSSerialNumber", queryObj["SerialNumber"].ToString());

            //мышь
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_PointingDevice");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("MouseID", queryObj["DeviceID"].ToString());

            //звук
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("SoundCardID", queryObj["DeviceID"].ToString());

            //CD-ROM
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_CDROMDrive");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("CDROMID", queryObj["DeviceID"].ToString());

            //UUID
            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT UUID FROM Win32_ComputerSystemProduct");
            foreach (ManagementObject queryObj in searcher.Get())
                ids.Add("UUID", queryObj["UUID"].ToString());

            foreach (var x in ids)
                richTextBox1.Text += x.Key + ": " + x.Value + "\r\n";*/

            /*StringBuilder sb = new StringBuilder();

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2",
                  "SELECT * FROM Win32_Processor");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["NumberOfCores"]);
                sb.Append(queryObj["ProcessorId"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["SocketDesignation"]);

                //Console.WriteLine(queryObj["ProcessorId"]);
                //Console.WriteLine(queryObj["Name"]);
                //Console.WriteLine(queryObj["SocketDesignation"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_BIOS");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Manufacturer"]);
                sb.Append(queryObj["Name"]);
                sb.Append(queryObj["Version"]);

                //Console.WriteLine(queryObj["Manufacturer"]);
                //Console.WriteLine(queryObj["Name"]);
                //Console.WriteLine(queryObj["Version"]);
            }

            searcher = new ManagementObjectSearcher("root\\CIMV2",
                   "SELECT * FROM Win32_BaseBoard");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                sb.Append(queryObj["Product"]);
                //Console.WriteLine(queryObj["Product"]);
            }

            var bytes = Encoding.ASCII.GetBytes(sb.ToString());
            SHA256Managed sha = new SHA256Managed();

            byte[] hash = sha.ComputeHash(bytes);

            return BitConverter.ToString(hash);*/

            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT UUID FROM Win32_ComputerSystemProduct");

            foreach (ManagementObject queryObj in searcher.Get())
                return queryObj["UUID"].ToString();

            return "";
        }
    }
}
