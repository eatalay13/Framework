using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Middlewares
{
    public static class MiddlewareExtension
    {
        public static IApplicationBuilder UseLicenseCheck(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<LicenseCheckMiddleware>();
        }
    }
}
