using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using VaultSharp.V1.Commons;

namespace TodoApi.RequestHelpers;

public class VaultSecretProvider
{
    private readonly IVaultClient _vaultClient;

    public VaultSecretProvider(string vaultUri, string vaultToken)
    {
        var authMethod = new TokenAuthMethodInfo(vaultToken);
        var vaultClientSettings = new VaultClientSettings(vaultUri, authMethod);
        _vaultClient = new VaultClient(vaultClientSettings);
    }

    public async Task<Secret<SecretData>> GetSecretAsync(string path, string mountPoint)
    {
        return await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: path, mountPoint: mountPoint);
    }

}