using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleHostedApplication;
using SampleHostedApplication.Services;
using SampleHostedApplication.ViewModels;


var builder = App.CreateBuilder(args, BuildAvaloniaApp);

builder.Logging.AddProvider(new UiLoggerProvider());

builder.Services.AddTransient<MainViewModel>();

builder.Services.AddHostedService<MyBackgroundService>();

App app = builder.Build();

app.Run();


static AppBuilder BuildAvaloniaApp()
    => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .WithInterFont()
        .LogToTrace();
