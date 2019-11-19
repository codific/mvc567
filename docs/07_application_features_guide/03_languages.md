## Languages

The platform mvc567 has implemented a multi-language feature which allows developers to create their applications without worrying about the translations.

#### Languages, translation keys, translations

All languages, translation keys, and translations are able to be managed through the administration panel. Because the client front-end is developed to be used with Vue.js, mvc567 use i18n as a library for translations. This library requires a JSON file that contains all translations. This JSON file has to and could be generated from the table view of the languages.

#### Language routes and cookies

When you create at least one language, the **AbstractController** check the active language and set up a cookie which the library i18n reads. This cookie is used to be loaded the correct JSON file at the beginning. In case when is detected differently than the main language (by the route e.g. */bg/something*) the language cookie is going to be set up with the detected language.

#### Using the translations

The platform uses the library **vue-i18n** to visualize the translations so to learn how to use this library check their [official documentation](https://kazupon.github.io/vue-i18n/).