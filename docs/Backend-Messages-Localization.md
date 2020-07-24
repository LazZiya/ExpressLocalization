Custom error messages can be localized by injecting [`ISharedCultureLocalizer`][1] to the controller or PageModel as below:

````cs
using LazZiya.ExpressLocalization

public class IndexModel : PageModel
{
    private readonly ISharedCultureLocalizer _loc;

    public string CustomMessage { get; set; }

    public IndexModel(ISharedCultureLocalizer loc)
    {
        _loc = loc;
    }

    public void OnGet() 
    {
        CustomMessage = _loc["Localized custom backend message"];
    }
}
````


> Extra: If you want to have bootstrap styled messages (Success, Warning, Danger, etc.) I suggest you have a look at [AlertTagHelper][2] in [LazZiya.TagHelpers][3] nuget package.

[1]:https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/ISharedCultureLocalizer.cs
[2]:https://github.com/LazZiya/TagHelpers/wiki/Alert-TagHelper-Overview
[3]:https://github.com/LazZiya/TagHelpers/wiki/