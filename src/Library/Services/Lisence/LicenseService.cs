using Core.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;

namespace Services.Lisence
{
    public class LicenseService : ILicenseService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptionService _encryptionService;

        public LicenseService(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEncryptionService encryptionService)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _encryptionService = encryptionService;
        }

        public string GetLicense()
        {
            var isExist = _configuration.GetSection("Lisence").Exists();
            if (!isExist)
                throw new Exception("Lisans bulunamadı.");

            return _configuration.GetSection("Lisence:Key").Value;
        }

        public string GetBrand()
        {
            var isExist = _configuration.GetSection("Lisence").Exists();
            if (!isExist)
                throw new Exception("Lisans bulunamadı.");

            return _configuration.GetSection("Lisence:Brand").Value;
        }

        public bool IsValidLicense()
        {
            try
            {
                var systemDomain = _httpContextAccessor.HttpContext.Request.Host.Value;
                var key = GetLicense();
                var brand = GetBrand();
                var encryptData = _encryptionService.DecryptByHex(key).Split(';');

                return systemDomain == encryptData[0] && brand == encryptData[1];
            }
            catch
            {
                return false;
            }
        }
    }
}
