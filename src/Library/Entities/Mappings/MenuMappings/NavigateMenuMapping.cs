using AutoMapper;
using Entities.Dtos;
using Entities.Models.Menu;

namespace Entities.Mappings.MenuMappings
{
    public class NavigateMenuMapping : Profile
    {
        public NavigateMenuMapping()
        {
            CreateMap<NavigationMenu, NavigationMenuForListDto>();
        }
    }
}
