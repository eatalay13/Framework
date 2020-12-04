﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models.Menu;

namespace MvcWeb.Areas.Admin.Models.NavigatMenu
{
    public class UpdateNavigationMenuViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Guid? ParentMenuId { get; set; }

        public string Area { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public bool IsExternal { get; set; }

        public string ExternalUrl { get; set; }

        public bool Permitted { get; set; }

        public int DisplayOrder { get; set; }

        public bool Visible { get; set; }

        public IList<NavigationMenu> NavigationMenus { get; set; }
    }
}
