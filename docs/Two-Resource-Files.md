### Two resource files for each culture:
Some localized resources are fixed for all projects (e.g.: Data annotations errors, identity errors, model binding errors). So, it can be useful to have separate resource file for these framework messages, and another resource file for views.
- Create empty class for framework messages under _LocalizationResources_ folder and name it: `LocSourceData.cs`
````cs
public class LocSourceData
{
}
````

- Create empty class for views localization under _LocalizationResources_ folder and name it: `LocSourceViews.cs`
````cs
public class LocSourceViews
{
}
````

- Setup ExpressLocalization to use two resource files:
````cs
services.AddRazorPages()
    .AddExpressLocalization<LocSourceData, LocSourceViews>(ops => 
    {
        /* Add options here*/
    });
````


> Notice: the above code demonstrats only the differentiated part for localizing with two resource files, the rest of the code must be done as described in **[Setup for Razor Pages][1]**


- Create two resource files for each culture in the same folder for our dummy classes:
  - Turkish culture:
    - LocSourceData.tr.resx
    - LocSourceViews.tr.resx
  - Arabic culture:
    - LocSourceData.ar.resx
    - LocSourceViews.ar.resx

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0

[1]:https://github.com/LazZiya/ExpressLocalization/wiki/Setup-for-Razor-Pages