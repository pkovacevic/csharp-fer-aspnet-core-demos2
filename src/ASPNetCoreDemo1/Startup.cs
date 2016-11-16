using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASPNetCoreDemo1.Core;
using ASPNetCoreDemo1.Core.Interfaces;
using ASPNetCoreDemo1.Core.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace ASPNetCoreDemo1
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            // Configure a logger that will log to db
            Log.Logger = new LoggerConfiguration()
                              .Enrich.FromLogContext()
                              .WriteTo.MSSqlServer(Configuration["ConnectionStrings:DefaultConnection"], "Logs", LogEventLevel.Error, autoCreateSqlTable:true)
                              .CreateLogger();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Adding database as a transient service.
            // 
            services.AddTransient<IStudentRepository, StudentRepository>();
            services.AddScoped<StudentDbContext>((s) =>
            {
                return new StudentDbContext(Configuration["ConnectionStrings:DefaultConnection"]);
            });
            // Add framework services.
            services.AddMvc();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // adds serilog logger we configured in startup constructor
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                // nice exception information page when developing
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // Use custom error page (/home/error)
                // for users when not in development (for production)
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
