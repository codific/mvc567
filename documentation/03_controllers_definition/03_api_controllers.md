## API Controllers

The platform allows creation of fully functional API controller by using just an abstract class. By using **AbstractApiController** developers are able to make entity API controller which contains following functionality:

* **Get** [GET] - Get all entities.
* **Get By Id** [GET] - Get specific element by using the entity id.
* **Filter** [GET] - Get specific element/s by using customized query string.
* **Create** [POST] - Create element by using the DTO version of the entity.
* **Modify** [PUT] - Modify element by using the DTO version of the entity and entity id.
* **Delete** [DELETE] - Delete entity by using the entity id.

#### Get
The main purpuse of this end point is to allow users to get list of all entities. An example **Get** request is:
```
[HTTP GET] https://example-domain.com/api/entities/
```

#### Get By Id
The main purpuse of this end point is to allow users to get specific element by element Id. An example **Get By Id** request is:
```
[HTTP GET] https://example-domain.com/api/entities/83bc4eda-f7ee-4b34-be4c-cddf03d2acbe
```

#### Filter
Filter end poind is using to make specific query to entities from the database. This end point has more complex logic than other API request because there are many possible options you can pass to the query string:

##### Query String Options:
| Options | Specifications | Description |
| --- | :---: | --- |
| `query` | In case of pure string like `dog` the search engine will search for match with entity properties that contain **SearchCriteriaAttribute**. The other option is to use custom made query patterns (available in the table below). | Filter elements. |
| `order` | A list of properties of the current entity in specific pattern (available in the table below). The order of the properties is the order which be used in the order expression. | Order elements. |
| `page` | A number that represent the current page of the paginated entities. | Get elements on selected page. |
| `page-size` | A number that represent the page size of paginated result. | Separate elements on pages with specified size. |


###### Query Option Patterns
| Pattern | Description |
| --- | --- |
| `@PropertyName=value;` | Check whether **PropertyName** is equal to **value** |
| `@PropertyName~=value;` | Check whether **PropertyName** contains **value** |
| `@PropertyName>=value;` | Check whether **PropertyName** is greater than or equal to **value** |
| `@PropertyName<=value;` | Check whether **PropertyName** is less than or equal to **value** |
| `@PropertyName>value;` | Check whether **PropertyName** is greater than **value** |
| `@PropertyName<value;` | Check whether **PropertyName** is equal than **value** |
| `@PropertyName!=value;` | Check whether **PropertyName** is not equal to **value** |


###### Order Option Patterns
| Pattern | Description |
| --- | --- |
| `@PropertyName;` | Order filtered entities by **PropertyName** ascending. |
| `@PropertyName:a;` | Order filtered entities by **PropertyName** ascending. |
| `@PropertyName:d;` | Order filtered entities by **PropertyName** descending. |

An example **Filter** request is:
```
[HTTP GET] https://example-domain.com/api/entities/filter?query=@Name~=Ivan;@Age>32;&order=@Created:d;@Age:a;&page-size=20&page=2
```

#### Create

Create end point is using to create entities by using the provided API. To create an entity there is a requirement of entitity DTO passed into the body of HTTP POST request. An example **Create** request is:

```
[HTTP POST] https://example-domain.com/api/entities
```
Example Request Body:
```
{
    "name": "Ivan",
    "age": 32
}
```

#### Modify

Modify end point is using to modify entities by using the provided API. To modify an entity there is a requirement of entitity DTO passed into the body of HTTP PUT request and the Id of target entity. An example **Modify** request is:

```
[HTTP PUT] https://example-domain.com/api/entities/83bc4eda-f7ee-4b34-be4c-cddf03d2acbe
```
Example Request Body:
```
{
    "id": "83bc4eda-f7ee-4b34-be4c-cddf03d2acbe"
    "name": "Ivanka",
    "age": 33
}
```

#### Delete

Delete end point is using to delete entities by using the provided API. To delete an entity its Id is a must. An example **Delete** request is:

```
[HTTP DELETE] https://example-domain.com/api/entities/83bc4eda-f7ee-4b34-be4c-cddf03d2acbe
```

---

In general API controllers have non-specific requirements than ordinary ASP.NET Core API controllers. In case you have issues with these type of controllers check the [official ASP.NET Core documentation](https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-2.2).