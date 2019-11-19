## Search Engine Optimization

As a part of the platform, this feature won't make your system on the first order in Google. To apply SEO your development must be SEO oriented. 

The platform allows you to follow the best practices in the SEO development process by provides you instruments for improving the quality of your code from SEO aspects.

#### Meta tags

The first important part of each page is the head and more specific - the meta tags. The platform mvc567 provides you a tag helper **MetaTagsTagHelper** (more information on [Tag helpers section](https://mvc567.com/documentation/views-definition/tag-helpers)) which will fill the head with the right for your purpose meta tags.

#### Sitemap

In your administration panel, you can create a sitemap item pattern which allows you to modify your sitemap based on your database. The pattern has a related entity which is used for data extraction and pattern string in the following format:
```
/entities/[CreationDate]/[UniqueName]
```
where **CreationDate** and **UniqueName** are properties of our imagine entity

The SEO engine will get all entities and will apply them into the sitemap like this:
```
/entities/12-01-19/name-1
/entities/13-04-19/name-2
/entities/14-06-19/name-3
```

#### Friendly URL

This criteria quite depends on your routs so in case you do not follow the best practices - mvc567 is powerless.
After all in case you use multi-languages in your application - the platform automatically (in case of static page) set up the language code at the beginning of the routes like this:

Original route => */something*
Original route in French => */fr/something*
Original route in Bulgarian => */bg/something*