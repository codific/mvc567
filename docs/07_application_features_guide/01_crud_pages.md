## CRUD pages

The main feature of mvc567 is the content management system integrated into the admin panel. The platform allows developers to create very customizable CRUD pages related to database entities. To create a CRUD page you have to do:

#### Create admin entity controller

These controllers are entity related admin controllers. You could check how to create **Admin Entity Controller** on [Admin controllers section](https://mvc567.com/controllers-definition/admin-controllers).

#### Define entity DTO's properties for CRUD actions

The platform gives you the opportunity to skip the front-end creation while creating CRUD pages but you have to define which CRUD actions are going to be available for the entity. This availability is possible by using the attribute decorators. They are as follows:

##### Table view action

To allow a property to be shown on table view you must decorate the DTO property with **TableCellAttribute**. This attribute has a constructor which require the order of the property in the table (int), name of the property for table header (string) and type of visualization in table (enum TableCellType). Available table cell types are as follows:
* Text
* Date
* Time
* DateTime
* Number
* File
* Flag
* TextArea

##### Detailed view action

To allow a property to be shown in the detailed view you must decorate the DTO property with **DetailsOrderAttribute**. This attribute's constructor requires just order (int) in the details sheet.

##### Create/modify view action

To allow property be shown on create/modify forms you must decorate the DTO property with **CreateEditEntityInputAttribute** and the DTO class must inherit **CreateEditEntityViewModel**. This attribute's constructor requires name of the property (string) used for input label and input type (enum CreateEntityInputType). The available input types are:

* Text
* TextArea
* Email
* Password
* Date
* Time
* File
* Integer
* Double
* EnumSelect
* EnumCheckbox
* EnumRadio
* DatabaseSelect => *Requires using of additional attribute **DatabaseEnumAttribute***
* DatabaseCheckbox => *Requires using of additional attribute **DatabaseEnumAttribute***
* DatabaseRadio => *Requires using of additional attribute **DatabaseEnumAttribute***
* BoolSelect
* BoolRadio
* DatabaseTablesSelect

###### **DatabaseEnumAttribute** is a class that provide direct access to database table records. This attribute must be set up with a type of the desired database entity and name of the desired property

###### In addition, it is recomended to decorate the **Id** of the DTO with **EntityIdentifierAttribute** which will be used in future versions features.

Example DTO could be found in the [platform's repository](https://github.com/intellisoft567/mvc567/tree/master/src/Mvc567.Entities/DataTransferObjects/Entities).

#### Disable unnecessary actions

In case you do not need some of the available actions in **Admin Entity Controller** you could stop them by set the action related flag to false. By default, each flag is set to *true*:

```
protected bool HasGenericCreate { get; set; } = true;
protected bool HasDetails { get; set; } = true;
protected bool HasEdit { get; set; } = true;
protected bool HasDelete { get; set; } = true;
```
The best place where you can set different value to the flag is the constructor of the controller. 

#### Append actions to items

By default, each item from the table view has details, edit and delete actions. In addition to them, you might want to add other action related to each item separately. To that you have to add additional action by using the virtual method **TableViewActionsInit** which is part of **AbstractEntityController**:
```
protected override void TableViewActionsInit(refList<TableRowActionViewModel> actions)
{
    base.TableViewActionsInit(ref actions);
    actions.Insert(1, TableMapper.CreateAction(
                                    "Repository",
                                    MaterialDesignIcons.GithubFace,
                                    Color.SlateGray,
                                    TableRowActionMethod.Get,
                                    $"{{0}}",
                                    "[RepositoryUrl]"));
}
```

Action creation required two elements: 
* **Order** (int)
* **Action** (TableRowActionViewModel)

For action creation, you can use the static class **TableMapper** which allows you to create action easier than normal. 
Each action requires:
* **Name** (string)
* **Icon** (string) - you can use static class **MateriaDesignIcons** (you can check the icons from this [link](https://materialdesignicons.com/cdn/2.0.46/))
* **Color** (Color)
* **Method** (enum TableRowActionMethod)
* **Uri** (string) - you can set placeholders (e.g. *"/admin/entity/{0}/print"*)
* **Placeholder params** - you can use constant values or properties from the target entity in square brackets (e.g. [Id])

#### Append global entity actions

By default, each table view contains one global action **Create** which is part of the CRUD actions. In case when you want add additional action you can use virtual method **InitNavigationActionsIntoListPage** which is part of **AbstractEntityController**:

```
protected override void InitNavigationActionsIntoListPage(refAllEntitiesViewModel model)
{
    base.InitNavigationActionsIntoListPage(ref model);
    model.NavigationActions.Add(new NavigationActionViewModel
    {
        Name = "View Sitemap",
        ActionUrl = "/sitemap.xml",
        Icon = MaterialDesignIcons.Sitemap,
        SeparatePage = true
    });
}
```

where the additional action is represented by the **NavigationActionViewModel**:

| Property | Type | Description |
| --- | --- | --- |
| Name | string | Name of the action |
| Icon | string | Icon of the action. You can use the static class **MateriaDesignIcons** |
| ActionUrl | string | Target URL of the action |
| SeparatePage | bool | Flag that indicates whether the action to be opened on the same page or not |
| Method | HttpMethod | HTTP method of the action |
| Parameters | Dictionary\<string, string> | Parameters for actions with POST HTTP methods |
| HasConfirmation | bool | Flag that show or not popup for POST HTTP method-based actions. |
| ConfirmationTitle | string | Title of the visualized confirmation popup. |
| ConfirmationMessage | string | Message for the visualized confirmation popup. |

#### Search bar

To use the search bar of table view you must set up a single attribute with the properties you want to use as search criteria. All you need to do is to decorate the properties with **SearchCriteraAttribute** on the target database entity:
```
[Table("Logs")]
public class Log : AuditableEntityBase
{
    [SearchCriteria]
    [Column(TypeName = "ntext")]
    public string StackTrace { get; set; }

    [SearchCriteria]
    public string Source { get; set; }

    [SearchCriteria]
    public string Message { get; set; }

    [SearchCriteria]
    public string Method { get; set; }

    [SearchCriteria]
    public string Class { get; set; }
}
```