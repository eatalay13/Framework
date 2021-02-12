using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Configuration;
using MvcWeb.Areas.Admin.Models.AppSettingsViewModels;

namespace MvcWeb.Areas.Admin.Mapping.AppSettingMappings
{
    public class AppSettingMap : Profile
    {
        public AppSettingMap()
        {
            CreateMap<AppSettings, SettingIndexViewModel>();
            CreateMap<SettingIndexViewModel, AppSettings>();
        }
    }
}
