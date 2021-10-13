using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BackgroundJobs.Managers.RecurringJobs
{
    public class WakeUpManager
    {
        public void Process()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var baseUrl = configuration.GetSection("BaseUrl").Value;

            using var http = new HttpClient();
            http.PostAsync($"{baseUrl}/SessionRequest", new StringContent("")).Wait();
        }
    }
}
