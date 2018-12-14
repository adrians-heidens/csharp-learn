using JsonNetParse.Models;
using Newtonsoft.Json;
using System;

namespace JsonNetParse
{
    /// <summary>
    /// Demonstration of reference loop handling.
    /// </summary>
    static class SelfReferenceTest
    {
        public static void Run()
        {
            var node = new Node { Name = "Foo" };
            node.NextNode = node; // A loop to self.

            var node2 = new Node { Name = "Bar", NextNode = node };

            // Newtonsoft.Json.JsonSerializationException: 'Self referencing loop.
            //var json = JsonConvert.SerializeObject(node, Formatting.Indented);

            // Ignore self reference.
            var json = JsonConvert.SerializeObject(node, new JsonSerializerSettings {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            Console.WriteLine(json); // {"Name":"Foo"}


            json = JsonConvert.SerializeObject(node2, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
            Console.WriteLine(json); // {"Name":"Bar","NextNode":{"Name":"Foo"}}
        }
    }
}
