using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Components
{
    public class SubHeaderViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
