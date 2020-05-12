using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MiniMdb.Backend.Data;
using MiniMdb.Backend.Mappings;
using MiniMdb.Backend.Services;

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

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAutoMapper(typeof(VmMappingProfile));

            services.AddSingleton<ITimeService, TimeService>();
            services.AddTransient<IMediaTitlesService, MediaTitlesService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Initialize AutoMapper
            var mapper = app.ApplicationServices.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
            mapper.ConfigurationProvider.CompileMappings();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
