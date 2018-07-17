using JsonNetParse.Models;
using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    class SimpleParsing
    {
        public static void Run()
        {
            // Serialize object to Json.
            var productWatch = new Product
            {
                Id = 1,
                Name = "Mens Analog Quartz Wrist Watch",
                Description = "Classic Casual Watch with Brown Leather Band."
            };
            var jsonString = JsonConvert.SerializeObject(productWatch);
            Console.WriteLine(jsonString);

            // Deserialize object from Json string.
            jsonString = "{\"Id\": 2, \"Name\": \"Wood Swing Arm Desk Table Lamp\", " +
                "\"Description\": \"Nature Wood Frame The body is made of solid wood.\"}";
            var productLamp = JsonConvert.DeserializeObject<Product>(jsonString);
            Console.WriteLine(productLamp);
        }
    }
}
