DataAnnotations localization setup is already done during the express setup in `startup.cs` file for [razor pages][1] or [MVC][2].

All we have to do is just provide an error message to the relevant attribute or a `Name` value for `Display` attribute:

````cs
[Required(ErrorMessage = "The {0} field is required.")]
[Display(Name = "Full name")]
public string Name { get; set; }
````

### Default Error Messages
For easy implementation, all default framework error messages of data annotations, model binding and identity errors are predefined under [`LazZiya.ExpressLocalization.Messages`][3] namespace.

So we can use the already defined messages as below:

````cs
using LazZiya.ExpressLocalization.Messages

[Required(ErrorMessage = DataAnnotationsErrorMessages.RequiredAttribute_ValidationError)]
[Display(Name = "Full name")]
public string Name { get; set; }
````

See all [`DataAnnotationsErrorMessages`][4]

### Customizing Error Messages
You are still able to provide custom validation messages as below:
````cs
[Required(ErrorMessage = "Ooops! The {0} field is necessary to continue...")]
[Display(Name = "Full name")]
public string Name { get; set; }
````

> Notice : depending on `ExpressLocalization` setup, the error messages must be defined in the relevant resource file for DataAnnotations. For more details see [Creating Resources][5].

[1]:https://github.com/LazZiya/ExpressLocalization/wiki/Setup-for-Razor-Pages
[2]:https://github.com/LazZiya/ExpressLocalization/wiki/Setup-for-mvc
[3]:https://github.com/LazZiya/ExpressLocalization/tree/master/LazZiya.ExpressLocalization/Messages
[4]:https://github.com/LazZiya/ExpressLocalization/blob/master/LazZiya.ExpressLocalization/Messages/DataAnnotationsErrorMessages.cs
[5]:https://github.com/LazZiya/ExpressLocalization/wiki/Creating-Resources

### Applies to ExpressLocalization versions:
 4.0, 3.2, 3.1, 3.0, 2.0, 1.1, 1.0