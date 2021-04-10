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
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(new Data.Location() { Date = DateTime.Now, Latitude = 10.0, Longitude = 3.0 }));
            Startup.CreateUpdateWWWData();

            try
            {
                Startup.InitializeDB();
                sqlConnection?.Open();
                Startup.UpdateDatabase();
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
