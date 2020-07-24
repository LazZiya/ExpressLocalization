_ExpressLocalization_ will automatically configure app cookie to add culture value to the redirect path when redirect events are invoked.

The default events and paths are: 
- OnRedirectToLogin : `{culture}/Identity/Account/Login/`
- OnRedirectToLogout : `{culture}/Identity/Account/Logout/`
- OnRedirectToAccessDenied : `{culture}/Identity/Account/AccessDenied/`

You can define custom paths for login, logout and access denied using _ExpressLocalization_ as below:

````cs
services.AddRazorPages()
    .AddExpressLocalization<LocalizationResource>(
        ops =>
        {
            ops.LoginPath = "/CustomLoginPath/";
            ops.LogoutPath = "/CustomLogoutPath/";
            ops.AcceddDeniedPath = "/CustomAccessDeniedPath/";
            
            // culture name to use when no culture value is defined in the routed url
            // default value is "en"
            ops.DefaultCultureName = "tr-TR"; 
            
            // ...
        });
````

Or if you need to completely use custom cookie configurations using the identity extensions method, you need to set the value of `ConfigureRedirectPaths` to false as below:

````cs
services.AddRazorPages()
    .AddExpressLocalization<LocalizationResource>(
        ops =>
        {            
            // don't configure redirect to paths on redirect events
            ops.ConfigureRedirectPaths = false;
            
            // ...
        });
````

in this case you need to manually configure the app cookie to handle the culture value on redirect events as described in [issue #2][2]

### Applies to below ExpressLocalization versions
 3.1.3., 3.1.2, 3.1.1

[2]: https://github.com/LazZiya/ExpressLocalization/issues/6