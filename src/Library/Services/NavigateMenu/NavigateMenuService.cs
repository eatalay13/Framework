using Core.CustomAttributes;
using Core.Extensions;
using Core.Infrastructure.PagedList;
using Core.Validation;
using Data.UnitOfWork;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using DevExtreme.AspNet.Mvc;
using Entities.Dtos;
using Entities.Models.Menu;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.ValidationRules;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Services.NavigateMenu
{
    public class NavigateMenuService : INavigateMenuService
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public NavigateMenuService(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<LoadResult> BindDevExp(DataSourceLoadOptions loadOptions)
        {
            var data = _uow.NavigationMenuRepo
                .TableNoTracking
                .ProjectTo<NavigationMenuForListDto>(_mapper.ConfigurationProvider);

            return await DataSourceLoader.LoadAsync(data, loadOptions);
        }

        public async Task<IList<NavigationMenu>> GetMenuListAsync()
        {
            return await _uow.NavigationMenuRepo.GetAllAsync();
        }

        public async Task<IPagedList<NavigationMenu>> GetMenuPagedListAsync(int pageIndex, int pageSize = 10)
        {
            return await _uow.NavigationMenuRepo.GetAllPagedAsync(func: null, pageIndex: pageIndex, pageSize: pageSize);
        }

        public async Task<NavigationMenu> GetMenuByIdAsync(int id)
        {
            return await _uow.NavigationMenuRepo.GetByIdAsync(id);
        }

        public async Task AddNavigationMenuAsync(NavigationMenu menu)
        {
            ValidationTool.Validate(typeof(NavigateMenuValidator), menu);

            await _uow.NavigationMenuRepo.InsertAsync(menu);

            await _uow.SaveChangesAsync();
        }

        public async Task UpdateNavigationMenuAsync(NavigationMenu menu)
        {
            if (menu is null)
                return;

            _uow.NavigationMenuRepo.Update(menu);

            await _uow.SaveChangesAsync();
        }

        public async Task MenuSyncAsync(Assembly assembly)
        {
            var allAuthController = GetAllAuthorizeController(assembly);

            var registerControllers = await _uow.NavigationMenuRepo.GetAllAsync();

            foreach (var menu in registerControllers)
            {
                allAuthController.RemoveAll(e => e.Area == menu.Area
                                                 && e.ControllerName == menu.ControllerName
                                                 && e.ActionName == menu.ActionName);
            }

            foreach (var menu in allAuthController)
            {
                NavigationMenu parentMenu = null;

                if (!menu.ParentMenuName.IsNullOrEmptyWhiteSpace())
                {
                    parentMenu = _uow.NavigationMenuRepo.TableNoTracking
                        .FirstOrDefault(e => e.Name == menu.ParentMenuName);

                    if (parentMenu is null)
                    {
                        parentMenu = new NavigationMenu
                        {
                            Name = menu.ParentMenuName,
                            DisplayOrder = menu.DisplayOrder,
                            Visible = menu.Visible
                        };

                        await _uow.NavigationMenuRepo.InsertAsync(parentMenu);
                        await _uow.SaveChangesAsync();
                    }
                }

                if (menu.IsTopMenu)
                {
                    if (_uow.NavigationMenuRepo.TableNoTracking.Any(e => e.Name == menu.Name))
                        continue;
                }

                var newMenu = new NavigationMenu
                {
                    Name = menu.Name,
                    ParentMenuId = parentMenu?.Id,
                    DisplayOrder = menu.DisplayOrder,
                    Area = menu.Area,
                    ControllerName = menu.ControllerName,
                    ActionName = menu.ActionName,
                    Visible = menu.Visible
                };

                await _uow.NavigationMenuRepo.InsertAsync(newMenu);

                await _uow.SaveChangesAsync();
            }
        }

        public List<AuthNavigationMenuDto> GetAllAuthorizeController(Assembly assembly)
        {
            var controllerActionList = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type) && type.CustomAttributes.Any(c => c.AttributeType == typeof(AuthorizeAttribute)))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public)).ToList();

            var navigateList = new List<AuthNavigationMenuDto>();

            foreach (var methodInfo in controllerActionList)
            {
                if (methodInfo.DeclaringType is null)
                    continue;

                var ctrlParentMenuAttr = methodInfo.DeclaringType.GetCustomAttributes(typeof(ParentMenuAttribute), true).Cast<ParentMenuAttribute>()
                    .FirstOrDefault();

                var actionParentMenuAttr = methodInfo.GetCustomAttributes(typeof(ParentMenuAttribute), true).Cast<ParentMenuAttribute>()
                    .FirstOrDefault();

                if (ctrlParentMenuAttr is not null && !navigateList.Exists(e => e.Name == ctrlParentMenuAttr.ParentMenuName))
                    navigateList.Add(new AuthNavigationMenuDto
                    {
                        Name = ctrlParentMenuAttr.ParentMenuName,
                        DisplayOrder = ctrlParentMenuAttr.Order,
                        Visible = ctrlParentMenuAttr.IsVisible,
                        IsTopMenu = true
                    });

                if (actionParentMenuAttr is not null && !navigateList.Exists(e => e.Name == actionParentMenuAttr.ParentMenuName))
                    navigateList.Add(new AuthNavigationMenuDto
                    {
                        Name = actionParentMenuAttr.ParentMenuName,
                        ParentMenuName = ctrlParentMenuAttr?.ParentMenuName,
                        DisplayOrder = actionParentMenuAttr.Order,
                        Visible = actionParentMenuAttr.IsVisible,
                        IsTopMenu = true
                    });

                var menuItemAttr = methodInfo.GetCustomAttributes(typeof(MenuItemAttribute), true).Cast<MenuItemAttribute>()
                    .FirstOrDefault();

                var areaAttr = methodInfo.DeclaringType.GetCustomAttributes(typeof(AreaAttribute), true).Cast<AreaAttribute>()
                    .FirstOrDefault();

                if (navigateList.Exists(x => x.Area == areaAttr?.RouteValue &&
                                            x.ControllerName == methodInfo.DeclaringType.Name.Replace("Controller", "") &&
                                            x.ActionName == methodInfo.Name)) continue;

                navigateList.Add(new AuthNavigationMenuDto
                {
                    ParentMenuName = actionParentMenuAttr?.ParentMenuName ?? ctrlParentMenuAttr?.ParentMenuName,
                    Name = menuItemAttr?.Name ?? string.Join(' ', methodInfo.DeclaringType.Name.Replace("Controller", ""), methodInfo.Name),
                    Visible = SetMenuVisible(menuItemAttr, actionParentMenuAttr, ctrlParentMenuAttr),
                    DisplayOrder = menuItemAttr?.Order ?? 1,
                    ControllerName = methodInfo.DeclaringType.Name.Replace("Controller", ""),
                    ActionName = methodInfo.Name,
                    Area = areaAttr?.RouteValue
                });
            }

            return navigateList;
        }

        private bool SetMenuVisible(MenuItemAttribute menuItemAttr, ParentMenuAttribute actionParentMenuAttr,
            ParentMenuAttribute ctrlParentMenuAttr)
        {
            if (menuItemAttr != null)
                return menuItemAttr.IsVisible;

            if (actionParentMenuAttr != null)
                return actionParentMenuAttr.IsVisible;

            return ctrlParentMenuAttr != null && ctrlParentMenuAttr.IsVisible;
        }

        public async Task DeleteNavigationMenuAsync(int id)
        {
            var menu = await GetMenuByIdAsync(id);

            _uow.NavigationMenuRepo.Delete(menu);

            await _uow.SaveChangesAsync();
        }
    }
}
