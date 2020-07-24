If you don't want to use all localization settings in one step, you can use customized setup methods as described below.

````cs
// Configure request localization options
services.Configure<RequestLocalizationOptions>(
    ops =>
    {
        ops.SupportedCultures = cultures;
        ops.SupportedUICultures = cultures;
        ops.DefaultRequestCulture = new RequestCulture("en");
    });
    
services.AddRazorPages()
    // Add view localization
    .AddViewLocalization(ops => { ops.ResourcesPath = "LocalizationResources"; })
    
    // Register route value request culture provider, 
    // and add route parameter {culture} at the beginning of every url
    .ExAddRouteValueRequestCultureProvider(cultures, "en")

    // Add view localization with shared resource, 
    .ExAddSharedCultureLocalizer<ViewLocalizationResource>()

    // Add DataAnnotations localization
    .ExAddDataAnnotationsLocalization<DataAnnotationsResource>()

    // Add ModelBinding localization
    .ExAddModelBindingLocalization<ModelBindingResource>()

    // Add IdentityErrors localization
    .ExAddIdentityErrorMessagesLocalization<IdentityErrorsResource>()
    
    // Register client side validation scripts taghelper 
    .ExAddClientSideLocalizationValidationScripts();
    
    // Configure identity redirect to paths (without culture value)
    .ExConfigureApplicationCookie(string loginPath, string logoutPath, string accessDeniedPath, string defCulture);
````

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0