using System.Net.Http;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Demonstrates return of multiple BrokeredMessage objects.
    /// </summary>
    public static class ServiceBusMultiMessageReturnFunction
    {
        [FunctionName("ServiceBusMultiMessageReturnFunction")]
        public static void Run(
            [HttpTrigger("get")]HttpRequestMessage req,
            TraceWriter log,
            [ServiceBus("%My.ServiceBusTopic%", Connection = "My.ServiceBusConnection")]ICollector<BrokeredMessage> output
            )
        {
            log.Info("ServiceBusMultiMessageReturnFunction");

            for (var i = 0; i < 3; i++)
            {
                var content = $"test-content: {i}";
                var message = new BrokeredMessage(Encoding.UTF8.GetBytes(content));
                message.Properties["MessageType"] = "Type Foo";
                message.ContentType = "application/json";
                output.Add(message);
            }
        }
    }
}
