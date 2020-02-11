# mvc567

mvc567 is a high performance, open-source ASP.NET Core based web application platform. It provides CMS functionality to its users. The platform is provided in the form of .NET Standard packages which provides simple integration in addition to features like content management, authentication, and authorization, security, and optimizations for search engines.
The mvc567 platform is a .NET Standard library which means that - the ordinary using requires just a reference to it. Unfortunately, it is not possible to make a customizable project with just a few references, that's why to use the platform you need a specific structure which is related to the structure of the platform. By using the platform command-line interface you are able to create the original structure which will provide you the optimal results. You can install the mvc567 cli by using the following command:

```
dotnet tool install --global Codific.Mvc567.Cli --version x.y.z
``` 
where x.y.z is the current version of the tool. You can check the current version from the following link: [Codific.Mvc567.Cli](https://www.nuget.org/packages/Codific.Mvc567.Cli/).

#### Create a new project

To create a new project all you have to do is to execute the following command in the command prompt/terminal from the folder you want to use for your project:

```
mvc567 init -n {projectName}
```
You must replace the placeholder 'projectName' with the name of your project.
The execution of the command must create a folder with the name of you set and the whole project structure.

##### For more details please check the [official documentation](https://codific.github.io/mvc567/) of mvc567.
