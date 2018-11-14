using System;
using System.Linq;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    public class Person
    {
        public string Name { get; set; }
    }

    public static class QueueReturnFunction
    {
        [FunctionName("QueueReturnFunction")]
        [return: Queue("returnqueue")]
        public static Person Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log
            )
        {
            log.Info("Function returning result to the queue.");

            string command = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "command", true) == 0)
                .Value;

            if (command == "null")
            {
                log.Info("Returning null");
                return null;
            }
            else if (command == "exception")
            {
                log.Info("Throwing exception");
                throw new Exception("Test exception");
            }
            else
            {
                log.Info("Returning Person");
                return new Person { Name = "Adrians" };
            }
        }
    }
}
