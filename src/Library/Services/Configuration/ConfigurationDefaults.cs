using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Configuration
{
    public static partial class ConfigurationDefaults
    {
        /// <summary>
        /// Gets the path to file that contains app settings
        /// </summary>
        public static string AppSettingsFilePath => "applicationSettings.json";
    }
}
