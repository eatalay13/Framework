using System.Collections.Generic;
using System.Linq;
using Core.Helpers;
using Microsoft.AspNetCore.Mvc.Razor;

namespace MvcWeb.Framework.Themes
{
    public class ThemeableViewLocationExpander : IViewLocationExpander
    {
        private const string THEME_KEY = "lib.themename";


        public void PopulateValues(ViewLocationExpanderContext context)
        {
            //no need to add the themeable view locations at all as the administration should not be themeable anyway
            if (context.AreaName?.Equals(AreaDefaults.AdminAreaName) ?? false)
                return;

            var themeContext =
                (IThemeContext) context.ActionContext.HttpContext.RequestServices.GetService(typeof(IThemeContext));
            context.Values[THEME_KEY] = themeContext.GetWorkingThemeNameAsync().Result;
        }


        public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
            IEnumerable<string> viewLocations)
        {
            if (context.Values.TryGetValue(THEME_KEY, out var theme))
                viewLocations = new[]
                    {
                        $"/Themes/{theme}/Views/{{1}}/{{0}}.cshtml",
                        $"/Themes/{theme}/Views/Shared/{{0}}.cshtml"
                    }
                    .Concat(viewLocations);


            return viewLocations;
        }
    }
}