using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public partial class HostingConfig : IConfig
    {
        public bool UseHttpClusterHttps { get; set; } = false;

        public bool UseHttpXForwardedProto { get; set; } = false;

        public string ForwardedHttpHeader { get; set; } = string.Empty;
    }
}
