using System.ComponentModel.DataAnnotations;

namespace MvcWeb.Areas.Admin.Models.Member
{
    public class AddRoleViewModel
    {
        [Required] public string Name { get; set; }
    }
}