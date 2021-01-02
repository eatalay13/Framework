using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public partial class PluginConfig : IConfig
    {
        public bool ClearPluginShadowDirectoryOnStartup { get; set; } = true;

        public bool CopyLockedPluginAssembilesToSubdirectoriesOnStartup { get; set; } = false;

        public bool UseUnsafeLoadAssembly { get; set; } = true;

        public bool UsePluginsShadowCopy { get; set; } = true;
    }
}
