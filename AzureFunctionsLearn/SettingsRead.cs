using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    /// <summary>
    /// Demonstrates getting application settings.
    /// </summary>
    public static class SettingsRead
    {
        [FunctionName("SettingsRead")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log
            )
        {
            log.Info("Demonstrating access to application settings.");

            // Listing all.
            var variableDict = Environment.GetEnvironmentVariables();
            foreach (DictionaryEntry entry in variableDict)
            {
                log.Info($"{entry.Key}: {entry.Value}");
            }

            // Getting specific.
            var syslogServer = Environment.GetEnvironmentVariable("SYSLOG_SERVER_HOST");
            log.Info($"Syslog server: {syslogServer}");

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
