using Microsoft.Azure.KeyVault;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace LabApplicationBackendLayer.Utilities
{
    internal class ConfigReader
    {
        private ClientCredential _clientCredential;
        private AuthenticationContext _authenticationContext;
        private static IOptions<AllConfiguration> _allSettings;
        private static KeyVaultClient keyVaultClient;

        public ConfigReader(IOptions<AllConfiguration> allSettings)
        {
            _allSettings = _allSettings == null ? allSettings : _allSettings;
            keyVaultClient = new KeyVaultClient(GetKeyVaultAccessToken);
        }

        public static string SQLConnectionString
        {
            get
            {
                var secret = keyVaultClient.GetSecretAsync(_allSettings.Value.KeyVaultBaseURL, 
                        _allSettings.Value.SQLConnectionstringSecretKey).GetAwaiter().GetResult();
                return secret.Value;
            }
        }

        public static string CosmosDBURL
        {
            get
            {
                var secret = keyVaultClient.GetSecretAsync(_allSettings.Value.KeyVaultBaseURL, 
                            _allSettings.Value.CosmosDBURLKey).GetAwaiter().GetResult();
                return secret.Value;
            }
        }

        public static string CosmosDBPrimaryKey
        {
            get
            {
                var secret = keyVaultClient.GetSecretAsync(_allSettings.Value.KeyVaultBaseURL,
                            _allSettings.Value.CosmosDBPrimaryKey).GetAwaiter().GetResult();
                return secret.Value;
            }
        }

        public static string CosmosDBName
        {
            get
            {
                var secret = keyVaultClient.GetSecretAsync(_allSettings.Value.KeyVaultBaseURL,
                            _allSettings.Value.CosmosDBNameKey).GetAwaiter().GetResult();
                return secret.Value;
            }
        }

        public static string CosmosCollectionName
        {
            get
            {
                var secret = keyVaultClient.GetSecretAsync(_allSettings.Value.KeyVaultBaseURL,
                            _allSettings.Value.CosmosCollectionNameKey).GetAwaiter().GetResult();
                return secret.Value;
            }
        }

        public static string BackendDBType
        {
            get
            {
                return _allSettings.Value.BackendDBType;
            }
        }

        private async Task<string> GetKeyVaultAccessToken(string authority, string resource, string scope)
        {
            string clientId = _allSettings.Value.KVClientIdKey;
            string clientSecret = _allSettings.Value.KVClientSecretKey;
            
            if (_clientCredential == null)
            {
                _clientCredential = new ClientCredential(clientId, clientSecret);
            }

            if (_authenticationContext == null)
            {
                _authenticationContext = new AuthenticationContext(authority, TokenCache.DefaultShared);
            }

            var result = await _authenticationContext.AcquireTokenAsync(resource, _clientCredential);

            return result.AccessToken;
        }
    }
}
