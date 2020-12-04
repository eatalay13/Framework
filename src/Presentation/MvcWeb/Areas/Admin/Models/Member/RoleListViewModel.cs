using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entities.Models.Auth;
using Microsoft.AspNetCore.Identity;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class RoleListViewModel
    {
        public IList<Role> Roles { get; set; }
    }
}
