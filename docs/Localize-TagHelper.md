### Setup
Localize views using `LocalizeTagHelper`, first it must be added to `_ViewImports.cshtml_
````razor
@addTagHelper *, LazZiya.ExpressLocalization
````

> `LocalizeTagHelper` is included in `LazZiya.ExpressLocalization` starting from v4.0, for earlier versions it must be installed separately. See the bottom of this page for instructions.

### Localize views
Localize texts/html contents using `localize-content` attribute with any html tag
````html
<h1 localize-content>Sample header</h1>

<p localize-content>
    sample content...
</p>
````

### Localize Html Tag
Use `<localize>` html tag or to localize all its contents
````html
<localize>
    <h1>Sample title</h1>
    <p>sample contents...</p>
<localize>
````

### Localize attributes
Localize attributes like `title` inside `<img` tag using `localize-att-*`
````html
<img src="/images/picture.jpg" localize-att-title="Picture title"/>
````

### Specify culture
Force specific culture regardless the request culture
````html
<p localize-culture="tr">
    This content will always be localized with TR culture
</p>
````

### Provide arguments for localized contents
Localize a string that contains arguments by providing the arguments using `localize-args`
````html
@{
    var args = new[] { "http://demo.ziyad.info", 8, "Asp.Net Core" }
}

<p localize-args="args">
    Visit <a href="{0}">demos website</a> to see a collection of {1} demos about "{2}".
</p>
````
> Localized output sample: Visit [demos website](http://demo.ziyad.info) to see a collection of 8 demos about "Asp.Net Core".

### Localization resource
Specify localization resource type using `localize-source`
````html
<p localize-source="typeof(SharedResourceType)">
    This content will be localized from a specific shared resource.
</p>
````

See [demo page][1].

[1]:http://demo.ziyad.info/en/localize


### Applies to ExpressLocalization versions:
 4.0