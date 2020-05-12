using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Helpers;
using MiniMdb.Backend.Mappings;
using MiniMdb.Backend.Services;
using System;
using System.IO;
using System.Reflection;

namespace MiniMdb.Backend
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options => options
               .UseNpgsql(Configuration.GetConnectionString("MainDb"))
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddControllers()
                // Make error responses unified
                .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
                .AddJsonOptions(json =>
                {
                    json.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddRazorPages();

            services.AddSingleton<ValidRequestFilter>();
            services.AddMvcCore()
                //.AddCors()
                .AddApiExplorer();
            services.AddMvc().AddMvcOptions(o =>
            {
                o.Filters.Add<ValidRequestFilter>();
            });

            services.AddAutoMapper(typeof(VmMappingProfile));

            services.AddSingleton<ITimeService, TimeService>();
            services.AddTransient<IMediaTitlesService, MediaTitlesService>();

            #region Swagger docs

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MiniMdb API", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Initialize AutoMapper
            var mapper = app.ApplicationServices.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            app.UseExceptionHandler("/error");
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "MiniMdb API V1"));

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                //endpoints.MapRazorPages();
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    //spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
