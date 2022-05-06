using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using MiniProject.Data.Services;
using System.Data.Odbc;

namespace MiniProject.Data
{
    public class LoaderFileWatcher
    {
        private static FileSystemWatcher watcher = new FileSystemWatcher();

        public static void secondFileWatcher()
        {
            watcher.Path = @"C:\Users\User\Desktop\mini project\toBeLoaded";
            watcher.NotifyFilter = NotifyFilters.Attributes
            | NotifyFilters.CreationTime
            | NotifyFilters.DirectoryName
            | NotifyFilters.FileName
            | NotifyFilters.LastAccess
            | NotifyFilters.LastWrite
            | NotifyFilters.Security
            | NotifyFilters.Size;



            watcher.Changed += OnChanged;
            watcher.Filter = "*.csv";
            watcher.EnableRaisingEvents = true;
        }
        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            string filePath = e.FullPath;
            string fileName = e.Name;
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            conn.Open();
            String selectSql = $"SELECT loaded FROM logger where file_name = '{fileName}';";
            OdbcCommand cmd = new OdbcCommand(selectSql, conn);
            var check = cmd.ExecuteScalar().ToString();
            conn.Close();
            if (check == "False")
            {
                Loader Loader = new Loader(filePath, fileName);
            }
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
        }
    }
}
