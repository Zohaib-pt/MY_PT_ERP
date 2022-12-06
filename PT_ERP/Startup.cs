using DAL.oClasses;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;
using System;


namespace PT_ERP
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
            //---Added by Nooruddin the following lines, These are the Interface implementation in Startup class for dependency injection
            services.AddScoped<IOStock, OStock>();
            services.AddScoped<IConnectionStringManager, ConnectionStringManager>();
            services.AddScoped<ISqlHelper, SqlHelper>();
            services.AddScoped<IOBasicData, OBasicData>();
            services.AddScoped<IOHome, OHome>();
            services.AddScoped<IOPurchases, OPurchases>();
            services.AddScoped<IOSales, OSales>();
            services.AddScoped<IOHR, OHR>();
            services.AddScoped<IOAccounts, OAccounts>();
            services.AddScoped<IOAdmin, OAdmin>();
            services.AddScoped<IODelivery, ODelivery>();
            services.AddScoped<IOReports, oReports>();
            services.AddScoped<IOInventory, OInventory>();

            services.AddControllersWithViews();
            services.AddMvc().AddRazorRuntimeCompilation();
            services.AddRazorPages();

            //--Session and cookies settings
            services.AddHttpContextAccessor();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);//You can set Time  in seconds, minutes 
            });

            services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue; // <-- ! long.MaxValue
                options.MultipartBoundaryLengthLimit = int.MaxValue;
                options.MultipartHeadersCountLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = int.MaxValue;
                options.MultipartHeadersLengthLimit = byte.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment env2)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            //---For session purpose
            app.UseSession();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            RotativaConfiguration.Setup(env2);
          
        }
    }
}
