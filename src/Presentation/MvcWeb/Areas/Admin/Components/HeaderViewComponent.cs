using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Components
{
    public class HeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
