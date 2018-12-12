using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Try logging various messages.
    /// </summary>
    public static class ILoggerTestFunction
    {
        [FunctionName("ILoggerTestFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            ILogger log)
        {
            log.LogInformation("ILogger info message.");
            log.LogDebug("ILogger debug message.");
            log.LogCritical("ILogger critical message.");
            log.LogError("ILogger error message.");
            log.LogTrace("ILogger trace message.");
            log.LogWarning("ILogger warning message.");

            int[] array = { 1, 2 };
            
            try
            {
                log.LogInformation(array[2].ToString()); // Exception.
            } catch (Exception e)
            {
                log.LogCritical(default(EventId), e, "Exception occured"); // This logs exception as well.
                //log.LogCritical(e.ToString()); // Logs only string message.
                return req.CreateResponse(HttpStatusCode.InternalServerError, "ERROR");
            }

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
