using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace JsonNetParse.EnumHandling
{
    static class EnumHandlingExample
    {
        public static void Run()
        {
            var json = "{\"Name\": \"Foo\", \"Ttl\": 12, \"RType\": \"CNAME\"}";
            var o = JsonConvert.DeserializeObject<Record>(json);
            Console.WriteLine(o.Name);
            Console.WriteLine(o.Ttl);
            Console.WriteLine(o.RType);
            var converters = new List<JsonConverter> { new StringEnumConverter() };
            var settings = new JsonSerializerSettings { Converters = converters };
            Console.WriteLine(JsonConvert.SerializeObject(o, settings));
        }
    }
}
