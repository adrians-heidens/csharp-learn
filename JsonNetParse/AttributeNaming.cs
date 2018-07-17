using JsonNetParse.Models;
using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    class AttributeNaming
    {
        public static void Run()
        {
            var jsonString = "{\"id\": 2, \"fullname\": \"John Smith\"}";
            var person = JsonConvert.DeserializeObject<Person>(jsonString);
            Console.WriteLine(person);

            jsonString = JsonConvert.SerializeObject(person);
            Console.WriteLine(jsonString);
        }
    }
}
