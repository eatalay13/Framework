using Core.Configuration;
using Core.Infrastructure.Providers;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Threading.Tasks;

namespace Services.Configuration
{
    public partial class AppSettingsService
    {
        private readonly ILibFileProvider _fileProvider;
        private AppSettings _appSettings;

        public AppSettingsService(ILibFileProvider fileProvider)
        {
            _fileProvider = fileProvider;
        }

        public AppSettings AppSettings => _appSettings ??= GetAppSettings();

        #region Methods

        private AppSettings GetAppSettings()
        {
            var filePath = _fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);

            if (!_fileProvider.FileExists(filePath))
                return new AppSettings();

            return JsonConvert.DeserializeObject<AppSettings>(_fileProvider.ReadAllText(filePath, Encoding.UTF8));
        }

        public async Task SaveAppSettingsAsync([NotNull] AppSettings appSettings)
        {
            //create file if not exists
            var filePath = _fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);
            _fileProvider.CreateFile(filePath);

            //save app settings to the file
            var text = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            await _fileProvider.WriteAllTextAsync(filePath, text, Encoding.UTF8);
        }

        public void SaveAppSettings([NotNull] AppSettings appSettings)
        {
            //create file if not exists
            var filePath = _fileProvider.MapPath(ConfigurationDefaults.AppSettingsFilePath);
            _fileProvider.CreateFile(filePath);

            //save app settings to the file
            var text = JsonConvert.SerializeObject(appSettings, Formatting.Indented);
            _fileProvider.WriteAllText(filePath, text, Encoding.UTF8);
        }

        #endregion
    }
}
