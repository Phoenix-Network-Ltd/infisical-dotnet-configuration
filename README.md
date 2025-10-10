# Infisical.IConfigurationProvider

This is a .NET library that makes it easy to use the [.NET configuration system](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0) with [Infisical](https://infisical.com/).

[![Nuget](https://img.shields.io/nuget/dt/Infisical.IConfigurationProvider)](https://www.nuget.org/packages/Infisical.IConfigurationProvider)

## Installation

```shell
dotnet add package Infisical.IConfigurationProvider
```

## Example usage

```csharp
using Infisical.IConfigurationProvider;

var builder = WebApplication.CreateBuilder(args);


builder.Configuration
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddInfisical(
        new InfisicalConfigBuilder()
            .SetProjectId("<your-project-id>")
            .SetEnvironment("<env-slug>")
            .SetSecretPath("<secret-path>") // Optional, defaults to "/"
            .SetInfisicalUrl("https://infisical-instance.com") // Optional, defaults to https://infisical.com
            .SetAuth(
                new InfisicalAuthBuilder()
                    .SetUniversalAuth(
                        "<machine-identity-client-id",
                        "<machine-identity-client-secret>"
                    )
                    .Build()
            )
            .Build()
    )
    .Build();

// Add services to the container.
```

## How do I format secret keys?

Secret keys should be formatted like environment variables. For example, consider this `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=db.sqlite3"
  }
}
```

The equivalent of this JSON would be a secret in Infisical with the key `CONNNECTIONSTRINGS__DEFAULTCONNECTION`. Note the double underscore. More information on this is available in [ASP.NET Core's environment variable naming documentation](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-7.0#naming-of-environment-variables).


### InfisicalConfigBuilder Setters

**SetProjectId()** 
- `projectId` (string): The ID of the project to fetch secrets from.

**SetEnvironment()**
- `environmentSlug` (string): The environment slug to fetch secrets from.

**SetSecretPath()**
- `secretPath` (string): The secret path to fetch secrets from. Optional, and defaults to `/`

**SetInfisicalUrl()**
- `infisicalUrl` (string): The URL of your Infisical instance. Optional, and defaults to `https://app.infisical.com`.

**SetPrefix()**
- `prefix` (string): A string to prefix your secret keys with. Optional, and defaults to no prefix.

**SetAuth()**
- `auth` (InfisicalAuth): The authentication details that will be used for authenticating against the Infisical API. See more details below.

**ShouldExpandSecretReferences()**
- 'expandSecretReferences' (boolean): Whether it should expand secret references when fetching secrets.

**ShouldFetchSecretsRecursively()**
- 'recursive' (boolean): Whether it should look through all subfolders when fetching secrets, eg. fetch all secrets recursively.


### InfisicalAuthBuilder Setters

#### SetUniversalAuth()
- `clientId` (string): The client ID of your universal auth machine identity.
- `clientSecret` (string): The client secret of your universal auth machine identity.

#### SetAzureAuth()
- `identityId` (string): The ID of the identity you wish to authenticate with.
- `tokenProvider` (function): The function that will be called to retrieve your Entra ID authentication token. The authentication token will be used to authenticate against Infisical with.


##### Example usage:

The following example assumes that you are logged into Visual Studio with your Entra account. The identity used for authentication must have the same Tenet ID as the directory that you are logged into in Visual Studio.

Instead of using `VisualStudioCredential()`, you can also use the following token providers.
- `AzureCliCredential()`: Fetches Entra credentials from the CLI.
- `VisualStudioCodeCredential()`: Fetches Entra credentials from Visual Studio Code.
- `DefaultAzureCredential()`: Tries to fetch Entra credentials from multiple sources on your machine. Sources include Environment variables, Managed identity credentials, Azure CLI, PowerShell, Visual Studio, Visual Studio Code, and more. You can read more [here](https://learn.microsoft.com/en-us/dotnet/api/azure.identity.defaultazurecredential?view=azure-dotnet).

```csharp
.SetAuth(
  new InfisicalAuthBuilder()
    .SetAzureAuth("<identity-id>", async () =>
      {
        var vsCredential = new VisualStudioCredential();

        // Get JWT token from Visual Studio
        var token = await vsCredential.GetTokenAsync(
            new TokenRequestContext(
                ["https://management.azure.com/.default"]
            ),
            CancellationToken.None
        );

        // JWT token can be used to authenticate with Infisical
        return token.Token;
      }
    ).Build()
)


