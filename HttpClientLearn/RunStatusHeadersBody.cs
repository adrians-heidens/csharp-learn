using System;
using System.Linq;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// Example of getting more data from HTTP response.
    /// </summary>
    static class RunStatusHeadersBody
    {
        public static void Run()
        {
            Console.WriteLine("Making HTTP request...");

            HttpClient httpClient = new HttpClient();

            // Show response content.
            var response = httpClient.GetAsync("http://127.0.0.1:8080").Result;
            Console.WriteLine($"StatusCode: '{response.StatusCode.ToString("d")}'");
            Console.WriteLine($"ReasonPhrase: '{response.ReasonPhrase}'");

            Console.WriteLine("Headers:");
            foreach (var header in response.Headers)
            {
                var value = header.Value.FirstOrDefault();
                var key = header.Key;
                Console.WriteLine($"  {key}: {value}");
            }

            Console.WriteLine("Content Headers:");
            foreach (var header in response.Content.Headers)
            {
                var value = header.Value.FirstOrDefault();
                var key = header.Key;
                Console.WriteLine($"  {key}: {value}");
            }

            // Get specific header value if it exists.
            string server = null;
            if (response.Headers.Contains("server"))
            {
                server = response.Headers.GetValues("server").FirstOrDefault();
            }
            Console.WriteLine($"Server: '{server}'");
            
            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
