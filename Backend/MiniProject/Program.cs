using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MiniProject.Data;
using MiniProject.Data.Services;

namespace MiniProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ParserWatcher.InitFileWatcher();
            LoaderFileWatcher.secondFileWatcher();
            CreateHostBuilder(args).Build().Run();

            //Parser p = new Parser("C:\\Users\\User\\Desktop\\mini project\\toBeParsed\\SOEM1_TN_RADIO_LINK_POWER_20200312_001500.txt", "SOEM1_TN_RADIO_LINK_POWER_20200312_001500.txt");
            //p.Parse();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
