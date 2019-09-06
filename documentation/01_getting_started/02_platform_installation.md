## Platform Installation

The mvc567 platform is a .NET Standard library which means that - the ordinary using requires just a reference to it. Unfortunately, it is not possible to make a customizable project with just a few references, that's why to use the platform you need a specific structure which is related to the structure of the platform. By using the platform command-line interface you are able to create the original structure which will provide you the optimal results. You can install the mvc567 cli by using the following command:

```
dotnet tool install --global Mvc567.Cli --version x.y.z
``` 
where x.y.z is the current version of the tool. You can check the current version from the following link: [Mvc567.Cli](https://www.nuget.org/packages/Mvc567.Cli/).

#### Create a new project

To create a new project all you have to do is to execute the following command in the command prompt/terminal from the folder you want to use for your project:

```
mvc567 init -n {projectName}
```
You must replace the placeholder 'projectName' with the name of your project.
The execution of the command must create a folder with the name of you set and the whole project structure.

#### Open the project

To open the project use the solution file (.sln) placed in the created folder. To use the optimal intellisense use Visual Studio or Rider.