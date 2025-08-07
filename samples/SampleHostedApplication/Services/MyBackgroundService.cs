using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SampleHostedApplication.Services;

public sealed class MyBackgroundService : BackgroundService
{
    readonly ILogger<MyBackgroundService> _logger;
    public MyBackgroundService(ILogger<MyBackgroundService> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        long tick = 0;

#if NET6_0_OR_GREATER
        using var pd = new PeriodicTimer(TimeSpan.FromSeconds(3));
        do
        {
            _logger.LogInformation("Ticking in the background.. {tick}", ++tick);
        }
        while (await pd.WaitForNextTickAsync(stoppingToken));
#else
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Ticking in the background.. {tick}", ++tick);
            await Task.Delay(3000, stoppingToken);
        }
#endif
    }
}
