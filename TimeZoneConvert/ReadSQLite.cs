﻿using System;
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
            string strTemp = AppDomain.CurrentDomain.BaseDirectory + @"TimeConvert.db";
            if (File.Exists(strTemp))
            {
                m_dbConnection = new SQLiteConnection($"Data Source={strTemp};Version=3;");
                m_dbConnection.Open();
                blGotDB = true;
            }
        }
        ~ReadSQLite()
        {
            // can't close this?
            //m_dbConnection.Close();
        }
        public bool GotDB()
        {
            return blGotDB;
        }
        public List<OutputFormat> GetOutputFormats()
        {
            List<OutputFormat> lsReturn = new List<OutputFormat>();
            string sql = "SELECT * FROM OutputFormat;";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                   // lsReturn.Add(Read[1],Read[2]);
                }
            }
            return lsReturn;
        }
    }
}
