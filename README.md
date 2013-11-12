# RollbarSharp

.NET bindings for [Rollbar](http://www.rollbar.com).

This project is still under development and should be considered in a preview release state.

I'm not affiliated with Rollbar, I just like their service.

As of now this binding is only for .NET 4. If there's a widespread desire for .NET 2.0 support, I'd consider doing that. There's also a dependency on `System.Web.Routing`, so you'll at least need ASP.NET MVC installed on the system.

## Installation

RollbarSharp is available on [Nuget](https://nuget.org/packages/RollbarSharp/) and can be installed by:

```
PM> Install-Package RollbarSharp
```

## Usage

### Configuration

The easiest way to get going is to add the `Rollbar.AccessToken` item to your app settings.

```xml
<configuration>
  <appSettings>
    <add key="Rollbar.AccessToken" value="YOUR_TOKEN_HERE"/>
    <add key="Rollbar.Environment" value="production"/>
  </appSettings>
</configuration>
```

### As an ASP.NET Filter

```csharp
public class RollbarExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext filterContext)
    {
        if (filterContext.ExceptionHandled)
            return;

        (new RollbarClient()).SendException(filterContext.Exception);
    }
}
```

And then in `Global.asax.cs` you can do

```csharp
GlobalFilters.Filters.Add(new RollbarExceptionFilter());
```

Or if you're using an inversion of control system that supports binding filters, you could do it there. This is an example with [Ninject](http://www.ninject.org/). You could even dependency-inject the `RollbarClient` if you want to create it in your own factory method.

```csharp
kernel.BindFilter<RollbarExceptionFilter>(FilterScope.Global, 10).InSingletonScope();
```

The `OnException` method could instead be used verbatim inside a `Controller` if you add the `override` keyword. `protected override void OnException(ExceptionContext filterContext) ...`


### As an ASP.NET application event handler (Global.asax.cs)

```csharp
protected void Application_Error(object sender, EventArgs e)
{
    var exception = Server.GetLastError().GetBaseException();

    (new RollbarClient()).SendException(exception);
}
```


### As an HttpModule in the Web.config

#### IIS Integrated Pipeline

```xml
<system.webServer>
    <modules>
        <add name="RollbarHttpModule" type="RollbarSharp.RollbarHttpModule"/>
    </modules>
</system.webServer>
```

#### IIS Classic Pipeline

```xml
<system.web>
    <httpModules>
        <add name="RollbarHttpModule" type="RollbarSharp.RollbarHttpModule"/>
    </httpModules>
</system.web>
```

### As an NLog target

You'll need to add the [NLog.RollbarSharp](https://github.com/mroach/NLog.RollbarSharp) assembly to your project for this to work but it's available on [NuGet](https://www.nuget.org/packages/NLog.RollbarSharp/). Check out [the NLog.RollbarSharp README](https://github.com/mroach/NLog.RollbarSharp/blob/master/README.md) for configuration details, but here's the skinny:

```xml
<nlog>
    <extensions>
        <add assembly="NLog.RollbarSharp" />
    </extensions>
    <targets>
        <target xsi:type="RollbarSharp" name="rollbar" />
    </targets>
    <rules>
        <logger name="*" minlevel="Warn" writeTo="rollbar" />
    </rules>
</nlog>
<appSettings>
    <add key="Rollbar.AccessToken" value="6703358e9f54081e59bb0d65ee066363"/>
    <add key="Rollbar.Environment" value="development"/>
</appSettings>
```

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
