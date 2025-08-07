using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace R86.Avalonia.Hosting;

public abstract partial class HostedApplication<T> where T : HostedApplication<T>, new()
{
    public sealed class AvaloniaApplicationBuilder
    {
        public IServiceCollection Services => _hostBuilder.Services;
        public ILoggingBuilder Logging => _hostBuilder.Logging;
        public IConfigurationManager Configuration => _hostBuilder.Configuration;
        public T Build(ServiceProviderOptions? serviceProviderOptions = null, Action<IClassicDesktopStyleApplicationLifetime>? lifetimeBuilder = null)
        {
            serviceProviderOptions ??= new ServiceProviderOptions()
            {
#if DEBUG
                ValidateOnBuild = true
#else
                ValidateOnBuild = false
#endif
            };

            _hostBuilder.ConfigureContainer(new DefaultServiceProviderFactory(serviceProviderOptions));

            // Will create the `Application` instance
            _appBuilder.SetupWithClassicDesktopLifetime(_args, lifetimeBuilder);

            // For `StartAsync` && `StopAsync`
            _hostBuilder.Services.AddHostedService<T>(x => (T)Application.Current!);

            IHost host = _hostBuilder.Build();

            HostedApplication<T> app = host.Services.GetRequiredService<T>();

            app.Services = host.Services;

            return (T)app;
        }

        readonly AppBuilder _appBuilder;
        readonly HostApplicationBuilder _hostBuilder;
        readonly string[] _args;
        internal AvaloniaApplicationBuilder(string[] args,
           Func<AppBuilder>? builderFactory = null)
        {
            _args = args;
            _hostBuilder = Host.CreateEmptyApplicationBuilder(null);

            _appBuilder = (builderFactory ?? AppBuilder.Configure<T>).Invoke();
        }


    }
}

