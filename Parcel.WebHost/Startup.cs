using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Parcel.WebHost.Models;
using Parcel.WebHost.Utils;

namespace Parcel.WebHost
{
    internal class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region Runtime Components
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WebHostRuntime>(WebHostRuntime.Singleton);
            
            // services.Configure<RazorPagesOptions>(options => options.RootDirectory = "/Pages");  // TODO: Potential Path-Breaking
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Custom first-level routing
            app.Use(async (context, next) =>
            {
                // Example
                // await context.Response.WriteAsync("Hello, world!");

                // Continue next routings
                await next();
            });
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // Automatic endpoints
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapGet("/Items", WebHostEndpoints.EndpointGetItems);
                // endpoints.MapGet("/Notes", WebHostEndpoints.EndpointGetNotes);
                
                // Blazor
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            
            // Final catch-all
            app.Use(async (context, next) => {
                await context.Response.WriteAsync("Cannot find target endpoint.");
            });
        }
        #endregion
    }
}