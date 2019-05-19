# ExpressLocalization
Express localization settings for Asp.NetCore 2.x.
All dirty localization settings in one clean step.

## Installation
````
Install-Package LazZiya.ExpressLocalization -Version 1.0.0
````

## How to use
- Install from nuget as mention above
- Relevant localizaed resource files are available in [LazZiya.ExpressLocalization.Resources](https://github.com/LazZiya/ExpressLocalization.Resources) repo.
Download the resources project and reference it to your main web project, or just create you own resource files with the relevant key names as in [ExpressLocalizationResource.tr.resx](https://github.com/LazZiya/ExpressLocalization.Resources/blob/master/LazZiya.ExpressLocalization.Resources/ExpressLocalizationResource.tr.resx) file.
- In your main project, add express localization setup in one step or customized steps as mentioned below

### One step setup:
```
