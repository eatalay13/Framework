using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Configuration
{
    public partial class CommonConfig : IConfig
    {
        public bool DisplayFullErrorStack { get; set; } = false;

        public bool UseSessionStateTempDataProvider { get; set; } = false;

        public bool MiniProfilerEnabled { get; set; } = false;
    }
}
