using Core.Infrastructure.Email;
using Core.Infrastructure.NotificationService;
using Data.Contexts;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcWeb.Framework.Configurations
{
    public static class ServicesConfigure
    {
        public static IServiceCollection AddServicesOptions(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailSender, EmailSender>();

            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INavigateMenuService, NavigateMenuService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
