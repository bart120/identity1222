using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi
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
            services.AddControllers();
            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            {
                options.Authority = "https://localhost:44361/";
                options.RequireHttpsMetadata = true;
                options.Audience = "api_demo";
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true
                };
                
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("read", p =>
                {
                    p.RequireClaim("scope", "api_demo_scope_read");
                });

                options.AddPolicy("update", p =>
                {
                    p.RequireClaim("scope", "api_demo_scope_delete");
                    p.RequireClaim("weather", "update");
                });

                options.AddPolicy("delete", p =>
                {
                    p.RequireClaim("scope", "api_demo_scope_delete");
                    p.RequireClaim("weather", "delete");
                });
            });

            /*services.AddAuthorization(options =>
            {
                options.AddPolicy("bob", p =>
                {
                    p.RequireClaim("scope", "api_demo_scope_read");
                });
            });*/
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
