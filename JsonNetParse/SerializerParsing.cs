using JsonNetParse.Models;
using Newtonsoft.Json;
using System;
using System.IO;

namespace JsonNetParse
{
    class SerializerParsing
    {
        public static void Run()
        {
            JsonSerializer serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;

            var productWatch = new Product
            {
                Id = 1,
                Name = "Mens Analog Quartz Wrist Watch",
                Description = "Classic Casual Watch with Brown Leather Band."
            };
            var streamWriter = new StreamWriter(Console.OpenStandardOutput());
            streamWriter.AutoFlush = true;
            serializer.Serialize(streamWriter, productWatch);
            streamWriter.WriteLine();
        }
    }
}
