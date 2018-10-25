using JsonNetParse.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonNetParse
{
    static class NestedObjectsTest
    {
        public static void Run()
        {
            var bundle = new Bundle { Id = 1, Name = "Bundle One" };
            bundle.Items.Add(new BundleItem { Id = 10, Quantity = 1 });
            bundle.Items.Add(new BundleItem { Id = 11, Quantity = 1 });

            string json = JsonConvert.SerializeObject(bundle, Formatting.Indented);
            Console.WriteLine(json);

            bundle = JsonConvert.DeserializeObject<Bundle>(json);
            Console.WriteLine(bundle);

            Console.WriteLine("Nested objects learn");
        }
    }
}
