using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace MvcWeb.Framework.Handlers
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            return true;
        }
    }
}
