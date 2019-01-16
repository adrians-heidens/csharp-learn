using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AzureFunctions2
{
    public static class Hello
    {
        [FunctionName("Hello")]
        public static IActionResult Run(
            [HttpTrigger("get")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Hello");
            return new OkObjectResult("OK");
        }
    }
}
