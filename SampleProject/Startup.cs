using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using SampleProject.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LazZiya.ExpressLocalization.DB;
using LazZiya.TranslationServices;
using LazZiya.TranslationServices.IBMWatsonTranslate;
using LazZiya.TranslationServices.MyMemoryTranslate;
using LazZiya.TranslationServices.SystranTranslate;
using LazZiya.TranslationServices.YandexTranslate;
using LazZiya.TranslationServices.GoogleTranslate;
using SampleProject.LocalizationResources;

namespace SampleProject
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<ITranslationService, IBMWatsonTranslateService>();
            services.AddScoped<ITranslationService, MyMemoryTranslateService>();
            services.AddScoped<ITranslationService, SystranTranslateService>();
            services.AddScoped<ITranslationService, YandexTranslateService>();
            services.AddScoped<ITranslationService, GoogleTranslateService>();

            services.AddRazorPages()
                .AddExpressLocalizationDB<ApplicationDbContext>(ops =>
                {
                    ops.AutoAddKeys = true;
                    ops.OnlineTranslation = true;
                    ops.TranslationService = typeof(MyMemoryTranslateService);
                    ops.ServeUnapprovedTranslations = true;
                    ops.ResourcesPath = "LocalizationResources";
                    ops.ResourceType = typeof(LocSource);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseRequestLocalization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}
