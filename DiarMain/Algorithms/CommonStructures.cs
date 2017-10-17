using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;
using System.IO;

namespace Condenser
{
    class DataSourceString
    {
        public DataSourceString(long key, string strVal)
        {
            m_key = key;
            m_strVal = strVal;
        }

        private long m_key;
        private string m_strVal;

        public string VAL
        {
            get { return m_strVal; }
            set { m_strVal = value; }
        }

        public long KEY
        {
            get { return m_key; }
            set { m_key = value; }
        }
    };

    class DataSourceInt
    {
        public DataSourceInt(long key, long val)
        {
            m_key = key;
            m_val = val;
        }

        private long m_key;
        private long m_val;

        public long VAL
        {
            get { return m_val; }
            set { m_val = value; }
        }

        public long KEY
        {
            get { return m_key; }
            set { m_key = value; }
        }
    };

    public partial class CondenserTest
    {
        public enum CondenserTestType
        {
            None = 0,
            Resource = 1,
            Acceptance = 2,
            Operational = 3,
        };

        public enum CondenserTestState
        {
            Work = 0,
            NoWork = 1,
        };

        public enum ReportType
        {
            Common = 0,
            Resource = 1,
            Acceptance = 2,
            Operational = 3,
        };

        public enum FunctionType
        {
            Degree = 0,
            Exp = 1,
            Log = 2,
        };
    }

}

