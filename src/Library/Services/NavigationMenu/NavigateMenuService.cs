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

namespace Services.NavigateMenu
{
    public class NavigateMenuService : INavigateMenuService
    {
        private readonly IUnitOfWork _uow;

        public NavigateMenuService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<LoadResult> BindDevExp(DataSourceLoadOptions loadOptions)
        {
            var data = _uow.NavigationMenuRepo.TableNoTracking;

            return await DataSourceLoader.LoadAsync(data, loadOptions);
        }

        public IList<NavigationMenu> GetMenuList()
        {
            return _uow.NavigationMenuRepo.GetAll();
        }

        public IPagedList<NavigationMenu> GetMenuList(int pageIndex, int pageSize = 10)
        {
            return _uow.NavigationMenuRepo.GetAllPaged(null, pageIndex, pageSize);
        }

        public NavigationMenu GetMenuById(int id)
        {
            return _uow.NavigationMenuRepo.GetById(id);
        }

        public void AddNavigationMenu(NavigationMenu menu)
        {
            ValidationTool.Validate(typeof(NavigateMenuValidator), menu);

            _uow.NavigationMenuRepo.Insert(menu);

            _uow.SaveChanges();
        }

        public void UpdateNavigationMenu(NavigationMenu menu)
        {
            if (menu is null)
                return;

            _uow.NavigationMenuRepo.Update(menu);

            _uow.SaveChanges();
        }

        public void MenuSync(Assembly assembly)
        {
            var allAuthController = GetAllAuthorizeController(assembly);

            var registerControllers = _uow.NavigationMenuRepo.GetAll();

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

                        _uow.NavigationMenuRepo.Insert(parentMenu);
                        _uow.SaveChanges();
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

                _uow.NavigationMenuRepo.Insert(newMenu);

                _uow.SaveChanges();
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

            else if (menuItemAttr == null && actionParentMenuAttr != null)
                return actionParentMenuAttr.IsVisible;

            else if (menuItemAttr == null && actionParentMenuAttr == null && ctrlParentMenuAttr != null)
                return ctrlParentMenuAttr.IsVisible;

            else
                return false;
        }

        public void DeleteNavigationMenu(int id)
        {
            var menu = GetMenuById(id);

            _uow.NavigationMenuRepo.Delete(menu);

            _uow.SaveChanges();
        }
    }
}
