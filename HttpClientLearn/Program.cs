using System;
using System.Net.Http;

namespace HttpClientLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            SimpleGetRequest.Run();

            Console.WriteLine("End.");
            Console.ReadKey();
        }
    }
}
