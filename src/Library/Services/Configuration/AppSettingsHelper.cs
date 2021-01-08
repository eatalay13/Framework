using Core.Configuration;
using Core.Infrastructure.Providers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Configuration
{
    public partial class AppSettingsHelper
    {
        #region Methods

        public static async Task SaveAppSettingsAsync([NotNull] AppSettings appSettings, [NotNull] ILibFileProvider fileProvider)
        {
            //create file if not exists
            var filePath = fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);
            fileProvider.CreateFile(filePath);

            //check additional configuration parameters
            var additionalData = JsonConvert.DeserializeObject<AppSettings>(await fileProvider.ReadAllTextAsync(filePath, Encoding.UTF8))?.AdditionalData;
            appSettings.AdditionalData = additionalData;

            //save app settings to the file
            var text = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            await fileProvider.WriteAllTextAsync(filePath, text, Encoding.UTF8);
        }

        public static void SaveAppSettings([NotNull] AppSettings appSettings, [NotNull] ILibFileProvider fileProvider)
        {
            //create file if not exists
            var filePath = fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);
            fileProvider.CreateFile(filePath);

            //check additional configuration parameters
            var additionalData = JsonConvert.DeserializeObject<AppSettings>(fileProvider.ReadAllText(filePath, Encoding.UTF8))?.AdditionalData;
            appSettings.AdditionalData = additionalData;

            //save app settings to the file
            var text = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        #endregion
    }
}
