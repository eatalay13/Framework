﻿using System.Collections.Generic;
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
        IList<NavigationMenu> GetMenuList();
        IPagedList<NavigationMenu> GetMenuList(int pageIndex, int pageSize = 10);
        NavigationMenu GetMenuById(int id);
        void AddNavigationMenu(NavigationMenu menu);
        void UpdateNavigationMenu(NavigationMenu menu);
        void DeleteNavigationMenu(int id);
        void MenuSync(Assembly assembly);
        List<AuthNavigationMenuDto> GetAllAuthorizeController(Assembly assembly);
    }
}