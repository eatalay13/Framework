using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;

namespace MvcWeb.Framework.Themes
{
    public static class ThemeConfigure
    {
        public static IServiceCollection AddThemes(this IServiceCollection services)
        {
            services.AddScoped<IThemeContext, ThemeContext>();

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new ThemeableViewLocationExpander());
            });

            return services;
        }
    }
}
