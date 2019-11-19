## Logging

The error handling is one of the most important parts of each application. There are many choices when you need to set up a logging system and this is the reason that mvc567 is not providing a complete logging solution. Following the structure of the ASP.NET Core, you can set up the logging system you desire.

After all the platform provides logging feature which is sandboxed in the business layer of the application. To use it you can use two methods which are part of the **AbstractService**:

| Method | Parameters | Return | Description |
| --- | --- | --- | --- |
| LogErrorAsync | Exception exception, string method | Task | Create log item to the database. |
| LogError | Exception exception, string method | void | Create log item to the database. |

The usage of these methods is usually in the catch block of your service:
```
public async Task<string[]> GetAllLanguageCodesAsync()
{
    try
    {
        var languages = await this.standardRepository.GetAllAsync<Language>();
        string[] languageCodes = languages.Select(x => x.Code).ToArray();
        return languageCodes;
    }
    catch (Exception ex)
    {
        await LogErrorAsync(ex, nameof(GetAllLanguageCodesAsync));
        return null;
    }
}
```

In the administration panel, there is a page where you can check and clear the logs.