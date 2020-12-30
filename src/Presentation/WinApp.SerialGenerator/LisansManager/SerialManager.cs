using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Helpers;
using Entities.Dtos;
using Newtonsoft.Json;
using Services.License;

namespace WinApp.SerialGenerator.LisansManager
{
    public class SerialManager
    {
        private readonly ILicenseService _encryptionService;

        public SerialManager(ILicenseService encryption)
        {
            _encryptionService = encryption;
        }

        public string GenerateSerial(GenerateLicenseDto generateLicense)
        {
            var newLicenseKey = _encryptionService.GenerateLicense(generateLicense);

            return newLicenseKey;
        }
    }
}
