using Core.Extensions;
using Core.Infrastructure.PagedList;
using Core.Validation;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Mvc;
using Services.ValidationRules;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Entities.Models.Menu;

namespace Services.Authentication
{
    public class NavigateMenuService : INavigateMenuService
    {
        private readonly IUnitOfWork _uow;

        public NavigateMenuService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public IList<NavigationMenu> GetMenuList()
        {
            return _uow.NavigationMenuRepo.GetAll();
        }

        public IPagedList<NavigationMenu> GetMenuList(int pageIndex, int pageSize = 10)
        {
            return _uow.NavigationMenuRepo.GetAllPaged(null, pageIndex, pageSize);
        }

        public NavigationMenu GetMenuById(string id)
        {
            return _uow.NavigationMenuRepo.GetById(id.ToGuid());
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

        public IList<NavigationMenu> GetAllAuthorizeController(Assembly assembly)
        {
            var controllerActionList = assembly.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type) && type.CustomAttributes.Any(c => c.AttributeType == typeof(AuthorizeAttribute)))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x =>
                {
                    if (x.GetCustomAttributes().Any(c => c is HttpPostAttribute))
                        return null;

                    if (x.DeclaringType is null)
                        return null;

                    return new NavigationMenu
                    {
                        ControllerName = x.DeclaringType.Name.Replace("Controller", ""),
                        ActionName = x.Name,
                        Area = x.DeclaringType.CustomAttributes
                            .FirstOrDefault(c => c.AttributeType == typeof(AreaAttribute))?.ConstructorArguments[0].Value
                            ?.ToString()
                    };
                }).ToList();

            controllerActionList.RemoveAll(e => e is null);

            return controllerActionList;
        }
    }
}
