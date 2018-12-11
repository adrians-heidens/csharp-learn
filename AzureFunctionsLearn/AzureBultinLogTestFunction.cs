using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    public static class AzureBultinLogTestFunction
    {
        [FunctionName("AzureBultinLogTestFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("Testing Azure built-in logging features.");
            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
