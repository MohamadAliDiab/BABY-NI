using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MiniProject.Data.Services;
using System.Data.Odbc;

namespace MiniProject.Data
{
    public class ParserWatcher
    {
        private static FileSystemWatcher watcher = new FileSystemWatcher();

        public static void InitFileWatcher()
        {
            watcher.Path = @"C:\Users\User\Desktop\mini project\toBeParsed";
            watcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;



            watcher.Changed += OnChanged;
            watcher.Filter += "*.txt";

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Finished initing");
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("change detected");

            string filePath = e.FullPath;
            string fileName = e.Name;
            fileName = fileName.Substring(0, fileName.Length - 3);
            fileName = fileName + "csv";
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String selectSql = "SELECT * FROM logger where file_name = ?";
            OdbcCommand cmd = new OdbcCommand(selectSql, conn);
            conn.Open();
            cmd.Parameters.Add("file_name", OdbcType.VarChar).Value = fileName;
            var check = cmd.ExecuteNonQuery().ToString();
            conn.Close();
            if (check == "0")
            {
                Parser Parser = new Parser(filePath, fileName);
            }
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
        }
    }

}