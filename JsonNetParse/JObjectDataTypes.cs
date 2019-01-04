using Newtonsoft.Json.Linq;
using System;

namespace JsonNetParse
{
    /// <summary>
    /// Example of explicit convertion of values to data types.
    /// </summary>
    static class JObjectDataTypes
    {
        public static void Run()
        {
            var json = "{'a_string': 'some string', 'an_int': 12, 'a_float': 12.5}";
            var obj = JObject.Parse(json);
            foreach (var item in obj)
            {
                var key = item.Key;
                var value = item.Value;

                if (value.Type == JTokenType.String)
                {
                    var v = value.ToObject<string>();
                    Console.WriteLine($"{v} ({v.GetType()})");
                }
                else if (value.Type == JTokenType.Integer)
                {
                    var v = value.ToObject<int>();
                    Console.WriteLine($"{v} ({v.GetType()})");
                }
                else if (value.Type == JTokenType.Float)
                {
                    var v = value.ToObject<double>();
                    Console.WriteLine($"{v} ({v.GetType()})");
                }
                else if (value.Type == JTokenType.Null)
                {
                    Console.WriteLine($"null");
                }
                else
                {
                    var v = value.ToObject<object>();
                    Console.WriteLine($"{v} ({v.GetType()})");
                }
            }
        }
    }
}
