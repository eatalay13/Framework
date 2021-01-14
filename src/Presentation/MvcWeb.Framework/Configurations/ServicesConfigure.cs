using Core.Helpers;
using Core.Infrastructure.Email;
using Core.Infrastructure.NotificationService;
using Core.Infrastructure.ViewToString;
using Data.Contexts;
using Data.Repositories;
using Data.UnitOfWork;
using Entities.Dtos;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MvcWeb.Framework.Handlers;
using Services.Authentication;
using Services.License;
using Services.NavigateMenu;

namespace MvcWeb.Framework.Configurations
{
    public static class ServicesConfigure
    {
        public static IServiceCollection AddServicesOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("Data"));
            });

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<ILicenseService, LicenseService>();
            services.AddScoped<IRazorViewToStringRenderer, RazorViewToStringRenderer>();

            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INavigateMenuService, NavigateMenuService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddHttpContextAccessor();

            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("Default")));
            services.AddHangfireServer();

            services.Configure<EmailSettings>(config => configuration.GetSection("MailSettings").Bind(config));
            services.Configure<LicenseDto>(config => configuration.GetSection("License").Bind(config));

            services.AddFacebookLogin(configuration);
            services.AddGoogleLogin(configuration);
            services.AddMicrosoftLogin(configuration);

            return services;
        }

        public static IServiceCollection AddApiServicesOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("Data"));
            });

            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEncryptionService, EncryptionService>();
            services.AddScoped<ILicenseService, LicenseService>();

            services.AddScoped<DbContext, AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<INavigateMenuService, NavigateMenuService>();
            services.AddScoped<IPermissionService, PermissionService>();

            services.AddScoped<IAuthorizationHandler, PermissionHandler>();

            services.AddHttpContextAccessor();

            services.Configure<EmailSettings>(config => configuration.GetSection("MailSettings").Bind(config));
            services.Configure<LicenseDto>(config => configuration.GetSection("License").Bind(config));

            services.AddFacebookLogin(configuration);
            services.AddGoogleLogin(configuration);
            services.AddMicrosoftLogin(configuration);

            return services;
        }
    }
}
