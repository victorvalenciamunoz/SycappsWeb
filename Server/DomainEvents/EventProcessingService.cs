using Polly;
using Polly.Retry;

namespace SycappsWeb.Server;

public class EventProcessingService : BackgroundService
{
    private readonly IEventQueue _eventQueue;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EventProcessingService> _logger;
    private readonly AsyncRetryPolicy _retryPolicy;

    public EventProcessingService(IEventQueue eventQueue, IServiceProvider serviceProvider, ILogger<EventProcessingService> logger)
    {
        _eventQueue = eventQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (exception, timeSpan, retryCount, context) =>
                {
                    _logger.LogWarning($"Retry {retryCount} encountered an error: {exception.Message}. Waiting {timeSpan} before next retry.");
                });
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var domainEvent = await _eventQueue.DequeueEventAsync<DomainEvent>(stoppingToken);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var handlerType = typeof(IEventHandler<>).MakeGenericType(domainEvent.GetType());
                    var handler = scope.ServiceProvider.GetRequiredService(handlerType);
                    await (Task)handlerType.GetMethod("Handle").Invoke(handler, new object[] { domainEvent });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while processing the event: {ex.Message}");
                // Optionally, move the event to a dead-letter queue or take other actions
            }
        }
    }
}
