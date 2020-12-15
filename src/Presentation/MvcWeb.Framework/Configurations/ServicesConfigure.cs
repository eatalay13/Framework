﻿using Core.Infrastructure.Email;
using Core.Infrastructure.NotificationService;
using Data.Contexts;
using Data.Repositories;
using Data.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcWeb.Framework.Handlers;
using Services.Authentication;
using Services.NavigationMenu;

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

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddHttpContextAccessor();

            return services;
        }
    }
}
