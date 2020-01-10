using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using HiSpaceWeb.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BMWWeb
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
            services.Configure<CookiePolicyOptions>(options =>
            {
				// This lambda determines whether user consent for non-essential cookies is needed for a given request.
				options.CheckConsentNeeded = context => false;
				options.MinimumSameSitePolicy = SameSiteMode.None;
			});

            Common.Instance.ApiAddress = Configuration["AppServiceURL:HiSpaceBaseAddress"];

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //session usage config
            services.AddDistributedMemoryCache();
            //services.AddSession(options =>
            //{
            //	options.IdleTimeout = TimeSpan.FromMinutes(1);//You can set Time   
            //});
            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Website}/{action=Index}/{id?}");
            });
        }
    }
}
