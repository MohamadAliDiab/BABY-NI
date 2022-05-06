using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dapper;
using System.Data.Odbc;
using System.Data;

namespace MiniProject.Data.Services
{
    public class Parser
    {
        private string filePath;
        private string fileName;
        private DateTime fileTimeStamp;
        private string outputFileName = "C:\\Users\\User\\Desktop\\mini project\\toBeLoaded\\";
        private readonly string finishedPath = "C:\\Users\\User\\Desktop\\mini project\\Parsed\\";
        private static readonly int[] colNumbersToExclude = { 0, 8, 16 };


        public Parser(string filePath, string fileName)
        {
            this.filePath = filePath;
            this.fileName = fileName;
            this.fileTimeStamp = this.getDateTimeKey(this.fileName);
            this.outputFileName = outputFileName + fileName;
            Parse();
        }

        public void Parse()
        {
            List<List<String>> rows = this.getCSVArray();
            writeListToFile(rows, this.outputFileName);
            File.Move(this.filePath, this.finishedPath + this.fileName);
            OdbcConnection conn = new OdbcConnection();
            conn.ConnectionString = @"Driver={Vertica};server=10.10.4.171;port=5433;database=mohamad;uid=mohamad;pwd=mohamad123;";
            String insertSql = "INSERT INTO logger (file_name, parsed, loaded, aggregated) VALUES (?, ?, ?, ?)";
            OdbcCommand cmd = new OdbcCommand(insertSql, conn);
            conn.Open();
            cmd.Parameters.Add("file_name", OdbcType.VarChar).Value = fileName;
            cmd.Parameters.Add("parsed", OdbcType.Bit).Value = 1;
            cmd.Parameters.Add("loaded", OdbcType.Bit).Value = 0;
            cmd.Parameters.Add("aggregated", OdbcType.Bit).Value = 0;
            cmd.ExecuteNonQuery();
            conn.Close();
        }
        public DateTime getDateTimeKey(string fileName)
        {
            string dt = fileName.Substring(26, 14);
            dt = dt.Replace("_" , " ");
            dt = string.Format("{0}{1}/{2}{3}/{4}{5}{6}{7} {8}{9}:{10}{11}", dt[6], dt[7], dt[4], dt[5], dt[0], dt[1], dt[2], dt[3], dt[9], dt[10], dt[11], dt[12]);
            DateTime oDate = Convert.ToDateTime(dt);
            return oDate;
        }

        private List<List<string>> getCSVArray()
        {
            List<List<string>> rows = new List<List<string>>();
            string[] lines = System.IO.File.ReadAllLines(this.filePath);
            int lineIndex = 1;
            while(lineIndex < lines.Length)
            {
                Console.WriteLine(lineIndex);
                string line = lines[lineIndex];
                List<string> row = new List<string>();

                string[] columns = line.Split(',');
                int networkSID = this.getNetworkSID(columns[2], columns[6]);
                string obj = columns[2];
                string failureDescription = columns[17];

                if (obj.Equals("Unreachable Bulk FC") || failureDescription != "-")
                {
                    lineIndex++;
                    continue;
                }
                string link = getLink(obj);
                string[] TidFtid = getTiFtid(obj);
                string Tid = TidFtid[1];
                string farendTid = TidFtid[2];
                string[] slotPort = getSlotPort(obj);
                string port = slotPort[0];
                string[] slot;
                if (slotPort.Length > 2)
                {
                    slot = new string[2];
                    slot[0] = slotPort[1];
                    slot[1] = slotPort[2];
                }
                else
                {
                    slot = new string[1];
                    slot[0] = slotPort[1];
                }
                int colIndex = 0;
                row.Add(networkSID.ToString());
                row.Add(this.fileTimeStamp.ToString());

                while (colIndex<columns.Length)
                {
                    if (colNumbersToExclude.Contains(colIndex))
                    {
                        colIndex++;
                        continue;
                    }
                    
                    string columnValue = columns[colIndex];
                    row.Add(columnValue);
                    colIndex++;
                }
                row.Add(link.ToString());
                row.Add(Tid.ToString());
                row.Add(farendTid.ToString());
                foreach (string s in slot)
                {
                    List<String> rowToAdd = new List<string>(row);
                    rowToAdd.Add(s);
                    rowToAdd.Add(port);
                    rows.Add(rowToAdd);
                }
                lineIndex++;
            }

            return rows;
        }

        private int getNetworkSID(string obj, string neAlias)
        {
            string toBeHashed = string.Format("{0}{1}{2}", this.fileTimeStamp, obj, neAlias);
            MD5 md5Hasher = MD5.Create();
            var hashing= md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(toBeHashed));
            var hashed = BitConverter.ToInt32(hashing, 0);
            return hashed;
        }
        private string getLink(string obj)
        {
            int idx = obj.IndexOf("_");
            string newObj = obj.Substring(0, idx);
            string[] objParts = newObj.Split("/");

            var dotIndex = objParts[1].IndexOf(".");
            var plusIndex = objParts[1].IndexOf("+");

            if (dotIndex == -1) // if middle has NO dot
            {
                return objParts[1] + "/" + objParts[2];
            }
            else if (plusIndex == -1) // if middle has a dot AND NO plus
            {
                return objParts[1].Replace('.', '/');
            }
            else // if middle has a dot AND a plus
            {
                return "";
            }
        }
        private string[] getTiFtid(string obj)
        {
            return obj.Split("_");
        }
        private string[] getSlotPort(string obj)
        {
            int idx = obj.IndexOf("_");
            string newObj = obj.Substring(0, idx);

            string[] objParts = newObj.Split("/");

            string[] ports = new string[1];
            ports[0] = objParts[0];

            string[] slots;

            var plusIndex = objParts[1].IndexOf("+");
            if (plusIndex == -1)
            {
                slots = new string[1];
                slots[0] = objParts[1];
            }
            else
            {
                slots = objParts[1].Split("+");
            }
            return ports.Concat(slots).ToArray();
        }
        
        private void writeListToFile(List<List<string>> list, string outputFileName)
        {
            StringBuilder builder = new StringBuilder();
            foreach(List<string> lineList in list)
            {
                builder.Append(string.Join(",", lineList.ToArray()) + "\n");
            }

            File.WriteAllText(outputFileName, builder.ToString());
        }
    }
}
