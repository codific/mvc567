## Run The Application

Before starting the application you must follow the instructions:

##### Add migrations

You must execute Entity Framework migration CLI to create initialized migration file which will the starting point for your database scheme.

In case you use **Package Manager Console** you can execute:
```
add-migration Init
```

In case you use the terminal you must execute:
```
dotnet ef migrations add Init
```

Application is configured to detect if there is non-executed migrations and will trigger the database update operation on application start.

##### NPM packages

In case you are with Visual Studio on Windows, your npm packages defined in **package.json** will be installed automatically. In case you are a macOS user you must execute:

```
npm install
```

##### Gulpfile tasks

In the main project generated from the CLI, there are a few gulp tasks. Some of them are used for Vue components and they are applied to be executed on each build of the project. Other tasks are related to Sass files for style definition. In case you use Visual Studio you must refresh your tasks in **Task Runner Explorer**. At the end, you must run the *styles* task to build the **css** files.

##### Run the application

If each step is completed you can run the application.
