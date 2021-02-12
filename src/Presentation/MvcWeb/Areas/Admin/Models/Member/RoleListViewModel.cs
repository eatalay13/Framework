using System.Collections.Generic;
using Entities.Models.Auth;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class RoleListViewModel
    {
        public IList<Role> Roles { get; set; }
    }
}