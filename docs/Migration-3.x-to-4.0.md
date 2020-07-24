Use this manual to upgrade from `LazZiya.ExpressLocalization v3.x` to `v4.0`.


### LocalizeTagHelper
Open __ViewImports.cshtml_ remove old reference and add new reference to localize tag helper as below:
````razor
@* remove this line *@
@addTagHelper *, LazZiya.TagHelpers.Localization

@* add new taghelpers *@
@addTagHelper *, LazZiya.ExpressLocalizaion
````

### SharedCultureLocalizer
If you where using `SharedCultureLocalizer` to localize backend messages or views, just replace it with `ISharedCultureLocalizer`.

````cs
// Replace all SharedCultureLocalizer instances 
// with ISharedCultureLocalizer
public class IndexModel : PageModel
{
    // remove this line
    // private readonly SharedCultureLocalizer _loc;

    // add IShared... instead
    private readonly ISharedCultureLocalizer _loc;

    public IndexModel(ISharedCultureLocalizer loc)
    {
        _loc = loc;
    }
}
````

### LocalizationValdiationScripts TagHelper Component
Previously the taghelper component for localization valdiation scripts taghlper `<localization-validation-scripts></localization-validation-scripts>` where automtically registered with `ExpressLocalization`. Starting from v4.0 it will require manually registration in startup:

````cs
services.AddTransient<ITagHelperComponent, LocalizationValidationScriptsTagHelperComponent>();
````
Read more about [Validating localized input][3]

### Validation Attributes
Previously all valdiation attributes where requiring an error message as below:
````cs
[Required(ErrorMessage = "The field {0} is required"]

// or

[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
````

Starting from v4.0 a new express attributes can be used. Express attributes produces localized error message by default, no need to manually provide an error message inside the attribute tags.

````cs
// remove the reference to the error messages namespace
using LazZiya.ExpressLocalization.Messages;

// add reference to express data annotations
using LazZiya.ExpressLocalization.DataAnnotations

// Use express validation attributes by adding Ex... prefix to the attribute name
[ExRequired]
````

See all [express valdiation attributes][1] and [default attributes][2] for more details.

[1]:https://github.com/LazZiya/ExpressLocalization/wiki/DataAnnotations-Localization-Using-Express-Attributes
[2]:https://github.com/LazZiya/ExpressLocalization/wiki/DataAnnotations-Localization-Using-Default-Attributes
[3]:https://github.com/LazZiya/ExpressLocalization/wiki/Validating-Localized-Input