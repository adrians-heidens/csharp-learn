using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Text;
using System.Threading.Tasks;

namespace AzureServiceBusLearn
{
    /// <summary>
    /// Example of directly receiving messages from Azure Service Bus subscription.
    /// </summary>
    static class ReceiveFromSubSync
    {
        public static void Run()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        private static async Task RunAsync()
        {
            string serviceBusConnectionString = "connection-string";
            string topicName = "testtopic";
            string subscriptionName = "testTopicSub";

            var entityPath = EntityNameHelper.FormatSubscriptionPath(topicName, subscriptionName);
            var receiver = new MessageReceiver(serviceBusConnectionString, entityPath, ReceiveMode.ReceiveAndDelete) {
                OperationTimeout = TimeSpan.FromSeconds(5),
            };

            var message = await receiver.ReceiveAsync();
            if (message == null)
            {
                Console.WriteLine("null");
            }
            else
            {
                Console.WriteLine($"Received message: " +
                    $"SequenceNumber:{message.SystemProperties.SequenceNumber} " +
                    $"Body:{Encoding.UTF8.GetString(message.Body)}");
            }

            Console.WriteLine("Done.");

            Console.ReadKey();
        }
    }
}
