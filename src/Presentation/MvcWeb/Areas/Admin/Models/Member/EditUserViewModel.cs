namespace MvcWeb.Areas.Admin.Models.Member
{
    public class EditUserViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public RoleViewModel[] Roles { get; set; }
    }
}