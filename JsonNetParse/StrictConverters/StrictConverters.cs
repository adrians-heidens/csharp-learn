using Newtonsoft.Json;
using System;

namespace JsonNetParse.StrictConverters
{
    static class StrictConverters
    {
        public static void Run()
        {
            // Expected json.
            var json = @"
{
""Name"": ""John"",
""Weight"": 73.75,
""BornDateTime"": ""1975-01-02T16:45"",
""Number"": 7
}
";
            
            var person = JsonConvert.DeserializeObject<Person>(json);
            Console.WriteLine(person);

            Console.WriteLine(JsonConvert.SerializeObject(person, Formatting.Indented));

            json = @"
{
""Model"": ""Focus"",
""Length"": 73.75
}
";

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new NumberConverter());
            var car = JsonConvert.DeserializeObject<Car>(json, settings);
            Console.WriteLine(car);

            //return;

            // By default strings are converted to numbers.
            json = @"
{
""Name"": ""John"",
""Weight"": ""73.75"",
""BornDateTime"": ""1975-01-02T16:45"",
""Number"": ""6""
}
";
            try
            {
                person = JsonConvert.DeserializeObject<Person>(json);
                Console.WriteLine(person);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e.Message);
            }
            
            // String numbers of decimal separator ','.
            json = @"
{
""Name"": ""John"",
""Weight"": ""73,75"",
""BornDateTime"": ""1975-01-02T16:45"",
""Number"": ""3""
}
";
            try
            {
                person = JsonConvert.DeserializeObject<Person>(json);
                Console.WriteLine(person);
            }
            catch (JsonReaderException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
