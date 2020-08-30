using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace WebNetCore
{
    public class Startup
    {

        public IConfigurationRoot configRoot { get; set; }
        public static string ConnectionString { get; set; }
        public IConfiguration Configuration { get; private set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            configRoot = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json")
                .Build();

        }

       // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
           
            services.AddSingleton(Configuration.GetSection("vkey"));
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

            //app.Use( async (context,next) =>
            //{
            //    await context.Response.WriteAsync("my first middle ware");
            //   await next();
            //});

            //app.Use( async (context,next)=> {
            //    await context.Response.WriteAsync($"\n this is second middleware & get Key from appsetting - {Configuration["vkey"]}");
            //    next();
            //});

            app.UseRouting();

            app.UseAuthorization();

            ConnectionString = configRoot.GetConnectionString("Conn");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
