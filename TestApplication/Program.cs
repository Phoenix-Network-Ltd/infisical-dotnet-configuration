using InfisicalConfiguration;

namespace TestApplication;

public class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        // Set up config options
        TryAddInfisicalSecrets(builder.Configuration);

        WebApplication app = builder.Build();

        app.MapControllers();

        app.Run();
    }

    private static void TryAddInfisicalSecrets(ConfigurationManager builder)
    {
        var clientId = "FILL_CLIENT_ID_HERE";
        var clientSecret = "FILL_SECRET_HERE";
        var projectId = "FILL_PROJECT_ID_HERE";
        var environment = "development";

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret)
        || string.IsNullOrEmpty(projectId) || string.IsNullOrEmpty(environment))
        {
            throw new InvalidOperationException("Infisical: ClientId, ClientSecret, ProjectId or Environment was null or empty!");
        }

        var infisicalAuth = new InfisicalAuthBuilder()
            .SetUniversalAuth(clientId, clientSecret)
            .Build();

        var infisicalConfig = new InfisicalConfigBuilder()
            .SetProjectId(projectId)
            .SetEnvironment(environment.ToLower())
            .SetSecretPath("/")
            .SetInfisicalUrl("https://infisical.phoenixnetwork.dev")
            .SetAuth(infisicalAuth)
            .Build();

        builder.AddInfisical(infisicalConfig);
    }
}
