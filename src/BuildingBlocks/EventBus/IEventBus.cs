using Dapr.Client;
using Microsoft.Extensions.Logging;

namespace Esilogue.BuildingBlocks.EventBus;

public interface IEventBus
{
    Task PublishAsync(IntegrationEvent integrationEvent);
}

public class EventBus : IEventBus
{
    private const string PubSubName = "esilogue-pubsub";

    private readonly DaprClient _dapr;
    private readonly ILogger _logger;

    public EventBus(ILogger logger, DaprClient dapr)
    {
        _logger = logger;
        _dapr = dapr;
    }

    public async Task PublishAsync(IntegrationEvent integrationEvent)
    {
        var topicName = integrationEvent.GetType().Name;

        _logger.LogInformation("Publishing event {@Event} to {PubSubName}.{TopicName}", integrationEvent, PubSubName,
            integrationEvent.GetType().Name);
        
        await _dapr.PublishEventAsync(PubSubName, topicName, integrationEvent);
    }
}