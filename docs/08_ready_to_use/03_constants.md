## Constants

Already defined constants:

##### Source: Mvc567.Common.Constants

| Name | Type | Value |
| --- | --- | --- |
| PrivateRootFolderName | string | *"privateroot"* |
| UploadFolderName | string | *"uploads"* |
| GlobalFolderName | string | *"global"* |
| UsersFolderName | string | *"users"* |
| ContentFolderName | string | *"content"* |
| TempFolderName | string | *"temp"* |
| AssetsFolderName | string | *"assets"* |
| ImagesFolderName | string | *"images"* |
| LanguagesFolderName | string | *"locales"* |
| DateFormat | string | *"dddd, dd MMMM yyyy"* |
| TimeFormat | string | *"HH:mm"* |
| DateTimeFormat | string | *"dddd, dd MMMM yyyy HH:mm"* |
| DefaultAreasViewsPath | string | *"/Views/AreasViews"* |
| DefaultControllersViewsPath | string | *"/Views/ControllersViews"* |
| DefaultEmailViewsPath | string | *"/Views/EmailViews"* |
| ControllerStaticPageRoute | string | *"{*route:regex(^([[a-zA-Z0-9-/]]+)$)}"* |
| LanguageControllerRouteKey | string | *"language"* |
| LanguageControllerPageRoute | string | *"{" + LanguageControllerRouteKey + ":length(2,2)}"* |
| LanguageControllerStaticPageRoute | string | *LanguageControllerPageRoute + "/" + ControllerStaticPageRoute* |
| LanguageCookieName | string | *".Mvc567.Language"* |
---
##### Source: Mvc567.DataAccess.Identity.CustomClaimTypes

| Name | Type | Value |
| --- | --- | --- |
| Permission | string | *"Permission"* |
---
##### Source: Mvc567.DataAccess.Identity.UserRoles

| Name | Type | Value |
| --- | --- | --- |
| Admin | string | *"Admin"* |
| User | string | *"User"* |
---
##### Source: Mvc567.DataAccess.Identity.ApplicationPermissions

| Name | Type | Value |
| --- | --- | --- |
| AccessAdministrationPolicy | string | *"Access Administration"* |
| AccessErrorLogsPolicy | string | *"Access Error Logs"* |
| UsersManagementPolicy | string | *"Users Management"* |
| PublicRootAccessPolicy | string | *"Public Root Access"* |
| PrivateRootAccessPolicy | string | *"Private Root Access"* |
| StaticPageManagementPolicy | string | *"Static Page Management"* |
| SearchEngineOptimizationManagementPolicy | string | *"Search Engine Optimization Management"* |
| LanguagesManagementPolicy | string | *"Languages Management"* |
