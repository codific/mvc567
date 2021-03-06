## Data Transfer Objects

Data transfer objects(DTO) are used for communication between the Business layer and Application layer and Client. The platform has a few specifications for DTO creation.

#### AutoMapper
AutoMapper is used for mapping between database entities and data transfer object entities. By default, AutoMapper is integrated into startup project when you using CLI of mvc567. To set AutoMapper to your entity DTO all you need to do is to set the following attribute to your DTO class.
```
[AutoMap(typeof(ExampleEntity), ReverseMap = true)]
``` 
For more information about AutoMapper and its setup, you can check their [official documentation](https://docs.automapper.org/en/stable/).
Example data transfer object would be:
```
[AutoMap(typeof(ExampleEntity), ReverseMap = true)]
public class ExampleEntityDto
{
    public string Name { get; set; }
    
    public int Order { get; set; }
}
```

#### Auto-generated data transfer objects

By using the **Mvc567.Cli** you can create DTO with just one command:
```
mvc567 entity-dto -e EntityName
```
To use this command without issues you must execute it when you are inside the folder of your solution and **cli-config.json** is a must. The CLI configuration file is a part of the initialized project.

#### CRUD attributes

Each DTO which will be used for administration panel could be extended by the platform DTO attributes. More information about these attributes is available on [Application Features Guide - CRUD Pages](https://mvc567.com/documentations/application-features-guide/crud-pages).
