using System;
using Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleHostedApplication.Services;
using SampleHostedApplication.ViewModels;

namespace SampleHostedApplication
{
    internal sealed class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            var builder = App.CreateBuilder(args, BuildAvaloniaApp);

            builder.Logging.AddProvider(new UiLoggerProvider());

            builder.Services.AddTransient<MainViewModel>();

            builder.Services.AddHostedService<MyBackgroundService>();

            App app = builder.Build();

            app.Run();
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();
    }
}
