using Core.Helpers;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;

namespace Services.License
{
    public class LicenseService : ILicenseService
    {
        private readonly LicenseDto _licenseOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEncryptionService _encryptionService;

        public LicenseService(IOptions<LicenseDto> licenseOptions,
            IHttpContextAccessor httpContextAccessor,
            IEncryptionService encryptionService)
        {
            _licenseOptions = licenseOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            _encryptionService = encryptionService;
        }

        public LicenseDto GetLicense()
        {
            if (_licenseOptions == null)
                throw new Exception("Lisans bulunamadı.");

            return _licenseOptions;
        }

        public string GenerateLicense(GenerateLicenseDto generateLicense)
        {
            var licenseDtoJson = JsonConvert.SerializeObject(generateLicense);

            var newLicenseKey = _encryptionService.Encrypt(licenseDtoJson);

            return newLicenseKey;
        }

        public bool IsValidLicense()
        {
            try
            {
                var systemDomain = _httpContextAccessor.HttpContext.Request.Host.Value;
                var license = GetLicense();
                var encryptData = _encryptionService.Decrypt(license.Key);

                var licenseDto = JsonConvert.DeserializeObject<GenerateLicenseDto>(encryptData);

                return systemDomain == licenseDto.Domain
                    && license.Brand == licenseDto.BrandName
                    && (!licenseDto.IsTimeExpiredLicense || licenseDto.TimeIsExpire);
            }
            catch
            {
                return false;
            }
        }
    }
}
