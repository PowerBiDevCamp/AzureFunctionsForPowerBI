using System;
using Microsoft.Identity.Client;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;
using Azure.Core;

namespace DevCampDemos.Services {

  class TokenManager {

    public static string GetAccessTokenWithLocalCredentials() {

      string appId = Environment.GetEnvironmentVariable("AppId");
      string appSecret = Environment.GetEnvironmentVariable("AppSecret");
      string tenentId = Environment.GetEnvironmentVariable("TenantId");
      string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenentId;

      var appConfidential = ConfidentialClientApplicationBuilder.Create(appId)
                                .WithClientSecret(appSecret)
                                .WithAuthority(tenantSpecificAuthority)
                                .Build();

      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };

      var authResult = appConfidential.AcquireTokenForClient(scopes).ExecuteAsync().Result;

      return authResult.AccessToken;

    }

    public static string GetAccessTokenWithCredentialsFromKeyVault() {

      // use Managed Identity credentials to get SecretClient to access key vault
      var credentials = new DefaultAzureCredential();
      var keyVaultUrl = "https://demodevcampvault.vault.azure.net/";
      var keyVaultSecretClient = new SecretClient(new Uri(keyVaultUrl), credentials);

      // retrieve values for secrets from key vault
      string appId = keyVaultSecretClient.GetSecret("AppId").Value.Value;
      string appSecret = keyVaultSecretClient.GetSecret("AppSecret").Value.Value;
      string tenentId = keyVaultSecretClient.GetSecret("TenantId").Value.Value;

      // acquire access token using client credentials flow
      string tenantSpecificAuthority = "https://login.microsoftonline.com/" + tenentId;
      var appConfidential = ConfidentialClientApplicationBuilder.Create(appId)
                                .WithClientSecret(appSecret)
                                .WithAuthority(tenantSpecificAuthority)
                                .Build();

      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };

      var authResult = appConfidential.AcquireTokenForClient(scopes).ExecuteAsync().Result;

      return authResult.AccessToken;

    }

    public static string GetAccessTokenWithManagedIdentity() {
      
      // very nice! - no need to track AppId, AppSecret or TenantId
      
      var credential = new ManagedIdentityCredential();
      string[] scopes = { "https://analysis.windows.net/powerbi/api/.default" };
      AccessToken token = credential.GetToken(new Azure.Core.TokenRequestContext(scopes));

      return token.Token;

    }

  }

}


