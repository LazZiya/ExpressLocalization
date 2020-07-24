Asp.Net Core uses [localization culture providers][5] to detect the request culture and respond accordingly. The culture checking process goes one by one through all registered providers, whenever a request culture is detected the check process stops and the localization process starts accordingly.

If the request culture is not found in the first provider, next provider will be checked. Finally if no culture is detected the default culture will be used.

_ExpressLocalization_ is using the below order for request culture providers:
1) [RouteSegmentCultureProvider][6]
2) [QueryStringRequestCultureProvider][7]
3) [CookieRequestCultureProvider][3]
4) [AcceptedLanguageHeaderRequestCultureProvider][4]
5) Use default request culture from startup settings

In order to restrict culture fallback to route culture provider only use below implementation in startup:
````cs
services.AddRazorPages()
        .AddExpressLocalization<LocalizationResource>(ops =>
        {
            // Use only route segment culture provider
             ops.UseAllCultureProviders = false;
               
            // the rest of the code...
        });
````
_reference to issue [#13][8]_

[6]: https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/RouteSegmentCultureProvider.cs
[7]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.localization.querystringrequestcultureprovider
[3]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.localization.cookierequestcultureprovider
[4]: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.localization.acceptlanguageheaderrequestcultureprovider
[5]: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/localization-extensibility?view=aspnetcore-3.1#localization-culture-providers
[8]: https://github.com/LazZiya/ExpressLocalization/issues/13
