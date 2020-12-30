using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Services.License;
using System;
using System.IO;

namespace MvcWeb
{
    public class Program
    {
        [Obsolete]
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
               .WriteTo.MSSqlServer(
                   connectionString: configuration.GetSection("Serilog:ConnectionStrings:LogDatabase").Value,
                   tableName: configuration.GetSection("Serilog:TableName").Value,
                   appConfiguration: configuration,
                   autoCreateSqlTable: true,
                   columnOptionsSection: configuration.GetSection("Serilog:ColumnOptions"),
                   schemaName: configuration.GetSection("Serilog:SchemaName").Value,
                   restrictedToMinimumLevel: LogEventLevel.Warning).CreateLogger();

            CreateHostBuilder(args).Build().Run();

            Log.CloseAndFlush();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseSerilog();
    }
}
