using Entities.Models.Auth;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models.Menu
{
    public class RoleMenu : BaseModel
    {
        public int RoleId { get; set; }
        public int NavigationMenuId { get; set; }

        [NotMapped]
        public new int Id { get; set; }

        public virtual NavigationMenu NavigationMenu { get; set; }
        public virtual Role Role { get; set; }
    }
}