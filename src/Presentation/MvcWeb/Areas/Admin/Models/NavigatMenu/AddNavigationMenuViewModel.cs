using Entities.Models.Menu;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcWeb.Areas.Admin.Models.NavigatMenu
{
    public class AddNavigationMenuViewModel
    {
        [Required]
        public string Name { get; set; }

        public Guid? ParentMenuId { get; set; }

        public string Area { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsExternal { get; set; }

        public string ExternalUrl { get; set; }

        public int DisplayOrder { get; set; }

        public bool Visible { get; set; }

        public IList<NavigationMenu> NavigationMenus { get; set; }
    }
}
