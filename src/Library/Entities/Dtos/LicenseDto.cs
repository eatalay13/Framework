using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public class GenerateLicenseDto
    {
        public string Domain { get; set; }
        public string BrandName { get; set; }
        public bool IsTimeExpiredLicense { get; set; }
        public DateTime TimeExpire { get; set; }

        public bool TimeIsExpire => DateTime.Now.Date > TimeExpire.Date;

    }
}
