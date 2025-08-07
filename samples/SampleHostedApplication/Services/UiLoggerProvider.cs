using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace SampleHostedApplication.Services;

public sealed class UiLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName) => UiLogger.Current;

    public void Dispose()
    {

    }
}

public sealed class UiLogger : ILogger
{
    public static UiLogger Current { get; } = new();

    private UiLogger() { }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {

    }
}
