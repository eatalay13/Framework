using Microsoft.Extensions.DependencyInjection;
using MvcWeb.Framework.Configurations;
using System;
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
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SerialGenerateForm());

            var services = new ServiceCollection();
            services.AddScoped<SerialManager>();
            services.AddScoped<SerialGenerateForm>();

            services.AddServicesOptions();

            using ServiceProvider serviceProvider = services.BuildServiceProvider();
            var mainForm = serviceProvider.GetRequiredService<SerialGenerateForm>();
            Application.Run(mainForm);
        }
    }
}
