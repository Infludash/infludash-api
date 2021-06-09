using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MySql;
using infludash_api.Data;
using infludash_api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace infludash_api
{
    public class Startup
    {
        readonly string myOrigins = "myOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<InfludashContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));

            services.AddControllers().AddNewtonsoftJson();
            services.AddTokenAuthentication(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy( 
                    name: myOrigins,
                    builder =>
                    {
                        builder.WithOrigins(Configuration["Cors:frontend"])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                    });
            });

            services.AddHangfire(x => x.UseStorage(new MySqlStorage(mySqlConnectionStr, new MySqlStorageOptions { 
                PrepareSchemaIfNecessary = true
            })));
            services.AddHangfireServer();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsProduction() || env.IsStaging())
            {
                app.UseExceptionHandler("/api/error");
                app.UseHsts();
            }

            /*
             Throw a hard error instead! When a automatic redirect is in place, a poorly configured client could unknowingly leak request parameters over the unencrypted endpoint
             app.UseHttpsRedirection();
            */

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(myOrigins);

            app.UseAuthentication();

            app.UseAuthorization();

            var options = new DashboardOptions
            {
                Authorization = new[]
                {
                    new BasicAuthAuthorizationFilter(
                        new BasicAuthAuthorizationFilterOptions
                        {
                            // Require secure connection for dashboard
                            RequireSsl = true,
                            // Case sensitive login checking
                            LoginCaseSensitive = true,
                            // Users
                            Users = new[]
                            {
                                new BasicAuthAuthorizationUser
                                {
                                    Login = Configuration["Hangfire:user"],
                                    // Password as plain text, SHA1 will be used
                                    PasswordClear = Configuration["Hangfire:password"]
                                },
                            }
                    })
                }
            };

            app.UseHangfireDashboard("/hangfire", options);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
