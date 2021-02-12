using System.Collections.Generic;
using Entities.Models.Auth;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class UserListViewModel
    {
        public IList<User> Users { get; set; }
    }
}