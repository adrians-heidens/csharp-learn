using System;
using System.Collections.Generic;
using System.Net.Http;

namespace HttpClientLearn
{
    /// <summary>
    /// Example of making form POST requests.
    /// </summary>
    static class FormPostExample
    {
        public static void Run()
        {
            HttpClient httpClient = new HttpClient {
                BaseAddress = new Uri("http://127.0.0.1:8080"),
            };
            
            PostFormData(httpClient);
            PostFormUrlEncoded(httpClient);

        }

        /// <summary>
        /// Encode form data with content type 'multipart/form-data'.
        /// </summary>
        private static void PostFormData(HttpClient httpClient)
        {
            var httpContent = new MultipartFormDataContent();
            httpContent.Add(new StringContent("12"), "foo");
            httpContent.Add(new StringContent("spam"), "bar");
            var response = httpClient.PostAsync("/form-data", httpContent).Result;
            Console.WriteLine(response);
        }

        /// <summary>
        /// Encode form data with content type 'application/x-www-form-urlencoded'.
        /// </summary>
        private static void PostFormUrlEncoded(HttpClient httpClient)
        {
            var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "foo", "12" },
                { "bar", "spam" },
            });
            var response = httpClient.PostAsync("/form-encode", httpContent).Result;
            Console.WriteLine(response);
        }
    }
}
