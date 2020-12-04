using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Components
{
    public class FooterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
