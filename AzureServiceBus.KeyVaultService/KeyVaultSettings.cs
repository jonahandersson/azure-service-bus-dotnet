using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBus.Services
{
    public sealed class KeyVaultSettings
    {
        public string AzureKeyVaultName { get; set; }
        public string AzureKeyVaultConnectionString { get; set; }

        public bool AzureKeyVaultEnabled { get; set; }
        public string AzureKeyVaultUri { get; set; }

    }
}
