using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Services.Authentication;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Handlers
{
    public class AuthorizationRequirement : IAuthorizationRequirement { }

    public class PermissionHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly IPermissionService _permissionService;

        public PermissionHandler(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            if (context.Resource is DefaultHttpContext defaultHttpContext)
            {
                var controller = defaultHttpContext.GetRouteValue("controller");
                var action = defaultHttpContext.GetRouteValue("action");
                var area = defaultHttpContext.GetRouteValue("area");

                var isAuthenticated = context.User.Identity != null && context.User.Identity.IsAuthenticated;

                if (isAuthenticated && controller != null && action != null &&
                    await _permissionService.GetMenuItemsAsync(context.User, controller.ToString(), action.ToString(), area?.ToString()))
                {
                    context.Succeed(requirement);
                }
            }

            await Task.CompletedTask;
        }
    }
}
