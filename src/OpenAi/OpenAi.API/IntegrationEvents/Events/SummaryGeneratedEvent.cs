namespace Esilogue.Services.OpenAi.API.IntegrationEvents.Events;

public record SummaryGeneratedIntegrationEvent(int PostId, string Summary) : IntegrationEvent;