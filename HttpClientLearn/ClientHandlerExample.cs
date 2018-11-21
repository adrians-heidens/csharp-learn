using System;
using System.Net;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// An example of using http client handler to set initial cookie
    /// value. By default cookie values are being set as a result from
    /// according HTTP response and used in subsequent requests.
    /// </summary>
    static class ClientHandlerExample
    {
        public static void Run()
        {
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri("http://127.0.0.1:8080");

            // Default headers.
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Firefox");
            httpClient.DefaultRequestHeaders.Add("Host", "f00.lv");
            httpClient.DefaultRequestHeaders.Add("DNT", "1");

            handler.UseCookies = true; // This is the default.
            var cookieContainer = handler.CookieContainer;
            cookieContainer.Add(httpClient.BaseAddress, new Cookie("counter", "1000"));

            var response = httpClient.GetAsync("/").Result;
            Console.WriteLine(response);

            response = httpClient.GetAsync("/").Result;
            Console.WriteLine(response);

            // Override default header value.
            var request = new HttpRequestMessage(HttpMethod.Get, "/foo");
            request.Headers.Add("User-Agent", "Opera");
            response = httpClient.SendAsync(request).Result;
            Console.WriteLine(response);
        }
    }
}
