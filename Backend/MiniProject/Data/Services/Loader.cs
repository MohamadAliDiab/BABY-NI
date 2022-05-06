using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Data.Services
{
    public class Loader
    {
        private string filePath;
        private string fileName;
        private readonly string finishedPath = "C:\\Users\\User\\Desktop\\mini project\\Loaded\\";
        public Loader(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
            Load();
        }
        public void Load()
        {
            //var csvConfig = new CsvConfiguration(CultureInfo.CurrentCulture)
            //{
            //    HasHeaderRecord = false
            //};

            //using var streamReader = File.OpenText(filePath);
            //using var csvReader = new CsvReader(streamReader, csvConfig);
            //using (FileStream f = File.Open(filePath, FileMode.Open))
            //{
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String copySql = $"COPY TRANS_MW_ERC_PM_TN_RADIO_LINK_POWER FROM LOCAL '{filePath}' DELIMITER ',';";
            OdbcCommand cmd = new OdbcCommand(copySql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
            String logLoad = $"UPDATE logger SET loaded = 'true' WHERE file_name = '{fileName}' ";
            OdbcCommand cmd2 = new OdbcCommand(logLoad, conn);
            cmd2.ExecuteNonQuery();
            conn.Close();

            File.Move(this.filePath, this.finishedPath + this.fileName);
            Aggregator Aggregator = new Aggregator(fileName);

        }
    }
}
