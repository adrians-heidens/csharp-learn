using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    public static class ReturnStuffFunction
    {
        [FunctionName("ReturnStuffFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log
            )
        {
            log.Info("Function returning various results.");

            string command = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "command", true) == 0)
                .Value;

            if (command == null || command == "null")
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
                log.Info("Returning null");
                return null;
            }
        }
    }
}
