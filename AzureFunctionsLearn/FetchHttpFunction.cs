using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AzureFunctionsLearn
{
    public static class FetchHttpFunction
    {
        [FunctionName("FetchHttpFunction")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req,
            TraceWriter log)
        {
            log.Info("Making http request and fetch response as string.");

            var httpClient = new HttpClient();
            var content = await httpClient.GetStringAsync("http://f00.lv/foo");

            log.Info($"String received: '{content}'");

            return req.CreateResponse(HttpStatusCode.OK, "OK");
        }
    }
}
