## Admin Menus

By adding more features to the platform you have to visualize them on the administration panel. The structure of the administration panel is available in a single JSON file called **adminmenus.json** (in case you use the CLI of the mvc567). 

```
{
  "navigation": [
    {
      "title": "Dashboard",
      "controller": "AdminDashboard",
      "action": "Index",
      "area": "Admin",
      "single": true,
      "icon": "mdi mdi-television"
    },
    {
      "title": "Roots",
      "controller": "AdminRoots",
      "single": false,
      "icon": "mdi mdi-folder-open",
      "children": [
        {
          "title": "Public Root",
          "action": "PublicRoot",
          "controller": "AdminRoots",
          "area": "Admin"
        },
        {
          "title": "Private Root",
          "action": "PrivateRoot",
          "controller": "AdminRoots",
          "area": "Admin"
        }
      ]
    }
  ]
}
```

The the JSON file is structured on two levels:
* Main sections:
  * Title
  * Controller 
  * Action 
  * Area
  * Single 
  * Icon
  * Children
* Secondary sections:
  *  Title
  *  Action
  *  Controller
  *  Area

The correct set up of actions, controllers, and areas will give you better user experience and it is quite recommended. 