using Esilogue.Services.OpenAi.API.Infrastructure.Repositories;

namespace Esilogue.Services.OpenAi.API.IntegrationEvents.EventHandling;

public class PostCreatedIntegrationEventHandler : IIntegrationEventHandler<PostCreatedIntegrationEvent>
{
    private readonly IOpenAIService _openAiService;
    private readonly OpenAiSettings _settings;
    private readonly ILogger<PostCreatedIntegrationEventHandler> _logger;
    private readonly IOpenAiRepository _openAiRepository;
    private readonly IEventBus _eventBus;

    public PostCreatedIntegrationEventHandler(IOpenAIService openAiService, IOptions<OpenAiSettings> settings, ILogger<PostCreatedIntegrationEventHandler> logger, IOpenAiRepository openAiRepository, IEventBus eventBus)
    {
        _openAiService = openAiService;
        _logger = logger;
        _openAiRepository = openAiRepository;
        _eventBus = eventBus;
        _settings = settings.Value;
    }

    public async Task Handle(PostCreatedIntegrationEvent @event)
    {
        var generatedSummary = await GenerateSummaryAsync(@event.Contents);
        
        var updatedSummary = await _openAiRepository.UpdateSummaryAsync(@event.PostId.ToString(), generatedSummary);
        
        await _eventBus.PublishAsync(new SummaryGeneratedIntegrationEvent(@event.PostId, updatedSummary));
    }
    
    private async Task<string> GenerateSummaryAsync(string contents) {
        var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
        {
            Messages = new List<ChatMessage>
            {
                ChatMessage.FromSystem("You are a helpful assistant for summarize the given article."),
                ChatMessage.FromUser(_settings.SummaryPrompt.Replace("{content}", contents)),
            },
            Model = Models.ChatGpt3_5Turbo,
            MaxTokens = 3000,
        });

        return !completionResult.Successful 
            ? "Failed to generate summary." 
            : string.Join("", completionResult.Choices.Select(x => x.Message.Content));
    }
}