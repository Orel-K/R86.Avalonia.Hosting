using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace R86.Avalonia.Hosting;

public abstract partial class HostedApplication<T> : Application, IHostedService
    where T : HostedApplication<T>, new()
{
    public static new T Current => (T)Application.Current!;
    public IServiceProvider Services { get; internal set; } = default!;

    public static AvaloniaApplicationBuilder CreateBuilder(string[]? args = null,
        Func<AppBuilder>? builderFactory = null)
    {
        // For `top level statements`
        Thread.CurrentThread.TrySetApartmentState(ApartmentState.Unknown);
        Thread.CurrentThread.TrySetApartmentState(ApartmentState.STA);

        args ??= [.. Environment.GetCommandLineArgs().Skip(1)];

        var builder = new AvaloniaApplicationBuilder(args, builderFactory);

        builder.Services.AddSingleton(x => (T)Application.Current!);

        return builder;
    }

    public int Run()
    {
        Dispatcher.UIThread.VerifyAccess();

        var appLifeTime = (ClassicDesktopStyleApplicationLifetime)this.ApplicationLifetime!;

        var hostLifeTime = Services.GetRequiredService<IHostApplicationLifetime>();

        var host = Services.GetRequiredService<IHost>();

        appLifeTime.Startup += async delegate
        {
            await HostMain().ConfigureAwait(false);

            // `Shutdown` and not `TryShutdown`, not cancelable
            Dispatcher.UIThread.Invoke(delegate { appLifeTime.Shutdown(); });
        };

        appLifeTime.ShutdownRequested += (sender, request) =>
        {
            request.Cancel = true;
            hostLifeTime.StopApplication();
        };

        return Environment.ExitCode = appLifeTime.Start();
    }

    public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public virtual Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    async Task HostMain()
    {
        var hostLifeTime = Services.GetRequiredService<IHostApplicationLifetime>();

        var host = Services.GetRequiredService<IHost>();

        try
        {
            await host.StartAsync(hostLifeTime.ApplicationStopping);

            await host.WaitForShutdownAsync(hostLifeTime.ApplicationStopping).ConfigureAwait(false);
        }
        finally
        {
            if (host is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else
            {
                host.Dispose();
            }
        }
    }
}

