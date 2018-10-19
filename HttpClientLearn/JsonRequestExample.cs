using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HttpClientLearn
{
    static class JsonRequestExample
    {
        public static void Run()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("http://127.0.0.1:8080");
            httpClient.Timeout = TimeSpan.FromSeconds(5);
            string jsonString = "{foo: 1, bar: 'spam'}";
            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            try
            {
                response = httpClient.PostAsync("/foo", content).Result;
            } catch (Exception e)
            {
                Console.WriteLine($"Exception: {e.Message}");
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                var body = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"Error: status={(int)response.StatusCode}, body='{body}'");
            }
            else
            {
                var body = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine($"Success: status={(int)response.StatusCode}, body='{body}'");
            }
        }
    }
}
