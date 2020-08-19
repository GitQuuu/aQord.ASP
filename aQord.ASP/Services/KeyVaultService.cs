using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using SendGrid;

namespace aQord.ASP.Services
{
    public class KeyVaultService
    {
        // Authenticate and create a client to retrieve keys&Secrets from the KeyVault https://docs.microsoft.com/en-us/azure/key-vault/secrets/quick-create-net

        /// <summary>
        /// authenticate and create client to retrieve keys & secret from keyvault
        /// </summary>
        /// <returns>SecretClient</returns>
        public static SecretClient AuthenticateCreateClient()
        {
            var keyVaultName = ConfigurationManager.AppSettings["KEY_VAULT_NAME"]; // Read webconfig value and azure app settings. https://stackoverflow.com/questions/44542409/how-to-read-azure-web-site-app-settings-values , https://docs.microsoft.com/en-us/dotnet/api/system.configuration.configurationmanager.appsettings?redirectedfrom=MSDN&view=dotnet-plat-ext-3.1
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";
            var clientVault = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            
            return clientVault;
        }

        // Use this with AuthencicateCreateClient() to retrieve secrets from vault
        public static KeyVaultSecret KeyVaultSecret(string secret, SecretClient clientVault) 
        {
            var keyVaultSecret = clientVault.GetSecret(secret);


            return keyVaultSecret;
        }

    }
}