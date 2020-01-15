## What is ExpressLocalization? 
A nuget package to simplify the localization setup of any Asp.Net Core web application.

## Installation
Install from nuget :
````
Install-Package LazZiya.ExpressLocalization
````

### How to use
This one step will do all localization configurations:
````cs
using LazZiya.ExpressLocalization;

public void ConfigureServices(IServiceCollection services)
{    
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

Then configure the app to use RequestLocalizationMiddleware :
````cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // Other codes...
    
    // Add localization middleware to the app
    app.UseRequestLocalization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
    });
}
````

## Setup and options
For all settings see [wiki pages](https://github.com/LazZiya.ExpressLocalization/wiki)

## Step by step tutorial 
http://ziyad.info/en/articles/36-Develop_Multi_Cultural_Web_Application_Using_ExpressLocalization

## Sample projects
 * Asp.Net Core 2.2 : https://github.com/LazZiya/ExpressLocalizationSample
 * Asp.Net Core 3.0 : https://github.com/LazZiya/ExpressLocalizationSampleCore3

## License
MIT
