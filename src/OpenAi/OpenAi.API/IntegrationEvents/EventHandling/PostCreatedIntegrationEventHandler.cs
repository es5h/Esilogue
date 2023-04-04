namespace Esilogue.Services.OpenAi.API.IntegrationEvents.EventHandling;

public class PostCreatedIntegrationEventHandler : IIntegrationEventHandler<PostCreatedIntegrationEvent>
{
    private readonly IOpenAIService _openAiService;
    private readonly OpenAiSettings _settings;

    public PostCreatedIntegrationEventHandler(IOpenAIService openAiService, IOptions<OpenAiSettings> settings)
    {
        _openAiService = openAiService;
        _settings = settings.Value;
    }

    public async Task Handle(PostCreatedIntegrationEvent @event)
    {
        var contents = @event.Contents;
        
    }
    
    private async Task GenerateSummary(string contents) {
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
        
        
    }
}