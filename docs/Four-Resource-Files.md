### Four resource files for each culture:
If you still want to split error messages to different resource files, you can create a resource file for each type as below:
- Create a dummy class for views localization `LocSourceViews.cs`
````cs
public class LocSourceViews
{
}
````

- Create a dummy class for data annotations errors localization `LocSourceData.cs`
````cs
public class LocSourceData
{
}
````

- Create a dummy class for model binding errors localization `LocSourceModelBinding.cs`
````cs
public class LocSourceModelBinding
{
}
````

- Create a dummy class for identity errors localization `LocSourceIdentity.cs`
````cs
public class LocSourceIdentity
{
}
````

- Setup `ExpressLocalization` to use four resource files:
````cs
services.AddRazorPages()
    .AddExpressLocalization<LocSourceViews, LocSourceData, LocSourceModelBinding, LocSourceDataIdentity>(ops => 
    {
        /* Add options here */
    });
````

> Notice: the above code demonstrats only the differentiated part for localizing with four resource files, the rest of the code must be done as described in **[Setup for Razor Pages][1]**


- Create four resource files for each culture in the same folder with the dummy classes:
  - Turkish culture:
    - LocSourceViews.tr.resx
    - LocSourceData.tr.resx
    - LocSourceModelBinding.tr.resx
    - LocSourceIdentity.tr.resx
  - Arabic culture:
    - LocSourceViews.ar.resx
    - LocSourceData.ar.resx
    - LocSourceModelBinding.ar.resx
    - LocSourceIdentity.ar.resx

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0

[1]:https://github.com/LazZiya/ExpressLocalization/wiki/Setup-for-Razor-Pages