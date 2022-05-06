using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Data.Services
{
    public class GetDataByDate
    {
        private DateTime dateFrom;
        private DateTime dateTo;
        private string tableName;
        public List<Dictionary<String, String>> GetData(string tableName, DateTime dateFrom, DateTime dateTo)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.tableName = tableName;
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String selectSql = $"SELECT * FROM {tableName} where DATETIME_KEY BETWEEN  '{dateFrom}' AND '{dateTo}'";
            OdbcCommand cmd = new OdbcCommand(selectSql, conn);
            conn.Open();
            OdbcDataReader reader = cmd.ExecuteReader();

            List<Dictionary<String, String>> data = new List<Dictionary<string, string>>();
            while (reader.Read())
            {
                Dictionary<String, String> row = new Dictionary<string, string>();
                row.Add("DateTime_key", reader.GetString(0));
                row.Add("Link", reader.GetString(1));
                row.Add("Slot", reader.GetString(2));
                row.Add("NeType", reader.GetString(3));
                row.Add("NeAlias", reader.GetString(4));
                row.Add("MAX_RX_LEVEL", reader.GetString(5));
                row.Add("MAX_TX_LEVEL", reader.GetString(6));
                row.Add("RSL_DEVIATION", reader.GetString(7));

                data.Add(row);

            }
            conn.Close();
            return data;
        }
    }
}
