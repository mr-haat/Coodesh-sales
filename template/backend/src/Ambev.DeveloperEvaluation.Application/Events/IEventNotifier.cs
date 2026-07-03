namespace Ambev.DeveloperEvaluation.Application.Events;

/// <summary>
/// Publishes domain events raised while handling application requests.
/// </summary>
public interface IEventNotifier
{
    /// <summary>
    /// Publishes the given domain event.
    /// </summary>
    /// <param name="domainEvent">The event to publish.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task NotifyAsync(object domainEvent, CancellationToken cancellationToken = default);
}
