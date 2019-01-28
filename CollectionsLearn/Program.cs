using System;
using System.Collections.Generic;
using System.Linq;

namespace CollectionsLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            IDictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("foo", "Foo");
            dict.Add("bar", "Bar");
            dict.Add("spam", null);

            Console.WriteLine(dict.ContainsKey("foo"));

            var value = dict["foo"]; // It not found throws KeyNotFoundException.
            Console.WriteLine(value);

            dict.TryGetValue("Foo", out value); // Get value or null.
            Console.WriteLine(value == null);

            Console.WriteLine("----");

            List<string> list = new List<string> { "foo", "bar", "spam" }; // Collection initializer.
            value = list.Find(x => x == "Foo"); // Find value or null.
            Console.WriteLine(value == null);

            // Print list.
            Console.WriteLine(string.Join(',', list));

            // Print dict.
            var s = string.Join(';', dict.Select(x => $"{x.Key}='{x.Value}'"));
            Console.WriteLine(s);

            Console.WriteLine("Press any key...");
            Console.ReadKey();
        }
    }
}
