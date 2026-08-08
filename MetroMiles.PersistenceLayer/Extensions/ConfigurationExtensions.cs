using Microsoft.Extensions.Configuration;

namespace MetroMiles.PersistenceLayer.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequiredConnectionString(this IConfiguration configuration, string name)
        => configuration.GetConnectionString(name)
            ?? throw new InvalidOperationException($"\"ConnectionStrings:{name}\" is not configured.");
}
