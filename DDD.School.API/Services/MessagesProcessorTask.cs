using DDD.School.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.API.Services
{
    public class MessageProcessorTaskOptions
    {
        public MessageProcessorTaskOptions(TimeSpan interval, int batchSize)
        {
            Interval = interval;
            BatchSize = batchSize;
        }

        public TimeSpan Interval { get; }
        public int BatchSize { get; }
    }

    public class MessagesProcessorTask : BackgroundService
    {
        private readonly IMessageProcessor _processor;
        private readonly ILogger<MessagesProcessorTask> _logger;
        private readonly MessageProcessorTaskOptions _options;
        
        public MessagesProcessorTask(MessageProcessorTaskOptions options, ILogger<MessagesProcessorTask> logger, IMessageProcessor processor)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Starting message processor...");

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Processing new messages..."); 
                
                await _processor.ProcessMessagesAsync(_options.BatchSize);

                _logger.LogInformation($"Messages processed, next execution in {_options.Interval}.");

                await Task.Delay(_options.Interval, stoppingToken);
            }
        }

    }
}
