using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace TimeZoneConvert
{

    class ReadSQLite
    {
        private SQLiteConnection m_dbConnection;
        private bool blGotDB = false;
        public ReadSQLite()
        {
            string strTemp = AppDomain.CurrentDomain.BaseDirectory + @"TimeConverter.db";
            if (File.Exists(strTemp))
            {
                m_dbConnection = new SQLiteConnection($"Data Source={strTemp};Version=3;");
                m_dbConnection.Open();
                blGotDB = true;
            }
        }
        public bool GotDB()
        {
            return blGotDB;
        }
    }
}
