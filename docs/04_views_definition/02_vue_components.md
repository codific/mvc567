## Vue Components

Vue components in mvc567 are a combination between real Vue.js component wrapped into tag helper. 

#### Load Vue component
To execute Vue component into Razor view you do the following actions:

* Create Vue init file **App.js** into **~\Scripts\VueComponents**, e.g.:
  ````
  let appInitPromises = [];

  const routes = [
      { path: '/' },
      { path: '/:language' }
  ];

  const router = new VueRouter({
      routes: routes,
      mode: 'history'
  });

  const cookieKey = '.Mvc567.Language';
  const loadSystemLanguage = function (language) {
      return new Promise((resolve, reject) => {
          axios.get("/locales/" + language + ".json")
              .then(response => {
                  i18n.setLocaleMessage(language, response.data);
                  resolve(response);
              })
              .catch(error => {
                  reject(error);
              });
      });
  };

  const englishLanguageCode = 'en';
  const defaultLanguage = window.$cookies.get(cookieKey) ||englishLanguageCode;

  let i18n = new VueI18n({
      locale: defaultLanguage,
      fellbackLocale: defaultLanguage
  });

  appInitPromises.push(loadSystemLanguage(defaultLanguage));

  Promise.all(appInitPromises).then(function () {
      new Vue({
          el: "#app",
          i18n,
          router
      });
  });
  ````
* Create Vue component into **~\Scripts\VueComponents**, e.g. Home.js :
  ````
  Vue.component('home', {
    data: function () {
        return {
            platformName: 'mvc567'
        };
    },
    template: `
        <div>
            <h1>Hello {{platformName}}</h1>
        </div>
    `
    });
  ````
* Create a Vue app element in the layout:
  ```
  <div id="app" v-cloak>
      @RenderBody()
  </div>
  ```
* Insert Vue init sections into end layout body:
  ```
  <vue-init></vue-init>
  @RenderSection("Vue", required: false)
  <vue-import name="App"></vue-import>
  ```
After you have completed all required steps all you need to do in your Razor view is:
```
<div>
    <home></home>
</div>

@section Vue{
    <vue-import name="Home" />
}
```

###### How it works:
At the moment when you create a Vue component into **~\Scripts\VueComponents** folder and build the project, a task from **gulpfile.js** read the file and generate a Razor view (_{NameOfComponent}.Vue.cshtml). Then **vue-import** tag helper use the name of the component to import this Razor view into target view.

#### Nested Vue components

To use a component from component all you need to do is to import your Vue components in Vue section, ordered by their hierarchy of calls, e.g. if you want to use the component **'link'** from component **'navigation'** you must import component **'link'** before the other one.

#### Vue router

To use the features of Vue router you must define the different routes into **App.js**.

#### Auto-generated components

By using the **Mvc567.Cli** you can create Vue components by just one command:
```
mvc567 vue-component -n ComponentName
```
To use this command without issues you must execute it when you are inside the folder of your project (The folder where is placed **Script** folder).