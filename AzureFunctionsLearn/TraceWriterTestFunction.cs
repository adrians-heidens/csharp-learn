using System.Net;
using System.Linq;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System;

namespace AzureFunctionsLearn
{
    public static class TraceWriterTestFunction
    {
        [FunctionName("TraceWriterTestFunction")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("Testing Azure TraceWriter logging features.");

            var parameters = req.GetQueryNameValuePairs();
            var aStr = parameters.FirstOrDefault(x => x.Key == "a").Value;
            var bStr = parameters.FirstOrDefault(x => x.Key == "b").Value;

            int a = 0;
            int b = 0;
            try
            {
                a = int.Parse(aStr);
                b = int.Parse(bStr);
            } catch (Exception e)
            {
                log.Error("Arg parse error", e);
                return req.CreateResponse(HttpStatusCode.BadRequest, "ERROR");
            }
            
            log.Info($"a = {a}, b = {b}");

            int c = 0;
            try
            {
                c = CalculateValue(a, b);
            }
            catch (Exception e)
            {
                log.Error(e.ToString(), e);
                throw; // Indicate this request as failed.
            }

            log.Info($"c = {c}");

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }

        private static int CalculateValue(int a, int b)
        {
            return a / b;
        }
    }
}
