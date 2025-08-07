

# Change the application base class

## Before:
``` csharp
public partial class App : Application { ... }
```

## After:
``` csharp
public partial class App : HostedApplication<App> { ... }
```

## In Program.Main
```csharp
var builder = App.CreateBuilder(args, BuildAvaloniaApp);

App app = builder.Build();

app.Run();
```

## To get access to the service collection in any place in your project:
```csharp
App.Current.Services.GetRequiredService<IService>();
```
