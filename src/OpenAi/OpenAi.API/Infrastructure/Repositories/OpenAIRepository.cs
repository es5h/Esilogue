using System.Reflection.Metadata;

namespace Esilogue.Services.OpenAi.API.Infrastructure.Repositories;

public interface IOpenAiRepository
{
    Task<string> GetSummaryAsync(string postId);
    Task<string> UpdateSummaryAsync(string postId, string summary);
    Task DeleteSummaryAsync(string postId);
}

public class OpenAiRepository : IOpenAiRepository
{
    private const string StateStoreName = "openai-statestore";

    private readonly DaprClient _daprClient;
    private readonly ILogger _logger;

    public OpenAiRepository(DaprClient daprClient, ILogger<OpenAiRepository> logger)
    {
        _daprClient = daprClient;
        _logger = logger;
    }
    
    public Task<string> GetSummaryAsync(string postId) =>
        _daprClient.GetStateAsync<string>(StateStoreName, postId);

    public async Task<string> UpdateSummaryAsync(string postId, string summary)
    {
        var state = await _daprClient.GetStateEntryAsync<string>(StateStoreName, postId);
        state.Value = summary;
        
        await state.SaveAsync();

        return await GetSummaryAsync(postId);
    }
    
    public Task DeleteSummaryAsync(string postId) =>
        _daprClient.DeleteStateAsync(StateStoreName, postId);
}