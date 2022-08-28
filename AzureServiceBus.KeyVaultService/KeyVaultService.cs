
using Microsoft.Extensions.Configuration;

namespace AzureServiceBus.Services
{
    public class KeyVaultService
    {

       

       //SecretClientOptions options = new SecretClientOptions()
        //{
        //    Retry =
        //         {
        //            Delay= TimeSpan.FromSeconds(2),
        //            MaxDelay = TimeSpan.FromSeconds(16),
        //            MaxRetries = 5,
        //            Mode = RetryMode.Exponential
        //         }
        //};

        //var client = new SecretClient(new Uri("https://<your-unique-key-vault-name>.vault.azure.net/"), new DefaultAzureCredential(), options);

        //KeyVaultSecret secret = client.GetSecret("<mySecret>");

        //string secretValue = secret.Value;


    }
}