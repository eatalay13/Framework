using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Core.Infrastructure.PagedList;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Entities.Dtos;
using Entities.Models.Menu;

namespace Services.NavigateMenu
{
    public interface INavigateMenuService
    {
        Task<LoadResult> BindDevExp(DataSourceLoadOptions loadOptions);
        Task<IList<NavigationMenu>> GetMenuListAsync();
        Task<IPagedList<NavigationMenu>> GetMenuPagedListAsync(int pageIndex, int pageSize = 10);
        Task<NavigationMenu> GetMenuByIdAsync(int id);
        Task AddNavigationMenuAsync(NavigationMenu menu);
        Task UpdateNavigationMenuAsync(NavigationMenu menu);
        Task DeleteNavigationMenuAsync(int id);
        Task MenuSyncAsync(Assembly assembly);
        List<AuthNavigationMenuDto> GetAllAuthorizeController(Assembly assembly);
    }
}
