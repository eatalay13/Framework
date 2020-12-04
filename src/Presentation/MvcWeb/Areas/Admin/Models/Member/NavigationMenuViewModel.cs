using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models.Menu;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class NavigationMenuViewModel
    {
        public Guid Id { get; set; }

        public IList<NavigationMenu> Permissions { get; set; }
    }
}
