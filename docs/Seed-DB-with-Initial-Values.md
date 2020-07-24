## Seed DB with initial values for ExpressLocalization

Under `Data` folder create a new file for seeding initial localization values, in the below sample I'm adding two cultures (en, tr) to the cultures table, and a few localized strings to the resources table.
````cs
public static class SeedInitialDBValues
{
    public static void SeedCultures(this ModelBuilder modelBuilder)
    {
        // Seed initial data for localization stores
        modelBuilder.Entity<XLCulture>().HasData(
            new XLCulture { IsActive = true, IsDefault = true, ID = "en" },
            new XLCulture { IsActive = true, IsDefault = false, ID = "tr" }
            );
    }
````
Then call these methods in `ApplicationDbContext`:
````cs
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.SeedCultures();            
    base.OnModelCreating(builder);
}
````
Finally, add a new migration:
````
add-migration SeedCultures
```` 
and apply it:
````
update-database
````

The initial culture values are seeded to the database.