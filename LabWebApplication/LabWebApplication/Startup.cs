using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LabApplicationBackendLayer.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LabWebApplication
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
            services.AddMvc();
            services.Configure<AllConfiguration>(Configuration.GetSection("RequiredConfigurations"));
            services.Configure<ApplicationInsightsConfiguration>(Configuration.GetSection("ApplicationInsights"));
            services.AddSession(options =>
            {
                options.Cookie.Name = "CosmosDB";
                options.Cookie.HttpOnly = true;
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseSession(new SessionOptions
            {
                Cookie = new Microsoft.AspNetCore.Http.CookieBuilder
                {
                    Name = "CosmosDB",
                    HttpOnly = true                    
                }
            });

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Employee}/{action=Index}/{id?}");
            });
        }
    }
}
