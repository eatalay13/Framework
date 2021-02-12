using System.Collections.Generic;
using Entities.Models.Menu;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class NavigationMenuViewModel
    {
        public int Id { get; set; }

        public IList<NavigationMenu> Permissions { get; set; }
    }
}