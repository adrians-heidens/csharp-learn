using System;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// Most basic example of making HTTP GET request.
    /// </summary>
    static class SimpleGetRequest
    {
        public static void Run()
        {
            Console.WriteLine("Making HTTP request...");

            HttpClient httpClient = new HttpClient();

            var response = httpClient.GetAsync("https://google.lv").Result;
            Console.WriteLine(response);
        }
    }
}
