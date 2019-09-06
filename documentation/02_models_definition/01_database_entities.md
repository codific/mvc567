## Database Entities

Database entities are used for application model definition. To update the model you must create business entities and apply them to database context.

#### Create database entity

The creation of entity is just a making of class like one you would create to define Entity Framework context. The only difference than normally is that you must use one of the abstract classes provided by the platform.

* **EntityBase** - Contains just an **Id** of type *Guid*
* **AuditableEntityBase** - Inherit **EntityBase** and contains properties:
  * **CretedOn** (DateTime)
  * **CreatedBy** (string)
  * **UpdatedOn** (DateTime)
  * **UpdatedBy** (string)
* **AuditableByUserEntityBase\<TUser>** Inherit **AuditableEntityBase** and because it is a generic class - requires **IdentityUser\<Guid>** as a generic parameter. The class contains:
  * **UserId** (Guid)
  * **User** (TUser) where TUser is IdentityUser\<Guid>

Example database entity could be:
```
[Table("ExampleEntities")]
public class ExampleEntity : AuditableEntityBase
{
    public string Name { get; set; }
    
    public int Order { get; set; }
}
```

#### Apply database entity into database context

To apply database entity all you need to do is to make a property into **EntityContext** class:
```
public class EntityContext : AbstractDatabaseContext<EntityContext>
{
    public EntityContext(DbContextOptions<EntityContext> options) : base(options)
    { }

    public DbSet<ExampleEntity> ExampleEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
```