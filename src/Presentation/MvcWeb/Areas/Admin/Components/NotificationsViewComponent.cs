using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Components
{
    public class NotificationsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
