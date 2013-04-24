# RollbarSharp

.NET bindings for [Rollbar](http://www.rollbar.com).

This project is still under development and should be considered in a preview release state. I haven't published any usage examples yet because the method signatures and workflow may well change.

I'm not affiliated with Rollbar, I just like their service.

As of now this binding is only for .NET 4. If there's a widespread desire for .NET 2.0 support, I'd consider doing that. There's also a dependency on `System.Web.Routing`, so you'll at least need ASP.NET MVC installed on the system.

## Bugs

* If you encounter a bug, performance issue, or malfunction, please add an [Issue](https://github.com/mroach/rollbarsharp/issues) with steps on how to reproduce the problem.


## Building

I'm using [Albacore](http://albacorebuild.net/) for managing the build and bundling process. You'll need Ruby on your system to use it.

Installing the necessary gems (you only need to do this once):

```
gem install albacore version_bumper
```

Fetch nuget dependencies (from the `build` directory):

```
rake fetch_packages
```

And finally, building:

```
rake build
```

## TODO

* Add more tests
* Test NLog extension
* Create nuget package for the NLog extension
