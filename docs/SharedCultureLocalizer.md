The other option to localize views is using the built-in [`ISharedCultureLocalizer`][1]

- Inject [`ISharedCultureLocalizer`][1] into the view:
````html
@using LazZiya.ExpressLocalization
@inject ISharedCultureLocalizer _loc
````
- call localization function to get localized strings in views:
````html
<h1 class="display-4">@_loc["Welcome"]</h1>
````

But when it comes to localize long strings with html tags this approach become unfriendly. That's why the recommended localization approach is using [`Localize TagHelper`][2].

[1]:https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/ISharedCultureLocalizer.cs
[2]:https://github.com/LazZiya/ExpressLocalization/wiki/Localize-TagHelper

### Applies to ExpressLocalization versions:
 4.0