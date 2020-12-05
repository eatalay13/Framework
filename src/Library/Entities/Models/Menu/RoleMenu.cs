using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    public class RoleMenu
    {
        public int RoleId { get; set; }
        public int NavigationMenuId { get; set; }

        public virtual NavigationMenu NavigationMenu { get; set; }
    }
}