## Public Controllers

Public controllers are used for all public requests outside of the administration. An ordinary Public controller might be:

```
[Route("example-entities/")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ExampleEntitiesController : AbstractController
{
    public ExampleEntitiesController(
        IConfiguration configuration,
        IEmailService emailService,
        ILanguageService languageService)
        : base(configuration, emailService, languageService)
    {
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
}
```
The attributes which decorated these controllers has the same functionality as [Admin Controllers](https://mvc567.com/documentations/controllers-definition/admin-controllers).

By inheriting the **AbstractController** the Public controller receive the all functionality for auto languages management. More about Language feature could be found on [Application Features Guide - Languages](https://mvc567.com/documentations/application-features-guide/languages).