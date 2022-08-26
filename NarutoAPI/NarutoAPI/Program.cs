using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using NLog.Targets;
using System.IO;

namespace NarutoAPI
{
    public class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static void Main(string[] args)
        {
            Configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

            var logger = ConfigureLog();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureHostConfiguration((config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false);
                })
                .UseNLog();

        private static Logger ConfigureLog()
        {
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

            var targetDatabase = (DatabaseTarget)LogManager.Configuration.FindTargetByName("database");
            targetDatabase.ConnectionString = Configuration["ConnectionStrings:DB_NARUTO"];

            LogManager.ReconfigExistingLoggers();
            return logger;
        }
    }
}
