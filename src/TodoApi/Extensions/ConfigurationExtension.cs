using TodoApi.RequestHelpers;

namespace TodoApi.Extensions;

public static class ConfigurationExtension
{
    public static IConfigurationBuilder AddVaultSecrets(this IConfigurationBuilder builder, VaultSecretProvider vaultSecretProvider, string path, string mountPoint)
    {
        var secret = vaultSecretProvider.GetSecretAsync(path, mountPoint).GetAwaiter().GetResult();
        // Convert the secret data to a dictionary of with string keys and string values.
        var secrets = new Dictionary<string, string>();
        foreach (var kv in secret.Data.Data)
        {
            secrets.Add(kv.Key, kv.Value.ToString());
        }

        builder.AddInMemoryCollection(secrets);
        return builder;
    }
}