using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Helpers;

namespace WinApp.SerialGenerator.LisansManager
{
    public class SerialManager
    {
        private readonly IEncryptionService _encryptionService;

        public SerialManager(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public string GenerateSerial(string domain, string brandName)
        {
            var serial = _encryptionService.EncryptByHex(string.Join(';', domain, brandName));

            return serial;
        }
    }
}
