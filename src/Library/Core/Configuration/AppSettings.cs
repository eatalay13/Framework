﻿using Newtonsoft.Json;
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
        #region Properties

        public CacheConfig CacheConfig { get; set; } = new CacheConfig();

        public HostingConfig HostingConfig { get; set; } = new HostingConfig();

        public PluginConfig PluginConfig { get; set; } = new PluginConfig();

        public CommonConfig CommonConfig { get; set; } = new CommonConfig();

        [JsonExtensionData]
        public IDictionary<string, JToken> AdditionalData { get; set; }

        #endregion
    }
}