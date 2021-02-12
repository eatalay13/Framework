using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MvcWeb.Areas.Admin.Models.AppSettingsViewModels
{
    public class SettingIndexViewModel
    {
        [Required]
        public string AppName { get; set; }
        public string Logo { get; set; }
        public string Favicon { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile LogoFormFile { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile FaviconFormFile { get; set; }
    }
}
