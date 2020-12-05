using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    [Table("AspNetRoleMenuPermission")]
    public class RoleMenuPermission
    {
        public int RoleId { get; set; }

        public int NavigationMenuId { get; set; }

        public NavigationMenu NavigationMenu { get; set; }
    }
}