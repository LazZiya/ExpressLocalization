# ExpressLocalization

## What is ExpressLocalization?
A nuget package to simplify the localization of any Asp.Net Core web app to one step only. 

## What ExpressLocalization is offering?
All below localization settings in one clean step:

- Global route template: Add {culture} paramter to all routes, so urls will be like http://www.example.com/en-US/
- RouteValueRequestCultureProvider : Register route value request culture provider, so culture selection will be based on route value
- ViewLocalization : Use [LocalizeTagHelper](https://github.com/lazziya/TagHelpers.Localization) for localizing all razor pages depending on a shared resource. In order to use LocalizetagHelper [LazZiya.TagHelpers.Localization](https://github.com/lazziya/TagHelpers.Localization) must be installed separately.
- DataAnnotations Localization : All data annotations validation messages and display names attributes localization
- ModelBinding Localization : localize model binding error messages
- IdentityErrors Localization : localize identity describer error messages
- Client Side Validation : include all client side libraries for validating localized input fields like decimal numbers. This option requires [LazZiya.TagHelpers](http://github.com/lazziya/TagHelpers) package that will be installed automatically.

v3.1.1 :
 - Identity redirect Paths : Auto configure idenetity RedirectTo (Login, LogOut, AccessDenied) path to include culture value

## Installation
````
Install-Package LazZiya.ExpressLocalization -Version 3.0.0
````

## Dependencies
[LazZiya.TagHelpers](https://github.com/LazZiya/TagHelpers/) package will be installed automatically, it is necessary for adding client side validation libraries for localized input fields like decimal numbers.

## Step by step tutorial 
http://ziyad.info/en/articles/36-Develop_Multi_Cultural_Web_Application_Using_ExpressLocalization

## How to use
- Install from nuget as mention above
- Relevant localization resource files are available in [LazZiya.ExpressLocalizationSample](https://github.com/LazZiya/ExpressLocalizationSample) repo.
Download the resources and add them to your main web project, or just create you own resource files with the relevant key names as in [ExpressLocalizationResource.tr.resx](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ExpressLocalizationResource.tr.resx) file.
- In your main project' startup.cs file, define supported cultures list then add express localization setup in one step or customized steps as mentioned below

### One step setup:
This step will add all localization settings :
````cs
//add reference to :
using LazZiya.ExpressLocalization;

//setup express localization under ConfigureServices method:
public void ConfigureServices(IServiceCollection services)
{
    //other configuration settings....
    
    var cultures = new CultureInfo[]
    {
        new CultureInfo("en"),
        new CultureInfo("tr"),
        new CultureInfo("ar")
    };

    services.AddRazorPages()
        //ExpressLocalizationResource and ViewLocalizationResource are available in :
        // https://github.com/LazZiya/ExpressLocalizationSample
        .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(
            exOps =>
            {
                exOps.ResourcesPath = "LocalizationResources";
                exOps.RequestLocalizationOptions = ops =>
                {
                    ops.SupportedCultures = cultures;
                    ops.SupportedUICultures = cultures;
                    ops.DefaultRequestCulture = new RequestCulture("en");
                };
            });
}
````

Then configure the app to use RequestLocalizationMiddleware :
````cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //other codes...
    
    //add localization middleware to the app
    app.UseRequestLocalization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapRazorPages();
    });
}
````

if you are using Mvc just add the culture parameter to the route as below:
````cs
app.UseMvc(routes =>
{
    routes.MapRoute(
    name: "default",
    template: "{culture=en}/{controller=Home}/{action=Index}/{id?}",
    );
});
````

Also it is possible to add culture parameter to the route attributes as well:
````cs
[Route("{culture}/Home")]
public class HomeController : Controller {
     // ...
 }
````

### Customized steps (optional)
If you don't need all settings in one step, you can use below methods for manually configuring localizaton steps.
For example if you need to provide separate localization resouce files for each of DataAnnotations, Identity and ModelBinding:
````cs
//configure request localizaton options
services.Configure<RequestLocalizationOptions>(
    ops =>
    {
        ops.SupportedCultures = cultures;
        ops.SupportedUICultures = cultures;
        ops.DefaultRequestCulture = new RequestCulture("en");
    });
    
services.AddRazorPages()
    //add view localization
    .AddViewLocalization(ops => { ops.ResourcesPath = "LocalizationResources"; })
    
    //register route value request culture provider, 
    //and add route parameter {culture} at the beginning of every url
    .ExAddRouteValueRequestCultureProvider(cultures, "en")

    //add shared view localization, 
    //use by injecting SharedCultureLocalizer to the views as below:
    //@inject SharedCultureLocalizer _loc
    //_loc.GetLocalizedString("Hello world")
    .ExAddSharedCultureLocalizer<ViewLocalizationResource>()

    //add DataAnnotations localization
    .ExAddDataAnnotationsLocalization<DataAnnotationsResource>()

    //add ModelBinding localization
    .ExAddModelBindingLocalization<ModelBindingResource>()

    //add IdentityErrors localization
    .ExAddIdentityErrorMessagesLocalization<IdentityErrorsResource>()
    
    //add client side validation libraries for localized inputs
    .ExAddClientSideLocalizationValidationScripts();
    
    // configure identity redirect to paths (without culture value)
    .ExConfigureApplicationCookie(string loginPath, string logoutPath, string accessDeniedPath, string defCulture);
````

Notic: if you are creating your own resource files, the relevant key names must be defined as in [ExpressLocalizationResource](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ExpressLocalizationResource.tr.resx) file.

### _Notice_
_All localization resources can be combined in one single resource or separate resources._

## Identity RedirectTo Paths (v3.1.1)
ExpressLocalization will automatically configure app cookie to add culture value to the redirect path when redirect events are invoked.
The default events and paths after configurations are: 
- OnRedirectToLogin : "{culture}/Identity/Account/Login/"
- OnRedirectToLogout : "{culture}/Identity/Account/Logout/"
- OnRedirectToAccessDenied : "{culture}/Identity/Account/AccessDenied/"

You can define custom paths for login, logout and access denied using ExpressLocalization as below:

````cs
services.AddRazorPages()
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(
        exOps =>
        {
            exOps.LoginPath = "/CustomLoginPath/";
            exOps.LogoutPath = "/CustomLogoutPath/";
            exOps.AcceddDeniedPath = "/CustomAccessDeniedPath/";
            
            // culture name to use when no culture value is defined in the routed url
            // default value is "en"
            exOps.DefaultCultureName = "tr-TR"; 
            
            exOps.RequestLocalizationOptions = ops =>
            {
                // ...
            };
        });
````

Or if you need to completely use custom cookie configurations using the identity extensions method, you need to set the value of `ConfigureRedirectPaths` to false as below:

````cs
services.AddRazorPages()
    .AddExpressLocalization<ExpressLocalizationResource, ViewLocalizationResource>(
        exOps =>
        {            
            // don't configure redirect to paths on redirect events
            exOps.ConfigureRedirectPaths = false;
            
            exOps.RequestLocalizationOptions = ops =>
            {
                // ...
            };
        });
````

in this case you need to manually configure the app cookie to handle the culture value on redirect events as described in this [issue][2]. 

## DataAnnotations
All system data annotations error messages are defined in ExpressLocalizationResource. You can add your own localized texts to the same file.

For easy access there is a struct with all pre-defined validation messages can be accessed as below:

````cs
using LazZiya.ExpressLocalization.Messages

public class MyModel
{
    [Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
    [StringLength(maximumLength: 25, 
                ErrorMessage = DataAnnotationsErrorMessages.StringLengthAttribute_ValidationErrorIncludingMinimum, 
                MinimumLength = 3)]
    [Display(Name = "Name")]
    public string Name { get; set; }
}
````

## View localization

### Option 1 (recommended)
Localize views using Localize tag helper, require installation of [LocalizeTagHelper](https://github.com/lazziya/TagHelpers.Localization):
````razor
<localize>Hello world!</localize>
````
or 
````razor
<div localize-content>
    <h1>Title</h1>
    <p>More text for localization.....</p>
</div>
````
for more details see [Live demo](http://demo.ziyad.info/en/localize) and [TagHelpers.Localization](http://github.com/lazziya/TagHelpers.Localization)


### Option 2
- inject shared culture localizer directly to the view or to _ViewImports.cshtml :
````razor
@using LazZiya.ExpressLocalization
@inject SharedCultureLocalizer _loc
````
- call localization function to get localized strings in views:
````razor
<h1 class="display-4">@_loc.GetLocalizedString("Welcome")</h1>
````
Views are using shared resource files like: [ViewLocalizationResource](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ViewLocalizationResource.tr.resx)

## Client side validation libraries
All required libraries to valdiate localized inputs like decimal numbers
- register TagHelpers in _ViewImports.cshtml :
````cshtml
@addTagHelper *, LazZiya.TagHelpers
````
- add tag helper to the view to validate localized input:
````cshtml
<localization-validation-scripts></localization-validation-scripts>
````

For more details see [LazZiya.TagHelpers](https://github.com/LazZiya/TagHelpers/) 

## Sample project
Asp.Net Core 2.2 : https://github.com/LazZiya/ExpressLocalizationSample

Asp.Net Core 3.0 : https://github.com/LazZiya/ExpressLocalizationSampleCore3

## Project website
For discussion please visit: http://ziyad.info/en/articles/33-Express_Localization

## More
To easily create a language navigation dropdown for changing the culture use [LazZiya.TagHelpers](http://ziyad.info/en/articles/27-LazZiya_TagHelpers)

## License
MIT

[1]: https://github.com/LazZiya/ExpressLocalization/tree/ExpressLocalizationCore3
[2]: https://github.com/LazZiya/ExpressLocalization/issues/6
