using Microsoft.AspNetCore.Http;
using Services.License;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Middlewares
{
    public class LicenseCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public LicenseCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, ILicenseService lisenceService)
        {

            if (!lisenceService.IsValidLicense())
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsJsonAsync("/InvalidLisance");
                return;
            }
            await _next.Invoke(httpContext);
        }
    }
}
