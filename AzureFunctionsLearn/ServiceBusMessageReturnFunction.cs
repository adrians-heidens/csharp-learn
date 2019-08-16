using System.Net.Http;
using System.Text;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Demonstrates function integration of sending data to a service bus by
    /// returning the BrokeredMessage where it is possible to set additional
    /// message parameters.
    /// </summary>
    public static class ServiceBusMessageReturnFunction
    {
        [FunctionName("ServiceBusMessageReturnFunction")]
        [return: ServiceBus("%My.ServiceBusTopic%", Connection = "My.ServiceBusConnection")]
        public static BrokeredMessage Run([HttpTrigger("get")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("ServiceBusMessageReturnFunction");

            var content = "test-content: \u0123\u0137";
            var message = new BrokeredMessage(Encoding.UTF8.GetBytes(content));
            message.Properties["MessageType"] = "Type Foo";
            message.ContentType = "application/json";

            return message;
        }
    }
}
