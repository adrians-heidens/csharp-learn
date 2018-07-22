using System;
using System.Net.Http;

namespace HttpClientLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Making HTTP request...");

            HttpClient httpClient = new HttpClient();

            var response = httpClient.GetAsync("https://google.lv").Result;
            Console.WriteLine(response);

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
