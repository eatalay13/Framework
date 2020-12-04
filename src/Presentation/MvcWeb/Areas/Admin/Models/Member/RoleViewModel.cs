using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool Selected { get; set; }
    }
}
