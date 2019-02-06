using JsonNetParse.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace JsonNetParse
{
    /// <summary>
    /// A common scenario where it is necessary to translate from one Json
    /// representation to another.
    /// </summary>
    static class TranslationExample
    {
        public static void Run()
        {
            // Incoming Json.
            var json = @"
{
    ""Model"": ""Focus"",
    ""Manufacturer"": ""Ford"",
    ""Details"": {
        ""Class"": ""Compact"",
        ""Layout"": ""FF""
    }
}
";
            // We aim to translate it to following.
            /*
{
    ""Model"": ""Focus"",
    ""Manufacturer"": ""Ford"",
    ""Class"": ""Compact"",
    ""Layout"": ""FF"",
    ""ProcessedDate"": ""2001-02-03T00:00:00""
}
             */

            // 1. Deserialize to expected object.
            var car1 = JsonConvert.DeserializeObject<Car>(json);
            Console.WriteLine(car1);

            // 2. Get JObject to manipulate.
            var carObj = JObject.FromObject(car1);
            carObj.Remove("Details");
            carObj.Add("ProcessedDate", new DateTime(2001, 2, 3));
            
            // 3. Convert JObject to Json.
            json = JsonConvert.SerializeObject(carObj, Formatting.Indented);
            Console.WriteLine();
            Console.WriteLine(json);
        }
    }
}
