## Tag Helpers

Tag Helpers enable server-side code to participate in creating and rendering HTML elements in Razor files. The platform contains a long list of tag helpers which allows users to have HTML-friendly development experience.

All tag helpers provided from the platform are available into package [Mvc567.Components](https://www.nuget.org/packages/Mvc567.Components/):

| Tag helper | Using | Description |
| --- | :---: | --- |
| Body | `<body></body>` | Make body accessible for other tag helpers. |
| Bool radio | `<bool-radio name="IsCorrect" value="true"></bool-radio>` | Create radio buttons that provide **yes** and **no** options. |
| Bool select | `<bool-select name="IsCorrect" value="true"></bool-select>` | Create a select dropdown that provides **yes** and **no** options. |
| Database checkbox | `<database-checkbox entity-type="typeof(Entity)" visible-property="Name" selected-values="Model.ClickedItems" model-name="ClickedItems"></database-checkbox>` | Create a list of checkboxes based on entity from the database. |
| Database radio | `<database-radio entity-type="typeof(Entity)" visible-property="Name" selected-value="Model.ClickedItem" model-name="ClickedItem"></database-radio>` | Create a list of radio buttons based on entity from the database. |
| Database select | `<database-select entity-type="typeof(Entity)" visible-property="Name" selected-value="Model.SelectedItem" name="SelectedItem" has-empty="true"></database-select>` | Create select dropdown based on entity from the database. |
| Database tables select | `<database-tables-select selected-value="Model.SelectedTable" name="SelectedTable" has-empty="true"></database-tables-select>` | Create select dropdown with database tables. |
| Date picker | `<date-picker name="TargetDay" value="12/25/2019"></date-picker>` | Create a date picker. |
| Enum checkbox | `<enum-checkbox enum="typeof(EntityEnum)" selected-values="Model.ClickedItems" model-name="ClickedItems"></enum-checkbox>` | Create a list of checkboxes based on enum type. |
| Enum radio | `<enum-radio enum="typeof(EntityEnum)" selected-value="Model.ClickedItem" model-name="ClickedItem"></enum-radio>` | Create a list of radio buttons based on enum type. |
| Enum select | `<enum-select enum="typeof(EntityEnum)" selected-value="Model.ClickedItem" name="ClickedItem"></enum-select>` | Create select dropdown based on enum type. |
| File uploader | `<file-uploader name="FileId" value="Model.FileId"></file-uploader>` | Create a file picker. |
| Form checkbox input | `<form-checkbox-input label="Contains" label-class="text-small" class="text-white"></form-checkbox-input>` | Create a style specific checkbox |
| Form text input | `<form-text-input label="Name" label-class="text-small" class="text-white" placeholder="Enter name.." type="text"></form-text-input>` | Create a style specific text input. |
| Form visible ReCaptcha | `<form-visible-recaptcha></form-visible-recaptcha>` | Import visible recaptcha into the form. Scripts and styles are uploaded automatically. |
| Head | `<head></head>` | Make head accessible for other tag helpers. |
| Menu section | `<menu-section controller="AdminDashboard" icon="mdi mdi-television" title="Dashboard" href="/admin" single="true"></menu-section>` | Create a section for dashboard sidebar |
| Meta tags | `<meta-tags title="HomePage" description="This is the home page of the website" keywords="page,website,seo" image="https://mvc567.com/assets/images/image001.jpg"></meta-tags>` | Create meta tags based on input information and global configuration. |
| Root file link | `<root-file-link id="Model.FileId"></root-file-link>` | Create a link to file from specific root. |
| Sidebar navigation link | `<sidebar-navigation-link title="Get all" area="Admin" action="GetAll" controller="Cars"/>` | Create a sub-menu of **menu section** for dashboard sidebar. |
| Time picker | `<time-picker name="StartTime" value="23:07"></time-picker>` | Create a time picker. |
| User full name | `<user-full-name/>` | Create a span that contains the full name of logged user. |
| Vue import | `<vue-import name="ComponentName"/>` | Import Vue component JavaScript. |
| Vue init | `<vue-init/>` | Import required Vue packages. |