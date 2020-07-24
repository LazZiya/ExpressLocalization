Less is more, do full localization in fewer steps...

Install from nuget :
````
Install-Package LazZiya.ExpressLocalization
````

## What is ExpressLocalization? 
A nuget package to simplify the localization setup of any Asp.Net Core web app to fewer steps. 

_ExpressLocalization_ does all below localization settings easily:

- Defines global route template by adding `{culture}` pattern to all routes, so urls will be like `http://www.example.com/en-US/`.
- Registers [`RouteSegmentRequestCultureProvider`][1], so culture selection can be based on route value.
- Defines [`SharedCultureLocalizer`][3] for localizing all razor pages depending on a shared resource.
- Localization of all DataAnnotations error messages.
- Localization of ModelBinding error messages.
- Localizing IdentityDescriber errors messages.
- Registers client side validation libraries for validating localized input fields like decimal numbers. 
- Configures app cookie to add `{culture}` value to the redirect paths when redirect events are invoked.

Some options requires installing additional nuget packages like : 
- [LazZiya.TagHelpers][5]

[1]:https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/RouteSegmentCultureProvider.cs
[3]:https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/SharedCultureLocalizer.cs
[5]:http://github.com/lazziya/TagHelpers