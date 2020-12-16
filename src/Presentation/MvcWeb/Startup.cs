using Core.Helpers;
using Core.Infrastructure.Email;
using Data.Contexts;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcWeb.Framework.Configurations;
using MvcWeb.Framework.Handlers;

namespace MvcWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"),
                    b => b.MigrationsAssembly("Data"));
            });

            services.AddServicesOptions();

            services.AddIdentityOptions();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(PolicyDefaults.AuthorizationPolicy, policyCorrectUser =>
                {
                    policyCorrectUser.Requirements.Add(new AuthorizationRequirement());
                });
            });

            services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("Default")));
            services.AddHangfireServer();

            services.Configure<EmailSettings>(config => Configuration.GetSection("MailSettings").Bind(config));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseHangfireDashboard("/Hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() },
                DashboardTitle = "Arkaplan servisleri",
            });

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
