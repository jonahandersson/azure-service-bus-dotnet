using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace AzureServiceBus.Functions
{
    /// <summary>
    /// This is an Azure Function with Service Bus Trigger that receives 
    /// messages fromm Azure Service Bus Queueue and send an email using SendGrid API
    /// </summary>
    public class ServiceBusQueueReceiver
    {
        [FunctionName("GetMessagesFromAzureBackToSchoolQueue")]
        public void Run([ServiceBusTrigger("azurebacktoschoolqueue", Connection = "AzureServiceBusConnectionString")]string myQueueMessageItem, ILogger log)
        {
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueMessageItem}");
            log.LogInformation($"Sending email with message: {myQueueMessageItem}");

            try
            {
                if (myQueueMessageItem != null)
                {
                    SendEmailAsync(myQueueMessageItem);
                }
            }
            catch (Exception ex)
            {
                log.LogError($"Exception: {ex.Message}");              
            }           
        }

        private async Task SendEmailAsync(string myQueueMessageItem)
        {   
            var apiKey = Environment.GetEnvironmentVariable("SendGrid_API_KEY");
            var adminEmailAddress = Environment.GetEnvironmentVariable("AdminEmailAddress");
            var recipientEmailAddress = Environment.GetEnvironmentVariable("RecipientEmailAddress");

            var client = new SendGridClient(apiKey);
            var emailMessage = new SendGridMessage()
            {
                From = new EmailAddress(adminEmailAddress, "Jonah Andersson"),
                Subject = "Email from Service Bus Receive Function for Azure Back to School 2022!",
                PlainTextContent = myQueueMessageItem
            };

            emailMessage.AddTo(new EmailAddress(recipientEmailAddress, "Jonah at Work Email"));
            var response = await client.SendEmailAsync(emailMessage);

            // A success status code means SendGrid received the email request and will process it.
            // Errors can still occur when SendGrid tries to send the email. 
            // If email is not received, use this URL to debug: https://app.sendgrid.com/email_activity 
            Console.WriteLine(response.IsSuccessStatusCode ? "Email queued successfully!" : "Something went wrong!");
        }
    }
}
