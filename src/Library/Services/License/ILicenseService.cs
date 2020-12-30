using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.License
{
    public interface ILicenseService
    {
        LicenseDto GetLicense();
        string GenerateLicense(GenerateLicenseDto generateLicense);
        bool IsValidLicense();
    }
}
