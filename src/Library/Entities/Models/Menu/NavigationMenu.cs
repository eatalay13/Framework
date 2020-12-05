using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    public class NavigationMenu : BaseModel
    {
        public NavigationMenu()
        {
            RoleMenus = new HashSet<RoleMenu>();
            InverseParentMenu = new HashSet<NavigationMenu>();
        }


        public string Name { get; set; }
        public int? ParentMenuId { get; set; }
        public string Area { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool IsExternal { get; set; }
        public string ExternalUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool Visible { get; set; }

        public virtual NavigationMenu ParentMenu { get; set; }
        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
        public virtual ICollection<NavigationMenu> InverseParentMenu { get; set; }


        [NotMapped]
        public bool Permitted { get; set; }
    }
}