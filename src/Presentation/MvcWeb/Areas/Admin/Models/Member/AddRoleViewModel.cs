using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class AddRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
