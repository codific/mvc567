## Admin Controllers

Admin controllers are applicable only for the administration panel. They are separated into two main types:

#### Entity related

These controllers are used for CRUD operations and have following structure:
```
[Area("Admin")]
[Route("admin/example-entities/")]
[ApiExplorerSettings(IgnoreApi = true)]
[ValidateAdminCookie]
[Authorize(Policy = ApplicationPermissions.AccessAdministrationPolicy)]
public class AdminExampleEntitiesController : AbstractEntityController<ExampleEntity, ExampleEntityDto>
{
    public AdminExampleEntitiesController(IEntityManager entityManager) : base(entityManager)
    {
    }
}
```
The attributes of the controller has following functions:
* **Area** - Define administration area
* **Route** - Define route to the controller
* **ApiExplorerSettings** - Remove controller from Swagger
* **ValidateAdminCookie** - Protects the controller by the third factor authentication
* **Authorize** - Protects the controller by policy authorization

The inheritance of **AbstractEntityController** and the features provided by the base controller are explained in details in [Application Features Guide - CRUD Pages](https://mvc567.com/documentations/application-features-guide/crud-pages).

#### Independent

These controllers are used for any reason and have no specific difference than **Public Controllers**. The only requirement for these controllers are the attributes which must be the same as the **Entity related admin controllers**.