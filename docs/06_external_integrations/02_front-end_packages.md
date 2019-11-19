## Front-end Packages

In case you want to import additional JavaScript package you have to use the **package.json** to add the desired libraries. In case you are not using Visual Studio on Windows you must execute the following command when you add the packages:
```
npm install
```

When the desired packages are installed and available in **node_modules** folder you must modify the **gulpfile.js** to add the new libraries to vendor partial. Follow the structure defined into a file:
```
gulp.task('vue:vendors', function () {
    return gulp.src([
        'node_modules/vue/dist/vue.min.js',
        'node_modules/vue-router/dist/vue-router.min.js',
        'node_modules/axios/dist/axios.min.js',
        'node_modules/vuelidate/dist/vuelidate.min.js',
        'node_modules/vuelidate/dist/validators.min.js',
        'node_modules/vue-cookies/vue-cookies.js',
        'node_modules/vue-i18n/dist/vue-i18n.min.js'
    ])
        .pipe(uglify())
        .pipe(inject.prepend('<script type="text/javascript">'))
        .pipe(inject.append('</script>'))
        .pipe(replace('@', '@@'))
        .pipe(concat('_VueVendors.cshtml'))
        .pipe(gulp.dest(csHtmlResultDirectory));
});
```
Be strictly careful with the order of the vendors.