using System;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;
using SampleHostedApplication.Models;

namespace SampleHostedApplication.Services;

public static class DiExt
{
    public static ILoggingBuilder AddUiLoggerProvider(this ILoggingBuilder builder)
    {
        builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<ILoggerProvider, UiLoggerProvider>());
        builder.Services.AddKeyedSingleton<IMessenger, WeakReferenceMessenger>("logs");
        return builder;
    }
}

public sealed class UiLoggerProvider : ILoggerProvider, ILogger
{
    readonly IMessenger _messenger;
    public UiLoggerProvider([FromKeyedServices("logs")] IMessenger messenger)
    {
        _messenger = messenger;
    }

    public ILogger CreateLogger(string categoryName) => this;

    public void Dispose()
    {

    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _messenger.Send(new LogMessageAdded(formatter(state, exception)));
    }
}

