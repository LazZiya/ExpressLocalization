> ## THE INFO IN THIS PAGE STILL UNDER DEVELOPMENT, AND IT IS SUBJECT TO CHANGE TILL THE OFFICIAL RELEASE REACH OUT.

### Migration steps from 4.x to 5.0
- [RequestLocalizationOptions](#requestlocalizationoptions)
- [RouteSegmentRequestCultureProvider](#routesegmentrequestcultureprovider)
- [RouteTemplateModelConvention](#routetemplatemodelconvention)

### RequestLocalizationOptions
Property setup moved to startup
#### Old setup:
It was a part of `ExpressLocalizationOptions`:
````cs
service.AddExpressLocalization<LocSource>(ops =>
{
    // ...
    ops.RequestLocalizationOptions = o => 
    {
        o.SupportedCultures = cultures;
        o.SupportedUICultures = cultures;
        o.DefaultRequestCulture = new RequestCulture("en");
    };
});
````

#### New setup
````cs
services.Configure<RequestLocalizationOptions>(ops =>
{
    ops.SupportedCultures = cultures;
    ops.SupportedUICultures = cultures;
    ops.DefaultRequestCulture = new RequestCulture("en");
});

service.AddExpressLocalization<LocSource>(ops =>
{
    // ...
});
````

### RouteSegmentRequestCultureProvider
Provides localization based on `{culture}` route segment e.g. `/en/Index`.
In previous versions it was automatically inserted to the `RequestCultureProviders`. New setup requires manually adding it.
````cs
services.Configure<RequestLocalizationOptions>(ops =>
{
    // ...
    ops.RequestCultureProviders.Insert(0, new RouteSegmentRequestCultureProvider(cultures));
});
````

### RouteTemplateModelConvention
Adds `{culture}` route segment to razor pages conventions.
It was automatically added in previouse versions. New version requires manually adding the setup:
#### Razor pages
````cs
services.AddRazorPages()
    .AddRazorPagesOptions(ops => { ops.Conventions?.Insert(0, new RouteTemplateModelConventionRazorPages()); })
    .AddExpressLocalization<LocSource>(ops =>
    {
        // ...
    });
````
#### Mvc
````cs
services.AddMvc()
    .AddMvcOptions(ops => { ops.Conventions?.Insert(0, new RouteTemplateModelConventionMvc()); })
    .AddExpressLocalization<LocSource>(ops =>
    {
        // ...
    });
````
