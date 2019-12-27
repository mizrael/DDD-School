using DDD.School.Persistence;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DDD.School.Services
{

    public class MessageProcessor : IMessageProcessor
    {
        private readonly ISchoolUnitOfWork _unitOfWork;
        private readonly IMessagePublisher _publisher;
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(ISchoolUnitOfWork unitOfWork, IMessagePublisher publisher, ILogger<MessageProcessor> logger)
        {
            _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public async Task ProcessMessagesAsync(int batchSize, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching messages...");

            var messages = await _unitOfWork.MessagesRepository.FetchUnprocessedAsync(batchSize, cancellationToken);
            foreach (var message in messages)
            {
                try
                {
                    await message.Process(_publisher, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"an error has occurred while processing message {message.Id}: {ex.Message}");
                }
            }
        }
    }
}
