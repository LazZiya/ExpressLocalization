## Localization with DB Support
Starting from v5 ExpressLocalization offers localization with DB support.

> ### IMPORTANT: DB support still in beta mode and below instructions are subject to change till the final release of .net5 and then ExpressLocalization 5.</span>

### Setup for DB Localization
#### 1. Add ExpressLocalization stores to the `ApplicationDbContext`:
````cs
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Cultures table will hold the supported cultures entities
    public DbSet<XLCultures> Cultures { get; set; }

    // All resource keys will be saved in this table
    public DbSet<XLCultures> Resources { get; set; }

    // All locaized values will be saved in this table
    public DbSet<XLTranslation> Translations { get; set; }
}
````

#### 2. Add new migration and apply it
Create a new migration
````
add-migration ExpressLocalizationStores
````
Apply migration
````
update-database
````
#### 3. Add initial values
Add items to the localization tables manually or seed the database with initial values as described here: [Seed DB with Initial Values](https://github.com/LazZiya/ExpressLocalization/wiki/Seed-DB-with-Initial-Values)

#### 4. Add ExpressLocalization setup 
Use ExpressLocalizatin with DB support in startup:
````cs
services.AddRazorPages()
        .AddExpressLocalizationDB<ApplicationDbContext>();
````

Use localization middleware:
````cs
app.UseRequestLocalization();
````

Thats it, now you can continue by using [LocalizeTagHelper](https://github.com/LazZiya/ExpressLocalization/wiki/Localize-TagHelper). 

#### Sample Repo:
[ExpressLocalization Sample .net5](https://github.com/LazZiya/ExpressLocalization/tree/DbResource/SampleProject)

### Applies to ExpressLocalization versions:
 5.0