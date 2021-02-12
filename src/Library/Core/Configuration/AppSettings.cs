using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public partial class AppSettings
    {
        public AppSettings()
        {
            AppName = "AtalaySoft";
            Logo = "/media/logos/logo-letter-13.png";
            Favicon = "/media/icon2-192x192.png";
        }

        #region Properties

        public string AppName { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }

        public CacheConfig CacheConfig { get; set; } = new CacheConfig();

        public HostingConfig HostingConfig { get; set; } = new HostingConfig();

        public PluginConfig PluginConfig { get; set; } = new PluginConfig();

        public CommonConfig CommonConfig { get; set; } = new CommonConfig();

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        #endregion
    }
}
