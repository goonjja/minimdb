using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using MiniMdb.Auth;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Helpers;
using MiniMdb.Backend.Mappings;
using MiniMdb.Backend.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MiniMdb.Backend
{
    public class Startup
    {
        private readonly IWebHostEnvironment _env;

        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            #region Storage
            
            services.AddDbContext<AppDbContext>(options => options
               .UseNpgsql(Configuration.GetConnectionString("MainDb"))
               .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

            #endregion

            #region Web

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(cfg => cfg.RootPath = "ClientApp/dist");

            services.AddControllers()
                // Make error responses unified
                .ConfigureApiBehaviorOptions(o => o.SuppressModelStateInvalidFilter = true)
                .AddJsonOptions(json => json.JsonSerializerOptions.IgnoreNullValues = true);

            services.AddSingleton<ValidRequestFilter>();
            services.AddMvcCore().AddApiExplorer();
            services.AddMvc().AddMvcOptions(o => o.Filters.Add<ValidRequestFilter>());

            if(!_env.IsDevelopment())
            {
                services.AddCors();
            }

            #endregion

            #region Caching

            services.AddMemoryCache(cfg =>
            {
                cfg.SizeLimit = 1024;
            });

            #endregion

            #region Localization

            services.AddLocalization();
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en-US"),
                        new CultureInfo("ru"),
                        new CultureInfo("si"),
                    };

                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            #endregion

            #region Auth

            services.AddTokenAuthentication(
                "zNh23k84AyQ7wcrqoUevaYXgDKBpS5jC",
                new JwtSettings { ValidFor = TimeSpan.FromMinutes(10) }
            );
            services.AddAuthorization(o =>
            {
                o.AddPolicy(MiniMdbRoles.AdminPolicy, _ => _.RequireRole(MiniMdbRoles.AdminRole));
                o.AddPolicy(MiniMdbRoles.UserPolicy, _ => _.RequireRole(MiniMdbRoles.UserRole));
                o.AddPolicy(MiniMdbRoles.AuthenticatedPolicy, _ => _.RequireAuthenticatedUser());
            });

            #endregion

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

            services.AddAutoMapper(typeof(VmMappingProfile));

            services.AddSingleton<ITimeService, TimeService>();
            services.AddTransient<IMediaTitlesService, MediaTitlesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region Different configurations demo

            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            logger.LogInformation($"Welcome to: {Configuration["TestMessage"]}");

            #endregion

            #region Apply migrations, seed data

            // Wait for database to start for 5 seconds
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var timeout = 5;
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                while (timeout > 0)
                {
                    try
                    {
                        if (db.Database.CanConnect())
                            break;
                    }
                    catch (Exception) { }
                    Thread.Sleep(1000);
                    --timeout;
                }
            }

            if (!env.IsTesting())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    db.Database.Migrate();
                    new DataSeed(db).Apply();
                }
            }

            #endregion

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ru"),
                new CultureInfo("si"),
            };
            var localizationOpts = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures,
            };
            app.UseRequestLocalization(localizationOpts);

            // Initialize AutoMapper
            var mapper = app.ApplicationServices.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
                app.UseExceptionHandler("/error");
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "MiniMdb API V1"));

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
            });
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    //spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}
