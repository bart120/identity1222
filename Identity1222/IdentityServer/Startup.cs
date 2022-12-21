using IdentityServer.AspIdentity;
using IdentityServer.Services;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
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
            services.AddControllersWithViews();

            services.AddDbContext<AuthenticationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });

            //aspidentity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                //options.Password.RequiredLength = 6;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.User.RequireUniqueEmail = true;
                //...
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<AuthenticationDbContext>()
                .AddUserManager<UserManager<User>>();


            //service identityserver
            var builder = services.AddIdentityServer(options =>
            {
                options.Endpoints.EnableDeviceAuthorizationEndpoint = false;
                //options.Endpoints.EnableUserInfoEndpoint = false;
                options.UserInteraction.LoginUrl = "/login";
                options.UserInteraction.LogoutUrl = "/logout";
            }).AddConfigurationStore(store =>
            {
                store.DefaultSchema = "configuration";
                store.ConfigureDbContext = db => db.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"),
                    sql => sql.MigrationsAssembly("IdentityServer"));
            }).AddOperationalStore(store =>
            {
                store.DefaultSchema = "operational";
                store.ConfigureDbContext = db => db.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"),
                    sql => sql.MigrationsAssembly("IdentityServer"));
            }).AddAspNetIdentity<User>();

            services.AddTransient<IProfileService, ProfileService>();

            //uniquement en dev
            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage();
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

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
