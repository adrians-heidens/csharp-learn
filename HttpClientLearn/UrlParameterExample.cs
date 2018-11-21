using System;
using System.Net.Http;
using System.Web;

namespace HttpClientLearn
{
    /// <summary>
    /// Passing parameters in URLs.
    /// </summary>
    static class UrlParameterExample
    {
        public static void Run()
        {
            HttpClient httpClient = new HttpClient();
            var response = httpClient.GetAsync(GetUrl2()).Result;
            Console.WriteLine(response);
        }

        /// <summary>
        /// Using only http query util to build url.
        /// </summary>
        private static string GetUrl1()
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["foo"] = "foo bar <spam>";
            query["bar"] = "12";
            var baseUrl = "http://127.0.0.1:8080";
            return baseUrl + "/?" + query;
        }

        /// <summary>
        /// Using builder to compose the url.
        /// </summary>
        private static string GetUrl2()
        {
            var builder = new UriBuilder("http://127.0.0.1:8080");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["foo"] = "foo bar <spam>";
            query["bar"] = "12";
            builder.Query = query.ToString();
            return builder.ToString();
        }
    }
}
