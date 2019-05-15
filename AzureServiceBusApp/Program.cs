using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Core;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace AzureServiceBusApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if help requested.
            if (args.Any(x => x == "--help"))
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                PrintUsage(name);
                Environment.Exit(0);
            }

            // Gather arguments and their values in a dictionary.
            var argsDict = new Dictionary<string, List<string>>();
            List<string> current = null;
            foreach (var arg in args)
            {
                if (arg.StartsWith("--"))
                {
                    var key = arg.Substring(2);
                    if (argsDict.ContainsKey(key))
                    {
                        current = argsDict[key];
                    }
                    else
                    {
                        current = new List<string>();
                        argsDict[key] = current;
                    }
                }
                else
                {
                    current.Add(arg);
                }
            }

            // Check mandatory command parameter.
            if (!argsDict.ContainsKey("command"))
            {
                var name = AppDomain.CurrentDomain.FriendlyName;
                PrintUsage(name);
                Environment.Exit(2);
            }

            // Execute specified command.
            var command = argsDict["command"][0];

            if (command == "topic-send")
            {
                var connectionString = argsDict["connection-string"][0];
                var topicName = argsDict["topic-name"][0];

                SendToTopic(connectionString, topicName)
                    .GetAwaiter().GetResult();
            }

            else if (command == "topic-subscription-receive")
            {
                var connectionString = argsDict["connection-string"][0];
                var topicName = argsDict["topic-name"][0];
                var subscriptionName = argsDict["subscription-name"][0];

                ReceiveFromSubscription(connectionString, topicName, subscriptionName)
                    .GetAwaiter().GetResult();
            }

            else
            {
                Console.WriteLine($"Unknown command: '{command}'");
                Environment.Exit(1);
            }
        }

        private static void PrintUsage(string prog)
        {
            Console.WriteLine($@"
Usage: {prog} --command COMMAND COMMAND_PARAMS...

A command line tool for interacting with Azure Service Bus.

topic-send

  Send message to an Azure Service Bus topic.

  --connection-string CONN    Azure Service Bus connection string.
  --topic-name TOPIC          Azure Service Bus topic.

topic-subscription-receive

  Receive message from Azure Service Bus topic subscription.

  --connection-string CONN    Azure Service Bus connection string.
  --topic-name TOPIC          Azure Service Bus topic.
  --subscription-name SUB     Azure Service Bus subscription.

EXAMPLES:

  {prog} --command topic-send --connection-string ""Endpoint=sb://url"" --topic-name ""example""

".Trim());
        }

        private static async Task ReceiveFromSubscription(
            string connectionString,
            string topicName,
            string subscriptionName
            )
        {
            var entityPath = EntityNameHelper.FormatSubscriptionPath(topicName, subscriptionName);
            var receiver = new MessageReceiver(connectionString, entityPath, ReceiveMode.ReceiveAndDelete)
            {
                OperationTimeout = TimeSpan.FromSeconds(5),
            };

            Console.WriteLine("Receiving...");
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
        }

        private static async Task SendToTopic(string connectionString, string topicName)
        {
            var topicClient = new TopicClient(connectionString, topicName);

            string messageBody = "Hello World!";
            var message = new Message(Encoding.UTF8.GetBytes(messageBody));

            await topicClient.SendAsync(message);
        }
    }
}
