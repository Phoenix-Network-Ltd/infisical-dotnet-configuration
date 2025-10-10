using System.Text.Json;

namespace InfisicalConfiguration;

public class MachineIdentityLogin
{
    public string AccessToken { get; set; }

    public static MachineIdentityLogin Deserialize(string content)
    {
        var result = JsonSerializer.Deserialize<MachineIdentityLogin>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        if (result == null)
        {
            throw new InvalidOperationException("Failed to deserialize MachineIdentityLogin");
        }

        return result;
    }
}

public class SecretsList
{
    public List<Secret> Secrets { get; set; } = [];

    public static SecretsList Deserialize(string content)
    {
        var result = JsonSerializer.Deserialize<SecretsList>(content, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        return result;
    }
}

public class Secret
{
    public string SecretKey { get; set; }
    public string SecretValue { get; set; }
}
