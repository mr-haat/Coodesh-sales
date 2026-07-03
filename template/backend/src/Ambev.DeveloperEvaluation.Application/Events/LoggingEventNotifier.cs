using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Event notifier that records published events in the application log.
/// </summary>
/// <remarks>
/// The challenge does not require an actual message broker, so events are written
/// to the application log. Replacing this implementation is enough to publish the
/// same events to a real broker.
/// </remarks>
public class LoggingEventNotifier : IEventNotifier
{
    private readonly ILogger<LoggingEventNotifier> _logger;

    /// <summary>
    /// Initializes a new instance of LoggingEventNotifier.
    /// </summary>
    /// <param name="logger">The application logger.</param>
    public LoggingEventNotifier(ILogger<LoggingEventNotifier> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Writes the published event to the application log.
    /// </summary>
    /// <param name="domainEvent">The event to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    public Task NotifyAsync(object domainEvent, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Domain event published: {EventType}", domainEvent.GetType().Name);
        return Task.CompletedTask;
    }
}
