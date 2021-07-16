using Microsoft.AspNetCore.Mvc;

namespace MvcWeb.Areas.Admin.Components
{
    public class NavigationMenuViewComponent : ViewComponent
    {
        
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}