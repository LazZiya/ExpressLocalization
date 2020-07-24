**ExpressLocalization** supports shared resources, it is possible to use one resource file for each culture, or multiple resources depending on resource content group.

#### Resource Content Groups
The locailzable contents in an Asp.NetCore web app is defined in four main groups:

- Views : all strings to be locaized in razor views
- Data annotations : all data annotation error messages and display name attributes
- Model binding : all model binding error messages
- Identity errors: all identity error messages

Each of these groups can have its own resource file `.resx`, or all can be combined together in one resource file. Custom error messages in the backend can take place with **Views** resource group.

The resource files are shared ressources, that's mean they don't define public members for the keys. So the resource keys can't be accessed directly from the code behind like `ResourceFileName.KeyName`! 

Instead we will create a dummy class for each resource, so the culture localizer will be able to access these resources via the relevant dummy class.

#### Creating The resources
First we need to create a folder to keep all resource files in one place. So create a new folder in the project root and name it _LocalizationResources_, we will create all our dummy classes and resource files under this folder.

> Notices : 
> - In order for **ExpessLocalization** to access the shared resource files a dummy class must be created for each resource type.
> - The folder can be in the project root or in a shared class library project as well
> - You are free to use your own naming for the dummy classes and resource files.

## Setup method
You are free to choose how many resource files will be used to handle localized strings for each culture.
- [A - One resource file for each culture](https://github.com/LazZiya/ExpressLocalization/wiki/One-Resource-File)
- [B - Two resource files for each culture](https://github.com/LazZiya/ExpressLocalization/wiki/Two-Resource-Files)
- [C - Four resource files for each culture](https://github.com/LazZiya/ExpressLocalization/wiki/Four-Resource-Files)

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0