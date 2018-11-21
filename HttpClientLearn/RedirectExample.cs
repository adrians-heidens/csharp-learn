using System;
using System.Net;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// Example of manual redirect handling (AllowAutoRedirect = false).
    /// </summary>
    static class RedirectExample
    {
        public static void Run()
        {
            HttpClientHandler handler = new HttpClientHandler {
                AllowAutoRedirect = false,
            };
            HttpClient httpClient = new HttpClient(handler);
            var response = httpClient.GetAsync("http://127.0.0.1:8080/redirect2").Result;
            Console.WriteLine(response);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var location = response.Headers.Location;
                Console.WriteLine($"Redirect to '{location}'");

                response = httpClient.GetAsync(location).Result;
                Console.WriteLine(response);
            }

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                var location = response.Headers.Location;
                Console.WriteLine($"Redirect again to '{location}'");

                response = httpClient.GetAsync(location).Result;
                Console.WriteLine(response);
            }
        }
    }
}
