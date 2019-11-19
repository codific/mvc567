## Back-end Resources Registration

To register external resources like your services or middlewares you might use pre-defined virtual methods which provide you the possibility to register whatever you want. You can find these options into your startup class - the class which base is **ApplicationStartup**:

##### Register Database Context 

```
protected override void RegisterDbContext(ref IServiceCollection services)
{
    base.RegisterDbContext(ref services);
}
```

##### Register Services

```
protected override void RegisterServices(ref IServiceCollectionservices)
{
    base.RegisterServices(ref services);
}
```

##### Add Authorization Options

```
protected override void AddAuthorizationOptions(ref AuthorizationOptions options)
{
    base.AddAuthorizationOptions(ref options);
}
```

##### Configure Middleware Before Authentication

```
protected override void ConfigureMiddlewareBeforeAuthenticatio(ref IApplicationBuilder app)
{
    base.ConfigureMiddlewareBeforeAuthentication(ref app);
}
```

##### Configure Middleware After Authentication

```
protected override void ConfigureMiddlewareAfterAuthenticatio(ref IApplicationBuilder app)
{
    base.ConfigureMiddlewareAfterAuthentication(ref app);
}
```

##### Register Mapping Profiles

```
protected override void RegisterMappingProfiles(ref IMapperConfigurationExpression configuration)
{
    base.RegisterMappingProfiles(ref configuration);
}
```

##### Register Feature Providers

```
protected override void RegisterFeatureProviders(ref ApplicationPartManager applicationPartManager)
{
    base.RegisterFeatureProviders(ref applicationPartManager);
}
```

##### Register Routes

```
protected override void RegisterRoutes(ref IRouteBuilder routes)
{
    base.RegisterRoutes(ref routes);
}
```