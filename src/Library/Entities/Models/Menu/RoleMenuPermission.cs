using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    [Table("AspNetRoleMenuPermission")]
    public class RoleMenuPermission
    {
        public Guid RoleId { get; set; }

        public Guid NavigationMenuId { get; set; }

        public NavigationMenu NavigationMenu { get; set; }
    }
}