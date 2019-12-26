using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.API.Services
{
    public class EventDispatcherOptions
    {
        public EventDispatcherOptions(TimeSpan interval)
        {
            this.Interval = interval;
        }

        public TimeSpan Interval { get; }
    }
    public class EventDispatcher : BackgroundService
    {
        private readonly ILogger<EventDispatcher> _logger;
        private readonly EventDispatcherOptions _options;

        public EventDispatcher(ILogger<EventDispatcher> logger, EventDispatcherOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting events dispather...");

            await ProcessEvents(stoppingToken);
        }

        private async Task ProcessEvents(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Fetching events...");

                _logger.LogInformation($"Events dispatched, next execution in {_options.Interval}.");

                await Task.Delay(_options.Interval, stoppingToken);
            }
        }
    }
}
