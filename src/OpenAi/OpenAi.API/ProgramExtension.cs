namespace Esilogue.Services.OpenAi.API;

public static class ProgramExtension
{
    private const string AppName = "OpenAi.API";

    public static void AddCustomConfiguration(this WebApplicationBuilder builder) =>
        builder.Configuration.AddDaprSecretStore(
            "esilogue-secretstore",
            new DaprClientBuilder().Build());

    public static void AddCustomSwagger(this WebApplicationBuilder builder) =>
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = $"esilogue - {AppName}", Version = "v1" });
        });

    public static void UseCustomSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{AppName} V1");
        });
    }

    public static void AddCustomApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IEventBus, EventBus>();
        builder.Services.AddScoped<PostCreatedIntegrationEventHandler>();
        builder.Services.AddOpenAIService(setup =>
        {
            setup.ApiKey = builder.Configuration["OpenAi:ApiKey"]!;
        });
        builder.Services.AddSingleton<IOpenAIService, OpenAIService>();

        builder.Services.Configure<OpenAiSettings>(builder.Configuration);
    }
}