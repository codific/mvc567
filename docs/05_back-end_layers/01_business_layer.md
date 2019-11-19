## Business Layer

The business layer is the business logic container of the application. Communication between application layer (controllers) and the data layer (database repositories). The platform has nothing specific on business layer creation procedure compared with other ASP.NET Core applications. It provides a few ready to use services which help developers to improve their work.

#### Abstract Service

**Abstract Service** is an abstract class that provide general service accessibility to **IUnitOfWork** instance (facade class that provide access to all repositories), **IMapper** instance (class that provide AutoMapper functionality). In addition to these resources abstract class provides following functions:


| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| LogErrorAsync | Exception exception, string method | Task | Create log item to database. |
| LogError | Exception exception, string method | void | Create log item to database. |
| GetEntitySearchQueryExpression\<TEntity> | string searchQuery | Expression<Func<TEntity, bool>> | Create where expression from query string. |
---
#### IEntityManager

Generic service that contains the main CRUD functions for entities that implement **IEntityBase**.

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetEntityAsync\<TEntity, TEntityDto> | Guid id | Task\<TEntityDto> | Get entity by Id |
| GetAllEntitiesPaginatedAsync\<TEntity, TEntityDto> | int page, string searchQuery | Task\<PaginatedEntitiesResult\<TEntityDto>> | Get entities on pages |
| CreateEntityAsync\<TEntity, TEntityDto> | TEntityDto entity | Task\<Guid?> | Create entity |
| ModifyEntityAsync\<TEntity, TEntityDto> | Guid id, TEntityDto modifiedEntity | Task\<Guid?> | Modify entity |
| DeleteEntityAsync\<TEntity> | Guid id | Task\<bool> | Delete entity |
| MoveTempFileAsync\<TEntity> | TEntity entity | Task | Move temp entity file into its pre-defined directory. |
| FilterEntitiesAsync\<TEntity, TEntityDto> | FilterQueryRequest filterQuery | Task\<PaginatedEntitiesResult<TEntityDto>> | Filter entities by using parsed query string parameters. |
---
#### IEmailService

Service that provides the email functionality.

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| SendEmailAsync | string viewName, AbstractEmailModel model | Task\<EmailServiceResult> | Send email by using pre-defined email view and specific email model. |
---
#### IAuthenticationService

Service that provides login functionality.

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| GetUserClaimsAsync | User user | Task\<IEnumerable\<Claim>> | Get claims of user. |
| UserHasAdministrationAccessRightsAsync | User user | Task\<bool> | Check that user has access rights. |
| SignInAsync | User user, string password, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties | Task\<SignInResult> | Sign in user |
| SignInWith2faAsync | User user, string authenticationCode, bool rememberBrowser, HttpContext httpContext, string authenticationScheme, AuthenticationProperties authenticationProperties | Task\<SignInResult> | Sign in user using two factor authentication. |
| GetTwoFactorAuthenticationUserAsync | HttpContext httpContext | Task\<User> | Get user that activate its two factor authentication form from HttpContext. |
| SignOutAsync | HttpContext httpContext | Task | Sign out user. |
| BuildTokenAsync | string email, string password | Task\<BearerAuthResponse> | Build json web token from user credentials. |
| RefreshTokenAsync | Guid? userId, string refreshToken | Task\<BearerAuthResponse> | Refresh json web token by using refresh token. |
| ResetUserRefreshTokensAsync | Guid userId | Task\<bool> | Reset refresh token and its expiration time. |
---
#### Other services

Into package Mvc567.Services there are available more services than shown ones. These services are directly related to platform functionality and it is quite possible to be useless for your purposes. In case you want to explore them find their source on [GitHub Repository](https://github.com/intellisoft567/mvc567/tree/master/src/Mvc567.Services/Infrastructure).