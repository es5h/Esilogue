using Dapr;
using Esilogue.Services.OpenAi.API.IntegrationEvents.EventHandling;
using Esilogue.Services.OpenAi.API.IntegrationEvents.Events;
using Microsoft.AspNetCore.Mvc;

namespace Esilogue.Services.OpenAi.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class IntegrationEventController : ControllerBase
{
    private const string PubSubName = "esilogue-pubsub";

    [HttpPost("PostCreated")]
    [Topic(PubSubName, "PostCreatedIntegrationEvent")]
    public Task HandleAsync(
        PostCreatedIntegrationEvent @event,
        [FromServices] PostCreatedIntegrationEventHandler handler) =>
        handler.Handle(@event);
}
