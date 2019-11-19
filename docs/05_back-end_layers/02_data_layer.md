## Data Layer

The data layer is the place in your application where you have full access to your database. There is the place where you encapsulate your data to be used form the business layer. Into mvc567 there are two applied design patterns - Unit of work pattern and Repository pattern.

#### Unit of work

It is a design pattern that provides facade like access to all data resources like data repositories. To use it you have to access **IUnitOfWork** from your class. In details this unit of work provides:

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetStandardRepository | | IStandardRepository | Get the generic repository. |
| SaveChanges | | int | Save changes to the database. |
| SaveChanges | bool acceptAllChangesOnSuccess | int | Save changes to the database. |
| SaveChangesAsync | --- | Task\<int> | Save changes to database. |
| SaveChangesAsync | CancellationToken cancellationToken | Task\<int> | Save changes to database. |
| GetRepository\<TEntity> | | IRepository\<TEntity> | Get custom repository by entity type. |
| GetCustomRepository\<TRepository> | | TRepository | Get custom repository by repository type. |

#### IStandardRepository

The standard repository is a generic repository that provides all operations with entities from the database.

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetAll\<TEntity> |  IEnumerable\<TEntity> | Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get all entities. |
| GetAllAsync\<TEntity> | Task\<IEnumerable\<TEntity>> | Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get all entities. |
| Load\<TEntity> | void | Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Load entity. |
| LoadAsync\<TEntity> | Task | Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Load entity. |
| GetPage\<TEntity> | IEnumerable\<TEntity> | int startRow, int pageLength, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get page of entities. |
| GetPageAsync\<TEntity> | Task\<IEnumerable\<TEntity>> | int startRow, int pageLength, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get page of entities. |
| Get\<TEntity> | TEntity | Guid id, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get entity by Id. |
| GetAsync\<TEntity> | Task\<TEntity> | Guid id, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Get entity by Id. |
| Query\<TEntity> | IEnumerable\<TEntity> | Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Make query for entities. |
| QueryAsync\<TEntity> | Task\<IEnumerable\<TEntity>> | Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Make query for entities. |
| Load\<TEntity> | void | Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Load entity. |
| LoadAsync\<TEntity> | Task | Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Load entity. |
| QueryPage\<TEntity> | IEnumerable\<TEntity> | int startRow, int pageLength, Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Make query for entities on pages. |
| QueryPageAsync\<TEntity> | Task\<IEnumerable\<TEntity>> | int startRow, int pageLength, Expression\<Func\<TEntity, bool>> filter, Func\<IQueryable\<TEntity>, IOrderedQueryable\<TEntity>> orderBy = null, Func\<IQueryable\<TEntity>, IQueryable\<TEntity>> includes = null | Make query for entities on pages. |
| SetUnchanged\<TEntity> | void | TEntity entity | Set entity unchanged. |
| Add\<TEntity> | void | TEntity entity | Add entity. |
| Update\<TEntity> | TEntity | TEntity entity | Update entity. |
| Remove\<TEntity> | void | TEntity entity | Remove entity. |
| Remove\<TEntity> | void | Guid id | Remove entity. |
| CountAsync\<TEntity> | Task\<int> | Expression\<Func\<TEntity, bool>> filter | Count entities. |
| GetAllByEntityTableName | IEnumerable\<IEntityBase> |  string entityTableName| Get entities by table name. |
| GetAllByType | IEnumerable\<IEntityBase> | Type entityType | Get entities by type of the entity. |
| DeleteAll\<TEntity> | void | | Delete all entities. |
| DeleteAllAsync\<TEntity> | Task | | Delete all entities. |

#### Create custom repository

To create custom repository all you need to do is to create a repository interface and class then the class must inherit **RepositoryBase\<TContext>** abstract class.