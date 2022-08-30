using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AzureServiceBus.Services;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace AzureServiceBus.Queue.Sender
{

    public class Program
    {
        static async Task Main()
        {

            // Build a config object, using env vars and JSON providers.
            var config = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .AddEnvironmentVariables()
                        .Build();

            //TODO: Use KeyVault and setup service
            //KeyVaultSettings keyVaultSettings = config.GetRequiredSection("KeyVaultSettings").Get<KeyVaultSettings>();

            ServiceBusSettings serviceBusSettings = config.GetRequiredSection("ServiceBusSettings").Get<ServiceBusSettings>();
            var connectionString = serviceBusSettings.ServiceBusNameSpaceConnectionString;
            var queueName = serviceBusSettings.ServiceBusQueueName;

            ServiceBusClient client;
            ServiceBusSender sender;

            // The Service Bus client types are safe to cache and use as a singleton for the lifetime
            // of the application, which is best practice when messages are being published or read
            // regularly. Set the transport type to AmqpWebSockets so that the ServiceBusClient uses the port 443. 
            // If you use the default AmqpTcp, you will need to make sure that the ports 5671 and 5672 are open

            var clientOptions = new ServiceBusClientOptions() { TransportType = ServiceBusTransportType.AmqpWebSockets };
            client = new ServiceBusClient(connectionString, clientOptions);
            sender = client.CreateSender(queueName);

            var persons = GetListOfPeopleFromJsonFile();
            // create a batch 
            using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

            foreach (var person in persons)
            {
                // try adding a message to the batch
               
                if (!messageBatch.TryAddMessage(
                    new ServiceBusMessage($"Hello {person.FirstName} {person.LastName} ! Welcome to Azure Back to School!")))
                {

                    // if it is too large for the batch
                    throw new Exception($"The queue message for {person}  is too large to fit in the batch.");
                }
            }
          
            try
            {
                // Use the producer client to send the batch of messages to the Service Bus queue
                await sender.SendMessagesAsync(messageBatch);
                Console.WriteLine($"A batch of {persons.Count} messages has been published to the queue.");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }

            Console.WriteLine("Press any key to end the application");
            Console.ReadKey();
        }

        private static List<Person> GetListOfPeopleFromJsonFile()
        {
            try
            {
                List<Person> persons = new List<Person>();
                using (StreamReader streamReader = new StreamReader("data/people.json"))
                {
                    string json = streamReader.ReadToEnd();
                    if (json != null)
                    {                      

                        persons = JsonConvert.DeserializeObject<List<Person>>(json);
                    } 
                }
                return persons;
            }
            catch (Exception)
            {
              
                throw new Exception($"Hey! Something went wrong reading the json file. Check what's wrong. ");
            }           

        }
    }
}