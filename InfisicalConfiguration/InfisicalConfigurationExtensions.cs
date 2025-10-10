using Microsoft.Extensions.Configuration;

namespace InfisicalConfiguration;

public static class InfisicalConfigurationExtensions
{
    public static IConfigurationBuilder AddInfisical(this IConfigurationBuilder builder, InfisicalConfig config)
    {
        return builder.Add(new InfisicalConfigurationSource(config));
    }
}
