Configure _ExpressLocalization_ in `startup.cs`:
````cs
using System.Globalization;
using LazZiya.ExpressLocalization;
using Microsoft.AspNetCore.Localization;

//setup express localization under ConfigureServices method:
public void ConfigureServices(IServiceCollection services)
{
    // Other configuration settings....
    
    var cultures = new CultureInfo[]
    {
        new CultureInfo("en"),
        new CultureInfo("tr"),
        new CultureInfo("ar")
    };

    services.AddRazorPages()
        .AddExpressLocalization<LocalizationResource>(
            ops =>
            {
                ops.ResourcesPath = "LocalizationResources";
                ops.RequestLocalizationOptions = o =>
                {
                    o.SupportedCultures = cultures;
                    o.SupportedUICultures = cultures;
                    o.DefaultRequestCulture = new RequestCulture("en");
                };
            });
}
````

Use `RequestLocalization` middleware :
````cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other codes...
    
    // Use localization middleware
    app.UseRequestLocalization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
    });
}
````

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0