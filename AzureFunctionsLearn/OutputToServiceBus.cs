using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Demonstrates function integration of sending data to a service bus.
    /// </summary>
    public static class OutputToServiceBus
    {
        [FunctionName("OutputToServiceBus")]
        [return: ServiceBus("test", Connection = "ServiceBusConnection")]
        public static string Run([HttpTrigger("get")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("OutputToServiceBus");
            return "test-content";
        }
    }
}
