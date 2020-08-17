##

> ### ATTENTION: Due to several major breaking changes in the planned v5, the next version has been moved to a new project! ExpressLocalization will continue to get support and hotfixes. To continue with the next vesion please see:
> ### New project repository: [XLocalizer](https://github.com/LazZiya/XLocalizer)
> ### New project docs: [DOCS.Ziyad.info/XLocalizer](http://docs.ziyad.info/)
> ### Sample repository: [XLocalizer.Samples](https://github.com/LazZiya/XLocalizer.Samples)

##

## What is ExpressLocalization? 
A nuget package to simplify the localization setup of any Asp.Net Core web application.

### How to use
Install from nuget :
````
Install-Package LazZiya.ExpressLocalization
````

Add _ExpressLocalization_ to `startup.cs`:
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

Then configure the app to use `RequestLocalization` middleware :
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

### Setup and options
For all details goto [wiki pages](https://github.com/LazZiya/ExpressLocalization/wiki)

### Step by step tutorial 
 * [Develop Multi Cultural Web Application Using ExpressLocalization](https://www.codeproject.com/Articles/5061604/Developing-Multicultural-ASP-NET-Core-3-2-1-Projec)

### Sample projects
 * [Asp.Net Core 2.2](https://github.com/LazZiya/ExpressLocalizationSample)
 * [Asp.Net Core 3.0](https://github.com/LazZiya/ExpressLocalizationSampleCore3)

### License
MIT
