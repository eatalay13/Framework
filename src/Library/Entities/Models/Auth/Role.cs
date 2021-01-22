using Entities.Models.Menu;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models.Auth
{
    public class Role : IdentityRole<int>
    {
        public Role()
        {
            RoleMenus = new HashSet<RoleMenu>();
        }

        public virtual ICollection<RoleMenu> RoleMenus { get; set; }
    }
}
