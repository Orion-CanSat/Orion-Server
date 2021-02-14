#nullable enable

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrionServer
{
    public class Program
    {
        internal static SQLServerConnection.SQLConnection? sqlConnection = null;

        public static void Main(string[] args)
        {
            Startup.CreateUpdateWWWData();

            try
            {
                Startup.InitializeDB();
                sqlConnection?.Open();
            }
            catch { }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
