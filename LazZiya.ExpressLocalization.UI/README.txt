Register LazZiya.EFGenericDataManager in startup

````
services.AddTransient<IEFGenericDataManager, EFGenericDataManager<ApplicationDbContext>>();
````