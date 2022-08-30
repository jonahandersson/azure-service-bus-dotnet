using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBus.Services
{
    public sealed class ServiceBusSettings
    {
        public string ServiceBusQueueName { get; set; }
        public string ServiceBusNameSpaceConnectionString { get; set; }
    }
}
