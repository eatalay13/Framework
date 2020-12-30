using Entities.Dtos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcWeb.Framework.Configurations;
using System;
using System.IO;
using System.Windows.Forms;
using WinApp.SerialGenerator.LisansManager;

namespace WinApp.SerialGenerator
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName)
                .AddJsonFile("appsettings.json")
                .Build();

            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SerialGenerateForm());

            var services = new ServiceCollection();
            services.AddScoped<SerialManager>();
            services.AddScoped<SerialGenerateForm>();
            services.Configure<LicenseDto>(config => configuration.GetSection("License").Bind(config));

            services.AddServicesOptions();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<SerialGenerateForm>();
            Application.Run(mainForm);
        }
    }
}
