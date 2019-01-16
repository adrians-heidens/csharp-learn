using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    static class DateTimeExample
    {
        public static void Run()
        {
            var now = DateTime.Now;
            var json = JsonConvert.SerializeObject(now);
            Console.WriteLine(json);
        }
    }
}
