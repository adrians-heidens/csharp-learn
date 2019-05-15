using log4net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ServiceBus.Messaging;
using System.Threading;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Example of Azure Function triggered by Service Bus.
    /// </summary>
    public static class ServiceBusTopicTriggerFunction
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ServiceBusTopicTriggerFunction));

        [Singleton(Mode = SingletonMode.Function)] // Only one instance of this function.
        [FunctionName("ServiceBusTopicTriggerFunction")]
        public static void Run(
            [ServiceBusTrigger("campaignsend", "sub", AccessRights.Listen, Connection = "My.ServiceBusConnection")]string mySbMsg, TraceWriter traceWriter)
        {
            LogUtils.ConfigureLogger(traceWriter);
            log.Info($"C# ServiceBus topic trigger function processed message: {mySbMsg}");

            // Sleep to test concurrent invocations of this function.
            log.Info("Sleeping");
            Thread.Sleep(5000);
            log.Info("Done");
        }
    }
}
