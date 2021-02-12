using Entities.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MvcWeb.Framework.Configurations;
using Newtonsoft.Json.Serialization;

namespace MvcWeb
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddServicesOptions(Configuration);

            services.AddIdentityOptions();
            services.AddAutoMapper(typeof(BaseModel), typeof(Startup));

            services.AddControllersWithViews()
               .AddNewtonsoftJson(options =>
               {
                   options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                   options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
                   options.SerializerSettings.ContractResolver = new DefaultContractResolver
                   {
                       NamingStrategy = null
                   };
               });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseAllAppBuilderConfig();

            app.UseAllEndpoints();
        }
    }
}
