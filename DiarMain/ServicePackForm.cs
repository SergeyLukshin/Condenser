using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace Condenser
{
    public partial class ServicePackForm : DevExpress.XtraEditors.XtraForm
    {
        [SQLiteFunction(Arguments = 2, FuncType = FunctionType.Scalar, Name = "EQUAL_STR")]
        class EQUAL_STR : SQLiteFunction
        {
            public override object Invoke(object[] args)
            {
                string s1 = (args[0] as string).ToLower();
                string s2 = (args[1] as string).ToLower();
                return s1 == s2 ? 0 : 1;
            }
        }

        public class ServicePackInfo            
        {
            public ServicePackInfo(Decimal Version_, string strDescript, string strCommand)
            {
                m_Version = Version_;
                m_strDescript = strDescript;
                m_strCommand = strCommand;
            }
            public Decimal m_Version;
            public string m_strDescript;
            public string m_strCommand;
        }
        
        //public Decimal m_CurVersion = new Decimal(1.01);
        public Decimal m_DBVersion = new Decimal(0);
        //public string m_strDateVersion = "13.03.2017";
        public List<ServicePackInfo> m_full_list = new List<ServicePackInfo>();
        public Dictionary<Decimal, List<ServicePackInfo>> m_list = new Dictionary<Decimal, List<ServicePackInfo> >();
        int iCntRequests = 0;
        private bool m_bEnd = false;
        private bool m_bSuccess = false;

        public ServicePackForm()
        {
            InitializeComponent();
        }

        private void bActivation_Click(object sender, EventArgs e)
        {
            bRefresh.Enabled = false;
            bCancel.Enabled = false;

            worker.RunWorkerAsync();
        }

        private void ServicePackForm_Load(object sender, EventArgs e)
        {
            lCaptionVersion.Text = "Доступна версия базы данных " + Program.m_CurVersionDB.ToString().Replace(",", ".") + " от " + Program.m_strDateVersion;

            m_full_list.Add(new ServicePackInfo(new Decimal(1.01), "Обновление структуры и данных (1.01.1)",
                "ALTER TABLE Condensers ADD CondenserTestType INTEGER;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.01), "Обновление структуры и данных (1.01.2)",
                "UPDATE Condensers SET CondenserTestType = (SELECT CondenserTestType FROM CondenserTest " +
                        "WHERE CondenserTest.CondenserID = Condensers.CondenserID ORDER BY CondenserTestDate DESC LIMIT 1);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.02), "Обновление структуры и данных (1.02.1)",
                "ALTER TABLE CondenserTypeParameters ADD CondenserParameterType INTEGER;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.02), "Обновление структуры и данных (1.02.2)",
                "ALTER TABLE CondenserTypeParameters ADD FunctionType INTEGER;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.02), "Обновление структуры и данных (1.02.3)",
                "UPDATE CondenserTypeParameters SET FunctionType = 0;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.03), "Обновление структуры и данных (1.03.1)",
                "DELETE FROM CondenserTypeParameters WHERE CondenserTypeID IS NULL;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.03), "Обновление структуры и данных (1.03.2)",
                "FROM FILE UpdateData_1_03_2"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.03), "Обновление структуры и данных (1.03.3)",
                "CREATE TABLE ParameterRecommendations " + 
	            "(ID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, " + 
	            "KoefID INTEGER REFERENCES CondenserTypeParameters (KoefID) ON DELETE CASCADE ON UPDATE CASCADE NOT NULL, " +
                "Position INTEGER NOT NULL, " +
	            "Value NUMERIC (20, 6), " +
                "Conclusion NVARCHAR (2000), " +
                "Recommendation NVARCHAR (2000));"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.1)",
                "ALTER TABLE CondenserTypeParameters ADD KoefR2 NUMERIC(20, 6);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.2)",
                "ALTER TABLE CondenserTest ADD CO2_CO NUMERIC(20, 6);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.3)",
                "ALTER TABLE CondenserTest ADD CO2_CO_Vabs NUMERIC(20, 6);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.4)",
                "ALTER TABLE CondenserTest ADD CO2_CO_Votn NUMERIC(20, 6);"));

            /*m_full_list.Add(new ServicePackInfo(new Decimal(1.02), "Обновление структуры и данных (1.02.1)", 
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (55, 1, 4, 94, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (56, 3, 4, 94, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (57, 1, 4, 134, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (58, 3, 4, 134, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (59, 1, 4, 130, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (60, 3, 4, 130, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (61, 1, 4, 126, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (62, 3, 4, 126, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (63, 1, 4, 122, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (64, 3, 4, 122, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (65, 1, 4, 118, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (66, 3, 4, 118, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (67, 1, 4, 114, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, ParameterID, EquipmentKindID) VALUES (68, 3, 4, 114, 1);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.03), "Обновление структуры и данных (1.03.1)",
                "UPDATE ParameterExcludes SET ExcludeType = 5 WHERE ParameterID = 134;" +
                "UPDATE ParameterExcludes SET ExcludeType = 6 WHERE ParameterID = 130;" +
                "UPDATE ParameterExcludes SET ExcludeType = 7 WHERE ParameterID = 126;" +
                "UPDATE ParameterExcludes SET ExcludeType = 8 WHERE ParameterID = 122;" +
                "UPDATE ParameterExcludes SET ExcludeType = 9 WHERE ParameterID = 118;" +
                "UPDATE ParameterExcludes SET ExcludeType = 10 WHERE ParameterID = 114;" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (69, 3, 4, 4, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (70, 3, 5, 29, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (71, 3, 6, 30, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (72, 3, 7, 31, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (73, 3, 8, 32, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (74, 3, 9, 33, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (75, 3, 10, 34, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (76, 2, 4, 3, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (77, 2, 5, 23, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (78, 2, 6, 24, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (79, 2, 7, 25, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (80, 2, 8, 26, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (81, 2, 9, 27, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (82, 2, 10, 28, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (83, 3, 4, 3, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (84, 3, 5, 23, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (85, 3, 6, 24, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (86, 3, 7, 25, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (87, 3, 8, 26, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (88, 3, 9, 27, 1);" +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, EquipmentKindID) VALUES (89, 3, 10, 28, 1);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.1)",
                "UPDATE Parameters SET ParameterDescript = 'Класс промышленной частоты, ед.', ParameterType = 4, MinValue = 0, MaxValue = 17 WHERE ParameterDescript = 'Наличие механических примесей, КПЧ';"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.2)",
                "UPDATE SystemItems SET SystemItemName = 'ВН фаза A' WHERE SystemItemName = 'Внутренняя изоляция (ВН фаза A)';" +
                "UPDATE SystemItems SET SystemItemName = 'ВН фаза B' WHERE SystemItemName = 'Внутренняя изоляция (ВН фаза B)';" +
                "UPDATE SystemItems SET SystemItemName = 'ВН фаза C' WHERE SystemItemName = 'Внутренняя изоляция (ВН фаза C)';" +
                "UPDATE SystemItems SET SystemItemName = 'СН фаза A' WHERE SystemItemName = 'Внутренняя изоляция (СН фаза A)';" +
                "UPDATE SystemItems SET SystemItemName = 'СН фаза B' WHERE SystemItemName = 'Внутренняя изоляция (СН фаза B)';" +
                "UPDATE SystemItems SET SystemItemName = 'СН фаза C' WHERE SystemItemName = 'Внутренняя изоляция (СН фаза C)';" +
                "UPDATE SystemItems SET SystemItemName = 'Нейтраль' WHERE SystemItemName = 'Внутренняя изоляция (нейтраль)';"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.04), "Обновление структуры и данных (1.04.3)",
                "DELETE FROM ParameterLimits WHERE AlgorithmID = 2;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.1)",
                "FROM FILE FunctionalSystemResults"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.2)",
                "FROM FILE Inspections"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.3)",
                "FROM FILE Inputs"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.4)",
                "INSERT INTO EquipmentKinds (EquipmentKindID, EquipmentKindName) VALUES (2, 'Выключатели воздушные');" +
                "INSERT INTO EquipmentKinds (EquipmentKindID, EquipmentKindName) VALUES (3, 'Выключатели маломасляные');" +
                "INSERT INTO EquipmentKinds (EquipmentKindID, EquipmentKindName) VALUES (4, 'Выключатели масляные баковые');" +
                "UPDATE EquipmentKinds SET ReadOnly = 1;"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.5)",
                "INSERT INTO EquipmentClasses (EquipmentKindID, EquipmentClassName, EquipmentClassSort, ReadOnly) VALUES (2, 'Выключатель воздушный', 3, 1);" +
                "INSERT INTO EquipmentClasses (EquipmentKindID, EquipmentClassName, EquipmentClassSort, ReadOnly) VALUES (3, 'Выключатель маломасляный', 4, 1);" +
                "INSERT INTO EquipmentClasses (EquipmentKindID, EquipmentClassName, EquipmentClassSort, ReadOnly) VALUES (4, 'Выключатель масляный баковый', 5, 1);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.6)",
                "CREATE TABLE SwitchDriveTypes ( " +
                    "SwitchDriveTypeID    INTEGER        PRIMARY KEY AUTOINCREMENT NOT NULL, " +
                    "EquipmentKindID INTEGER        NOT NULL, " +
                    "SwitchDriveTypeName  NVARCHAR (128) NOT NULL, " +
                    "ReadOnly        INTEGER        NOT NULL DEFAULT 0, " +
                    "FOREIGN KEY (EquipmentKindID) REFERENCES EquipmentKinds (EquipmentKindID) );"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.7)",
                "FROM FILE Equipments"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.8)",
                "DROP TABLE NominalPowers"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.9)",
                "FROM FILE UpdateData1"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.10)",
                "FROM FILE UpdateData2"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.11)",
                "FROM FILE UpdateData3"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.12)",
                "FROM FILE UpdateData4"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.13)",
                "FROM FILE UpdateData5"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.14)",
                "FROM FILE UpdateData6"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.15)",
                "ALTER TABLE SystemItems ADD UseInParameter INTEGER; " +
                "ALTER TABLE SystemItems ADD AlgorithmParameter NVARCHAR(128); " +
                "ALTER TABLE EquipmentKinds ADD Disable INTEGER; " +
                "UPDATE EquipmentKinds SET Disable = 0 WHERE EquipmentKindID = 1; " +
                "UPDATE EquipmentKinds SET Disable = 1 WHERE EquipmentKindID <> 1; " +
                "UPDATE SystemItems SET UseInParameter = 0; " +
                "UPDATE SystemItems SET AlgorithmParameter = '';"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.16)",
                "FROM FILE UpdateData7"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.05), "Обновление структуры и данных (1.05.17)", 
                "INSERT INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterName, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, ParameterAlgorithm, Sort, EquipmentKindID) " +
                "VALUES (302, 1, 17, NULL, 'Газ, скорость нарастания концентрации которого максимальна', 'Votn_gas', 2, '', '', '', '', 'calc_votngas', 15, 1); " +
                "INSERT INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterName, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, ParameterAlgorithm, Sort, EquipmentKindID) " +
                "VALUES (303, 1, 18, NULL, 'Газ, скорость нарастания концентрации которого максимальна', 'Votn_gas', 2, '', '', '', '', 'calc_votngas', 15, 1); " +
                "UPDATE Parameters SET ParameterDescript = 'Относительная скорость нарастания концентрации газа (максимальная из скоростей), %/мес.' WHERE ParameterID IN (48, 158);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.06), "Обновление структуры и данных (1.06.1)",
               "ALTER TABLE SystemItems ADD UseInElectrical INTEGER; " +
               "ALTER TABLE SystemItems ADD AlgorithmElectrical NVARCHAR(128); " +
               "UPDATE SystemItems SET UseInElectrical = 0; " +
               "UPDATE SystemItems SET AlgorithmElectrical = '';"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.06), "Обновление структуры и данных (1.06.2)",
                "FROM FILE UpdateData_1_06_2"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.06), "Обновление структуры и данных (1.06.3)",
                "CREATE TABLE Settings ( SettingID INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, MainImage IMAGE ); "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.06), "Обновление структуры и данных (1.06.4)",
                "INSERT INTO Settings (SettingID) VALUES (1);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.07), "Обновление структуры и данных (1.07.1)",
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (132, 1, 4, NULL, 260, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (133, 3, 4, NULL, 260, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (134, 1, 5, NULL, 264, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (135, 3, 5, NULL, 264, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (136, 1, 6, NULL, 268, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (137, 3, 6, NULL, 268, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (138, 1, 7, NULL, 272, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (139, 3, 7, NULL, 272, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (140, 1, 8, NULL, 276, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (141, 3, 8, NULL, 276, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (142, 1, 9, NULL, 280, 4); " +
                "INSERT INTO ParameterExcludes (ParameterExcludeID, ExcludeObjectID, ExcludeType, SystemItemID, ParameterID, EquipmentKindID) VALUES (143, 3, 9, NULL, 280, 4);"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.1)",
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (331, 5, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 2); " +
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (332, 5, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 3); " +
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (333, 5, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 4); " +
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (334, 6, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 2); " +
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (335, 6, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 3); " +
                "INSERT OR REPLACE INTO Parameters (ParameterID, InspectionType, SystemItemID, InspectionSubType, ParameterDescript, ParameterType, ParameterSelect1, ParameterSelect2, ParameterSelect3, ParameterSelect4, Sort, EquipmentKindID) " +
                "VALUES (336, 6, NULL, NULL, 'Примечание', 2, '', '', '', '', 100, 4); "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.2)",
                "UPDATE Parameters SET ParameterSelect3 = '', ParameterSelect2 = 'Выявлено' WHERE ParameterID = 228; " +
                "UPDATE Parameters SET ParameterSelect4 = 'Давление за границами допустимого диапазона. Давление не может быть отрегулировано без вывода оборудования из работы' WHERE ParameterID = 237; " +
                "UPDATE Parameters SET ParameterDescript = 'Класс промышленной чистоты, ед.' WHERE ParameterDescript LIKE '%Класс промышленной%'; " +
                "UPDATE SystemItems SET SystemItemName = 'Бак выключателя' WHERE SystemItemID = 51; "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.3)",
                "FROM FILE UpdateData_1_08_3"));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.4)",
                "DELETE FROM ParameterLimits WHERE ParameterLimitID IN (SELECT ParameterLimits.ParameterLimitID FROM ParameterLimits " + 
                "INNER JOIN Parameters ON Parameters.ParameterID = ParameterLimits.ParameterID " + 
                "WHERE InspectionType = 1 AND Parameters.SystemItemID IS NULL AND Parameters.EquipmentKindID <> 1); "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.5)",
                "UPDATE ParameterLimits SET SystemItemID = 17 WHERE ParameterLimitID IN " +
                "(SELECT ParameterLimits.ParameterLimitID FROM ParameterLimits " +
                "INNER JOIN Parameters ON Parameters.ParameterID = ParameterLimits.ParameterID " +
                "WHERE InspectionType = 1 AND Parameters.SystemItemID IS NULL AND Parameters.EquipmentKindID = 1); "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.6)",
                "INSERT INTO ParameterLimits (ParameterLimitID, ParameterID, AlgorithmID, VoltageMin, VoltageMax, ValueMin, ValueMax, SystemItemID) " + 
                "SELECT ParameterLimits.ParameterLimitID + 1310, ParameterLimits.ParameterID, ParameterLimits.AlgorithmID, ParameterLimits.VoltageMin, ParameterLimits.VoltageMax,  " + 
                "ParameterLimits.ValueMin, ParameterLimits.ValueMax, 18 FROM ParameterLimits " +
                "INNER JOIN Parameters ON Parameters.ParameterID = ParameterLimits.ParameterID  " + 
                "WHERE InspectionType = 1 AND Parameters.SystemItemID IS NULL AND Parameters.EquipmentKindID = 1; "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.7)",
               "ALTER TABLE Manufacturers ADD EquipmentKindID INTEGER; " +
               "ALTER TABLE ManufacturersInputs ADD EquipmentKindID INTEGER; "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.08), "Обновление структуры и данных (1.08.8)",
               "UPDATE Manufacturers SET EquipmentKindID = 1; " +
               "UPDATE ManufacturersInputs SET EquipmentKindID = 1; "));

            m_full_list.Add(new ServicePackInfo(new Decimal(1.09), "Обновление структуры и данных (1.09.1)",
               "UPDATE Equipments SET RPNCnt = 1 WHERE RPNCnt = 3 AND COALESCE(ConstructionType, -1) <> 3; " +
               "UPDATE Equipments SET RPNVoltage = 35 WHERE RPNCnt >= 1 AND RPNVoltage IS NULL; "));
            */
            iCntRequests = 0;
            for (int i = 0; i < m_full_list.Count; i++)
            {
                if (m_full_list[i].m_Version > m_DBVersion)
                {
                    if (!m_list.ContainsKey(m_full_list[i].m_Version)) m_list[m_full_list[i].m_Version] = new List<ServicePackInfo>();
                    m_list[m_full_list[i].m_Version].Add(new ServicePackInfo(m_full_list[i].m_Version, m_full_list[i].m_strDescript, m_full_list[i].m_strCommand));
                    iCntRequests++;
                }
            }

            /*
            Переименованиe столбца temp на new в тaблице Temper можно реализовать так:
            
            BEGIN TRANSACTION;
            -- создаем временную таблицу
            CREATE TEMPORARY TABLE Temper_backup(name,temp);
            -- копируем данные из таблицы Temper во временную таблицу Temper_backup
            INSERT INTO Temper_backup SELECT name,temp FROM Temper;
            -- удаляем таблицу Temper
            DROP TABLE Temper;
            -- создаем таблицу Temper
            CREATE TABLE Temper(name,new);
            -- вставляем данные из таблицы Temper_backup в таблицу Temper
            INSERT INTO Temper SELECT name,temp FROM Temper_backup;
            -- удаляем таблицу Temper_backup
            DROP TABLE Temper_backup;
            COMMIT;

            Этим же способом можно изменить тип поля.

            Удалить столбец temp в тaблице Temper можно так:
            BEGIN TRANSACTION;
            -- создаем временную таблицу Temper
            CREATE TEMPORARY TABLE Temper_backup(name);
            -- копируем данные из таблицы Temper во временную таблицу Temper_backup
            INSERT INTO Temper_backup SELECT name FROM Temper;
            -- удаляем таблицу Temper
            DROP TABLE Temper;
            -- создаем таблицу Temper
            CREATE TABLE Temper(name);
            -- вставляем данные из таблицы Temper_backup в таблицу Temper
            INSERT INTO Temper SELECT name FROM Temper_backup;
            -- удаляем таблицу Temper_backup
            DROP TABLE Temper_backup;
            COMMIT;
            */
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            m_bSuccess = false;
            SQLiteConnection con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
            SQLiteTransaction tran = null;
            try
            {
                con.Open();
                SQLiteCommand com = new SQLiteCommand(con);

                foreach (KeyValuePair<Decimal, List<ServicePackInfo>> key in m_list)
                {
                    tran = con.BeginTransaction();
                    for (int i = 0; i < key.Value.Count; i++)
                    {
                        worker.ReportProgress(i, key.Value[i].m_strDescript);

                        System.Threading.Thread.Sleep(500);

                        if (key.Value[i].m_strCommand.IndexOf("FROM FILE") >= 0)
                        {
                            string strFileName = key.Value[i].m_strCommand;
                            string strVersion = key.Value[i].m_Version.ToString();
                            strFileName = AppDomain.CurrentDomain.BaseDirectory + "ServicePack\\" + strVersion.Replace(",", ".") + "\\" + strFileName.Replace("FROM FILE", "").Trim();
                            if (!File.Exists(strFileName))
                            {
                                tran.Rollback();
                                con.Close();
                                MyLocalizer.XtraMessageBoxShow("Не удалось найти файл " + strFileName, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            FileStream fs = File.OpenRead(strFileName);
                            StreamReader srFile = new StreamReader(fs);
                            com.CommandText = srFile.ReadToEnd();
                        }
                        else
                            com.CommandText = key.Value[i].m_strCommand;
                        com.CommandType = CommandType.Text;
                        com.ExecuteNonQuery();
                    }

                    com.CommandText = "UPDATE Version SET CurVersion = @ver";
                    com.CommandType = CommandType.Text;

                    com.Parameters.Clear();
                    AddParam(com, "@ver", DbType.Decimal, key.Key);
                    com.ExecuteNonQuery();

                    tran.Commit();
                }

                worker.ReportProgress(iCntRequests + 1, "");
                System.Threading.Thread.Sleep(500);

                con.Close();

                m_bSuccess = true;
            }
            catch (SQLiteException ex)
            {
                if (tran != null) tran.Rollback();
                con.Close();
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void AddParam(SQLiteCommand com, string name, DbType type, object value)
        {
            SQLiteParameter param = new SQLiteParameter(name, type);
            param.Value = value;
            com.Parameters.Add(param);
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (m_bSuccess)
                {
                    /*SQLiteConnection con = new SQLiteConnection(global::Condenser.Properties.Settings.Default.condenserConnectionString);
                    con.Open();
                    SQLiteCommand com = new SQLiteCommand(con);
                    com.CommandText = "UPDATE Version SET CurVersion = @ver";
                    com.CommandType = CommandType.Text;

                    AddParam(com, "@ver", DbType.Decimal, Program.m_CurVersionDB);
                    com.ExecuteNonQuery();*/

                    m_bEnd = true;
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                else
                {
                    m_bEnd = true;
                    DialogResult = System.Windows.Forms.DialogResult.Cancel;
                    Close();
                }
            }
            catch (SQLiteException ex)
            {
                MyLocalizer.XtraMessageBoxShow("Ошибка при работе с базой данных. Описание: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            lCaption.Text = e.UserState.ToString();
            progress.Position = (int)((e.ProgressPercentage + 1) * 100 / (float)(iCntRequests + 1));
        }

        private void ServicePackForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_bEnd) e.Cancel = true;
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            m_bEnd = true;
        }
    }
}