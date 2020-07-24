### One resource file for each culture:
All localized strings for views, data annotations, identity errors ...etc. will take place in one resource file.
- Create a new folder named "LocalizationResources" and create an empty class inside it and name it `LocSource.cs`
````cs
public class LocSource
{
}
````
- Setup `ExpressLocalization` to use one resource file:
````cs
services.AddRazorPages()
    .AddExpressLocalization<LocSource>(ops => 
    {
        /* Add options here */
    });
````


> Notice: the above code demonstrats only the differentiated part for localizing with one resource file, the rest of the code must be done as described in **[Setup for Razor Pages][1]**


- Create one resource file for each culture in the same folder with our dummy class `LocSource.cs`:
  - LocSource.tr.resx
  - LocSource.ar.resx

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0

[1]:https://github.com/LazZiya/ExpressLocalization/wiki/Setup-for-Razor-Pages