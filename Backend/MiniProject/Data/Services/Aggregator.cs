using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Data.Services
{
    public class Aggregator
    {
        private string fileName;
        public Aggregator(string fileName)
        {
            this.fileName = fileName;
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String selectSql = $"SELECT aggregated FROM logger where file_name = '{fileName}'";
            OdbcCommand cmd = new OdbcCommand(selectSql, conn);
            conn.Open();
            var check = cmd.ExecuteScalar().ToString();
            conn.Close();
            if (check == "False")
            {
                Aggregate();
            }
        }

        public void Aggregate()
        {
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String createDailyTable = "CREATE TABLE aggregate_daily(DateTime_key DateTime,"
            + "Link varchar(64),"
            + "Slot varchar(64),"
            + "NeType varchar(64),"
            + "NeAlias varchar(64),"
            + "MAX_RX_LEVEL float,"
            + "MAX_TX_LEVEL float,"
            + "RSL_DEVIATION float);"
            + "INSERT INTO aggregate_daily "
            + "SELECT date_trunc('day', DATETIME_KEY) as DateTime_key, LINK, SLOT, NETYPE, NEALIAS, "
            + "MAX(MAXRXLEVEL) as MAX_RX_LEVEL, MAX(MAXTXLEVEL) as MAX_TX_LEVEL, ABS(MAX(MAXRXLEVEL) - MAX(MAXTXLEVEL)) FROM TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER GROUP BY 1,2,3,4,5;";
            OdbcCommand cmd = new OdbcCommand(createDailyTable, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            String createHourlyTable = "CREATE TABLE aggregate_hourly(DateTime_key DateTime,"
            + "Link varchar(64),"
            + "Slot varchar(64),"
            + "NeType varchar(64),"
            + "NeAlias varchar(64),"
            + "MAX_RX_LEVEL float,"
            + "MAX_TX_LEVEL float,"
            + "RSL_DEVIATION float);"
            + "INSERT INTO aggregate_hourly "
            + "SELECT date_trunc('hour', DATETIME_KEY) as DateTime_key, LINK, SLOT, NETYPE, NEALIAS, "
            + $"MAX(MAXRXLEVEL) as MAX_RX_LEVEL, MAX(MAXTXLEVEL) as MAX_TX_LEVEL, ABS(MAX(MAXRXLEVEL) - MAX(MAXTXLEVEL)) FROM TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER GROUP BY 1,2,3,4,5;";
            OdbcCommand cmd2 = new OdbcCommand(createHourlyTable, conn);
            cmd2.ExecuteNonQuery();
            String logAgg = $"UPDATE logger SET aggregated = 'true' WHERE file_name = '{fileName}' ";
            OdbcCommand cmd3 = new OdbcCommand(logAgg, conn);
            cmd3.ExecuteNonQuery();
            conn.Close();
        }
    }
}
