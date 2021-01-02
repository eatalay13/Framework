using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public partial class CacheConfig : IConfig
    {
        public int DefaultCacheTime { get; set; } = 60;

        public int ShortTermCacheTime { get; set; } = 3;

        public int BundledFilesCacheTime { get; set; } = 120;
    }
}
