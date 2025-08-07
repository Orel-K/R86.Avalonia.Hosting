# R86.Avalonia.Hosting

## For modern familiar dotnet experience

```csharp
var builder = App.CreateBuilder(args, BuildAvaloniaApp);

builder.Logging.AddProvider(new UiLoggerProvider());

builder.Services.AddTransient<MainViewModel>();

builder.Services.AddHostedService<MyBackgroundService>();

App app = builder.Build();

app.Run();
```
