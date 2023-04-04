using Esilogue.BuildingBlocks.EventBus;

namespace Esilogue.Services.OpenAi.API.IntegrationEvents.Events;

public record PostCreatedIntegrationEvent(int PostId, string Contents) : IntegrationEvent;
