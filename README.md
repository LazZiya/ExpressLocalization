# ExpressLocalization
Express localization settings for Asp.NetCore 2.x.

## What is ExpressLocalization?
A nuget package to simplify the localization of any Asp.Net Core 2.x web app to one step only.

## What ExpressLocalization is offering?
All below localization settings in one clean step:

- Global route template: Add {culture} paramter to all routes, so urls will be like http://www.example.com/en-US/
- RouteValueRequestCultureProvider : register route value request culture provider, so culture selection will be based on route value
- ViewLocalization : create a string localizer for localizing all razor pages depending on a shared resource
- DataAnnotations Localization : All data annotations validation messages and display names attributes localization
- ModelBinding Localization : localize model binding error messages
- IdentityErrors Localization : localize identity describer error messages
- Client Side Validation : include all client side libraries for validating localized input fields like decimal numbers 

## Installation
````
Install-Package LazZiya.ExpressLocalization -Version 1.1.2
````
it will install [LazZiya.TagHelpers v2.1.0](https://github.com/LazZiya/TagHelpers/) package as well, it is necessary for adding client side validation libraries for localized input fields like decimal numbers.

## Step by step tutorial 
http://ziyad.info/en/articles/36-Develop_Multi_Cultural_Web_Application_Using_ExpressLocalization

## How to use
- Install from nuget as mention above
- Relevant localization resource files are available in [LazZiya.ExpressLocalizationSample](https://github.com/LazZiya/ExpressLocalizationSample) repo.
Download the resources project and reference it to your main web project, or just create you own resource files with the relevant key names as in [ExpressLocalizationResource.tr.resx](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ExpressLocalizationResource.tr.resx) file.
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

    services.AddMvc()
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
            })
        .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
````

Then configure the app to use RequestLocalizationMiddleware :
````cs
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
    //other codes...
    
    //add localization middleware to the app
    app.UseRequestLocalization();

    app.UseMvc();
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
    
services.AddMvc()
    //add view localization
    .AddViewLocalization(ops => { ops.ResourcesPath = "LocalizationResources"; })
    
    //register route value request culture provider, 
    //and add route parameter {culture} at the beginning of every url
    .ExAddRouteValueRequestCultureProvider(cultures, "en")

    //add shared view localization, 
    //use by injecting SharedCultureLocalizer to the views as below:
    //@inject SharedCultureLocalizer _loc
    //_loc.Text("Hello world")
    .ExAddSharedCultureLocalizer<ViewLocalizationResource>()

    //add DataAnnotations localization
    .ExAddDataAnnotationsLocalization<DataAnnotationsResource>()

    //add ModelBinding localization
    .ExAddModelBindingLocalization<ModelBindingResource>()

    //add IdentityErrors localization
    .ExAddIdentityErrorMessagesLocalization<IdentityErrorsResource>()
    
    //add client side validation libraries for localized inputs
    .ExAddClientSideLocalizationValidationScripts()

    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
````

Notic: if you are creating your own resource files, the relevant key names must be defined as in [ExpressLocalizationResource](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ExpressLocalizationResource.tr.resx) file.

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

### Option 1
Localize views using Localize tag helper:
````razor
<localize>Hello world!</localize>
````
see http://demo.ziyad.info/en/localize and http://github.com/lazziya/TagHelpers.Localization for more details.


### Opiton 2
- inject shared culture localizer directly to the view or to _ViewImports.cshtml :
````razor
@using LazZiya.ExpressLocalization
@inject SharedCultureLocalizer _loc
````
- call localization function to get localized strings in views:
````razor
<h1 class="display-4">@_loc.Text("Welcome")</h1>
````
Views are using shared resource files like: [ViewLocalizationResource](https://github.com/LazZiya/ExpressLocalizationSample/blob/master/ExpressLocalizationSampleProject/LocalizationResources/ViewLocalizationResource.tr.resx)

## Client side validation libraries
All required libraries to valdiate localized inputs like decimal numbers
- register TagHelpers in _ViewImports.cshtml :
````cshtml
@using LazZiya.TagHelpers
@addTagHelper *, LazZiya.TagHelpers
````
- add tag helper to the view to validate localized input:
````cshtml
<localization-validation-scripts></localization-validation-scripts>
````
For more details see [LazZiya.TagHelpers v2.1.0](https://github.com/LazZiya/TagHelpers/) 

## Sample project
See this sample project : https://github.com/LazZiya/ExpressLocalizationSample

## Project website
For discussion please visit: http://ziyad.info/en/articles/33-Express_Localization

## More
To easily create a language navigation dropdown for changing the culture use [LazZiya.TagHelpers](http://ziyad.info/en/articles/27-LazZiya_TagHelpers)

## License
MIT
